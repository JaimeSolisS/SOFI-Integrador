using Core.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.MFG.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string EmployeNumber)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                //valida que el  EmployeNumber exista asociado a un usuario
                //de lo contrario solo se guardaria el EmployeNumber como una referencia
                //var user = Core.Service.UserService.List(null, null, null, null, EmployeNumber, null, null, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID).FirstOrDefault();
                //if (user != null)
                //{
                //    BaseGenericRequest.UserID = user.UserID;
                //    BaseGenericRequest.OperatorNumber = EmployeNumber;
                //    result.ErrorCode = 0;
                //    result.ErrorMessage = Resources.Common.ntf_ValidUser;
                //}
                //else
                //{
                //    result.ErrorCode = 1;
                //    result.ErrorMessage = Resources.Common.ntf_InValidUser;
                //}
                if (!String.IsNullOrEmpty(EmployeNumber))
                {
                    BaseGenericRequest.OperatorNumber = EmployeNumber;
                    result.ErrorCode = 0;
                    result.ErrorMessage = Resources.Common.ntf_ValidUser;
                }
                else
                {
                    result.ErrorCode = 1;
                    result.ErrorMessage = Resources.Common.ntf_InValidUser;
                }
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);

        }

    }
}