using MVCTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCTest.Repository
{
    public class EmployeeRepository:Repository<DbContext, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context):base(context)
        {
            
        }
        public string GetNameById(int id)
        {
            string sql = "SELECT FIRSTNAME +' ' + LASTNAME AS NAME FROM T_BASE_EMPLOYEE WHERE EMPLOYEEID=@ID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ID",id)
            };
            var result = _dbContext.Database.SqlQuery<string>(sql, paras);
            return result == null ? string.Empty : result.FirstOrDefault();
        }
    }
}