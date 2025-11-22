using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ukkFauzanRizaldy
{
    internal class Connection
    {
        private const string ConnectionString =
            @"Data Source=MYPCPRO\SQLEXPRESS; Initial Catalog=db_ukk_FR; Integrated Security=true; TrustServerCertificate=true";

        public static SqlConnection GetConnection()
        {
            SqlConnection sqlConnect = new SqlConnection(ConnectionString);
            return sqlConnect;
        }
    }
}
