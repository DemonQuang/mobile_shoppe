using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Mobile_app_shoppe.DataAccess
{
    public static class DbHelper
    {
        private static string connectionString =
            ConfigurationManager.ConnectionStrings["MobileStoreDB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
