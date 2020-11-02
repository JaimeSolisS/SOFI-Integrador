using Core.Entities;
using Core.Service;
using iTextSharp.text.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.HR.Models.ViewModels.OpportunitiesProgram;
using WebSite.Models;

namespace WebSite.Areas.HR.Controllers
{
    public class OpportunitiesProgramController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var UserAccess = OpportunitiesProgramService.GetUserAccessToOProgramOptions(BaseGenericRequest);
                model.AllowFullAccess = UserAccess.FirstOrDefault().AllowFullAccess;
                model.AllowNewVacant = UserAccess.FirstOrDefault().AllowNewVacant;
                model.AllowDiscardAccept = UserAccess.FirstOrDefault().AllowDiscardAccept;
                model.AllowSendNotifications = UserAccess.FirstOrDefault().AllowSendNotifications;
                model.AllowEnableDisable = UserAccess.FirstOrDefault().AllowEnableDisable;
                model.AllowEdit = UserAccess.FirstOrDefault().AllowEdit;

                var OpportunitiesProgramDateTypes = vw_CatalogService.List4Select("OpportunitiesProgramDateTypes", BaseGenericRequest, false);
                var CreationDateID = OpportunitiesProgramDateTypes.Where(d => d.ValueID == "OP_Creation").FirstOrDefault().CatalogDetailID;

                model.DepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest, true, Resources.Common.TagAll), "DepartmentID", "DepartmentName");
                model.DateTypesList = new SelectList(OpportunitiesProgramDateTypes, "CatalogDetailID", "DisplayText", CreationDateID);
                model.OpportunitiesProgramList = OpportunitiesProgramService.List(BaseGenericRequest);
                model.NotificationTypeID = vw_CatalogService.List("KioskNotificationsMessageTypes", BaseGenericRequest).Where(x => x.ValueID == "Information").FirstOrDefault().CatalogDetailID;
                model.ShiftsList = new SelectList(ShiftService.List(BaseGenericRequest), "ShiftID", "ShiftDescription");
                model.GradesList = new SelectList(vw_CatalogService.List("OPGrades", BaseGenericRequest), "CatalogDetailID", "DisplayText");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(model);
        }

        public ActionResult NewEditOpportunity(int? OpportunityProgramID)
        {
            NewEditOpportunityViewModel model = new NewEditOpportunityViewModel();
            try
            {
                model.DescriptionTypesList = vw_CatalogService.List("OPDescriptionTypes", BaseGenericRequest);
                model.CurrentUserName = UserService.Get(VARG_UserID, BaseGenericRequest).FullName;
                if (OpportunityProgramID != null)
                {
                    var OpportunityProgramEntity = OpportunitiesProgramService.List(OpportunityProgramID, null, null, null, null, null, null, null, BaseGenericRequest).FirstOrDefault();
                    model.VacantName = OpportunityProgramEntity.Name;
                    model.DepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest, true, Resources.Common.noneSelected), "DepartmentID", "DepartmentName", OpportunityProgramEntity.DepartmentID);
                    model.FacilitiesList = new SelectList(FacilityService.ListUserAccessByFacility(BaseGenericRequest, true), "FacilityID", "FacilityName", OpportunityProgramEntity.FacilityID);
                    model.ExpirationDate = OpportunityProgramEntity.ExpirationDateFormatted;
                    model.Btn_Save_ID = "btn_SaveEditedOpportunity";
                    model.NotificationTypeID = vw_CatalogService.List("KioskNotificationsMessageTypes", BaseGenericRequest).Where(x => x.ValueID == "Information").FirstOrDefault().CatalogDetailID;
                    model.ResponsibleList = OpportunitiesProgramResponsibleService.List(OpportunityProgramID, BaseGenericRequest);
                    model.OpportunityProgramID = OpportunityProgramID;
                    model.OpportunityMediaList = OpportunitiesProgramService.MediaList(Convert.ToInt32(OpportunityProgramID), BaseGenericRequest);
                    model.DescriptionTypeID = OpportunityProgramEntity.DescriptionTypeID;
                    model.Title = Resources.HR.Kiosk.lbl_EditOpportunity + " : " + OpportunityProgramEntity.OpportunityNumber;
                    model.Comments = OpportunityProgramEntity.Description;
                    model.GradesList = new SelectList(vw_CatalogService.List4Select("OPGrades", BaseGenericRequest), "CatalogDetailID", "DisplayText", OpportunityProgramEntity.GradeID);
                    model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "ShiftID", "ShiftDescription", OpportunityProgramEntity.ShiftID);

                }
                else
                {
                    var ExpirationDays = Convert.ToInt32(MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_OpportunitiesProgram_VacantExpirationDate", "10"));
                    model.DepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest, true, Resources.Common.noneSelected), "DepartmentID", "DepartmentName");
                    model.FacilitiesList = new SelectList(FacilityService.ListUserAccessByFacility(BaseGenericRequest, true), "FacilityID", "FacilityName", VARG_FacilityID);
                    model.GradesList = new SelectList(vw_CatalogService.List4Select("OPGrades", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");
                    model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.chsn_SelectOption, true), "ShiftID", "ShiftDescription");
                    model.Btn_Save_ID = "btn_SaveNewOpportunity";
                    model.ExpirationDate = DateTime.Now.AddDays(ExpirationDays).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(model);
        }

        public ActionResult Search(int DepartmentID, string ShiftIDs, string Grades, string OpportunityNumber, int? DateTypeID, DateTime? StartDate, DateTime? EndDate)
        {
            string ViewPath = "~/Areas/HR/Views/OpportunitiesProgram/_Tbl_OpportunitiesProgram.cshtml";
            IndexViewModel model = new IndexViewModel();

            try
            {
                var UserAccess = OpportunitiesProgramService.GetUserAccessToOProgramOptions(BaseGenericRequest);
                model.AllowFullAccess = UserAccess.FirstOrDefault().AllowFullAccess;
                model.AllowNewVacant = UserAccess.FirstOrDefault().AllowNewVacant;
                model.AllowDiscardAccept = UserAccess.FirstOrDefault().AllowDiscardAccept;
                model.AllowSendNotifications = UserAccess.FirstOrDefault().AllowSendNotifications;
                model.AllowEnableDisable = UserAccess.FirstOrDefault().AllowEnableDisable;
                model.AllowEdit = UserAccess.FirstOrDefault().AllowEdit;
                model.OpportunitiesProgramList = OpportunitiesProgramService.List(null, OpportunityNumber, DateTypeID, DepartmentID, ShiftIDs, Grades, StartDate, EndDate, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetCandidatesTable(int? OpportinutyProgramID)
        {
            IndexViewModel model = new IndexViewModel();
            var UserAccess = OpportunitiesProgramService.GetUserAccessToOProgramOptions(BaseGenericRequest);
            string ViewPath = "~/Areas/HR/Views/OpportunitiesProgram/_Tbl_OpportunitiesProgramCandidates.cshtml";
            try
            {
                model.Candidate = OpportunitiesProgramCandidatesService.List(OpportinutyProgramID, BaseGenericRequest);
                model.AllowFullAccess = UserAccess.FirstOrDefault().AllowFullAccess;
                model.AllowNewVacant = UserAccess.FirstOrDefault().AllowNewVacant;
                model.AllowDiscardAccept = UserAccess.FirstOrDefault().AllowDiscardAccept;
                model.AllowSendNotifications = UserAccess.FirstOrDefault().AllowSendNotifications;
                model.AllowEnableDisable = UserAccess.FirstOrDefault().AllowEnableDisable;
                model.AllowEdit = UserAccess.FirstOrDefault().AllowEdit;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public JsonResult SaveNewOpportunity(string Name, string Description, List<User> Responsibles, int? DescriptionTypeID, int? DepartmentID, int? ShiftID, int? GradeID, int? DdlFacilityID, DateTime? ExpirationDate)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                string DescriptionEncoded = System.Net.WebUtility.HtmlEncode(Description);
                result = OpportunitiesProgramService.Insert(Name, Description, Responsibles, DescriptionTypeID, DepartmentID, ShiftID, GradeID, DdlFacilityID, ExpirationDate, true, VARG_UserID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(int? OpportunityProgramID, string OpportunityNumber, string Name, string Description, List<User> Responsibles, int? DescriptionTypeID, int? DepartmentID, int? ShiftID, int? GradeID, int? DdlFacilityID, DateTime? ExpirationDate, bool? Enabled, int NotificationTypeID, string FilesToDelete)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OpportunitiesProgramService.Update(OpportunityProgramID, OpportunityNumber, Name, Description, Responsibles, DescriptionTypeID, DepartmentID, ShiftID, GradeID, DdlFacilityID, ExpirationDate, Enabled, NotificationTypeID, FilesToDelete, null, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetEnableDisable(int? OpportunityProgramID, bool Enabled, int NotificationTypeID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                string StatusName;
                if (Enabled)
                    StatusName = Resources.HR.Kiosk.lbl_Enabled;
                else
                    StatusName = Resources.HR.Kiosk.lbl_Disabled;


                result = OpportunitiesProgramService.SetEnableDisable(OpportunityProgramID, Enabled, NotificationTypeID, StatusName, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendNotifications(int OpportunityProgramID, string Comment, string CandidateID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OpportunitiesProgramService.SendNotifications(OpportunityProgramID, Comment, CandidateID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetNotificationModal(string MassiveOrSingleCandidate)
        {
            NotificationsViewModel model = new NotificationsViewModel();
            string ViewPath = "~/Areas/HR/Views/OpportunitiesProgram/_Mo_Notification.cshtml";
            try
            {
                model.NotificationTypeID = vw_CatalogService.List("KioskNotificationsMessageTypes", BaseGenericRequest).Where(x => x.ValueID == "Information").FirstOrDefault().CatalogDetailID;
                model.ModalTitle = Resources.HR.Kiosk.title_SendNotify;
                model.Btn_SaveChanges = Resources.HR.Kiosk.title_SendNotify;
                model.IsAble = false;
                if (MassiveOrSingleCandidate == "Single")
                {
                    model.Btn_SendNotificationID = "btn_Mo_OpportunityProgram_SendSingleNotification";
                    model.ModalAlertText = Resources.HR.Kiosk.msg_AlertSendSingleNotificationInstructions;
                }
                else
                {
                    model.Btn_SendNotificationID = "btn_Mo_OpportunityProgram_SendMassiveNotification";
                    model.ModalAlertText = Resources.HR.Kiosk.msg_AlertSendNotificationInstructions;
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetDiscardCandidateModal(int? CandidateID, bool IsDiscarted)
        {
            NotificationsViewModel model = new NotificationsViewModel();
            string ViewPath = "~/Areas/HR/Views/OpportunitiesProgram/_Mo_Notification.cshtml";
            try
            {
                model.NotificationTypeID = vw_CatalogService.List("KioskNotificationsMessageTypes", BaseGenericRequest).Where(x => x.ValueID == "Information").FirstOrDefault().CatalogDetailID;
                model.Btn_SendNotificationID = "btn_Mo_OpportunityProgram_AcceptDiscardCandidate";
                model.IsAble = !(IsDiscarted);
                if (IsDiscarted)
                {
                    model.ModalTitle = Resources.Common.btn_Accept;
                    model.Btn_SaveChanges = Resources.Common.btn_Accept;
                    model.ModalAlertText = Resources.HR.Kiosk.msg_AlertCandidateToAcceptModal;
                }
                else
                {
                    model.ModalTitle = Resources.HR.Kiosk.lbl_Discard;
                    model.Btn_SaveChanges = Resources.HR.Kiosk.lbl_Discard;
                    model.ModalAlertText = Resources.HR.Kiosk.msg_AlertCandidateToDiscardModal;
                }

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public JsonResult UpdateCandidates(int? OPCandidateID, int? OpportunityProgramID, string CandidateID, string ShortMessage, bool? IsDiscarted)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OpportunitiesProgramCandidatesService.Update(OPCandidateID, OpportunityProgramID, CandidateID, ShortMessage, IsDiscarted, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult OpportunitiesExportToExcel(string txt_NumVacant, int ddl_DateTypes, string[] ddl_Departments,
            DateTime? txt_OpportunitiesProgramStartDate, DateTime? txt_OpportunitiesProgramEndDate, string[] ddl_Shifts, string[] ddl_Grades)
        {
            DataSet ds = new DataSet();
            ds = OpportunitiesProgramService.ListFilteredDataset(txt_NumVacant, ddl_DateTypes, ddl_Departments,
            txt_OpportunitiesProgramStartDate, txt_OpportunitiesProgramEndDate, ddl_Shifts, ddl_Grades, BaseGenericRequest);
            return this.ExcelExport(ds, Resources.HR.Kiosk.title_OpportunitiesProgramExcelReport);
        }

        public JsonResult GetShiftList(int Facility)
        {
            var list = ShiftService.List4Select(new GenericRequest { FacilityID = Facility, CultureID = VARG_CultureID, UserID = VARG_UserID }, Resources.Common.chsn_SelectOption, true);
            return Json(list.Select(s => new { value = s.ShiftID, text = s.ShiftDescription }), JsonRequestBehavior.AllowGet);
        }
    }
}