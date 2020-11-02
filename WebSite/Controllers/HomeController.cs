using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Models.ViewModels.Home;

namespace WebSite.Controllers
{

    public class HomeController : BaseController
    {
        public List<t_ChartData> ChartHours;


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestGraphics()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult Charts(int type)
        {
            ChartsViewModel model = new ChartsViewModel();
            int DFT_ShiftID = MiscellaneousService.Param_GetValue(VARG_FacilityID, "Chart_DFT_ShiftID", "0").ToInt();
            int[] DFT_selecccionados = MiscellaneousService.Param_GetValue(VARG_FacilityID, "Chart_DFT_SelectedDefects", "0").Split(',').Select(int.Parse).ToArray();
            int DFT_Top = MiscellaneousService.Param_GetValue(VARG_FacilityID, "Chart_DFT_Top", "0").ToInt();
            try
            {
                var Process = vw_CatalogService.Get(type, BaseGenericRequest);
                model.ProcessName = Process.DisplayText;
                model.UseLines = Process.Param1 == "1" ? true : false;
                ChartHours = DashboardService.GetShiftHours(DFT_ShiftID, BaseGenericRequest);
                var horas = ChartHours.Select(s => s.xdata + ":00").ToArray();
                model.HoursArray = string.Join(", ", horas);
                model.DefectsList = new SelectList(DefectService.SelectList(null, null, type, true, BaseGenericRequest), "DefectID", "DefectName");
                List<UsersProcessLine> productionLineList = new List<UsersProcessLine>();
                productionLineList = UsersProcessesLinesService.AccessList(type, BaseGenericRequest, false);
                productionLineList.Insert(0, new UsersProcessLine() { ProductionLineID = 0, ProductionLineName = Resources.Common.TagAll });
                model.LinesList = new SelectList(productionLineList, "ProductionLineID", "ProductionLineName");
                model.TopDefectList = new SelectList(vw_CatalogService.List4Select("ProductionTopDefects", BaseGenericRequest), "CatalogDetailID", "DisplayText", DFT_Top);
                model.VAList = new SelectList(vw_CatalogService.List4Select("ProductionVA", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                model.DesignList = new SelectList(vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                model.ShiftList = new SelectList(ShiftService.List(null, true, BaseGenericRequest), "ShiftID", "ShiftDescription");
                model.ProductionProcessID = type;
                model.DashboardSelectedDefects = string.Join(",", DFT_selecccionados);
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }
        public JsonResult GetYieldDefectsChartData(int ProductionProcessID, int LineID, int? VAID, int? DesignID, int? Top, int? ShiftID)
        {
            var result = DashboardService.YieldDefectsChart(null, ProductionProcessID, LineID, VAID, DesignID, ShiftID, Top, BaseGenericRequest);
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
        public JsonResult GetScrapChartData(int ProductionProcessID, int LineID, int? DesignID, int ShiftID)
        {
            var scrap = DashboardService.MoldScrapChart(null, ProductionProcessID, LineID, DesignID, ShiftID, BaseGenericRequest);
            var pieces = scrap.Select(s => s.ydata ?? "null").ToArray();
            var GoalValue = scrap.FirstOrDefault().GoalValue;
            return Json(new { GoalValue, pieces }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductionChartData(int ProductionProcessID, int LineID, int? VAID, int? DesignID, int ShiftID)
        {
            string LabelsColor = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ProductionChartLabelsColor", "#000000");
            var prod = DashboardService.ProductionChart(null, ProductionProcessID, LineID, VAID, DesignID, ShiftID, BaseGenericRequest);
            var pieces = prod.Select(s => s.ydata ?? "null").ToArray();
            var goals = prod.Select(s => s.GoalValue).ToArray();
            return Json(new { goals, pieces, LabelsColor }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetASNChartData(int ProductionProcessID, int LineID, int? VAID, int ShiftID)
        {
            string LabelsColor = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ASNChartLabelsColor", "#000000");
            var asns = DashboardService.ASNChart(null, ProductionProcessID, LineID, ShiftID, BaseGenericRequest);
            var pieces = asns.Select(s => s.ydata ?? "null").ToArray();
            var goals = asns.Select(s => s.GoalValue).ToArray();
            return Json(new { goals, pieces, LabelsColor }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopDefectsChart(int ProductionProcessID, int LineID, int? VAID, int? DesignID, int[] SelectedDefects, int ShiftID, string fontSize)
        {
            string DFT_TopDefectsChartAnnotationBGColor = MiscellaneousService.Param_GetValue(VARG_FacilityID, "TopDefectsChartAnnotationBGColor", "#00E396");
            string DFT_TopDefectsChartAnnotationColor = MiscellaneousService.Param_GetValue(VARG_FacilityID, "TopDefectsChartAnnotationColor", "#000000");
            string LabelsColor = MiscellaneousService.Param_GetValue(VARG_FacilityID, "TopDefectsChartLabelsColor", "#000000");
            var TopDefects = DashboardService.TopDefectsChart(DateTime.Now, ProductionProcessID, LineID, VAID, DesignID, SelectedDefects.ToList(), ShiftID, BaseGenericRequest);
            var defects = TopDefects.Select(s => s.xdata).ToArray();
            var pieces = TopDefects.Select(s => s.ydata ?? "null").ToArray();
            var colors = TopDefects.Select(s => s.color ?? "null").ToArray();
            var fontcolors = TopDefects.Select(s => s.FontColor ?? "null").ToArray();
            decimal MaxGoal = 0;
            if (TopDefects.Any())
            {
                MaxGoal = TopDefects.Select(s => s.GoalValue).Max();
            }
            var goalspoints = TopDefects.Select(s => new { x = s.xdata, y = s.GoalValue, label = new { text = "Goal:" + s.GoalValue + "%", style = new { color = DFT_TopDefectsChartAnnotationColor, background = DFT_TopDefectsChartAnnotationBGColor, fontSize, fontFamily = "Helvetica, Arial, sans-serif,Bold" } } }).ToList();
            return Json(new { defects, pieces, colors, fontcolors, goalspoints, MaxGoal, LabelsColor }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDefectChart(int ProductionProcessID, int LineID, int? VA, int? DesignID, string Defect, int ShiftID)
        {
            var defectsData = DashboardService.DefectChart(null, ProductionProcessID, LineID, VA, DesignID, ShiftID, Defect, BaseGenericRequest);
            var defects = defectsData.Select(s => s.ydata ?? "null").ToArray();
            var defectColor = defectsData.FirstOrDefault().color;
            var GoalValue = defectsData.FirstOrDefault().GoalValue;
            return Json(new { defects, defectColor, GoalValue }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShiftList(int ShiftID)
        {
            ChartHours = DashboardService.GetShiftHours(ShiftID, BaseGenericRequest);
            var horas = ChartHours.Select(s => s.xdata + ":00").ToArray();
            var HoursArray = string.Join(", ", horas);
            return Json(HoursArray, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetYieldGoal(int ProductionProcessID, int LineID, int? VAID, int? DesignID, int ShiftID)
        {
            var goal = DashboardService.GetYieldGoal(null, ProductionProcessID, LineID, VAID, DesignID, ShiftID, null, BaseGenericRequest).FirstOrDefault();
            if (goal == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(goal.GoalValue, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetYieldTitle(int ProductionProcessID, int LineID, int? VAID, int? DesignID, int ShiftID)
        {
            string Title = DashboardService.GetYieldChartTitle(null, ProductionProcessID, LineID, VAID, DesignID, ShiftID, "", BaseGenericRequest);
            return Json(new { Title }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetScrapGoal(int ProductionProcessID, int LineID, int? DesignID, int ShiftID)
        {
            var scrap = DashboardService.MoldScrapChart(null, ProductionProcessID, LineID, DesignID, ShiftID, BaseGenericRequest);
            var GoalValue = scrap.FirstOrDefault().GoalValue;
            return Json(new { GoalValue }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopDefectsGoalsPoints(int ProductionProcessID, int LineID, int? VAID, int? DesignID, int[] SelectedDefects, int ShiftID, string fontSize)
        {
            var TopDefects = DashboardService.TopDefectsChart(DateTime.Now, ProductionProcessID, LineID, VAID, DesignID, SelectedDefects.ToList(), ShiftID, BaseGenericRequest);
            var goalspoints = TopDefects.Select(s => new { x = s.xdata, y = s.GoalValue, label = new { text = "Goal:" + s.GoalValue + "%", style = new { color = "#ffffff", background = "#00E396", fontSize } } }).ToList();
            return Json(new { goalspoints }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ITCharts()
        {
            int DFT_LastYears = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ITChart_DFT_LastYears", "0").ToInt();
            int DFT_Top = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ITChart_DFT_Top", "0").ToInt();
            ITChartsViewModel model = new ITChartsViewModel();
            model.YearList = new SelectList(Enumerable.Range(DateTime.Now.Year - (DFT_LastYears - 1), DFT_LastYears).Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }).ToList(), "Value", "Text", DateTime.Now.Year);
            var months = StaticModels.MonthsList;
            months.Insert(0, new Month() { monthid = 0, monthname = Resources.Common.TagAll });
            model.MonthsList = new SelectList(months, "monthid", "monthname", DateTime.Now.Month);
            model.TopTicketsList = new SelectList(vw_CatalogService.List4Select("ITTopTickets", BaseGenericRequest), "CatalogDetailID", "DisplayText", DFT_Top);
            return View(model);
        }
        public JsonResult GetTicketsChartData(int? Year)
        {
            var series = DashboardService.TicketsChart(Year, BaseGenericRequest);
            return Json(new { series }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServicesChartData(int? Year)
        {
            var series = DashboardService.ServicesChart(Year, BaseGenericRequest);
            return Json(new { series }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectsChartData(int? Year)
        {
            var series = DashboardService.ProjectsChart(Year, null, BaseGenericRequest);
            var Projects = series.Select(s => s.ProjectName).ToArray();
            var Annotations = series.Select(s => s.Annotations).ToArray();
            return Json(new { series, count = series.Count, Projects, Annotations }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMonthsList(int Year)
        {
            var Months = DashboardService.GetYearMonths(Year, BaseGenericRequest);
            var monthdates = Months.Select(s => s.xdata).ToArray();
            var MonthsArray = string.Join(", ", monthdates);
            return Json(MonthsArray, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopTicketsChartData(int? Year, int? Month, int? Top)
        {
            var TopTickets = DashboardService.TopTicketsChart(Year, Month, Top, BaseGenericRequest);
            var Types = TopTickets.Select(s => s.xdata).ToArray();
            var Tickets = TopTickets.Select(s => s.ydata ?? "null").ToArray();
            return Json(new { Types, Tickets }, JsonRequestBehavior.AllowGet);
        }


    }
}