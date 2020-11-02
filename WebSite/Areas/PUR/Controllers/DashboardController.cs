using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.PUR.Controllers
{
    public class DashboardController : Controller
    {
        // GET: PUR/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}