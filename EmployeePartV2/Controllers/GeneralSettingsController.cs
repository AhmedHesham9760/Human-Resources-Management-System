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
    public class GeneralSettingsController : Controller
    {
        HRdbContext db = new HRdbContext();

        public ActionResult Create()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 2 && perm.PermissionID == 2)
                    {
                        ViewBag.depts = new SelectList(db.Departments.ToList(), "DeptId", "DeptName");
                        ViewBag.Days = db.Days.ToList();
                        ViewBag.gs = db.GeneralSettings.ToList();

                        return View();
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }


        [HttpPost]
        public ActionResult Create(GeneralSetting G , int[] selecteddays)
        {
            if (db.GeneralSettings.Where(n => n.GSettingID == G.GSettingID).FirstOrDefault() == null && selecteddays != null)
            {
                //    ViewBag.Days = db.Days.ToList();

                var offH = db.officialHolidays.ToList();
                //    ViewBag.depts = new SelectList(db.Departments.ToList(), "DeptId", "DeptName");
                G.Department = db.Departments.Find(G.GSettingID);
                for (int i = 0; i < selecteddays.Length; i++)
                {
                    G.Days.Add(db.Days.Find(selecteddays[i]));
                }

                foreach (var i in offH)
                {
                    G.generalSettingOfficialHolidays.Add(new generalSettingOfficialHoliday { gsID = G.GSettingID, officialHolidayID = i.officialHolidayID , notes = null });
                }
                //G.Department = db.Departments.Where(n => n.DeptId == G.GsID).SingleOrDefault();

                db.GeneralSettings.Add(G);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            else if (selecteddays == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("update", new { id = G.GSettingID });
            }
        }


        public ActionResult delete(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 2 && perm.PermissionID == 4)
                    {
                        var g = db.GeneralSettings.Find(id);
                        db.GeneralSettings.Remove(g);
                        db.SaveChanges();
                        return RedirectToAction("Create");
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
                    if (perm.ModuleID == 2 && perm.PermissionID == 3)
                    {
                        GeneralSetting gs = db.GeneralSettings.Include(s => s.Days).Where(i => i.GSettingID == id).Single();

                        Assigned(gs);
                        var g = db.GeneralSettings.Find(id);

                        return View(g);
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public ActionResult update(int id, GeneralSetting g, string[] selecedDays)
        {

            GeneralSetting gsUpdate = db.GeneralSettings.Include(s => s.Days).Where(i => i.GSettingID == id).Single();
            gsUpdate.DiscountPerHour = g.DiscountPerHour;
            gsUpdate.ExtraPerHour = g.ExtraPerHour;
           
            try
            {
                updateGsDays(selecedDays, gsUpdate);
                db.Entry(gsUpdate).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "unable save try again");
            }
            
            Assigned(gsUpdate);

            return RedirectToAction("Create");

        }
        private void updateGsDays(string[] selecedDays, GeneralSetting gsUpdate)
        {

            if (selecedDays == null)
            {
                gsUpdate.Days = new List<Day>();
                return;
            }

            var GsDaysHs = new HashSet<string>(selecedDays);
            var GsDays = new HashSet<int>(gsUpdate.Days.Select(e => e.DayID));
            foreach (var day in db.Days)
            {
                if (GsDaysHs.Contains(day.DayID.ToString()))
                {
                    if (!GsDays.Contains(day.DayID))
                    {
                        gsUpdate.Days.Add(day);
                    }
                }
                else
                {
                    if (GsDays.Contains(day.DayID))
                    {
                        gsUpdate.Days.Remove(day);
                    }
                }
            }
        }


        private void Assigned(GeneralSetting gs)
        {
            var alldays = db.Days;
            var genDays = new HashSet<int>(gs.Days.Select(S => S.DayID));
            var viewModel = new List<SettingDays>();
            foreach (var days in alldays)
            {
                viewModel.Add(new SettingDays
                {
                    dayId = days.DayID,
                    dayName = days.DayName,
                    Ischecked = genDays.Contains(days.DayID)
                }
                    );
            }
            ViewBag.days = viewModel;
        }
    }
}