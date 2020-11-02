using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebSite.Filters
{
    public class SecurityFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            //validar si es nullo si lo es expiro la sesion
            if (filterContext.HttpContext.Session.Contents["UserID"] != null && filterContext.HttpContext.Session.Contents["CultureID"] != null)
            {
                int UserID = Int32.Parse(filterContext.HttpContext.Session.Contents["UserID"].ToString());
                string CultureID = filterContext.HttpContext.Session.Contents["CultureID"].ToString();

                //bool ValidUser = Core.Services.UserService.IsValidPageAccess(UserID, filterContext.HttpContext.Request.RawUrl.ToString(), CultureID);
                //if (!ValidUser)
                //{
                //    var controller = (BaseController)filterContext.Controller;
                //    filterContext.Result = controller.InvalidAccess("Login", "User");
                //}
            }
            else
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Login", "User", new { area = "" });
                filterContext.Result = new RedirectResult(url);
            }
            OnActionExecuting(filterContext);
        }

    }
}