using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Mobile_app_shoppe.Forms
{
    public partial class LoginAdmin : Form
    {
        public LoginAdmin()
        {
            InitializeComponent();

            // Ẩn mật khẩu
            textBox2.PasswordChar = '*';

            // Gán sự kiện
            btnLogin.Click += BtnLogin_Click;
            lnkBack.LinkClicked += LnkBack_LinkClicked;
            lnkForget.LinkClicked += LnkForget_LinkClicked;
        }

        // Hàm hash mật khẩu SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // Sự kiện Login
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter Username and Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MobileShop;Integrated Security=True"))
                {
                    con.Open();
                    string hashedPass = HashPassword(password);
                    string query = "SELECT COUNT(*) FROM tbl_Admin WHERE UserName=@user AND PasswordHash=@pass";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", hashedPass);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Admin Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Mở form AdminHomePage
                        Admin_Homepage adminHome = new Admin_Homepage();
                        adminHome.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện link Back
        private void LnkBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        // Sự kiện link Forgot Password
        private void LnkForget_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forget forgotForm = new Forget();
            forgotForm.Show();
            this.Hide();
        }
    }
}
