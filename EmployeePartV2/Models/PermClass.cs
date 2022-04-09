using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeePartV2.Models
{
    public class PermClass : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["user_id"] == null || HttpContext.
                Current.Session["user_id"].ToString() == "")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        {"Controller","Login" } ,
                        {"action","loginin" }
                    }

                    );
            }
        }
    }

    public class ScreenPermissionFilter : ActionFilterAttribute
    {
        public int ScreenId { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var screen_permission = (LoginViewModel)HttpContext.Current.Session["ScreenPermission"];
                if (!screen_permission.screenIds.Contains(ScreenId))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary
                        {
                            {"controller" ,"login" },
                            {"action" , "loginin"}
                        }
                            );
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                filterContext.Result = new RedirectToRouteResult(
                       new System.Web.Routing.RouteValueDictionary
                       {
                            {"controller" ,"login" },
                            {"action" , "loginin"}
                       }
                           );
            }
        }

    }
}