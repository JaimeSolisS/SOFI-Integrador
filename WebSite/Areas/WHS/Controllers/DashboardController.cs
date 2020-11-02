using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.WHS.Controllers
{
    public class DashboardController : Controller
    {
        // GET: WHS/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}