using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.ActionPlan;
using WebSite.Models;
using WebSite.Models.ViewModels.Attachments;

namespace WebSite.Areas.MFG.Controllers
{
    public class ActionPlanController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var shiftList = ShiftService.List(null, null, BaseGenericRequest);
                shiftList.Insert(0, new ShiftsMaster { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
                var statusList = vw_CatalogService.List4Select("MFG_OperationTaskStatus", BaseGenericRequest, false);
                statusList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
                int DFT_DateType = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ActionPlan_DFT_DateType", "1168").ToInt();
                var dateList = vw_CatalogService.List4Select("MFG_OperationTaskDateTypes", BaseGenericRequest, false);
                dateList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });


                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "MachineID", "MachineName");
                model.ShiftsList = new SelectList(shiftList, "ShiftID", "ShiftDescription");
                model.StatusList = new SelectList(statusList, "CatalogDetailID", "DisplayText");
                model.TypeOfDateList = new SelectList(dateList, "CatalogDetailID", "DisplayText", DFT_DateType);
                model.DateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.CultureID = BaseGenericRequest.CultureID.ToUpper();                
                model.OperationTasksList = OperationTaskService.List(null, null, null, null, null, null, DFT_DateType, DateTime.Now, DateTime.Now, null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }

        public ActionResult Search(string ResponsibleName, int? MachineID, int? ShiftID, int? DateType, DateTime? StartDate, DateTime? EndDate, int? StatusID)
        {
            List<OperationTask> model = new List<OperationTask>();
            string ViewPath = "~/Areas/MFG/Views/ActionPlan/_Tbl_OperationTask.cshtml";

            try
            {
                model = OperationTaskService.List(null, null, null, ResponsibleName, MachineID, ShiftID, DateType, StartDate, EndDate, StatusID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult LoadOperationTaskDetails(int OperationTaskID)
        {
            List<OperationTaskDetails> model = new List<OperationTaskDetails>();
            try
            {
                model = OperationTaskDetailsService.List(null, OperationTaskID, null, null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView("~/Areas/MFG/Views/ActionPlan/_Tbl_OperationTaskDetails.cshtml", model);
        }

        public JsonResult GetModalAssignTask(int OperationTaskID)
        {
            AssignTaskViewModel model = new AssignTaskViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/ActionPlan/_Mo_AssignTask.cshtml";

            try
            {

                var currentTime = DateTime.Now.TimeOfDay.ToString();
                TimeSpan Hour = TimeSpan.FromHours(1);
                TimeSpan ts = TimeSpan.Parse(currentTime).Add(Hour);

                var UsersList = UserService.List(null, null, null, null, null, null, BaseGenericRequest.FacilityID, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID);
                model.UsersList = UsersList;
                model.OperationTaskID = OperationTaskID;
                model.DateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.CurrentDate = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd HH:mm"));
                model.CurrentTime = ts.ToString(@"hh\:mm");
                model.Culture = BaseGenericRequest.CultureID.ToUpper();
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalCloseTask(int OperationTaskID)
        {
            CloseTaskViewModel model = new CloseTaskViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/ActionPlan/_Mo_CloseTask.cshtml";

            try
            {
                var comments = "";
                var OperationTaskEntity = OperationTaskService.List(OperationTaskID, null, null, null, null, null, null, null, null, null, BaseGenericRequest).FirstOrDefault();
                var OperationTaskDetailEntity = OperationTaskDetailsService.List(null, OperationTaskID, null, null, BaseGenericRequest).FirstOrDefault();
                if (OperationTaskDetailEntity != null) comments = OperationTaskDetailEntity.Comments;
                model.OperationTaskID = OperationTaskID;
                model.Date = OperationTaskEntity.TargetDate;
                model.Machine = OperationTaskEntity.MachineName;
                model.Shift = OperationTaskEntity.ShiftID;
                model.Problem = OperationTaskEntity.ProblemDescription == null ? "" : OperationTaskEntity.ProblemDescription;
                model.Comments = comments;
                model.CloseDate = OperationTaskEntity.ClosedDate;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalAttachments(int OperationTaskDetailID)
        {
            AttachmentViewModel model = new AttachmentViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/ActionPlan/_Mo_Attachments.cshtml";

            try
            {
                model.OperationTaskDetailID = OperationTaskDetailID;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetOperationTaskCancelled(int OperationTaskID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = OperationTaskService.Update(OperationTaskID, null, null, null, null, null, null, null, null, null, BaseGenericRequest);
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
                result.ID
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UpdateOperationTaskResponsable(int OperationTaskID, int ResponsableID, string SuggestedAction, string AttendantUserName, DateTime TargetDate, DateTime TargetTime)
        {
            GenericReturn result = new GenericReturn();
            TargetDate = TargetDate - (DateTime.Today.Date - TargetTime);

            try
            {
                result = OperationTaskService.Update(OperationTaskID, null, null, ResponsableID, SuggestedAction, AttendantUserName, TargetDate, null, null, null, BaseGenericRequest);
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
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCloseTask(int OperationTaskID, string Comments, bool Close, DateTime? CloseDate)
        {
            GenericReturn result = new GenericReturn();
            var VarID = 0;
            try
            {

                result = OperationTaskDetailsService.Insert(OperationTaskID, Comments, BaseGenericRequest);
                VarID = result.ID;

                if (Close)
                {
                    result = OperationTaskService.Update(OperationTaskID, null, null, null, null, null, null, CloseDate, null, null, BaseGenericRequest);
                    result.ErrorCode = 0;
                }

                result.ID = VarID;
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
                result.ID
            }, JsonRequestBehavior.AllowGet);

        }
    }
}