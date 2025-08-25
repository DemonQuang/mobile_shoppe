using MobileAppShoppe.DataAccess;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileAppShoppe.Forms.User
{
    public partial class Conform_Details : Form
    {
        string custName, mobile, address, email, company, model, imei, price, modelId, warranty;

        private void Conform_Details_Load_1(object sender, EventArgs e)
        {
            lblCustomerName.Text = custName;
            lblMobile.Text = mobile;
            lblAddress.Text = address;
            lblEmail.Text = email;
            lblCompany.Text = company;
            lblModel.Text = model;
            lblIMEI.Text = imei;
            lblPrice.Text = price;
            lblWarranty.Text = warranty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public Conform_Details(
            string custName, string mobile, string address, string email,
            string company, string model, string imei, string price, string modelId, string warranty)
        {
            InitializeComponent();

            this.custName = custName;
            this.mobile = mobile;
            this.address = address;
            this.email = email;
            this.company = company;
            this.model = model;
            this.imei = imei;
            this.price = price;
            this.modelId = modelId;
            this.warranty = warranty;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string custId = "CUST" + Guid.NewGuid().ToString("N").Substring(0, 5);
                string saleId = "SALE" + Guid.NewGuid().ToString("N").Substring(0, 5);

                // Insert Customer
                string q1 = "INSERT INTO tbl_Customer (CustId, Cust_Name, MobileNumber, EmailId, Address) " +
                            "VALUES (@CustId, @Name, @Mobile, @Email, @Addr)";
                SqlParameter[] p1 = {
                    new SqlParameter("@CustId", custId),
                    new SqlParameter("@Name", custName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Addr", address)
                };
                DbHelper.ExecuteNonQuery(q1, p1);

                // Insert Sales
                string q2 = "INSERT INTO tbl_Sales (SId, IMEINO, PurchaseDate, Price, CustId) " +
                            "VALUES (@SId, @IMEI, GETDATE(), @Price, @CustId)";
                SqlParameter[] p2 = {
                    new SqlParameter("@SId", saleId),
                    new SqlParameter("@IMEI", imei),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@CustId", custId)
                };
                DbHelper.ExecuteNonQuery(q2, p2);

                // Update Mobile Status
                string q3 = "UPDATE tbl_Mobile SET Status='sold' WHERE IMEINO=@IMEI";
                SqlParameter[] p3 = { new SqlParameter("@IMEI", imei) };
                DbHelper.ExecuteNonQuery(q3, p3);

                // Update AvailableQty
                string q4 = "UPDATE tbl_Model SET AvailableQty = AvailableQty - 1 WHERE ModelId=@ModelId";
                SqlParameter[] p4 = { new SqlParameter("@ModelId", modelId) };
                DbHelper.ExecuteNonQuery(q4, p4);

                MessageBox.Show("Sale saved successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }
}
