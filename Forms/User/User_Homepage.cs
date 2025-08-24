using Mobile_app_shoppe.Forms.User;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mobile_app_shoppe.Forms
{
    public partial class User_Homepage : Form
    {
        private readonly string connectionString =
            @"Data Source=MSI\SQLEXPRESS;Initial Catalog=MobileStoreDB;Integrated Security=True";
          

        private string currentWarranty = ""; // chỉ dùng cho tab Sale

        public User_Homepage()
        {
            InitializeComponent();
            InitializeTabs();
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
        }

        private void InitializeTabs()
        {
            // Tab Sale
            LoadCompanyNames();
            cboCompany.SelectedIndexChanged += cboCompany_SelectedIndexChanged;
            cboModel.SelectedIndexChanged += cboModel_SelectedIndexChanged;
            cboIMEI.SelectedIndexChanged += cboIMEI_SelectedIndexChanged;
            button1.Click += btnSubmitSale_Click;

            // Tab ViewStock
            LoadStock();

            // Tab Search
            btnSearch.Click += btnSearch_Click;
        }

        #region Tab Sale

        private void LoadCompanyNames()
        {
            cboCompany.Items.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT CName FROM tbl_Company", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        cboCompany.Items.Add(dr["CName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading companies: " + ex.Message);
            }
        }

        private void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboModel.Items.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT ModelNum FROM tbl_Model m
                          JOIN tbl_Company c ON m.CompId = c.CompId
                          WHERE c.CName=@CName", con);
                    cmd.Parameters.AddWithValue("@CName", cboCompany.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        cboModel.Items.Add(dr["ModelNum"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading models: " + ex.Message);
            }
        }

        private void cboModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboIMEI.Items.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT IMEINO FROM tbl_Mobile m
                          JOIN tbl_Model md ON m.ModelId = md.ModelId
                          WHERE md.ModelNum=@ModelNum AND m.Status='Available'", con);
                    cmd.Parameters.AddWithValue("@ModelNum", cboModel.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        cboIMEI.Items.Add(dr["IMEINO"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading IMEIs: " + ex.Message);
            }
        }

        private void cboIMEI_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT Price, Warranty FROM tbl_Mobile WHERE IMEINO=@IMEI", con);
                    cmd.Parameters.AddWithValue("@IMEI", cboIMEI.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtPrice.Text = dr["Price"].ToString();
                        currentWarranty = dr["Warranty"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading price/warranty: " + ex.Message);
            }
        }

        private void btnSubmitSale_Click(object sender, EventArgs e)
        {
            Conform_Details confirm = new Conform_Details(
                txtCustomerName.Text,
                txtMobileNo.Text,
                txtAddress.Text,
                txtEmail.Text,
                cboCompany.Text,
                cboModel.Text,
                cboIMEI.Text,
                txtPrice.Text,
                currentWarranty
            );
            confirm.Owner = this;
            var result = confirm.ShowDialog();
            if (result == DialogResult.OK)
            {
                MessageBox.Show("✅ Thông tin đã lưu thành công!");
            }
            else
            {
                this.Show();
            }
        }

        #endregion

        #region Tab ViewStock


        private void LoadStock()
        {
            try
            {
                // Clear các combobox trước
                comboBox1.Items.Clear(); // Model
                comboBox2.Items.Clear(); // Company
                textBox1.Text = "";      // Price

                // Load danh sách công ty vào comboBox2
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT CName FROM tbl_Company", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        comboBox2.Items.Add(dr["CName"].ToString());
                    }
                }

                // Load tất cả stock vào DataGridView
                LoadAllStock();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock: " + ex.Message);
            }
        }

        private void LoadAllStock()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT m.IMEINO, md.ModelNum, c.CName, m.Status, m.Price
                  FROM tbl_Mobile m
                  JOIN tbl_Model md ON m.ModelId = md.ModelId
                  JOIN tbl_Company c ON md.CompId = c.CompId", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock: " + ex.Message);
            }
        }

        // Khi chọn 1 công ty, load các model tương ứng vào comboBox1
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            textBox1.Text = "";
            if (string.IsNullOrEmpty(comboBox2.Text)) return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT ModelNum FROM tbl_Model m
                  JOIN tbl_Company c ON m.CompId = c.CompId
                  WHERE c.CName=@CName", con);
                    cmd.Parameters.AddWithValue("@CName", comboBox2.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        comboBox1.Items.Add(dr["ModelNum"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading models: " + ex.Message);
            }

            // Optional: Load DataGridView theo Company
            FilterStock();
        }

        // Khi chọn 1 model, hiển thị giá tương ứng
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text))
                return;

            //try
            //{
            //    using (SqlConnection con = new SqlConnection(connectionString))
            //    {
            //        con.Open();
            //        SqlCommand cmd = new SqlCommand(
            //            @"SELECT TOP 1 Price, IMEINO 
            //      FROM tbl_Mobile m
            //      JOIN tbl_Model md ON m.ModelId = md.ModelId
            //      JOIN tbl_Company c ON md.CompId = c.CompId
            //      WHERE c.CName=@CName AND md.ModelNum=@ModelNum AND m.Status='Available'", con);

            //        cmd.Parameters.AddWithValue("@CName", comboBox2.Text);
            //        cmd.Parameters.AddWithValue("@ModelNum", comboBox1.Text);

            //        SqlDataReader dr = cmd.ExecuteReader();
            //        if (dr.Read())
            //        {
            //            textBox1.Text = dr["Price"].ToString();
            //            // Nếu muốn hiển thị IMEI đầu tiên của model, có thể lưu ở biến riêng:
            //            string firstIMEI = dr["IMEINO"].ToString();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error loading price: " + ex.Message);
            //}
        
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT Price, Warranty FROM tbl_Mobile WHERE IMEINO=@IMEI", con);
                    cmd.Parameters.AddWithValue("@IMEI", cboIMEI.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox1.Text = dr["Price"].ToString();
                        currentWarranty = dr["Warranty"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading price/warranty: " + ex.Message);
            }


        // Filter DataGridView theo Company/Model
        FilterStock();
        }


        // Hàm filter DataGridView theo Company/Model
        private void FilterStock()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = @"SELECT m.IMEINO, md.ModelNum, c.CName, m.Status, m.Price
                           FROM tbl_Mobile m
                           JOIN tbl_Model md ON m.ModelId = md.ModelId
                           JOIN tbl_Company c ON md.CompId = c.CompId
                           WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(comboBox2.Text))
                        sql += " AND c.CName=@CName";
                    if (!string.IsNullOrEmpty(comboBox1.Text))
                        sql += " AND md.ModelNum=@ModelNum";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    if (!string.IsNullOrEmpty(comboBox2.Text))
                        cmd.Parameters.AddWithValue("@CName", comboBox2.Text);
                    if (!string.IsNullOrEmpty(comboBox1.Text))
                        cmd.Parameters.AddWithValue("@ModelNum", comboBox1.Text);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering stock: " + ex.Message);
            }
        }

        #endregion


        #region Tab Search

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtIMEs.Text.Trim();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT c.CustomerName, c.MobileNumber, c.Email, c.Address
                          FROM tbl_User c
                          JOIN tbl_Mobile m ON m.IMEINO = @kw
                          WHERE m.IMEINO=@kw", con);
                    cmd.Parameters.AddWithValue("@kw", keyword);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message);
            }
        }

        #endregion

        
    }
}
