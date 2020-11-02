using Core.Entities;
using Core.Service;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.Reports;

namespace WebSite.Areas.MFG.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                model.ReportsList = vw_CatalogService.List4Select("MFG_Reports", BaseGenericRequest, true, Resources.Common.chsn_SelectOption);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return View(model);
        }

        //acciones para traer los parametros de un reporte como partials, Basados en el VALUEID del Catalogo
        public ActionResult GetParams_EERDTLS()
        {
            EERDTLSParamsViewModel model = new EERDTLSParamsViewModel();
            string ViewPath = "/Areas/MFG/Views/Reports/_Div_Report_EERDTLSParams.cshtml";
            try
            {
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "MachineID", "MachineName");
                var shiftList = ShiftService.List(null, null, BaseGenericRequest);
                //shiftList.Insert(0, new ShiftsMaster { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
                model.ShiftsList = new SelectList(shiftList, "ShiftID", "ShiftDescription");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);
        }
        public ActionResult GetParams_PRMS()
        {
            PRMSParamsViewModel model = new PRMSParamsViewModel();
            string ViewPath = "/Areas/MFG/Views/Reports/_Div_Report_PRMSParams.cshtml";
            try
            {
                var paramslist = MachineParametersService.List(null, BaseGenericRequest);
                //paramslist.Add(new MachineParameters { MachineParameterID = 0, ParameterName = Resources.Common.TagAll });
                model.ParametersList = new SelectList(paramslist, "MachineParameterID", "ParameterName", 0);
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "MachineID", "MachineName", 0);
                model.ProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                var shiftList = ShiftService.List(null, null, BaseGenericRequest);
                //shiftList.Insert(0, new ShiftsMaster { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
                model.ShiftsList = new SelectList(shiftList, "ShiftID", "ShiftDescription");
                model.MaterialsList = new SelectList(vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                model.YesNoList = new SelectList(vw_CatalogService.List4Select("YESNO", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetParams_ACT()
        {
            string ViewPath = "/Areas/MFG/Views/Reports/_Div_Report_ACTParams.cshtml";
            ACTParamsViewModel model = new ACTParamsViewModel();
            try
            {
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "MachineID", "MachineName", 0);
                var setupslist = MachineSetupService.List(null, null, BaseGenericRequest);
                //setupslist.Insert(0, new MachineSetup() { MachineSetupID = 0, MachineSetupName = Resources.Common.TagAll });
                model.SetupsList = new SelectList(setupslist, "MachineSetupID", "MachineSetupName");
                model.MaterialsList = new SelectList(vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                model.ProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                var shiftList = ShiftService.List(null, null, BaseGenericRequest);
                //shiftList.Insert(0, new ShiftsMaster { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
                model.ShiftsList = new SelectList(shiftList, "ShiftID", "ShiftDescription");
                model.StatusList = new SelectList(vw_CatalogService.List4Select("MFG_OperationTaskStatus", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                var UsersList = UserService.List(null, null, null, null, null, null, BaseGenericRequest.FacilityID, VARG_FacilityID, VARG_UserID, VARG_CultureID);
                //UsersList.Add(new User { ID = 0, FirstName= Resources.Common.TagAll, LastName=""});
                model.ResponsiblesList = new SelectList(UsersList, "ID", "FullName", 0);
                model.TypeOfDateList = new SelectList(vw_CatalogService.List4Select("MFG_OperationTaskDateTypes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);
        }
        public ActionResult GetParams_GSKTDIAM()
        {
            GSKTDIAMParamsViewModel model = new GSKTDIAMParamsViewModel();
            string ViewPath = "/Areas/MFG/Views/Reports/_Div_Report_GSKTDIAMParams.cshtml";
            try
            {
                var paramslist = MachineParametersService.CavitiesList(null, BaseGenericRequest);
                //paramslist.Add(new MachineParameters { MachineParameterID = 0, ParameterName = Resources.Common.TagAll });
                model.GasketsList = new SelectList(vw_CatalogService.List4Select("GasketsProducts", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "MachineID", "MachineName", 0);
                model.CavitiesList = new SelectList(paramslist, "MachineParameterID", "ParameterName", 0);
                model.ProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                var shiftList = ShiftService.List(null, null, BaseGenericRequest);
                //shiftList.Insert(0, new ShiftsMaster { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
                model.ShiftsList = new SelectList(shiftList, "ShiftID", "ShiftDescription");
                model.MaterialsList = new SelectList(vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public ActionResult GetReport_EERDTLS(DateTime? txt_StartDatetime_EERDTLS, DateTime? txt_EndDatetime_EERDTLS, int[] ddl_Machines_EERDTLS, int[] ddl_Shifts_EERDTLS)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = OperationRecordService.EER_Yield_Details_Report(txt_StartDatetime_EERDTLS, txt_EndDatetime_EERDTLS, ddl_Shifts_EERDTLS, ddl_Machines_EERDTLS, BaseGenericRequest);
            }
            catch (Exception e)
            {
                var error = e.ToString();
            }
            return ExcelExport(ds, Resources.MFG.Reports.lbl_Report_EERDTLS);
        }
        [HttpPost]
        public ActionResult GetReport_PRMS(int[] ddl_Paramaters_PRMS, int[] ddl_Machines_PRMS, int[] ddl_Process_PRMS,
        int[] ddl_Shifts_PRMS, int[] ddl_Materials_PRMS, int[] ddl_YesNo_PRMS, DateTime? txt_StartDatetime_PRMS,
        DateTime? txt_EndDatetime_PRMS)
        {
            DataSet ds = new DataSet();
            bool? IsGoodAnswer_PRMS = null;
            string yesnovalue = "";
            try
            {
                var a = ddl_YesNo_PRMS.Count();
                if (ddl_YesNo_PRMS.Count() == 1)
                {
                    yesnovalue = MiscellaneousService.Param_GetValue(ddl_YesNo_PRMS.FirstOrDefault(), 1, "null");
                }

                if (yesnovalue == "1")
                {
                    IsGoodAnswer_PRMS = true;
                }
                else if (yesnovalue == "0")
                {
                    IsGoodAnswer_PRMS = false;
                }

                ds = OperationRecordService.Parameters_Report(ddl_Paramaters_PRMS, ddl_Machines_PRMS, ddl_Process_PRMS,
                    ddl_Shifts_PRMS, ddl_Materials_PRMS, IsGoodAnswer_PRMS, txt_StartDatetime_PRMS, txt_EndDatetime_PRMS,
                    BaseGenericRequest);
            }
            catch (Exception e)
            {
                var a = e.ToString();
            }
            return ExcelExport(ds, Resources.MFG.Reports.lbl_Report_PRMS);
        }
        [HttpPost]
        public ActionResult GetReport_ACT(int[] ddl_Machines_ACT, int[] ddl_Setups_ACT, int[] ddl_Materials_ACT,
        int[] ddl_Process_ACT, int[] ddl_Shifts_ACT, int[] ddl_Status_ACT, int[] ddl_Responsibles_ACT,
        string txt_Attendant_ACT, int? ddl_DateType_ACT, DateTime? txt_StartDatetime_ACT, DateTime? txt_EndDatetime_ACT)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = OperationTaskService.Tasks_Report(ddl_Machines_ACT, ddl_Setups_ACT, ddl_Materials_ACT, ddl_Process_ACT,
                    ddl_Shifts_ACT, ddl_Status_ACT, ddl_Responsibles_ACT, txt_Attendant_ACT, ddl_DateType_ACT,
                    txt_StartDatetime_ACT, txt_EndDatetime_ACT, BaseGenericRequest);
            }
            catch (Exception)
            {

            }
            return ExcelExport(ds, Resources.MFG.Reports.lbl_Report_ACT);
        }
        [HttpPost]
        public ActionResult GetReport_GSKTDIAM(int[] ddl_Gasket_GSKTDIAM, int[] ddl_Machines_GSKTDIAM, int[] ddl_Cavities_GSKTDIAM,
        int[] ddl_Process_GSKTDIAM, int[] ddl_Shifts_GSKTDIAM, int[] ddl_Materials_GSKTDIAM, DateTime? txt_StartDatetime_GSKTDIAM,
        DateTime? txt_EndDatetime_GSKTDIAM)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = OperationRecordService.GasketDiameters_Report(ddl_Gasket_GSKTDIAM, ddl_Machines_GSKTDIAM, ddl_Cavities_GSKTDIAM, ddl_Process_GSKTDIAM,
                    ddl_Shifts_GSKTDIAM, ddl_Materials_GSKTDIAM, txt_StartDatetime_GSKTDIAM, txt_EndDatetime_GSKTDIAM,
                    BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            return ExcelExport(ds, Resources.MFG.Reports.lbl_Report_GSKTDIAM);
        }
    }
}