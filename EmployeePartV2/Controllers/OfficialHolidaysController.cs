using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EmployeePartV2.ViewModel;
using System.Data.Entity.Infrastructure;
using System.Web.Routing;
using EmployeePartV2.Models;

namespace EmployeePartV2.Controllers
{
    public class OfficialHolidaysController : Controller
    {
        HRdbContext db = new HRdbContext();

        public ActionResult create()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 8 && perm.PermissionID == 2)
                    {
                        ViewBag.OffHolidays = db.officialHolidays.ToList();
                        return View();
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public ActionResult create(officialHoliday oh)
        {

            db.officialHolidays.Add(oh);
            db.SaveChanges();

            return RedirectToAction("create");
        }

        public ActionResult delete(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 8 && perm.PermissionID == 4)
                    {
                        var off = db.officialHolidays.Find(id);
                        db.officialHolidays.Remove(off);
                        db.SaveChanges();

                        return RedirectToAction("create");
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        public ActionResult update(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 8 && perm.PermissionID == 3)
                    {
                        return View(db.officialHolidays.Find(id));
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");

        }
        [HttpPost]
        public ActionResult update(officialHoliday of)
        {

            var o = db.officialHolidays.Find(of.officialHolidayID);
            o.officialHolidayName = of.officialHolidayName;
            o.startDate = of.startDate;
            o.endDate = of.endDate;
            //o. = of.officialHolidayDate;
            db.SaveChanges();
            return RedirectToAction("create");

        }
    } 
}