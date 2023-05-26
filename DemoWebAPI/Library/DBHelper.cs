using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DemoWebAPI.Library
{
    public static class DBHelper
    {
        public static SqlConnectionStringBuilder GetConnectionStringBuilder(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;

            var builder = new SqlConnectionStringBuilder(connectionString);

            return builder;
        }
    }
}