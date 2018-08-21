using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTest.Models;
using MVCTest.ViewModels;
using MVCTest.DataAccessLayer;

namespace MVCTest.Controllers
{
    public class EmployeeController : Controller
    {
        private MVCTestContext _context;
        private EmployeeBusinessLayer _employeeBusinessLayer;
        public EmployeeController()
        {
            _context = new MVCTestContext();
            _employeeBusinessLayer= new EmployeeBusinessLayer(_context);
        }
        public ActionResult Index()
        {
            EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel();
            List<Employee> employees = _employeeBusinessLayer.GetEmployees();
            List<EmployeeViewModel> empViewModels = new List<EmployeeViewModel>();
            foreach (Employee emp in employees)
            {
                EmployeeViewModel empViewModel = new EmployeeViewModel();
                empViewModel.EmployeeName = emp.FirstName + " " + emp.LastName;
                empViewModel.Salary = emp.Salary.ToString("C");
                if (emp.Salary > 15000)
                {
                    empViewModel.SalaryColor = "yellow";
                }
                else
                {
                    empViewModel.SalaryColor = "green";
                }
                empViewModels.Add(empViewModel);
            }
            employeeListViewModel.Employees = empViewModels;
            employeeListViewModel.UserName = "Admin";

            return View("Employee", employeeListViewModel);
        }

        public ActionResult AddNew()
        {
            return View("CreateEmployee");
        }
        public ActionResult SaveEmployee(Employee emp, string BtnSubmit)
        {
            switch (BtnSubmit)
            {
                case "Save Employee":
                    bool isSucess = _employeeBusinessLayer.Save(emp);
                    string message = isSucess ? "保存成功" : "保存失败";
                    return Content(message);
                case "Cancel":
                    return RedirectToAction("Index");
            }
            return new EmptyResult();
        }
    }
}