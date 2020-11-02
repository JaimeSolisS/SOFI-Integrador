using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.SG.Models;
using WebSite.Areas.SG.Models.SecurityGuard;
using WebSite.Areas.SG.Models.ViewModels.SecurityGuard;
using WebSite.Filters;
using WebSite.Models;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.SG.Controllers
{
    public class SecurityGuardController : BaseController
    {
        // GET: SG/SecurityGuard
        [SecurityFilter]
        public ActionResult Index()
        {
            var VisitorTypeID = vw_CatalogService.List("PersonCheckIngType", BaseGenericRequest).Where(v => v.ValueID == "Visitor").FirstOrDefault().CatalogDetailID;
            return View(VisitorTypeID);
        }

        public ActionResult GetCheckInStep1()
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_Step_1.cshtml";
            List<Catalog> PersonCheckIngTypeList = new List<Catalog>();
            CheckInViewModel model = new CheckInViewModel();
            try
            {
                model.PersonCheckIngTypeList = vw_CatalogService.List4Select("PersonCheckIngType", BaseGenericRequest, true, Resources.Common.chsn_SelectOption);
                model.CheckListTemplateEntity = CheckListService.GetTemplateInfo("CovidQuestionnaire", BaseGenericRequest);

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetCheckOut()
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckOut.cshtml";
            CheckOutViewModel model = new CheckOutViewModel();

            try
            {
                model.PersonCheckIngTypeList = new SelectList(vw_CatalogService.List4Select("PersonCheckIngType", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");
                model.IdentificationsList = new SelectList(vw_CatalogService.List4Select("SecurityGuardIdentifications", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetCovidQuestionnaire()
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_Questionnaire.cshtml";
            List<CheckListTemplatesDetail> list = new List<CheckListTemplatesDetail>();
            try
            {
                list = CheckListService.TemplatesDetail_List("CovidQuestionnaire", BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, list);
        }

        [HttpPost]
        public JsonResult SaveCheck(SecurityGuardLog SecurityLogEntity, bool IsCheckIn)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                if (IsCheckIn)
                {
                    SecurityLogEntity.CheckIn = DateTime.Now;
                }
                else
                {
                    SecurityLogEntity.CheckOut = DateTime.Now;
                }
                result = SecurityGuardService.Upsert(SecurityLogEntity, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCheckIn(SecurityGuardLog SecurityLogEntity)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                SecurityLogEntity.CheckIn = DateTime.Now;
                result = SecurityGuardService.Upsert(SecurityLogEntity, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCheckInStep2(string AccessCode, int PersonTypeID, string PersonalType, int VendorTypeID)
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_Step_2.cshtml";
            CheckInViewModel model = new CheckInViewModel();
            try
            {
                var SecurityGuardHistory = SecurityGuardService.GetPersonInfoHistory(AccessCode, PersonTypeID, BaseGenericRequest);

                var IdentificationTypeID = 0;
                model.PersonalType = PersonalType;
                if (SecurityGuardHistory != null)
                {
                    if (SecurityGuardHistory.IdentificationImgPath != null)
                    {
                        model.IdenTificationPath = SecurityGuardHistory.IdentificationImgPath;
                    }
                    if (SecurityGuardHistory.VehicleMarkID != null)
                    {
                        model.UseVehicle = true;

                    }
                    model.HaveTools = SecurityGuardHistory.HaveTools;
                    model.WhoVisits = SecurityGuardHistory.WhoVisit;
                    IdentificationTypeID = SecurityGuardHistory.IdentificationTypeID.ToInt();
                }

                model.VendorTypeID = VendorTypeID;
                model.BadgesList = new SelectList(SecurityGuardService.GetAvailableBadges(VendorTypeID, BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "BadgeID", "BadgeNumber");
                model.IdentificationsList = new SelectList(vw_CatalogService.List4Select("SecurityGuardIdentifications", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText", IdentificationTypeID);
                model.EquipmentList = EquipmentService.GetHistoryEquipment(AccessCode, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetCheckInStepVehicle(string AccessCode, int PersonTypeID)
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_Vehicle.cshtml";
            CheckInVehicleViewModel model = new CheckInVehicleViewModel();
            try
            {
                var SecurityGuardHistory = SecurityGuardService.GetPersonInfoHistory(AccessCode, PersonTypeID, BaseGenericRequest);
                var VehicleMarkID = 0;
                if (SecurityGuardHistory != null)
                {
                    VehicleMarkID = SecurityGuardHistory.VehicleMarkID.ToInt();
                    model.VehiclePlates = SecurityGuardHistory.VehiclePlates;
                }

                model.VehicleMarksList = new SelectList(vw_CatalogService.List4Select("VehicleMarks", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText", VehicleMarkID);
            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        public JsonResult GetCheckInStepTools(string AccesCode, int TempAttachmentID, bool AddMoreTools)
        {
            CheckInToolsViewModel model = new CheckInToolsViewModel();
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_Tools.cshtml";
            var ModalID = "mo_CheckInTools";

            try
            {
                var securityGuardToolsList = SecurityGuardToolsService.ListByCheckIn(AccesCode, TempAttachmentID, BaseGenericRequest);

                if (securityGuardToolsList.Count() == 0 || AddMoreTools)
                {
                    ModalID = "mo_CheckInToolsRegister";
                    model.ModalID = ModalID;
                    ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_ToolRegister.cshtml";
                }
                else
                {
                    model.SecurityGuardToolsList = securityGuardToolsList;
                    model.AvailableToolsList = SecurityGuardToolsService.GetAvailableToolsByUser(TempAttachmentID, AccesCode, BaseGenericRequest);
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                ModalID,
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonInfo(string EmployeeCode)
        {
            var UserName = "";
            var UserTypeID = 0;
            var UserTypeName = "";
            var ExpirationDate = "";
            var SessionStateStarted = false;
            var IsExpired = false;
            var VendorTypeID = 0;
            var VendorUserID = 0;
            int? SecurityGuardLogID = 0;

            var PersonInfo = SecurityGuardService.GetPersonInfoByCode(EmployeeCode, "CheckOut", BaseGenericRequest);
            var CurrentDate = MiscellaneousService.Facility_GetDate(VARG_FacilityID);

            if (PersonInfo.Count() > 0)
            {
                UserName = PersonInfo.FirstOrDefault().UserName;
                UserTypeID = PersonInfo.FirstOrDefault().UserTypeID;
                UserTypeName = PersonInfo.FirstOrDefault().UserTypeName;
                SessionStateStarted = PersonInfo.FirstOrDefault().SessionStateStarted;
                VendorTypeID = PersonInfo.FirstOrDefault().VendorTypeID;
                VendorUserID = PersonInfo.FirstOrDefault().VendorUserID;
                SecurityGuardLogID = PersonInfo.FirstOrDefault().SecurityGuardLogID;
                if (PersonInfo.FirstOrDefault().ExpirationDate != null)
                {
                    ExpirationDate = Convert.ToDateTime(PersonInfo.FirstOrDefault().ExpirationDate).ToString("yyyy-MM-dd");
                    if (PersonInfo.FirstOrDefault().ExpirationDate < CurrentDate)
                    {
                        IsExpired = true;
                    }
                }


            }


            return Json(new
            {
                UserName,
                UserTypeID,
                VendorTypeID,
                VendorUserID,
                UserTypeName,
                ExpirationDate,
                SessionStateStarted,
                IsExpired,
                SecurityGuardLogID
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBadgesList(int VendorTypeID)
        {
            var list = SecurityGuardService.GetAvailableBadges(VendorTypeID, BaseGenericRequest, true, Resources.Common.chsn_SelectOption);
            return Json(list.Select(s => new { value = s.BadgeID, text = s.BadgeNumber }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEquipmentInfo(string UPC)
        {
            var list = EquipmentService.List(UPC, BaseGenericRequest);
            return Json(list.Select(s => new { s.EquipmentName, s.EquipmentTypeName, s.EquipmentID }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFullScreenImgView()
        {
            FullScreenImgViewModel model = new FullScreenImgViewModel();
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_Mo_FullScreenImage.cshtml";
            try
            {

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public ActionResult GetCheckinToolsModal(string AccesCode, int TempAttachmentID)
        {
            CheckInToolsViewModel model = new CheckInToolsViewModel();
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_CheckIn_Tools.cshtml";
            try
            {
                model.SecurityGuardToolsList = SecurityGuardToolsService.OldNewToolsList(TempAttachmentID, BaseGenericRequest);

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public JsonResult SaveTools(SecurityGuardTool SecurityTool)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = SecurityGuardToolsService.Insert(SecurityTool, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompaniesList()
        {
            var list = VendorsService.List4Select("0,1", BaseGenericRequest, false, "");
            return Json(list.Select(s => new { value = s.VendorID, text = s.VendorName }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CopyAttachmentTool(int OldSecurityGuardToolID, int NewSecurityGuardToolID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                int ReferenceType;
                var Reference = vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == "TOOLSIMG");
                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\ToolsImagesFiles\\", Server.MapPath(@"\")));
                var pathString = Path.Combine(destinationDirectory.ToString(), NewSecurityGuardToolID.ToString());

                if (Reference != null)
                {
                    ReferenceType = Reference.CatalogDetailID;
                    var AttachmentData = AttachmentService.List(null, "", OldSecurityGuardToolID, ReferenceType, null, null, null, BaseGenericRequest).FirstOrDefault();
                    if (AttachmentData != null)
                    {
                        var NewFilePathName = AttachmentData.FilePathName.Replace("/" + OldSecurityGuardToolID + "/", "/" + NewSecurityGuardToolID + "/");

                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        var oldDirectory = Server.MapPath(AttachmentData.FilePathName);
                        var newDirectory = Server.MapPath(NewFilePathName);
                        System.IO.File.Copy(oldDirectory, newDirectory, true);

                        result = AttachmentService.Insert(NewSecurityGuardToolID, "TOOLSIMG", ReferenceType, null, 0, NewFilePathName, BaseGenericRequest);
                    }
                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCheckOutPersonInfo(string AccessCode)
        {
            var UserName = "";
            int? UserTypeID = 0;
            var UserTypeName = "";
            var FileImgPath = "";
            var SecurityGuardLogID = 0;

            var PersonInfo = SecurityGuardService.GetSecurityGuardCheckOutInfo(AccessCode, BaseGenericRequest);
            var CurrentDate = MiscellaneousService.Facility_GetDate(VARG_FacilityID);
            if (PersonInfo.Count() > 0)
            {
                UserName = PersonInfo.FirstOrDefault().PersonName;
                UserTypeID = PersonInfo.FirstOrDefault().PersonTypeID;
                UserTypeName = PersonInfo.FirstOrDefault().PersonTypeName;
                FileImgPath = PersonInfo.FirstOrDefault().IdentificationImgPath;
                SecurityGuardLogID = Convert.ToInt32(PersonInfo.FirstOrDefault().SecurityGuardLogID);
            }

            return Json(new
            {
                UserName,
                UserTypeID,
                UserTypeName,
                FileImgPath,
                CurrentDate,
                SecurityGuardLogID
            }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public JsonResult SetCheckOutHour(SecurityGuardLog SecurityLogEntity)
        //{
        //    GenericReturn result = new GenericReturn();

        //    try
        //    {
        //        result = SecurityGuardService.SetCheckOut(SecurityLogEntity.SecurityGuardLogID, DateTime.Now, BaseGenericRequest);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = ex.ToString();
        //    }

        //    return Json(new
        //    {
        //        ErrorCode = result.ErrorCode,
        //        ErrorMessage = result.ErrorMessage,
        //        notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
        //    }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult UpdateTempData(int OldSecurityGuardLogID, int NewSecurityGuardLogID, string ToolsToDisable, List<SecurityGuardTool> ToolsToAble)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = SecurityGuardToolsService.UpdateTempData(OldSecurityGuardLogID, NewSecurityGuardLogID, ToolsToDisable, ToolsToAble, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPrintBadgesLabelsModal()
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_PrintBadgesLabels.cshtml";
            List<Badge> list = new List<Badge>();
            try
            {
                list = BadgesService.List(BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, list);
        }

        public ActionResult GetPrintSuppliersLabelsModal()
        {
            SuppliersViewModel model = new SuppliersViewModel();
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_PrintSuppliersLabels.cshtml";

            try
            {
                var VendorID = 0;
                model.VendorsList = new SelectList(VendorsService.List4Select("0,1", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "VendorID", "VendorName");

                if (model.VendorsList.Count() > 0)
                {
                    VendorID = model.VendorsList.FirstOrDefault().Value.ToInt();
                }

                model.VendorUserList = VendorUsersService.ListByVendor(VendorID, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetNewCompanyModal()
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuard/_Mo_NewCompany.cshtml";

            try
            {

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, "");
        }

        [HttpPost]
        public JsonResult UpdateImgPath(int SecurityGuardToolID, string ToolImgPath)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = SecurityGuardToolsService.UpdateImgPath(SecurityGuardToolID, ToolImgPath, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddToolToHistorial(VendorUserTool VendorUserTool)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = VendorUsersToolsService.Insert(VendorUserTool, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LoadEmployeesByCompany(int? VendorID)
        {
            List<VendorUser> VendorsList = new List<VendorUser>();
            string viewPath = "~/Areas/SG/Views/SecurityGuard/_Tbl_VendorUsers.cshtml";
            try
            {
                VendorsList = VendorUsersService.ListByVendor(VendorID, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, VendorsList);
        }

    }
}