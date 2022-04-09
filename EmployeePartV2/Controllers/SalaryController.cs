using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePartV2.Models;
using PagedList;

namespace EmployeePartV2.Controllers
{
    public class SalaryController : Controller
    {
        HRdbContext db = new HRdbContext();
        // GET: Salary
        public ActionResult Index(int? pageNo)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 6 && perm.PermissionID == 1)
                    {
                        Company Company = db.Companies.Where(n => n.CompanyID == 1).SingleOrDefault();
                        ViewBag.years = new SelectList(calender.getYears(Company.StartDate), "yearValue", "displayYear", "0"); ;
                        List<Salary> employeesSalary = new List<Salary>();
                        List<Statue> employeeStatue = db.Statues.OrderBy(n => n.month).ToList();
                        foreach (var item in employeeStatue)
                        {
                            employeesSalary.Add(new Salary(item));
                        }
                        int no = pageNo == null ? 1 : pageNo.Value;
                        ViewBag.PageNo = no;
                        return View(employeesSalary.ToPagedList(no, 6));
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");

        }

        public ActionResult salaryMonths(int year, int? months)
        {
            ViewBag.monthsError = "";
            ViewBag.monthsNote = "";


            Company Company = db.Companies.Where(n => n.CompanyID == 1).SingleOrDefault();


            if (year != Company.StartDate.Year || year == 0)
            {
                if (months == null)
                {
                    if (year == DateTime.Today.Year)
                    {
                        months = DateTime.Today.Month;
                    }
                    else if (year == 0)
                    {
                        months = 0;
                    }
                    else
                    {
                        months = new DateTime(year, 1, 1).Month;
                    }
                }
                ViewBag.months = new SelectList(calender.Months, "month", "monthName", $"{months}");
            }
            else
            {
                ViewBag.monthsNote = $"Note: Pioneer company Start on :{Company.StartDate}";
                if (months == null || months < Company.StartDate.Month)
                {
                    months = Company.StartDate.Month;
                }

                ViewBag.months = new SelectList(calender.getMonths(Company.StartDate), "month", "monthName", $"{months}");
            }

            return PartialView();
        }
        public ActionResult SalaryReport(int years, int? months, string search, int? pageCount)
        {
            ViewBag.searchWarning = null;
            int no = pageCount == null ? 1 : pageCount.Value;
            if (years == DateTime.Today.Year && months > DateTime.Today.Month)
            {
                ViewBag.searchWarning = "Invalid Date: there Is no data available in the future!!";
                return PartialView();
            }

            List<Salary> employeesSalary = new List<Salary>();
            //int no = pageNo == null ? 1 : pageNo.Value;
            List<Statue> employeesStatus = db.Statues.OrderBy(n => n.month).ToList();
            if (years != 0) { employeesStatus = employeesStatus.Where(n => n.month.Year == years).ToList(); }
            if (months != 0) { employeesStatus = employeesStatus.Where(n => n.month.Month == months).ToList(); }
            if (employeesStatus.Count() == 0)
            {
                ViewBag.searchWarning = "There is No Data Available at this periode!!";
            }
            if (!string.IsNullOrEmpty(search))
            {
                employeesStatus = employeesStatus.Where(n => n.Employee.Name.StartsWith(search)).ToList();
                if (employeesStatus.Count() == 0)
                {
                    if (db.Employees.Where(n => n.Name.StartsWith(search)).Count() != 0)
                    {
                        ViewBag.searchWarning = "There is no recorded attendance available for this employee in this Period";
                    }
                    else
                    {
                        ViewBag.searchWarning = "Please Enter Valid Employee Name";
                    }
                }
            }
            ViewBag.years = years;
            ViewBag.months = months;
            ViewBag.search = search;
            foreach (var item in employeesStatus)
            {
                employeesSalary.Add(new Salary(item));
            }
            return PartialView(employeesSalary.ToPagedList(no, 6));
        }
        public ActionResult printedInvoic(int? id)
        {
            Statue employeeStatus = db.Statues.Where(n => n.StatusID == id).SingleOrDefault();
            Salary employeeSalary = new Salary(employeeStatus);
            return View(employeeSalary);
        }
    
    }
}