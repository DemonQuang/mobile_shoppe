using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mobile_app_shoppe.Forms
{
    public partial class Forget : Form
    {
        public Forget()
        {
            InitializeComponent();

            // Gán sự kiện
            btnSubmit.Click += BtnSubmit_Click;
            lnkLogin.LinkClicked += LnkLogin_LinkClicked;
        }

        // Sự kiện Submit
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string hint = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(hint))
            {
                MessageBox.Show("Please enter Username and Hint.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MobileShop;Integrated Security=True"))
                {
                    con.Open();
                    string query = "SELECT Password FROM tbl_User WHERE UserName=@user AND Hint=@hint";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@hint", hint);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show($"Your password is: {result.ToString()}", "Password Retrieved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Hint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Link quay lại Login Page
        private void LnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
}
