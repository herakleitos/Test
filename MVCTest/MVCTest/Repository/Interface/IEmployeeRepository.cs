using MVCTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCTest.Repository
{
    public interface IEmployeeRepository:IRepository<DbContext, Employee>
    {

    }
}