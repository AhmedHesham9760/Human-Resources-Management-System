using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePartV2.Models;

namespace EmployeePartV2.Controllers
{
    public class LoginController : Controller
    {
        HRdbContext db = new HRdbContext();
        // GET: Login
        public ActionResult LoginIn()
        {
            //check if cookies file is exist or not 
            if (Request.Cookies["User"] != null)
            {
                Session.Add("userid", Request.Cookies["User"].Values["userid"]);
                int groupId = int.Parse(Request.Cookies["User"].Values["groupId"].ToString());
                var screen = db.ModulePermissions.Where(p => p.groupID == groupId).ToList();
                Session.Add("ScreenPermission", screen);
                return RedirectToAction("displayUser", "user");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginIn(User u, bool? remeberme)
        {
            User user = db.Users.FirstOrDefault(n => n.Email == u.userName || n.userName == u.userName && n.Password == u.Password);
            if (user != null)
            {
                ////login
                if (remeberme == true)
                {
                    //add cookies
                    HttpCookie cookie = new HttpCookie("User");
                    cookie.Values.Add("userid", user.UserId.ToString());
                    cookie.Values.Add("userName", user.userName.ToString());
                    cookie.Values.Add("groupId", user.GroupID.ToString());
                    cookie.Expires = DateTime.Now.AddMonths(2);
                    Response.Cookies.Add(cookie);
                }
                Session.Add("userid", user.UserId);
                Session.Add("role_id", user.GroupID);
                LoginViewModel viewModel = new LoginViewModel();
                var screen = db.ModulePermissions.Where(p => p.groupID == user.GroupID).ToList();
                viewModel.screenIds = screen.Select(p => p.ModuleID).ToList();
                viewModel.GroupId = user.GroupID;
                Session.Add("ScreenPermission", screen);
                Session.Add("userid", user.UserId);
                Session.Add("Roleid", user.GroupID);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.status = "Incorrect Email or Password";
                return View();
            }

        }

        public ActionResult logout()
        {
            Session["userid"] = null;
            HttpCookie c = new HttpCookie("User");
            c.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(c);
            return RedirectToAction("loginIn");
        }
        public ActionResult Dashboard()
        {

            return View();
        }
            
    }
}