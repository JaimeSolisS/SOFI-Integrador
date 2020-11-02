using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.Dashboard;
using WebSite.Filters;
using WebSite.Models;

namespace WebSite.Areas.MFG.Controllers
{
    [SessionExpire]
    public class DashboardController : BaseController
    {
        public List<t_ChartData> ChartHours;
        [SecurityFilter]
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            int DFT_DateType;
            int DFT_Shift;
            DateTime DFT_Date = DateTime.Now;
            try
            {
                var a = GetSCADAStatusMachine("IMM11");

                DFT_Shift = DashboardService.GetCurrentShiftID(5, BaseGenericRequest);
                DFT_Date = DateTime.Now;

                model.MachinesList = MachineService.Dashboard_List(BaseGenericRequest);
                model.HeaderModel.JulianDay = OperationRecordService.GetJulianDay(DateTime.Now, "", BaseGenericRequest);
                model.HeaderModel.PlantName = FacilityService.List(null, BaseGenericRequest.FacilityID, true, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID).First().FacilityName;
                var shift = ShiftService.Get(DFT_Shift, true, BaseGenericRequest);
                model.HeaderModel.Shift = shift.ShiftDescription;  //mostrar todo el dia
                DFT_DateType = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "MFG_OperationTaskDateTypes", "CR", out string ErrorMessage);
                model.TaskList = OperationTaskService.List(DFT_DateType, DFT_Date, DFT_Date, BaseGenericRequest);
                model.HeaderModel.ShiftList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "ShiftID", "ShiftDescription", DFT_Shift);
                model.CultureID = BaseGenericRequest.CultureID.ToUpper();

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }
        public JsonResult UpdateMachinesList(DateTime? Date, int? ShiftID)
        {
            List<DashboardMachine> model = new List<DashboardMachine>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/Dashboard/_Div_Machines.cshtml";

            try
            {
                model = MachineService.Dashboard_List(Date, ShiftID, BaseGenericRequest);

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
        public JsonResult UpdateTasksList(DateTime? Date)
        {
            List<OperationTask> model = new List<OperationTask>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/Dashboard/_Tbl_Tasks.cshtml";
            int DFT_DateType;
            DateTime startDate;
            DateTime endDate;
            string JulianDay = "";
            try
            {
                startDate = Date.Value;
                endDate = startDate.AddDays(1);
                DFT_DateType = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "MFG_OperationTaskDateTypes", "CR", out string ErrorMessage);
                model = OperationTaskService.List(DFT_DateType, startDate, endDate, BaseGenericRequest);
                JulianDay = OperationRecordService.GetJulianDay(Date.Value, "", BaseGenericRequest);
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
                View = RenderRazorViewToString(ViewPath, model),
                JulianDay
            }, JsonRequestBehavior.AllowGet);
        }

        [SecurityFilter]
        public JsonResult Show(int MachineID, int OperationRecordID)
        {
            GenericReturn result = new GenericReturn();
            ShowViewModel model = new ShowViewModel();
            string ViewPath = "~/Areas/MFG/Views/Dashboard/Show.cshtml";
            try
            {
                var Parameters = MachineService.Dashboard_OperationParameters_List(OperationRecordID, BaseGenericRequest);
                if (Parameters != null && Parameters.Count() > 0)
                {
                    model.ParametersList = Parameters;
                    model.SectionsList = Parameters.GroupBy(g => new { g.ParameterSectionID, g.ParameterSectionName }).Select(s => new Catalog { CatalogDetailID = s.Key.ParameterSectionID, DisplayText = s.Key.ParameterSectionName }).ToList();
                }
                var Tasks = OperationTaskService.List(OperationRecordID, MachineID, BaseGenericRequest);
                if (Tasks != null && Tasks.Count() > 0)
                {
                    model.TaskList = Tasks;
                }
                var Material = OperationProductionService.Get(OperationRecordID, BaseGenericRequest);
                if (Material != null)
                {
                    model.MaterialName = Material.MaterialName;
                }
                var oper = OperationRecordService.Get(OperationRecordID, BaseGenericRequest);
                if (oper != null)
                {
                    model.SetupName = oper.MachineSetupName;
                }
                model.DashboardMachine = MachineService.Get(MachineID, BaseGenericRequest);
                model.HeaderModel.JulianDay = OperationRecordService.GetJulianDay(DateTime.Now, "", BaseGenericRequest);
                model.HeaderModel.PlantName = FacilityService.List(null, BaseGenericRequest.FacilityID, true, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID).First().FacilityName;
                model.HeaderModel.Shift = ShiftService.Get(DashboardService.GetCurrentShiftID(5, BaseGenericRequest), true, BaseGenericRequest).ShiftDescription;
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

        #region Reports
        public JsonResult GetExportToExcelModal()
        {
            ExportToExcelViewModel model = new ExportToExcelViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/Dashboard/_Mo_ExportToExcel.cshtml";
            try
            {
                int DFT_shift = DashboardService.GetCurrentShiftID(5, BaseGenericRequest);
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription", DFT_shift);
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "MachineID", "MachineName");
                model.CultureID = BaseGenericRequest.CultureID.ToUpper();

                var shift = ShiftService.Get(DFT_shift, true, BaseGenericRequest);
                model.StartDate = DateTime.Today;
                model.EndDate = DateTime.Today;

                model.StartTime = shift.StartTime;
                model.EndTime = shift.EndTime;
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
        public ActionResult ExportToExcel(int? ddl_ExcelShifts, int? ddl_ExcelMachines, DateTime txt_StartDateExcel, string txt_StartTimeExcel, DateTime txt_EndDateExcel, string txt_EndTimeExcel)
        {
            DataSet ds = new DataSet();

            DateTime StartTime = Convert.ToDateTime(txt_StartTimeExcel);
            DateTime StartDate = txt_StartDateExcel.Date + StartTime.TimeOfDay;

            DateTime EndTime = Convert.ToDateTime(txt_EndTimeExcel);
            DateTime EndDate = txt_EndDateExcel.Date + EndTime.TimeOfDay;


            ds = OperationRecordService.EER_Yield_Report(ddl_ExcelShifts, ddl_ExcelMachines, StartDate.Date, EndDate.Date, StartDate, EndDate, BaseGenericRequest);
            return EER_Yield_ReportExcelFormatExport(ds, "DailyReport");
        }

        public ActionResult EER_Yield_ReportExcelFormatExport(DataSet ds, string fileName)
        {

            using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.Columns[0].ColumnName.Contains("TableName"))
                    {
                        if (dt.Rows.Count > 0)
                        { dt.TableName = dt.Rows[0]["TableName"].ToString(); }

                        dt.Columns.RemoveAt(0);
                    }

                    wb.Worksheets.Add(dt);

                    //formato de colores de columna EER
                    wb.Worksheet(1).Range("AE2:AE" + dt.Rows.Count + 1).RangeUsed().AddConditionalFormat().WhenBetween(0, 50).Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.Red);
                    wb.Worksheet(1).Range("AE2:AE" + dt.Rows.Count + 1).RangeUsed().AddConditionalFormat().WhenBetween(51, 70).Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.Yellow);
                    wb.Worksheet(1).Range("AE2:AE" + dt.Rows.Count + 1).RangeUsed().AddConditionalFormat().WhenBetween(71, 200).Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.Green);
                    //formato de colores de columna yield
                    wb.Worksheet(1).Range("AI2:AI" + dt.Rows.Count + 1).RangeUsed().AddConditionalFormat().WhenBetween(0, 94).Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.Red);
                    wb.Worksheet(1).Range("AI2:AI" + dt.Rows.Count + 1).RangeUsed().AddConditionalFormat().WhenBetween(95, 96).Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.Yellow);
                    wb.Worksheet(1).Range("AI2:AI" + dt.Rows.Count + 1).RangeUsed().AddConditionalFormat().WhenBetween(97, 200).Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.Green);

                    //ajuste de colores columnas de defectos
                    wb.Worksheet(1).Range("F1:R" + dt.Rows.Count).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.DarkBlue;
                    wb.Worksheet(1).Range("F1:R" + dt.Rows.Count).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                    //ajuste de colores columnas de downtimes 
                    wb.Worksheet(1).Range("S1:AC" + dt.Rows.Count).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                    wb.Worksheet(1).Range("F2:AC" + dt.Rows.Count).Style.NumberFormat.Format = "#,##0.00";

                    //ajuste de tamanio de encabezados
                    wb.Worksheet(1).Row(1).Style.Alignment.SetTextRotation(90);
                    wb.Worksheet(1).Rows().AdjustToContents();
                    wb.Worksheet(1).Columns().AdjustToContents();
                    wb.Worksheet(1).Row(1).Height = 160;
                }

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
                }
            }
        }

        public ActionResult DashboardChartReports(DataSet ds, string fileName)
        {

            using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
            {
                int index = 0;
                string[] ReportNames = { Resources.CI.Dashboard.lbl_DashboardGasket_YieldDefectsReport
                        ,Resources.CI.Dashboard.lbl_DashboardGasket_DowntimesReport
                        ,Resources.CI.Dashboard.lbl_DashboardGasket_ProductionReport };

                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.Columns[0].ColumnName.Contains("TableName"))
                    {
                        if (dt.Rows.Count > 0)
                        { dt.TableName = dt.Rows[0]["TableName"].ToString(); }

                        dt.Columns.RemoveAt(0);
                    }

                    dt.TableName = ReportNames[index];
                    wb.Worksheets.Add(dt);
                    index++;
                }

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
                }
            }
        }

        #endregion

        #region Charts
        public ActionResult Charts()
        {
            ChartsViewModel model = new ChartsViewModel();
            int DFT_Shift;
            try
            {
                DFT_Shift = DashboardService.GetCurrentShiftID(5, BaseGenericRequest);
                model.ShiftList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "ShiftID", "ShiftDescription", DFT_Shift);
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "MachineID", "MachineName");
                var process = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
                process.Add(new Catalog { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
                model.ProcessList = new SelectList(process.OrderBy(o => o.CatalogDetailID), "CatalogDetailID", "DisplayText");
                var materials = vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false);
                materials.Add(new Catalog { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
                model.MaterialsList = new SelectList(materials.OrderBy(o => o.CatalogDetailID), "CatalogDetailID", "DisplayText");
                ChartHours = DashboardService.GetShiftHours(DFT_Shift, BaseGenericRequest);
                var horas = ChartHours.Select(s => s.xdata + ":00").ToArray();
                model.HoursArray = string.Join(", ", horas);
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }
        public JsonResult GetYieldDefectsChartData(int[] MachinesIDs, int? ProductionProcessID, int? MaterialID, DateTime OperationDate, int? ShiftID)
        {

            var result = OperationRecordService.YieldDefectsChart(OperationDate, MachinesIDs, ProductionProcessID, MaterialID, ShiftID, null, BaseGenericRequest);
            var series = result.SeriesList;
            var MaxYield = series.Where(w => w.label == "Yield").FirstOrDefault().data;


            decimal? MaxValue = 100;
            for (int i = 0; i < MaxYield.Length; i++)
            {
                decimal? thisNum = MaxYield[i];
                if (!MaxValue.HasValue || thisNum != 0)
                {
                    if (thisNum < MaxValue.Value)
                    {
                        MaxValue = thisNum;
                    }

                }
            }
            MaxValue = 100 - MaxValue;
            MaxValue = MaxValue * 1.62m;
            return Json(new { series, MaxValue }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEERDowntimesChartData(int[] MachinesIDs, int? ProductionProcessID, int? MaterialID, DateTime? OperationDate, int? ShiftID)
        {
            var result = OperationRecordService.EERDowntimesChart(OperationDate, MachinesIDs, ProductionProcessID, MaterialID, ShiftID, null, BaseGenericRequest);
            var series = result.SeriesList;
            var MaxYield = series.Where(w => w.label == "Yield").FirstOrDefault().data;

            decimal? MaxValue = 100;
            for (int i = 0; i < MaxYield.Length; i++)
            {
                decimal? thisNum = MaxYield[i];
                if (!MaxValue.HasValue || thisNum != 0)
                {
                    if (thisNum < MaxValue.Value)
                    {
                        MaxValue = thisNum;
                    }

                }
            }
            MaxValue = 100 - MaxValue;
            MaxValue = MaxValue * 1.62m;
            return Json(new { series, MaxValue }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductionChartData(int[] MachinesIDs, DateTime? OperationDate, int? ShiftID)
        {
            string LabelsColor = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ProductionChartLabelsColor", "#000000");
            var prod = OperationRecordService.ProductionChart(OperationDate, MachinesIDs, ShiftID, BaseGenericRequest);
            var pieces = prod.Select(s => s.ydata ?? "null").ToArray();
            var goals = prod.Select(s => s.GoalValue).ToArray();
            return Json(new { goals, pieces, LabelsColor }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDowntimesChartData(int[] MachinesIDs, int? ProductionProcessID, int? MaterialID, DateTime? OperationDate, int? ShiftID)
        {
            var result = OperationRecordService.DowntimesChart(OperationDate, MachinesIDs, ProductionProcessID, MaterialID, ShiftID, null, BaseGenericRequest);
            var series = result.SeriesList;
            return Json(new { series }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public JsonResult GetLabelHours(int ShiftID)
        {
            //var DFT_Shift = DashboardService.GetCurrentShiftID(5, BaseGenericRequest);
            ChartHours = DashboardService.GetShiftHours(ShiftID, BaseGenericRequest);
            var horas = ChartHours.Select(s => s.xdata + ":00").ToArray();
            var HoursArray = string.Join(", ", horas);
            return Json(HoursArray, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetShiftStartEndHours(int ShiftID)
        {
            var shift = ShiftService.Get(ShiftID, true, BaseGenericRequest);
            string start = shift.StartTime.ToString("HH:mm");
            string end = shift.EndTime.ToString("HH:mm");
            return Json(new { start, end }, JsonRequestBehavior.AllowGet);
        }

        public bool GetSCADAStatusMachine(string machineName)
        {
            bool result = false;
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                result = client.DownloadString("http://172.21.69.88:8080/sofi/getalarmasna/" + machineName).ToBoolean();
            }
            return result;
        }


        #region Reporte de las gráficas de MFG/Dashboard/Charts

        public ActionResult GetFiltersReportConfirmationModal(int? ProcessID, int? MaterialID, string ActionName, bool IsProductionRate)
        {
            string ViewPath = "~/Areas/MFG/Views/Dashboard/_Mo_FiltersReportConfirmation.cshtml";
            ModalExportToExcelViewModel model = new ModalExportToExcelViewModel();
            try
            {
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false), "MachineID", "MachineName");
                model.ActionName = ActionName;
                model.IsProduction = IsProductionRate;
                if (!(IsProductionRate))
                {
                    var process = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
                    process.Add(new Catalog { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
                    model.ProcessList = new SelectList(process.OrderBy(o => o.CatalogDetailID), "CatalogDetailID", "DisplayText", ProcessID);
                    var materials = vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false);
                    materials.Add(new Catalog { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
                    model.MaterialsList = new SelectList(materials.OrderBy(o => o.CatalogDetailID), "CatalogDetailID", "DisplayText", MaterialID);
                    model.WidthClass = "col-sm-4";
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return PartialView(ViewPath, model);
        }

        public ActionResult DashboardGasket_YieldDefectsReport(DateTime Mo_ExcelReport_Date, int[] ddl_Machines, int? Mo_ExcelReport_ShiftID, int? ddl_Process, int? ddl_Materials)
        {
            DataSet ds = new DataSet();
            ds = OperationRecordService.DashboardGasket_YieldDefectsReport(Mo_ExcelReport_Date, ddl_Machines, Mo_ExcelReport_ShiftID, ddl_Process, ddl_Materials, BaseGenericRequest);
            return DashboardChartReports(ds, Resources.CI.Dashboard.lbl_DashboardGasket_YieldDefectsReport);
        }

        public ActionResult DashboardGasket_DowntimesReport(DateTime Mo_ExcelReport_Date, int[] ddl_Machines, int? Mo_ExcelReport_ShiftID, int? ddl_Process, int? ddl_Materials)
        {
            DataSet ds = new DataSet();
            ds = OperationRecordService.DashboardGasket_DowntimesReport(Mo_ExcelReport_Date, ddl_Machines, Mo_ExcelReport_ShiftID, ddl_Process, ddl_Materials, BaseGenericRequest);
            return DashboardChartReports(ds, Resources.CI.Dashboard.lbl_DashboardGasket_DowntimesReport);
        }

        public ActionResult DashboardGasket_ProductionReport(DateTime Mo_ExcelReport_Date, int[] ddl_Machines, int? Mo_ExcelReport_ShiftID)
        {
            DataSet ds = new DataSet();
            ds = OperationRecordService.DashboardGasket_ProductionReport(Mo_ExcelReport_Date, ddl_Machines, Mo_ExcelReport_ShiftID, BaseGenericRequest);
            return DashboardChartReports(ds, Resources.CI.Dashboard.lbl_DashboardGasket_ProductionReport);
        }

        public ActionResult DashboardChartsAllReports(
            DateTime txt_Date
            , int? ShiftID
            //Datos para Yield
            , int[] yieldchart_SelectedMachines
            , int? yieldchart_process
            , int? yieldchart_materials
            //Datos para Downtimes
            , int[] dtchart_SelectedMachines
            , int? dtchart_process
            , int? dtchart_materials
            //Datos de Production
            , int[] prodchart_SelectedMachines
            )
        {
            DataSet ds = new DataSet();
            ds = OperationRecordService.DashboardChartsAllReports(
                txt_Date
                , ShiftID
                , yieldchart_SelectedMachines
                , yieldchart_process
                , yieldchart_materials
                , dtchart_SelectedMachines
                , dtchart_process
                , dtchart_materials
                , prodchart_SelectedMachines
                , BaseGenericRequest);
            return DashboardChartReports(ds, Resources.CI.Dashboard.lbl_DashboardGasket_AllDashboardReports);
        }

        #endregion
    }
}