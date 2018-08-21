using MVCTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCTest.DataAccessLayer;
using MVCTest.Repository;
namespace MVCTest.Controllers
{
    public class EmployeeBusinessLayer
    {
        private MVCTestContext _context;
        private EmployeeRepository _respository;
        public EmployeeBusinessLayer(MVCTestContext context)
        {
            _context = context;
            _respository = new EmployeeRepository(_context);
        }
        public List<Employee> GetEmployees()
        {
            return _respository.FindAll().ToList();
        }
        public bool Save(Employee emp)
        {
            return _respository.Insert(emp)>0;
        }
    }
}