using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.OperationRecords;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.MFG.Controllers
{
    public class OperationRecordsController : BaseController
    {
        public ActionResult List(int? MachineID)
        {
            ListViewModel model = new ListViewModel();
            DateTime CurrentTime = DateTime.Now;//.AddHours(19);
            int DFT_Status = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "OperationRecordStatus", "O", out string errormessage);
            try
            {
                model.ShiftList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll), "ShiftID", "ShiftDescription");
                model.DateFormat = string.Format("{0:yyyy-MM-dd}", CurrentTime);                
                model.JulianDay = OperationRecordService.GetJulianDay(CurrentTime, "",BaseGenericRequest);
                int ShiftID = DashboardService.GetCurrentShiftID(1, BaseGenericRequest);
                model.Shift = ShiftService.List(ShiftID, true, BaseGenericRequest).FirstOrDefault().ShiftDescription;
                model.StatusList = new SelectList(vw_CatalogService.List4Select("OperationRecordStatus", BaseGenericRequest,true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText",DFT_Status);
                var EventID = MiscellaneousService.Param_GetValue(VARG_FacilityID, "Event_MFG_OperationRecords_AllowFilters", "0").ToInt();
                var AllowFilters = MiscellaneousService.Event_ValidUser(EventID, VARG_UserID);
                model.AllowFilters = "0";
                if (AllowFilters)
                {
                    model.AllowFilters = "1";
                }
                if (MachineID.SelectedValue() != null)
                {
                    model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll), "MachineID", "MachineName",MachineID);
                    model.Operations = OperationRecordService.List(null, MachineID, null, CurrentTime, CurrentTime, DFT_Status, BaseGenericRequest);
                }
                else
                {
                    model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll), "MachineID", "MachineName");
                    model.Operations = OperationRecordService.List(null, null, null, CurrentTime, CurrentTime, DFT_Status, BaseGenericRequest);
                }

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return View(model);
        }
        public JsonResult Search(int? ddl_F_Machine,int? ddl_F_Shift,DateTime? txt_F_StartDay, DateTime? txt_F_EndDay,int? ddl_F_Status)
        {
            txt_F_StartDay = txt_F_StartDay ?? DateTime.Now;
            txt_F_EndDay = txt_F_EndDay ?? DateTime.Now;
            List<OperationRecord> model = new List<OperationRecord>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/OperationRecords/_Tbl_Operations.cshtml";
            try
            {
                model = OperationRecordService.List(null, ddl_F_Machine, ddl_F_Shift, txt_F_StartDay, txt_F_EndDay, ddl_F_Status, BaseGenericRequest);
                result.ErrorCode = 0;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult New()
        {
            NewViewModel model = new NewViewModel();
            try
            {
                List<Machine> MachinesList = MachineService.List(BaseGenericRequest);
                MachinesList.Add(new Machine { MachineID = 0, MachineName = Resources.MFG.OperationRecords.lbl_SelectMachine });
                model.MachinesList = new SelectList(MachinesList.OrderBy(O => O.MachineID), "MachineID", "MachineName");
                model.ShiftList = new SelectList(ShiftService.List(null, true, BaseGenericRequest), "ShiftID", "ShiftDescription");                
                model.DateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                List<Catalog> MaterialsList = vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false);
                MaterialsList.Add(new Catalog { CatalogDetailID = 0, DisplayText = Resources.MFG.OperationRecords.lbl_SelectMaterial });
                model.MaterialsList = new SelectList(MaterialsList.OrderBy(o => o.CatalogDetailID), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return View(model);
        }
        public JsonResult GetSetupsDDL(int MachineID, int MaterialID)
        {
            GenericReturn result = new GenericReturn();
            IEnumerable<SelectListItem> List = new SelectList(new List<SelectListItem>());
            try
            {
                List = new SelectList(MachineSetupService.List(MachineID, MaterialID, BaseGenericRequest), "MachineSetupID", "MachineSetupName");
                if (List != null)
                {
                    result.ErrorCode = 0;
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
                List,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Create(int? MachineID, int? MachineSetupID,int? MaterialID, string OperatorNumber )
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationRecordService.Insert(MachineID, null, null, MachineSetupID, MaterialID, OperatorNumber, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Edit(int OperationRecordID)
        {
            EditViewModel model = new EditViewModel();
            int DFT_ReferenceTypeID;
            int CurrentHour=0;
            string ErrorMessage = "";
            try
            {
                model.OperatorNumber = BaseGenericRequest.OperatorNumber;
                var oper = OperationRecordService.Get(OperationRecordID, BaseGenericRequest);
                if (oper != null)
                {
                    model.Operation = oper;
                    var Setup = OperationSetupService.Get(OperationRecordID, BaseGenericRequest);
                    model.TabSetup.Setup = Setup;                
                    model.TabSetup.SectionsList = Setup.Parameters.GroupBy(g => new { g.ParameterSectionID,g.ParameterSectionName}).Select(s => new Catalog { CatalogDetailID = s.Key.ParameterSectionID, DisplayText = s.Key.ParameterSectionName }).ToList();
                    model.CavitiesNumber = Setup.MachineSetupCavities;
                    model.Production = OperationProductionService.Get(OperationRecordID, BaseGenericRequest);
    
                    DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out  ErrorMessage);
                    model.DownTimesList = DownTimeService.List(OperationRecordID, DFT_ReferenceTypeID, BaseGenericRequest);
                    model.DepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest), "DepartmentID", "DepartmentName");
                    model.ReasonsList = new SelectList(vw_CatalogService.List4Select("DowntimeReasons", BaseGenericRequest), "CatalogDetailID", "DisplayText");

                    model.RejectsTypesList = new SelectList(vw_CatalogService.List4Select("ProductionGasketDefects", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                    CurrentHour = OperationProductionService.GetHour(model.Production.OperationProductionID,out  ErrorMessage);                
                    //model.HoursList = new SelectList(OperationProductionService.ShiftHours(null,OperationRecordID, BaseGenericRequest), "xdata", "xdata",CurrentHour);
                    model.ProductionProcess = Setup.ProductionProcessName;
                }
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return View(model);
        }
        public ActionResult GetParameterDiv(int OperationSetupParameterID)
        {
            ParameterViewModel model = new ParameterViewModel();
            string ViewPath = "~/Areas/MFG/Views/OperationRecords/_ParameterView.cshtml";
            OperationSetupParameter Parameter = new OperationSetupParameter();
            try
            {               
                Parameter = OperationSetupService.GetParameter(OperationSetupParameterID, BaseGenericRequest);
                var isCalculated = "";
                var OperationParameterNameID = 0;

                if (Parameter.FunctionValue != null)
                {
                    isCalculated = "isCalculated";
                    OperationParameterNameID = Parameter.OperationSetupParameterID;
                }

                model.OperationParameterName = Parameter.OperationParameterName;
                model.Parameter = Parameter;
                model.ControlName = "OperationSetupParameter_" + OperationSetupParameterID;
                model.isCalculated = isCalculated;
                model.OperationSetupParameterID = OperationParameterNameID;

                if (Parameter.ParameterTypeValue == "LIST")
                {
                    var catalog = vw_CatalogService.GetInfo(Parameter.ParameterListID.Value, BaseGenericRequest);
                    model.OptionSelectList = vw_CatalogService.List4Select(catalog.CatalogTag, BaseGenericRequest,false).Select(s => new SelectListItem
                    {
                        Text = s.DisplayText,
                        Value = s.CatalogDetailID.ToString()
                    });
                }
                if (Parameter.ReferenceTypeValue =="LIST")
                {
                    //sacar el tipo de process y filtrar en base a ese
                    var refcatalog = vw_CatalogService.GetInfo(Parameter.ReferenceListID.Value, BaseGenericRequest);
                    var ReferenceList = vw_CatalogService.List4Select(refcatalog.CatalogTag, BaseGenericRequest, false);
                    model.ReferenceSelectList = ReferenceList
                        .Where(w =>w.Param1 == Parameter.ProductionProcessValue)
                        .Select(s => new SelectListItem
                        {
                            Text = s.DisplayText,
                            Value = s.CatalogDetailID.ToString()
                        });
                }
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath,model);
        }
        public JsonResult SaveSetup(int OperationSetupID,List<OperationSetupParameter> ListParameters)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationSetupService.ParametersBulkUpsert(OperationSetupID,ListParameters, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Close(int OperationRecordID,int OperationProductionID, int? LastShotNumber)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationRecordService.Close(OperationRecordID, OperationProductionID, LastShotNumber, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CloseShift(int OperationRecordID, int OperationProductionID, int? LastShotNumber)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationRecordService.CloseShift(OperationRecordID, OperationProductionID, LastShotNumber, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductionDetailsList(int OperationProductionID)
        {
            List<OperationProductionDetail> model = new List<OperationProductionDetail>();
            string ViewPath = "~/Areas/MFG/Views/OperationRecords/_Tbl_ProductionDetails.cshtml";             
            try
            {                
                model = OperationProductionService.Details_List(OperationProductionID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);
        }
        public JsonResult GetProductionDetail(int OperationProductionDetailID)
        {
            GenericReturn result = new GenericReturn();
            OperationProductionDetail entity = new OperationProductionDetail();
            try
            {
                entity = OperationProductionService.Details_Get(OperationProductionDetailID, BaseGenericRequest);
                if (entity != null)
                {
                    result.ErrorCode = 0;
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
                entity,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProduction(int? OperationProductionID, int? OperationRecordID, decimal? CycleTime, int? CavitiesNumber, int? InitialShotNumber)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationProductionService.Upsert(OperationProductionID, OperationRecordID,CycleTime,CavitiesNumber,null,InitialShotNumber,null, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProductionDetail(int? OperationProductionDetailID,int? OperationProductionID, int? Hour, int? ShotNumber)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationProductionService.Details_Upsert(OperationProductionDetailID, OperationProductionID,Hour,ShotNumber,null, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProductionDetailLog(int? OperationProductionID, int? CurrentShotNumber,bool RecalculateProductionHours)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OperationProductionService.DetailsLogs_Insert(OperationProductionID, CurrentShotNumber, RecalculateProductionHours, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductionHour(int OperationProductionID)
        {
            int CurrentHour =0;
            GenericReturn result = new GenericReturn();
            try
            {
                CurrentHour = OperationProductionService.GetHour(OperationProductionID, out string ErrorMessage);
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    result.ErrorCode = 0;
                }
                else {
                    result.ErrorCode = 1;
                    result.ErrorMessage = ErrorMessage;
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
               CurrentHour,
               notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
           }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsCaptured(int OperationProductionID, int Hour)
        {
            bool IsCaptured = false;
            GenericReturn result = new GenericReturn();
            try
            {
                IsCaptured = OperationProductionService.IsCapturedHour(OperationProductionID,Hour, out string ErrorMessage);
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    result.ErrorCode = 0;
                }
                else
                {
                    result.ErrorCode = 1;
                    result.ErrorMessage = ErrorMessage;
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
                IsCaptured,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProduction(int OperationRecordID)
        {
            GenericReturn result = new GenericReturn();
            OperationProduction production = new OperationProduction();
            try
            {
                production = OperationProductionService.Get(OperationRecordID, BaseGenericRequest);
                if (production != null)
                {
                    result.ErrorCode = 0;
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
                production,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductionRejectList(int OperationRecordID, int? Hour)
        {
            List<ProductionReject> model = new List<ProductionReject>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/OperationRecords/_Tbl_ProductionRejects.cshtml";
            int DFT_ReferenceTypeID;
            try
            {
                DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out string ErrorMessage);
                model = ProductionRejectService.List(OperationRecordID, DFT_ReferenceTypeID, Hour, BaseGenericRequest);
                if (model != null)
                {
                    result.ErrorCode = 0;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProductionReject(int? ProductionRejectID, int? ReferenceID, int? RejectTypeID, decimal? Quantity, int? Hour)
        {
            GenericReturn result = new GenericReturn();
            string ErrorMessage = "";
            int DFT_DownTimeTypeID;
            int DFT_ReferenceTypeID;
            try
            {
                DFT_DownTimeTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "DownTimeTypes", "UD", out ErrorMessage);
                DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out ErrorMessage);

                result = ProductionRejectService.Upsert(ProductionRejectID, ReferenceID, DFT_ReferenceTypeID, RejectTypeID, Quantity, Hour, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInitialShot(int OperationProductionID)
        {
            GenericReturn result = new GenericReturn();
            OperationProduction entity = new OperationProduction();
            try
            {
                entity = OperationProductionService.GetbyID(OperationProductionID, BaseGenericRequest);
                if (entity != null)
                {
                    result.ErrorCode = 0;
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
                entity
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DowntimesList(int OperationRecordID)
        {
            List<DownTime> model = new List<DownTime>();
            string ViewPath = "~/Areas/MFG/Views/OperationRecords/_Tbl_Downtimes.cshtml"; 
            int DFT_ReferenceTypeID;
            try
            {
                DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out string ErrorMessage);
                model = DownTimeService.List(OperationRecordID, DFT_ReferenceTypeID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath,model);
        }
        public JsonResult GetDowntime(int DownTimeID)
        {
            GenericReturn result = new GenericReturn();
            DownTime entity = new DownTime();
            try
            {
                entity = DownTimeService.Get(DownTimeID, BaseGenericRequest);
                if (entity != null)
                {
                    result.ErrorCode = 0;
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
                entity,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDowntime(int? DownTimeID ,int? ReferenceID, string StartTime, string Endtime, int? DepartmentID, int? ReasonID,string Comments,bool CloseTime, int? ChangeInserts)
        {
            GenericReturn result = new GenericReturn();
            string ErrorMessage = "";
            int DFT_DownTimeTypeID;
            int DFT_ReferenceTypeID; 
            try
            {
                DFT_DownTimeTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "DownTimeTypes", "UD", out ErrorMessage);
                DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out ErrorMessage); 

                result = DownTimeService.Upsert(DownTimeID, ReferenceID, DFT_ReferenceTypeID, StartTime, Endtime, DepartmentID, ReasonID, Comments, CloseTime, DFT_DownTimeTypeID, null, ChangeInserts, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDowntime(int? DownTimeID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = DownTimeService.Delete(DownTimeID, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RejectList(int OperationRecordID)
        {
            List<ProductionReject> model = new List<ProductionReject>();
            string ViewPath = "~/Areas/MFG/Views/OperationRecords/_Tbl_Rejects.cshtml";
            int DFT_ReferenceTypeID;
            try
            {
                DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out string ErrorMessage);
                model = ProductionRejectService.List(OperationRecordID, DFT_ReferenceTypeID,null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);
        }        
        public JsonResult GetReject(int ProductionRejectID)
        {
            GenericReturn result = new GenericReturn();
            ProductionReject entity = new ProductionReject();
            try
            {
                entity = ProductionRejectService.Get(ProductionRejectID, BaseGenericRequest);
                if (entity != null)
                {
                    result.ErrorCode = 0;
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
                entity,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveReject(int? ProductionRejectID, int? ReferenceID, int? RejectTypeID, decimal? Quantity, int? Hour)
        {
            GenericReturn result = new GenericReturn();
            string ErrorMessage = "";
            int DFT_DownTimeTypeID;
            int DFT_ReferenceTypeID;
            try
            {
                DFT_DownTimeTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "DownTimeTypes", "UD", out ErrorMessage);
                DFT_ReferenceTypeID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemReferenceTypes", "PS", out ErrorMessage);

                result = ProductionRejectService.Upsert(ProductionRejectID, ReferenceID, DFT_ReferenceTypeID, RejectTypeID, Quantity,Hour, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteReject(int? ProductionRejectID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ProductionRejectService.Delete(ProductionRejectID, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }


        public string GetCalculateFunction(int SetupParameterID)
        {
            GenericReturn result = new GenericReturn();
            var functionString = "";
            try
            {
                functionString = OperationSetupParametersService.Get(SetupParameterID, BaseGenericRequest).FunctionValue;
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }
            return functionString;
        }
    }
}