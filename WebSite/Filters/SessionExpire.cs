using System.Web.Mvc;
namespace WebSite.Filters
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.Contents["UserID"] == null) //(int)filterContext.HttpContext.Session.Contents["UserID"] == 0
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    //filterContext.HttpContext.Response.StatusDescription = LangResources.Common.ntf_ExpiredSession;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    var Url = new UrlHelper(filterContext.RequestContext);
                    //filterContext.Result = new RedirectResult(Url.Action("Login", "User", new { area = "" }));
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Home", new { area = "" }));
                }
                return;
            }
        }
    }
}