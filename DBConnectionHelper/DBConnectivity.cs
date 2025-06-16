using Microsoft.Data.SqlClient;
using System.Configuration;

namespace DBConnectionHelper
{
    public class DBConnectivity
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FlyWithMeDB"].ConnectionString;
            return new SqlConnection(connectionString);
        }

    }
}
