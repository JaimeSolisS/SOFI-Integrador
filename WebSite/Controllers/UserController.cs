using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using WebSite.Models.ViewModels.User;
using static WebSite.Models.StaticModels;

namespace WebSite.Controllers
{
    public class UserController : BaseController
    {
        // GET: LoginAs
        public ActionResult Index()
        {
            var model = new LoginViewModel()
            {
                Domains = new SelectList(new List<Catalog>())
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public ActionResult Login(string Expired)
        {
            ViewBag.Tilte = Resources.Common.title_Login;
            ViewBag.UserNameID = Environment.UserName;// "";
            ViewBag.RememberMe = false;
            if (Request.Cookies["SOFI"] != null)
            {
                ViewBag.RememberMe = true;
                ViewBag.UserNameID = Request.Cookies["SOFI"].Value;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserNameID, string Password, string RememberMe, string lastURLvisited)
        {
            bool NeedsPasswordChange =false;
            string DefaultCultureID = string.Empty;
            int UserID=0;
            string Firstname = string.Empty;
            string Lastname = string.Empty;
            int FacilityID=0;
            int CompanyID=0;
            ViewBag.RememberMe = false;
            if (RememberMe != null)
            {
                ViewBag.RememberMe = true;
            }

            //var test2 = Environment.UserDomainName + @"\" + Environment.UserName;
            string CurrentUser = String.Empty;
            CurrentUser = HttpContext.User.Identity.Name;
            if (String.IsNullOrEmpty(CurrentUser))
            { CurrentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name; }

            var result = Core.Service.UserService.Login(UserNameID, Password, VARG_CultureID, out NeedsPasswordChange, out DefaultCultureID, out FacilityID, out CompanyID, out UserID, out Firstname, out Lastname);
            if (result.ErrorCode == 0)
            {
                if (RememberMe != null)
                {
                    HttpCookie cookie = new HttpCookie("SOFI");
                    cookie.Value = UserNameID;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                }
                Session.Add(USER_ID, UserID);
                Session.Add(USER_NAME, UserNameID);
                Session.Add(SESSION_KEY_CULTURE, DefaultCultureID);
                Session.Add(FACILITY_ID, FacilityID);
                Session.Add(FIRST_NAME, Firstname);
                Session.Add(LAST_NAME, Lastname);
                Session.Add(COMPANY_ID, CompanyID);
                //obtener el usuario y revisar si requiere cambio de contrasena
                var user = new User();//Core.Services.UserService.Get(UserID, VARG_FacilityID, VARG_CultureID);
                //if (user.NeedsPasswordChange)
                //{
                //    return RedirectToAction("ChangePassword", "User", new { fromlogin = true });
                //}
                //else if (!String.IsNullOrEmpty(lastURLvisited) && lastURLvisited != "null")
                //{
                //    string[] FullUrl = lastURLvisited.Split('/');
                //    return RedirectToAction("Index", FullUrl[2], new { area = FullUrl[1] });
                //}
                ////revisar si el profile del usuario tiene un default menu 
                //else if (user.NavigateTo != null)
                //{
                //    string[] FullUrl = user.NavigateTo.Split('/');
                //    return RedirectToAction(FullUrl[3], FullUrl[2], new { area = FullUrl[1] });
                //}
                //else
                //{ //no tiene default y no tiene last url
                //    return RedirectToAction("Index", "Home");
                //}

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.UserNameID = UserNameID;
                TempData["NotificationMessage"] = result.ErrorMessage.ToString();
                TempData["NotificationType"] = NotifyType.error.ToString();
                return View();
            }
        }

        //[HttpPost]
        //public ActionResult ForgotPassword(string username)
        //{
        //    //llamar a sp que resetee el password
        //    GenericReturn result = Core.Services.UserService.ForgotPassword(username, VARG_CultureID);
        //    return Json(new
        //    {
        //        ErrorCode = result.ErrorCode,
        //        ErrorMessage = result.ErrorMessage,
        //        Title = "",
        //        notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
        //        ID = result.ID
        //    }, JsonRequestBehavior.AllowGet);

        //}


        [ChildActionOnly]
        public PartialViewResult GetMenu()
        {
            MenuViewModel model = new MenuViewModel();
            try
            {
                model.CurrentUser = UserService.Get(VARG_UserID,BaseGenericRequest);
                model.Menus = UserService.UserMenus(VARG_UserID, VARG_CultureID);
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView("~/Views/Shared/_XNavigation.cshtml", model);
        }

        public ActionResult Logout()
        {
            string Currentculture = CultureInfo.CurrentCulture.ToString();
            string DefaultCulture = Core.Service.MiscellaneousService.Param_GetValue(0, "DefaultCultureId", Currentculture);
            //ChangeLanguage(DefaultCulture);

            Session.Add(SESSION_KEY_CULTURE, null);
            Session.Add(USER_ID, null);
            Session.Add(USER_NAME, null);
            Session.Add(FACILITY_ID, null);
            Session.Abandon();
            Session.Clear();

            return RedirectToAction("Login");
        }

        //public ActionResult ChangePassword(bool? fromlogin = false)
        //{
        //    if (Request.UrlReferrer != null)
        //    {
        //        ViewBag.PreviewPageURL = Request.UrlReferrer.ToString();
        //    }

        //    if (fromlogin.Value == true)
        //    {
        //        ViewBag.PreviewPageURL = "/Home/Index";
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult ChangePassword(string OldPassword, string Password, string ConfirmPassword)
        //{
        //    if (Password != ConfirmPassword)
        //    {
        //        return Json(new
        //        {
        //            ErrorCode = 99,
        //            ErrorMessage = LangResources.User.Msg_NotMatch,
        //            Title = "",
        //            notifyType = NotifyType.error.ToString()
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    //llamar a sp que resetee el password
        //    GenericReturn result = Core.Services.UserService.ChangePassword(VARG_UserID, OldPassword, ConfirmPassword, VARG_CultureID);
        //    return Json(new
        //    {
        //        Error = result.ErrorCode,
        //        result.ErrorMessage,
        //        notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
        //        result.ID
        //    }, JsonRequestBehavior.AllowGet);

        //}


    }
}