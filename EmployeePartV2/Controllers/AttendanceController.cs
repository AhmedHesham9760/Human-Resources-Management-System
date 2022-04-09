using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePartV2.Models;
using System.Data.Entity;
using PagedList;

namespace EmployeePartV2.Controllers
{
    public class AttendanceController : Controller
    {
        HRdbContext db = new HRdbContext();

        public ActionResult Index(int? pageNo, int? numberOfElements)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 5 && perm.PermissionID == 1)
                    {
                        List<object> elements = new List<object>();
                        elements.Add(new { num = 3 });
                        elements.Add(new { num = 6 });
                        elements.Add(new { num = 12 });
                        elements.Add(new { num = 25 });
                        int elementCount = numberOfElements == null ? 3 : numberOfElements.Value;
                        ViewBag.NumberOfElemnts = new SelectList(elements, "num", "num", $"{elementCount}");

                        int no = pageNo == null ? 1 : pageNo.Value;
                        ViewBag.PageNo = no;
                        return View(db.Attendances.OrderBy(n => n.AttendanceID).ToPagedList(no, elementCount));
                    }
                }
            }
                return RedirectToAction("Dashboard", "Login");
            }

        public ActionResult getAttendancesheet(int? pageNo, int? count, string search)
        {
            int no = pageNo == null ? 1 : pageNo.Value;
            int countno = count == null ? 1 : count.Value;
            var records = db.Attendances.OrderBy(n => n.AttendanceID);
            if (!string.IsNullOrEmpty(search))
            {
                records = db.Attendances.Where(n => n.Employee.Name.StartsWith(search)).OrderBy(n => n.AttendanceID);
            }
            return PartialView(records.ToPagedList(no, countno));
        }

        public ActionResult delete(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 5 && perm.PermissionID == 4)
                    {
                        Attendance record = db.Attendances.Find(id);
                        db.Attendances.Remove(record);
                        db.SaveChanges();
                        return RedirectToAction("index");
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        public ActionResult edit(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 5 && perm.PermissionID == 3)
                    {
                        Attendance record = db.Attendances.Where(n => n.AttendanceID == id).SingleOrDefault();
                        return View(record);
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }
        [HttpPost]
        public ActionResult edit(Attendance record)
        {
            ViewBag.invalidValues = null;
            ViewBag.startTime = null;
            ViewBag.empty = null;
            ViewBag.equal = null;
            //Attendance employeeAttendanceRecord = db.Attendances.Where(n=>n.AttendanceID==record.AttendanceID).SingleOrDefault();
            record.Employee.Name = record.Employee.Name;
            record.AttendanceDate = record.AttendanceDate;

            if (record.EndTime < record.StartTime)
            {
                ViewBag.invalidValues = "warning: note leave time couldnt be before start Time";
                return View(record);
            }
            if (record.StartTime == null && record.EndTime != null)
            {
                ViewBag.startTime = "Warning: you couldn't enter leave time without enter the start time";
                return View(record);
            }
            if (record.StartTime == record.EndTime)
            {
                ViewBag.equal = "Note: the start Time and leave time couldn't be equal each other";
                return View(record);
            }
            if (record.StartTime == null && record.EndTime == null)
            {
                ViewBag.empty = "Please Enter start and Leave time";
                return View(record);
            }
            Attendance line = db.Attendances.Where(n => n.AttendanceID == record.AttendanceID).SingleOrDefault();
            line.StartTime = record.StartTime;
            line.EndTime = record.EndTime;
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult RegisterAttendance()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 5 && perm.PermissionID == 2)
                    {
                        return View();
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public JsonResult registeredAttendanceData(string empName, string date)
        {
            DateTime recordDate = Convert.ToDateTime(date);
            Employee emp = db.Employees.Where(n => n.Name == empName).SingleOrDefault();
            if (emp == null) { return Json(new Attendance(), JsonRequestBehavior.AllowGet); }
            Attendance record = db.Attendances.Where(n => n.Employee.Name == empName && n.AttendanceDate == recordDate).SingleOrDefault();
            Attendance sentRecord = new Attendance();
            if (record == null)
            {
                return Json(new Attendance(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                sentRecord.StartTime = record.StartTime;
                sentRecord.EndTime = record.EndTime;
                sentRecord.AttendanceDate = record.AttendanceDate;
                sentRecord.empID = emp.EmpID;
                return Json(sentRecord, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RegisterAttendance(Attendance registeredEmployee)
        {
            ViewBag.InvalidEmployeeName = null;
            ViewBag.InvalidDateBeforeEmployeeContractDate = null;
            ViewBag.InvalidAttendanceDate = null;
            ViewBag.startTimeRequiredWarningMessage = null;
            ViewBag.InvalidAttendanceAndLeaveTime = null;
            ViewBag.equal = null;

            Employee emp = db.Employees.Where(n => n.Name == registeredEmployee.Employee.Name).SingleOrDefault();
            if (emp == null)
            {
                ViewBag.InvalidEmployeeName = "*Invalid Employee Name";
                return View();
            }
            if (registeredEmployee.AttendanceDate < emp.contractStartDate)
            {
                ViewBag.InvalidDateBeforeEmployeeContractDate = "*Invalid Attendance Date";
                return View();
            }
            if (registeredEmployee.EndTime == registeredEmployee.StartTime)
            {
                ViewBag.equal = "*Note start Time couldn't be equal to leave time";
                return View();
            }

            if (registeredEmployee.AttendanceDate > DateTime.Now)
            {
                ViewBag.InvalidAttendanceDate = "*Invalid Attendance Date: Date shouldn't be after today date";
                return View();
            }

            Attendance recordedAttendance = db.Attendances.Where(n => n.Employee.EmpID == emp.EmpID && n.AttendanceDate == registeredEmployee.AttendanceDate).SingleOrDefault();
            if (recordedAttendance == null)
            {
                if (registeredEmployee.StartTime == null && registeredEmployee.EndTime == null)
                {
                    ViewBag.missingData = "Please enter start and end date!";
                    return View();
                }
                if (registeredEmployee.StartTime == null)
                {
                    ViewBag.startTimeRequiredWarningMessage = "*Attendance start Time rquired for registration";
                    return View();
                }
                else if (registeredEmployee.EndTime < registeredEmployee.StartTime)
                {
                    ViewBag.InvalidAttendanceAndLeaveTime = "*Invalid Start and Leave Time, leave Time couldn't be before start Time";
                    return View();
                }
                else
                {
                    registeredEmployee.Employee = emp;
                    db.Attendances.Add(registeredEmployee);
                }
            }
            else
            {
                recordedAttendance.StartTime = registeredEmployee.StartTime == null ? recordedAttendance.StartTime : registeredEmployee.StartTime;
                recordedAttendance.EndTime = registeredEmployee.EndTime == null ? recordedAttendance.EndTime : registeredEmployee.EndTime;
            }
            // start to save employee status 
            bool found = true;
            Statue employeeStatus = db.Statues.Where(n => (n.empID == emp.EmpID) && (n.month.Month == registeredEmployee.AttendanceDate.Month) && (n.month.Year == registeredEmployee.AttendanceDate.Year)).SingleOrDefault();

            if (employeeStatus == null)
            {
                found = false;
                List<Statue> employeeListOfStatus = db.Statues.Where(n => n.empID == emp.EmpID).ToList();
                if (employeeListOfStatus.Count() != 0)
                {
                    DateTime lastEmployeeStatue = employeeListOfStatus.Max(n => n.month);
                    if (lastEmployeeStatue < registeredEmployee.AttendanceDate)
                    {
                        while (lastEmployeeStatue.ToString("MMM-yyyy") != registeredEmployee.AttendanceDate.AddMonths(-1).ToString("MMM-yyyy"))
                        {
                            lastEmployeeStatue = lastEmployeeStatue.AddMonths(1);
                            db.Statues.Add(new Statue() { empID = emp.EmpID, AttendanceDays = 0, BonusDays = 0, ExtraTime = TimeSpan.Zero, LateTime = TimeSpan.Zero, month = new DateTime(lastEmployeeStatue.Year, lastEmployeeStatue.Month, 1), WeekEndHolidaysNumber = RestDays.weekEndCounter(lastEmployeeStatue, emp.Department.GeneralSetting.Days.Select(n => n.DayName).ToList()) });
                        }
                    }
                    else
                    {
                        lastEmployeeStatue = employeeListOfStatus.Min(n => n.month);
                        while (lastEmployeeStatue.ToString("MMM-yyyy") != registeredEmployee.AttendanceDate.AddMonths(1).ToString("MMM-yyyy"))
                        {
                            lastEmployeeStatue = lastEmployeeStatue.AddMonths(-1);
                            db.Statues.Add(new Statue() { empID = emp.EmpID, AttendanceDays = 0, BonusDays = 0, ExtraTime = TimeSpan.Zero, LateTime = TimeSpan.Zero, month = new DateTime(lastEmployeeStatue.Year, lastEmployeeStatue.Month, 1), WeekEndHolidaysNumber = RestDays.weekEndCounter(lastEmployeeStatue, emp.Department.GeneralSetting.Days.Select(n => n.DayName).ToList()) });
                        }
                    }
                }
                employeeStatus = new Statue()
                {
                    empID = emp.EmpID,
                    AttendanceDays = 0,
                    BonusDays = 0,
                    LateTime = TimeSpan.Zero,
                    ExtraTime = TimeSpan.Zero,
                    month = new DateTime(registeredEmployee.AttendanceDate.Year, registeredEmployee.AttendanceDate.Month, 1),
                    WeekEndHolidaysNumber = RestDays.weekEndCounter(new DateTime(registeredEmployee.AttendanceDate.Year, registeredEmployee.AttendanceDate.Month, 1), emp.Department.GeneralSetting.Days.Select(n => n.DayName).ToList())
                };
            }
            bool checkedAsWeeklyHoliday = false;
            bool checkedAsHoliday = false;



            int generalSettingId = emp.Department.GeneralSetting.GSettingID;




            GeneralSetting setting = db.GeneralSettings.Include(n => n.Days).Where(n => n.GSettingID == generalSettingId).SingleOrDefault();
            var days = setting.Days.Where(n => n.DayName == registeredEmployee.AttendanceDate.ToString("dddd")).ToList();
            if (days.Count() != 0)
            {
                employeeStatus.BonusDays += 1;
                checkedAsWeeklyHoliday = true;
                checkedAsHoliday = true;
            }

            if (checkedAsWeeklyHoliday == false)
            {

                var currentOfficialHoliday = db.generalSettingOfficialHolidays.Include(n => n.officialHoliday)
                   .Where(n => ((n.officialHoliday.startDate <= registeredEmployee.AttendanceDate) && (n.officialHoliday.endDate >= registeredEmployee.AttendanceDate)) || ((n.officialHoliday.startDate == registeredEmployee.AttendanceDate) && (n.officialHoliday.endDate == null))).ToList();
                if (currentOfficialHoliday.Count() != 0)
                {
                    employeeStatus.BonusDays += 1;
                    checkedAsHoliday = true;
                }
            }
            if (checkedAsHoliday == false)
            {
                employeeStatus.AttendanceDays += 1;
            }
            if (registeredEmployee.StartTime > emp.StartTime)
            {
                employeeStatus.LateTime += registeredEmployee.StartTime - emp.StartTime;
            }
            else if (registeredEmployee.StartTime < emp.StartTime)
            {
                employeeStatus.ExtraTime += emp.StartTime - registeredEmployee.StartTime;
            }
            if (registeredEmployee.EndTime > emp.EndTime)
            {
                employeeStatus.ExtraTime += registeredEmployee.EndTime - emp.EndTime;
            }
            else if (registeredEmployee.EndTime < emp.EndTime)
            {
                employeeStatus.LateTime += emp.EndTime - registeredEmployee.EndTime;
            }
            //}
            if (found == false)
            {
                db.Statues.Add(employeeStatus);
            }



            db.SaveChanges();
            return RedirectToAction("RegisterAttendance");
        }
    }
}