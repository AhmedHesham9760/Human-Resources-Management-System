using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePartV2.Models;
using PagedList;


namespace EmployeePartV2.Controllers
{
    public class userController : Controller
    {

        HRdbContext db = new HRdbContext();
        // GET: user
        public ActionResult displayUser(int? pageno, string search)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 7 && perm.PermissionID == 1)
                    {
                        int no = pageno == null ? 1 : pageno.Value;
                        var users = db.Users.OrderBy(n => n.UserId);
                        if (!String.IsNullOrEmpty(search))
                        {
                            users = db.Users.Where(n => n.Name.Contains(search)).OrderBy(n => n.UserId);
                        }
                        //ViewBag.search = search;
                        return View(users.ToPagedList(no, 3));
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");

        }

        public ActionResult AddUser()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 7 && perm.PermissionID == 2)
                    {
                        ViewBag.Group = db.Groups.ToList();
                        return View();
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public ActionResult AddUser(User u)
        {
            TempData["msg"]= null;
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("displayUser");
            }
            catch(Exception e)
            {
                TempData["msg"] = "Invalid Email";
                TempData["Name"] = u.Name;
                TempData["UserName"] = u.userName;
                TempData["GroupID"] = u.GroupID;
                //TempData["msg"] = "Invalid Email";
                //TempData["msg"] = "Invalid Email";
                return RedirectToAction("AddUser");
            }
        }

        public ActionResult edit(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 7 && perm.PermissionID == 3)
                    {
                        ViewBag.Group = db.Groups.ToList();
                        User s = db.Users.Find(id);
                        return View(s);
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }
        [HttpPost]
        public ActionResult edit(User s)
        {
            User us = db.Users.Find(s.UserId);
            us.Name = s.Name;
            us.userName = s.userName;
            us.Email = s.Email;
            us.GroupID = s.GroupID;
            db.SaveChanges();
            return RedirectToAction("displayUser");

        }

        public ActionResult delete(int id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 7 && perm.PermissionID == 4)
                    {
                        User s = db.Users.Find(id);
                        db.Users.Remove(s);
                        db.SaveChanges();
                        return RedirectToAction("displayUser");
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");

        }

        public ActionResult search(int? pageno, string search)
        {
            int no = pageno == null ? 1 : pageno.Value;
            var users = db.Users.OrderBy(n => n.UserId);
            if (!String.IsNullOrEmpty(search))
            {
                users = db.Users.Where(n => n.Name.Contains(search)).OrderBy(n => n.UserId);
            }
            return PartialView(users.ToPagedList(no, 3));
        }
    }
}