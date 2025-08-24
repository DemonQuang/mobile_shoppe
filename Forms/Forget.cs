using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Mobile_app_shoppe.DataAccess;


namespace Mobile_app_shoppe.Forms
{
    public partial class Forget : Form
    {
        public Forget()
        {
            InitializeComponent();
        }

        // Sự kiện Submit

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string hint = txtHint.Text.Trim();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT PWD FROM tbl_User WHERE UserName=@u AND Hint=@h";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@h", hint);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lblPassword.Text = "Your password is: " + result.ToString();
                }
                else
                {
                    MessageBox.Show("Sai Username hoặc Hint!");
                }
            }
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
