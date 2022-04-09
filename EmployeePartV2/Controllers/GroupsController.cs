using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeePartV2.Models;

namespace EmployeePartV2.Controllers
{
    public class GroupsController : Controller
    {
        private HRdbContext db = new HRdbContext();


        public ActionResult Index()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 3 && perm.PermissionID == 1)
                    {
                        return View(db.Groups.ToList());
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }





        public ActionResult Create()
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 3 && perm.PermissionID == 2)
                    {
                        viewmodel vm = new viewmodel();
                        vm.modules = db.Modules.ToList();
                        vm.Permissions = db.Permissions.ToList();
                        ViewBag.modules = db.Modules.ToList();
                        ViewBag.permissions = db.Permissions.ToList();
                        return View(vm);
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public ActionResult Create(viewmodel vm, List<string> perms)
        {

            if (ModelState.IsValid)
            {
                Group group = new Group();
                group.Name = vm.group.Name;
                db.Groups.Add(group);
                db.SaveChanges();
                group = db.Groups.ToList().LastOrDefault();
                List<int> modules = new List<int>();
                modules = perms?.Select(mod => int.Parse(mod.Split(',')[1])).Distinct().ToList();
                for (int i = 0; i < perms?.Count; i++)
                {
                    ModulePermission modulePermission = new ModulePermission();
                    modulePermission.ModuleID = int.Parse(perms[i].Split(',')[1]);
                    modulePermission.groupID = group.GroupID;
                    modulePermission.PermissionID = int.Parse(perms[i].Split(',')[0]);
                    group.ModulePermissions.Add(modulePermission);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public ActionResult Edit(int? id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 3 && perm.PermissionID == 3)
                    {

                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Group group = db.Groups.Find(id);
                        if (group == null)
                        {
                            return HttpNotFound();
                        }
                        ViewBag.modules = db.Modules.ToList();
                        ViewBag.permissions = db.Permissions.ToList();
                        foreach (var per in group.ModulePermissions)
                        {
                            per.isChecked = true;
                        }
                        return View(group);
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }

        [HttpPost]
        public ActionResult Edit(Group group, List<string> isChecked)
        {

            if (ModelState.IsValid)
            {
                db.ModulePermissions.RemoveRange(db.ModulePermissions.Where(m => m.groupID == group.GroupID));
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                foreach (string perm in isChecked)
                {
                    if (perm != "false")
                    {
                        group.ModulePermissions.Add(new ModulePermission { groupID = group.GroupID, ModuleID = int.Parse(perm.Split(',')[1]), PermissionID = int.Parse(perm.Split(',')[0]) });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        public ActionResult Delete(int? id)
        {
            List<ModulePermission> perms = (List<ModulePermission>)Session["ScreenPermission"];
            if (perms != null)
            {
                foreach (var perm in perms)
                {
                    if (perm.ModuleID == 3 && perm.PermissionID == 4)
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Group groupe = db.Groups.Find(id);
                        if (groupe == null)
                        {
                            return HttpNotFound();
                        }
                        db.Groups.Remove(groupe);
                        db.SaveChanges();
                        return RedirectToAction("index");
                    }
                }
            }
            return RedirectToAction("Dashboard", "Login");
        }


        public ActionResult groupSearch(string groupName)
        {
            List<Group> groups = new List<Group>();
            if (groupName != null)
                groups = db.Groups.Where(n => n.Name.Contains(groupName)).ToList();
            else
                groups = db.Groups.ToList();
            return PartialView(groups);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}