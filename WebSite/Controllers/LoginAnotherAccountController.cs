using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using Core.Entities;
using Core.Service;
using static WebSite.Models.StaticModels;

namespace WebSite.Controllers
{
    public class LoginAnotherAccountController : BaseController
    {
        // GET: LoginAnotherAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginWithActiveDirectory(string Domain, string User, string Password)
        {
            bool isValid;
            int UserID = 0;
            string ErrorMessage;
            int ErrorCode = 0;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, Domain))
            {
                isValid = pc.ValidateCredentials(User, Password);
            }
            if (isValid)
            {
                UserID = UserService.GetUserID(User);
                ErrorMessage = Resources.Request.lbl_ValidatedUserSuccess;
            }
            else
            {
                ErrorMessage = Resources.Request.lbl_ValidatedUserWrong;
                ErrorCode = 99;
            }

            return Json(new
            {
                ErrorMessage,
                isValid,
                UserID,
                notifyType = ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModalLoginOptions()
        {
            string ViewPath = "~/Views/Shared/_mo_LoginAnotherAccount.cshtml";
            IEnumerable<SelectListItem> model = new SelectList(new List<Catalog>());
            try
            {
                model = new SelectList(vw_CatalogService.List4Select("SOFIDomains", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

    }
}