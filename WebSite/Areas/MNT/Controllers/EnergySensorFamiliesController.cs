using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.MNT.Models.ViewModels.EnergySensors;
using WebSite.Models;

namespace WebSite.Areas.MNT.Controllers
{
    public class EnergySensorFamiliesController : BaseController
    {
        public ActionResult Index()
        {
            EnergySensorFamiliesViewModel model = new EnergySensorFamiliesViewModel();
            try
            {
                model.EnergySensorFamiliesList = MNT_EnergySensorsFamiliesService.List(BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteFamily(int EnergySensorFamilyID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorsFamiliesService.Delete(EnergySensorFamilyID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string FamilyName)
        {
            List<EnergySensorFamilies> model = new List<EnergySensorFamilies>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MNT/Views/EnergySensorFamilies/_Tbl_FamiliesEnergySensors.cshtml";

            try
            {
                model = MNT_EnergySensorsFamiliesService.List(FamilyName, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            //return PartialView(ViewPath, model);

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalAddEditFamily(int? EnergySensorFamilyID)
        {
            GenericReturn result = new GenericReturn();
            EnergySensorFamiliesViewModel model = new EnergySensorFamiliesViewModel();
            string ViewPath = "~/Areas/MNT/Views/EnergySensors/_Mo_AddNewFamily.cshtml";

            try
            {
                if(EnergySensorFamilyID != null)
                {
                    var FamilyObject = MNT_EnergySensorsFamiliesService.List(EnergySensorFamilyID, BaseGenericRequest).FirstOrDefault();
                    model.EnergySensorFamilyID = EnergySensorFamilyID;
                    model.Familyame = FamilyObject.FamilyName;
                    model.MaxValueperHour = FamilyObject.MaxValueperHour;
                    model.IsEdit = true;
                }
                else
                {
                    model.IsEdit = false;
                }

            }catch(Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }


    }
}