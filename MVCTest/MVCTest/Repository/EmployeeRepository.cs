using MVCTest.DataAccessLayer;
using MVCTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
            string sql = "SELECT ID,FIRSTNAME,LASTNAME FROM T_BASE_EMPLOYEE WHERE EMPLOYEEID=@ID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ID",id)
            };
            var result = _dbContext.Database.SqlQueryForDataTable(sql, paras);
            string json = JsonConvert.SerializeObject(result);
            var dy = JsonConvert.DeserializeObject<dynamic>(json);
            
           

            var dt = JsonConvert.DeserializeObject<DataTable>(json);
            var name = result.Rows[0]["NAME"].ToString();
            return name;
        }
    }
}