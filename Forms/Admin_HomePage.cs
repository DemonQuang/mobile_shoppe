using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mobile_app_shoppe.DataAccess;


namespace Mobile_app_shoppe.Forms
{
    public partial class Admin_Homepage : Form
    {
        public Admin_Homepage()
        {
            InitializeComponent();
            LoadCompanyData();
            LoadCompanyToCombo();
            LoadModelData();
            LoadCompanyForMobile();
            LoadMobileData();
            // Sau khi load xong Company thì mới gắn event SelectedIndexChanged
            cmbCompanyMobile.SelectedIndexChanged += cmbCompanyMobile_SelectedIndexChanged;

            // Nếu có công ty mặc định thì load luôn model
            if (cmbCompanyMobile.SelectedValue != null)
            {
                LoadModelForMobile(cmbCompanyMobile.SelectedValue.ToString());
            }

            //Stock
            LoadCompanyForStock();
            LoadTransactionData();
            cmbCompanyStock.SelectedIndexChanged += cmbCompanyStock_SelectedIndexChanged;

            //Employee
            LoadEmployeeData();
        }

        // Load dữ liệu từ tbl_Company vào DataGridView
        private void LoadCompanyData()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT CompId, CName FROM tbl_Company";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCompany.DataSource = dt;
            }
        }

        // Load combobox công ty khi mở form
        private void LoadCompanyToCombo()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT CompId, CName FROM tbl_Company";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCompany.DataSource = dt;
                cmbCompany.DisplayMember = "CName";
                cmbCompany.ValueMember = "CompId";
            }
        }

        // Load combobox Company cho tab Mobile
        private void LoadCompanyForMobile()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT CompId, CName FROM tbl_Company";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCompanyMobile.DataSource = dt;
                cmbCompanyMobile.DisplayMember = "CName";
                cmbCompanyMobile.ValueMember = "CompId";
            }
        }

        private void LoadModelForMobile(string compId)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ModelId, ModelNum FROM tbl_Model WHERE CompId = @compId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@compId", compId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbModelMobile.DataSource = dt;
                cmbModelMobile.DisplayMember = "ModelNum";
                cmbModelMobile.ValueMember = "ModelId";
            }
        }

        // Khi chọn Company thì load Model tương ứng
        private void cmbCompanyMobile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompanyMobile.SelectedValue != null && cmbCompanyMobile.SelectedValue is string)
            {
                string compId = cmbCompanyMobile.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(compId))
                {
                    LoadModelForMobile(compId);
                }
            }
        }

        // Load danh sách model
        private void LoadModelData()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ModelId, ModelNum, AvailableQty, CompId FROM tbl_Model";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvModel.DataSource = dt;
            }
        }

        // Load dữ liệu Mobile vào DataGridView
        private void LoadMobileData()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT m.IMEINO, 
                   c.CName AS Company, 
                   md.ModelNum AS Model, 
                   m.Price, 
                   m.Warranty, 
                   m.Status
            FROM tbl_Mobile m
            INNER JOIN tbl_Model md ON m.ModelId = md.ModelId
            INNER JOIN tbl_Company c ON md.CompId = c.CompId";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvMobile.DataSource = dt;
            }
        }

        //Update Stock

        //load company và model
        private void LoadCompanyForStock()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT CompId, CName FROM tbl_Company";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCompanyStock.DataSource = dt;
                cmbCompanyStock.DisplayMember = "CName";
                cmbCompanyStock.ValueMember = "CompId";
            }
        }

        private void cmbCompanyStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompanyStock.SelectedValue != null && cmbCompanyStock.SelectedValue is string)
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT ModelId, ModelNum FROM tbl_Model WHERE CompId=@compId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@compId", cmbCompanyStock.SelectedValue.ToString());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbModelStock.DataSource = dt;
                    cmbModelStock.DisplayMember = "ModelNum";
                    cmbModelStock.ValueMember = "ModelId";
                }
            }
        }

        private void LoadTransactionData()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT t.TransId, c.CName AS Company, m.ModelNum AS Model, t.Quantity, t.Amount, t.Date
            FROM tbl_Transaction t
            INNER JOIN tbl_Model m ON t.ModelId = m.ModelId
            INNER JOIN tbl_Company c ON m.CompId = c.CompId";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvTransaction.DataSource = dt;
            }
        }

        //Employee
        //load
        private void LoadEmployeeData()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT UserName, EmployeeName, Address, MobileNumber, Hint FROM tbl_User";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvEmployee.DataSource = dt;
            }
        }


        //button
        private void btnAddC_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Company(CompId, CName) VALUES(@id, @name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtCompanyId.Text.Trim());
                cmd.Parameters.AddWithValue("@name", txtCompanyName.Text.Trim());
                cmd.ExecuteNonQuery();
            }
            LoadCompanyData();
            MessageBox.Show("Thêm công ty thành công!");
        }

        private void btnUpdateC_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE tbl_Company SET CName=@name WHERE CompId=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtCompanyId.Text.Trim());
                cmd.Parameters.AddWithValue("@name", txtCompanyName.Text.Trim());
                cmd.ExecuteNonQuery();
            }
            LoadCompanyData();
            MessageBox.Show("Cập nhật công ty thành công!");
        }

        private void btnDeleteC_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM tbl_Company WHERE CompId=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtCompanyId.Text.Trim());
                cmd.ExecuteNonQuery();
            }
            LoadCompanyData();
            MessageBox.Show("Xóa công ty thành công!");
        }

        private void btnClearC_Click(object sender, EventArgs e)
        {
            txtCompanyId.Clear();
            txtCompanyName.Clear();
        }

        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCompanyId.Text = dgvCompany.Rows[e.RowIndex].Cells["CompId"].Value.ToString();
                txtCompanyName.Text = dgvCompany.Rows[e.RowIndex].Cells["CName"].Value.ToString();
            }
        }

        private void btnInsertModel_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Model(ModelId, CompId, ModelNum, AvailableQty) VALUES(@id, @comp, @num, @qty)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtModelID.Text.Trim());
                cmd.Parameters.AddWithValue("@comp", cmbCompany.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@num", txtModelNum.Text.Trim());
                cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(txtQty.Text.Trim()));
                cmd.ExecuteNonQuery();
            }
            LoadModelData();
            MessageBox.Show("Thêm model thành công!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtModelID.Clear();
            txtModelNum.Clear();
            txtQty.Clear();
        }

        private void btnInsertMobile_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Mobile(IMEINO, ModelId, Status, Warranty, Price) VALUES(@imei, @model, @status, @warranty, @price)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@imei", txtIMEI.Text.Trim());
                cmd.Parameters.AddWithValue("@model", cmbModelMobile.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@status", "Available"); // mặc định
                cmd.Parameters.AddWithValue("@warranty", dtWarranty.Value);
                cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text.Trim()));

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Thêm Mobile thành công!");
            ClearMobileForm();
        }

        private void btnClearMobile_Click(object sender, EventArgs e)
        {
            ClearMobileForm();
        }

        private void ClearMobileForm()
        {
            txtIMEI.Clear();
            txtPrice.Clear();
            dtWarranty.Value = DateTime.Now;
            if (cmbCompanyMobile.Items.Count > 0) cmbCompanyMobile.SelectedIndex = 0;
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // Insert Transaction
                    string query = "INSERT INTO tbl_Transaction(TransId, ModelId, Quantity, Date, Amount) " +
                                   "VALUES(@id, @model, @qty, @date, @amount)";
                    SqlCommand cmd = new SqlCommand(query, conn, trans);
                    cmd.Parameters.AddWithValue("@id", txtTransId.Text.Trim());
                    cmd.Parameters.AddWithValue("@model", cmbModelStock.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(txtQtyStock.Text.Trim()));
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@amount", Convert.ToDecimal(txtAmount.Text.Trim()));
                    cmd.ExecuteNonQuery();

                    // Update AvailableQty trong tbl_Model
                    string updateQty = "UPDATE tbl_Model SET AvailableQty = AvailableQty + @qty WHERE ModelId=@model";
                    SqlCommand cmdUpdate = new SqlCommand(updateQty, conn, trans);
                    cmdUpdate.Parameters.AddWithValue("@qty", Convert.ToInt32(txtQtyStock.Text.Trim()));
                    cmdUpdate.Parameters.AddWithValue("@model", cmbModelStock.SelectedValue.ToString());
                    cmdUpdate.ExecuteNonQuery();

                    trans.Commit();
                    MessageBox.Show("Cập nhật kho thành công!");

                    LoadTransactionData();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                }
            }
        }

        private void btnClearStock_Click(object sender, EventArgs e)
        {
            txtTransId.Clear();
            txtQtyStock.Clear();
            txtAmount.Clear();
            if (cmbCompanyStock.Items.Count > 0) cmbCompanyStock.SelectedIndex = 0;
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() != txtRetype.Text.Trim())
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_User(UserName, PWD, EmployeeName, Address, MobileNumber, Hint) " +
                               "VALUES(@user, @pwd, @emp, @addr, @mobile, @hint)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", txtUserName.Text.Trim());
                cmd.Parameters.AddWithValue("@pwd", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@emp", txtEmpName.Text.Trim());
                cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@hint", txtHint.Text.Trim());

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm nhân viên thành công!");
                    ClearEmployeeForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message);
                }
            }
        }

        private void btnClearEmp_Click(object sender, EventArgs e)
        {
            ClearEmployeeForm();
        }

        private void ClearEmployeeForm()
        {
            txtEmpName.Clear();
            txtAddress.Clear();
            txtMobile.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            txtRetype.Clear();
            txtHint.Clear();
        }

        private void dgvEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployee.Rows[e.RowIndex];
                txtUserName.Text = row.Cells["UserName"].Value.ToString();
                txtEmpName.Text = row.Cells["EmployeeName"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtMobile.Text = row.Cells["MobileNumber"].Value.ToString();
                txtHint.Text = row.Cells["Hint"].Value.ToString();

                // Không load password vì trong DB đang lưu plain text -> để trống
                txtPassword.Clear();
                txtRetype.Clear();
            }
        }

        private void btnUpdateEmp_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE tbl_User SET EmployeeName=@emp, Address=@addr, MobileNumber=@mobile, Hint=@hint WHERE UserName=@user";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", txtUserName.Text.Trim());
                cmd.Parameters.AddWithValue("@emp", txtEmpName.Text.Trim());
                cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@hint", txtHint.Text.Trim());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cập nhật nhân viên thành công!");
                LoadEmployeeData();
            }
        }

        private void btnDeleteEmp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM tbl_User WHERE UserName=@user";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUserName.Text.Trim());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Xóa nhân viên thành công!");
                    LoadEmployeeData();
                    ClearEmployeeForm();
                }
            }
        }

        private void btnSearchByDate_Click(object sender, EventArgs e)
        {
            DateTime d = dtpSaleDate.Value.Date;

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string q = @"
            SELECT  s.SId,
                    c.CName      AS [Company Name],
                    md.ModelNum  AS [Model Name],
                    s.IMEINO,
                    s.Price
            FROM tbl_Sales s
            JOIN tbl_Mobile mb ON s.IMEINO = mb.IMEINO
            JOIN tbl_Model  md ON mb.ModelId = md.ModelId
            JOIN tbl_Company c ON md.CompId  = c.CompId
            WHERE CAST(s.PurchaseDate AS DATE) = @d
            ORDER BY s.SId";

                var da = new SqlDataAdapter(q, conn);
                da.SelectCommand.Parameters.AddWithValue("@d", d);

                var dt = new DataTable();
                da.Fill(dt);
                dgvSaleByDate.DataSource = dt;

                // Tính tổng tiền từ kết quả
                decimal total = 0m;
                foreach (DataRow r in dt.Rows)
                    if (r["Price"] != DBNull.Value) total += Convert.ToDecimal(r["Price"]);

                lblTotalByDate.Text = $"Total Sales Amount of {d:dd/MM/yyyy} = {total:N0}";
            }
        }

        private void btnSearchByRange_Click(object sender, EventArgs e)
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string q = @"
            SELECT  s.SId,
                    c.CName      AS [Company Name],
                    md.ModelNum  AS [Model Name],
                    s.IMEINO,
                    s.Price
            FROM tbl_Sales s
            JOIN tbl_Mobile mb ON s.IMEINO = mb.IMEINO
            JOIN tbl_Model  md ON mb.ModelId = md.ModelId
            JOIN tbl_Company c ON md.CompId  = c.CompId
            WHERE CAST(s.PurchaseDate AS DATE) BETWEEN @from AND @to
            ORDER BY s.PurchaseDate, s.SId";

                var da = new SqlDataAdapter(q, conn);
                da.SelectCommand.Parameters.AddWithValue("@from", from);
                da.SelectCommand.Parameters.AddWithValue("@to", to);

                var dt = new DataTable();
                da.Fill(dt);
                dgvSaleByRange.DataSource = dt;

                decimal total = 0m;
                foreach (DataRow r in dt.Rows)
                    if (r["Price"] != DBNull.Value) total += Convert.ToDecimal(r["Price"]);

                lblTotalByRange.Text = $"Total Sales from {from:dd/MM/yyyy} to {to:dd/MM/yyyy} = {total:N0}";
            }
        }
    }
}
