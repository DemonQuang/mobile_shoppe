using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mobile_app_shoppe.DataAccess;

namespace Mobile_app_shoppe
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DbHelper.TestConnection())
            {
                MessageBox.Show("✅ Kết nối SQL Server thành công!");
            }
            else
            {
                MessageBox.Show("❌ Kết nối thất bại!");
            }
            Application.Run(new Login());
        }
    }
}
