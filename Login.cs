using Mobile_app_shoppe.Forms;
using System;
using Mobile_app_shoppe.DataAccess;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Mobile_app_shoppe
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM tbl_User WHERE UserName=@u AND PWD=@p";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    User_Homepage home = new User_Homepage();
                    home.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
        }

        private void btnForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forget forget = new Forget();
            forget.Show();
            this.Hide();
        }

        private void btnAdmin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginAdmin adminLogin = new LoginAdmin();
            adminLogin.Show();
            this.Hide();
        }
    }
}
