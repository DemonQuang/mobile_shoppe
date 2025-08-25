using MobileAppShoppe.DataAccess;
using MobileAppShoppe.Forms.User;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MobileAppShoppe.DataAccess;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace MobileAppShoppe.Forms
{
    public partial class User_Homepage : Form
    {

        public User_Homepage()
        {
            InitializeComponent();
            LoadStock();
            LoadCompany();


        }

        private void User_HomePage_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT CompId, CName FROM tbl_Company";
                DataTable dt = DbHelper.ExecuteQuery(query);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu Company trong DB!");
                    return;
                }

                cboCompany.DataSource = dt;
                cboCompany.DisplayMember = "CName";
                cboCompany.ValueMember = "CompId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Company: " + ex.Message);
            }
        }

        private void LoadCompany()
        {
            string query = "SELECT CompId, CName FROM tbl_Company";
            DataTable dt = DbHelper.ExecuteQuery(query);
            cboCompany.DataSource = dt;
            cboCompany.DisplayMember = "CName";
            cboCompany.ValueMember = "CompId";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtMobile.Text) ||
                cboCompany.SelectedValue == null ||
                cboModel.SelectedValue == null ||
                cboIMEI.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            string warranty = "";
            string query = "SELECT Warranty FROM tbl_Mobile WHERE IMEINO=@IMEI";
            SqlParameter[] prms = { new SqlParameter("@IMEI", cboIMEI.Text) };

            DataTable dt = DbHelper.ExecuteQuery(query, prms);
            if (dt.Rows.Count > 0)
            {
                warranty = dt.Rows[0]["Warranty"].ToString();
            }

            Conform_Details frm = new Conform_Details(
                txtCustomerName.Text,
                txtMobile.Text,
                txtAddress.Text,
                txtEmail.Text,
                cboCompany.Text,
                cboModel.Text,
                cboIMEI.Text,
                txtPrice.Text,
                cboModel.SelectedValue.ToString(), // ModelId
                warranty
            );
            frm.ShowDialog();
        }

        private void cboIMEI_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboModel.SelectedValue == null) return;

            // tránh lỗi DataRowView
            if (cboModel.SelectedValue is DataRowView) return;

            string modelId = cboModel.SelectedValue.ToString();
            string query = "SELECT IMEINO, Price FROM tbl_Mobile WHERE ModelId=@ModelId AND (Status IS NULL OR Status <> 'sold')";
            SqlParameter[] p = { new SqlParameter("@ModelId", modelId) };
            DataTable dt = DbHelper.ExecuteQuery(query, p);

            cboIMEI.DataSource = dt;
            cboIMEI.DisplayMember = "IMEINO";
            cboIMEI.ValueMember = "IMEINO";

            if (dt.Rows.Count > 0)
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
            else
                txtPrice.Clear();

            //MessageBox.Show("Số IMEI load được: " + dt.Rows.Count); // debug
        }

        private void cboModel_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboModel.SelectedValue == null || cboModel.SelectedValue is DataRowView) return;

            string modelId = cboModel.SelectedValue.ToString();
            string query = "SELECT IMEINO, Price FROM tbl_Mobile WHERE ModelId=@ModelId AND (Status IS NULL OR Status <> 'sold')";
            SqlParameter[] p = { new SqlParameter("@ModelId", modelId) };
            DataTable dt = DbHelper.ExecuteQuery(query, p);

            cboIMEI.DataSource = dt;
            cboIMEI.DisplayMember = "IMEINO";
            cboIMEI.ValueMember = "IMEINO";

            if (dt.Rows.Count > 0)
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
            else
                txtPrice.Clear();
        }

        private void cboCompany_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboCompany.SelectedValue == null) return;

            string compId = cboCompany.SelectedValue.ToString();
            string query = "SELECT ModelId, ModelNum FROM tbl_Model WHERE CompId=@CompId";
            SqlParameter[] p = { new SqlParameter("@CompId", compId) };
            DataTable dt = DbHelper.ExecuteQuery(query, p);
            cboModel.DataSource = dt;
            cboModel.DisplayMember = "ModelNum";
            cboModel.ValueMember = "ModelId";
        }

        #region Tab ViewStock

        private void LoadStock()
        {
            //try
            //{
            //    cmbModelView.Items.Clear(); // Model
            //    cmbCompanyView.Items.Clear(); // Company
            //    txtStock.Clear();        // Stock

            //    using (SqlConnection con = new SqlConnection(connectionString))
            //    {
            //        con.Open();
            //        SqlCommand cmd = new SqlCommand("SELECT CName FROM tbl_Company", con);
            //        SqlDataReader dr = cmd.ExecuteReader();
            //        while (dr.Read())
            //        {
            //            cmbCompanyView.Items.Add(dr["CName"].ToString()); // load Company
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error loading stock: " + ex.Message);
            //}
            try
            {
                cmbModelView.DataSource = null;
                cmbCompanyView.DataSource = null;
                txtStock.Clear();

                string query = "SELECT CompId, CName FROM tbl_Company";
                DataTable dt = DbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    cmbCompanyView.DataSource = dt;
                    cmbCompanyView.DisplayMember = "CName";
                    cmbCompanyView.ValueMember = "CompId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock: " + ex.Message);
            }
        }


        private void cmbCompanyView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompanyView.SelectedValue == null) return;

            try
            {
                string query = "SELECT ModelId, ModelNum FROM tbl_Model WHERE CompId=@CompId";
                SqlParameter[] p = { new SqlParameter("@CompId", cmbCompanyView.SelectedValue.ToString()) };

                DataTable dt = DbHelper.ExecuteQuery(query, p);

                cmbModelView.DataSource = dt;
                cmbModelView.DisplayMember = "ModelNum";
                cmbModelView.ValueMember = "ModelId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading models: " + ex.Message);
            }
        }

        private void cmbModelView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbModelView.SelectedValue == null) return;

            try
            {
                string query = @"SELECT COUNT(*) AS StockAvailable
                         FROM tbl_Mobile
                         WHERE ModelId=@ModelId AND Status='Available'";
                SqlParameter[] p = { new SqlParameter("@ModelId", cmbModelView.SelectedValue.ToString()) };

                DataTable dt = DbHelper.ExecuteQuery(query, p);
                if (dt.Rows.Count > 0)
                    txtStock.Text = dt.Rows[0]["StockAvailable"].ToString();
                else
                    txtStock.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock: " + ex.Message);
            }
        }

        // Nút Cancel → reset
        private void button2_Click_1(object sender, EventArgs e)
        {
            cmbModelView.DataSource = null;
            cmbCompanyView.DataSource = null;
            txtStock.Clear();
            LoadStock(); // load lại từ đầu
        }

        #endregion

        #region Tab Search

        private void Search_Load(object sender, EventArgs e)
        {
            dgvSearch.DataSource = null; // Khi load thì DataGridView trống
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            string keyword = txtIMEs.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("⚠ Vui lòng nhập IMEI Number trước khi tìm kiếm!");
                return;
            }

            try
            {
                string query = @"
            SELECT 
                c.Cust_Name   AS [Customer Name],
                c.MobileNumber AS [Mobile Number],
                c.EmailId      AS [Email],
                c.Address      AS [Address],
                co.CName       AS [Company Name],
                m.ModelNum     AS [Model Number],
                mb.IMEINO      AS [IMEI Number],
                s.Price        AS [Price],
                mb.Warranty    AS [Warranty],
                s.PurchaseDate AS [Purchase Date]
            FROM tbl_Sales s
            INNER JOIN tbl_Customer c ON s.CustId = c.CustId
            INNER JOIN tbl_Mobile mb ON s.IMEINO = mb.IMEINO
            INNER JOIN tbl_Model m ON mb.ModelId = m.ModelId
            INNER JOIN tbl_Company co ON m.CompId = co.CompId
            WHERE s.IMEINO = @imei
            ORDER BY s.PurchaseDate DESC";

                SqlParameter[] p = { new SqlParameter("@imei", keyword) };
                DataTable dt = DbHelper.ExecuteQuery(query, p);

                if (dt.Rows.Count > 0)
                {
                    dgvSearch.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("❌ Không tìm thấy thông tin với IMEI này!");
                    dgvSearch.DataSource = null; // clear dữ liệu cũ
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message);
            }
        }



        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();

            // Khi Login đóng, form này cũng đóng
            login.FormClosed += (s, args) => this.Close();

            // Ẩn form hiện tại
            this.Hide();
        }
    }
}
