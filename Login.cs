using Mobile_app_shoppe.Forms;
using System;
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

            // Đặt PasswordChar
            textBox2.PasswordChar = '*';

            // Gán sự kiện
            btnLogin.Click += BtnLogin_Click;
            linkAdmin.LinkClicked += LinkAdmin_LinkClicked;
            linkForgot.LinkClicked += LinkForgot_LinkClicked;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Ví dụ: có thể kiểm tra username khi gõ
            // Nếu không cần xử lý, để trống
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

        // Sự kiện bấm nút Login
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
                    string query = "SELECT COUNT(*) FROM tbl_User WHERE UserName=@user AND PasswordHash=@pass";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", hashedPass);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        User_Homepage home = new User_Homepage();
                        home.Show();
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

        // Sự kiện bấm vào link Admin
        private void LinkAdmin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginAdmin loginAdmin = new LoginAdmin();
            loginAdmin.Show();
            this.Hide();
        }

        // Sự kiện bấm vào link Forgot Password
        private void LinkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forget forgotForm = new Forget();
            forgotForm.Show();
            this.Hide();
        }
    }
}
