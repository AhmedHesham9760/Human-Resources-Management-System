using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePartV2.Models;
using PagedList;

namespace EmployeePartV2.Controllers
{
    public class EmployeeController : Controller
    {
        
        HRdbContext db = new HRdbContext();
        // GET: Employee
        public ActionResult Index(int? pageno)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null) {
                foreach (var perm in perms) {
                    if (perm.ModuleID == 1 && perm.PermissionID == 1) {
                        int no = pageno == null ? 1 : pageno.Value;
                        ViewBag.PageNo = no;
                        return View(db.Employees.OrderBy(n => n.EmpID).ToPagedList(no, 3));
                    }
                }
            }
            return RedirectToAction("Dashboard","Login");    
        }

        public ActionResult Create()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 1 && perm.PermissionID == 2)
                    {
                        ViewBag.dept = new SelectList(db.Departments.ToList(), "Deptid", "DeptName");
                        return View();
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public ActionResult Create(Employee e, HttpPostedFileBase SSNFile)
        {
            if (ModelState.IsValid)
            {
                SSNFile.SaveAs(Server.MapPath($"~/attach/{SSNFile.FileName}"));
                e.SSNFile = SSNFile.FileName;

                db.Employees.Add(e);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.dept = new SelectList(db.Departments.ToList(), "Deptid", "DeptName");
                return View();
            }

        }

        public ActionResult Search(int? pageno, string search)
        {

            int no = pageno == null ? 1 : pageno.Value;

            var Employees = db.Employees.OrderBy(n => n.EmpID);
            if (!String.IsNullOrEmpty(search))
                Employees = db.Employees.Where(n => n.Name.Contains(search)).OrderBy(n => n.EmpID);

            return PartialView(Employees.ToPagedList(no, 3));
        }
    }
}