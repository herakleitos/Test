using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public static class DataAccessorFactory
    {
        public static DataAccessor Create(string dbType, string connectionString)
        {
            DataAccessor accessor;
            switch (dbType)
            {
                case "mysql":
                    accessor = new MySqlDataAccessor(connectionString);
                    break;
                case "sqlserver":
                    accessor = new SqlDataAccessor(connectionString);
                    break;
                default:
                    accessor = new SqlDataAccessor(connectionString);
                    break;
            }
            return accessor;
        }
    }
}
