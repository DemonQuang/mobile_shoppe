using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mobile_app_shoppe.Forms.User
{
    public partial class Conform_Details : Form
    {
        private string imei, model, warranty, company;
        private string customerName, mobile, address, email, price;

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                // --- Insert/Update Customer ---
                string customerId = Guid.NewGuid().ToString().Substring(0, 8);
                using (SqlCommand cmdCust = new SqlCommand(@"
            IF NOT EXISTS (SELECT * FROM tbl_Customer WHERE Cust_Name=@CustName AND MobileNumber=@Mobile)
            INSERT INTO tbl_Customer(CustId, Cust_Name, MobileNumber, EmailId, Address)
            VALUES(@CustId,@CustName,@Mobile,@Email,@Address)", con))
                {
                    cmdCust.Parameters.AddWithValue("@CustId", customerId);
                    cmdCust.Parameters.AddWithValue("@CustName", customerName);
                    cmdCust.Parameters.AddWithValue("@Mobile", mobile);
                    cmdCust.Parameters.AddWithValue("@Email", email);
                    cmdCust.Parameters.AddWithValue("@Address", address);
                    cmdCust.ExecuteNonQuery();
                }

                // --- Lấy CustId ---
                string finalCustId;
                using (SqlCommand cmdGetCust = new SqlCommand(
                    "SELECT CustId FROM tbl_Customer WHERE Cust_Name=@CustName AND MobileNumber=@Mobile", con))
                {
                    cmdGetCust.Parameters.AddWithValue("@CustName", customerName);
                    cmdGetCust.Parameters.AddWithValue("@Mobile", mobile);
                    finalCustId = cmdGetCust.ExecuteScalar().ToString();
                }

                // --- Insert Sales ---
                string saleId = Guid.NewGuid().ToString().Substring(0, 8);
                using (SqlCommand cmdSale = new SqlCommand(@"
            INSERT INTO tbl_Sales(SId, IMEINO, PurchaseDate, Price, CustId)
            VALUES(@SId,@IMEI,GETDATE(),@Price,@CustId)", con))
                {
                    cmdSale.Parameters.AddWithValue("@SId", saleId);
                    cmdSale.Parameters.AddWithValue("@IMEI", imei);
                    cmdSale.Parameters.AddWithValue("@Price", Convert.ToDecimal(price));
                    cmdSale.Parameters.AddWithValue("@CustId", finalCustId);
                    cmdSale.ExecuteNonQuery();
                }

                // --- Update Mobile & Model ---
                using (SqlCommand cmdUpdateMobile = new SqlCommand(
                    "UPDATE tbl_Mobile SET Status='Sold' WHERE IMEINO=@IMEI", con))
                {
                    cmdUpdateMobile.Parameters.AddWithValue("@IMEI", imei);
                    cmdUpdateMobile.ExecuteNonQuery();
                }
                using (SqlCommand cmdUpdateModel = new SqlCommand(@"
            UPDATE tbl_Model 
            SET AvailableQty = AvailableQty - 1
            WHERE ModelId = (SELECT ModelId FROM tbl_Mobile WHERE IMEINO=@IMEI)", con))
                {
                    cmdUpdateModel.Parameters.AddWithValue("@IMEI", imei);
                    cmdUpdateModel.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Sale confirmed and data saved!");

                // Chỉ set DialogResult → form cha sẽ nhận OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error saving data: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        // Nút Hủy
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private SqlConnection con = new SqlConnection(
            @"Data Source=MSI\SQLEXPRESS;Initial Catalog=MobileStoreDB;Integrated Security=True");

        public Conform_Details(
            string customerName, string mobile, string address, string email,
            string company, string model, string imei, string price, string warranty)
        {
            InitializeComponent();

            this.customerName = customerName;
            this.mobile = mobile;
            this.address = address;
            this.email = email;
            this.company = company;
            this.model = model;
            this.imei = imei;
            this.price = price;
            this.warranty = warranty;

            // Hiển thị dữ liệu
            lblCustomerName.Text = customerName;
            lblMobileNo.Text = mobile;
            lblAddress.Text = address;
            lblEmail.Text = email;
            lblCompany.Text = company;
            lblModel.Text = model;
            lblIMEI.Text = imei;
            lblPrice.Text = price;
            lblWarranty.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }
    }
}
