using MVCTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCTest.Repository
{
    public class EmployeeRepository:Repository<DbContext, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context):base(context)
        {

        }
    }
}