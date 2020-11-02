using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class ErrorPageController : Controller
    {
        public ActionResult Error(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            ViewBag.StatusCode = statusCode;
            ViewBag.exception = "Page not found";
            return View("~/Views/Shared/_Error.cshtml");
        }
        public ActionResult Test(int statusCode, string exception)
        {
            ViewBag.StatusCode = statusCode;
            ViewBag.exception = exception;
            return PartialView("~/Views/Shared/_Error.cshtml");
        }
    }
}