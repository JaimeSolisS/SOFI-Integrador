using Core.Entities;
using Core.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.MNT.Models.ViewModels.EnergyDashboard;
using WebSite.Areas.MNT.Models.ViewModels.EnergySensors;
using WebSite.Models;

namespace WebSite.Areas.MNT.Controllers
{
    public class EnergyDashboardController : BaseController
    {

        public ActionResult Index(DateTime? date)
        {
            EnergySensorFamiliesViewModel model = new EnergySensorFamiliesViewModel();
            try
            {
                var FormatDate = String.Format("{0:MM-yyyy}", DateTime.Now);

                if (date != null)
                {
                    FormatDate = String.Format("{0:MM-yyyy}", date);
                }

                var EnergySensorFamiliesList = MNT_EnergySensorsFamiliesService.Dashboard_List(date, BaseGenericRequest);
                if (EnergySensorFamiliesList != null && EnergySensorFamiliesList.Any())
                {
                    model.EnergySensorFamiliesList = EnergySensorFamiliesList;
                    model.MaxValue = EnergySensorFamiliesList.FirstOrDefault().TotalConsumptionQuantity * Convert.ToDecimal(1.5);
                }
                model.Date = FormatDate;
                model.LastHour = Resources.MNT.EnergySensors.lbl_LastHour + ": " + DateTime.Now.AddHours(-2).Hour + " - " + DateTime.Now.AddHours(-1).Hour + "      " + Resources.Common.lbl_Date + ": " + String.Format("{0:d-MMM-yyyy}", DateTime.Now);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }


        public ActionResult EnergyLinks()
        {
            return View();
        }

        public ActionResult Chart(int EnergySensorID, int? EnergySensorFamilyID)
        {
            EnergySensorValuesViewModel model = new EnergySensorValuesViewModel();

            try
            {
                string[] EnergySensorFamilyIDs = new string[1];
                EnergySensorFamilyIDs[0] = EnergySensorFamilyID.ToString();
                model.EnergySensorsList = new SelectList(MNT_EnergySensorsService.List(EnergySensorFamilyIDs, BaseGenericRequest), "EnergySensorID", "SensorName");
                model.EnergySensorID = EnergySensorID;
                model.EnergySensorFamilyID = EnergySensorFamilyID;
                model.FamilyName = MNT_EnergySensorsFamiliesService.List(EnergySensorFamilyID, BaseGenericRequest).FirstOrDefault().FamilyName;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return View(model);
        }

        public ActionResult Details(int EnergySensorFamilyID)
        {
            EnergySensorDetailsViewModel model = new EnergySensorDetailsViewModel();

            try
            {
                var SensorDate = DateTime.Today.Date;//DateTime.Now;
                var SensorHour = DateTime.Now.Hour;//la hora anterior
                int? EnergySensorFamiliyIDForm = Convert.ToInt32(EnergySensorFamilyID);
                model.EnergySensorsList = MNT_EnergySensorsService.DashboardList(EnergySensorFamilyID, SensorDate, SensorHour,  BaseGenericRequest);
                model.LastHour = Resources.MNT.EnergySensors.lbl_LastHour + ": " + DateTime.Now.Hour + " - " + DateTime.Now.AddHours(1).Hour + "      " + Resources.Common.lbl_Date + ": " + String.Format("{0:d-MMM-yyyy}", DateTime.Now);
                model.EnergySensorFamilyID = EnergySensorFamiliyIDForm;
                model.FamilyName = MNT_EnergySensorsFamiliesService.List(EnergySensorFamiliyIDForm, BaseGenericRequest).FirstOrDefault().FamilyName;

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }

        public JsonResult GetHourValuesByDayChartData(int EnergySensorID, DateTime? Date)
        {
            var result = MNT_EnergySensorsService.GetDataForConsumptionDayChart(EnergySensorID, Date, BaseGenericRequest);
            var series = result.SeriesList;

            bool isValidSeries = series[1].label == null ? false : true;

            decimal?[] MaxValueSerie;
            if (isValidSeries)
                MaxValueSerie = series.Where(w => w.label == "Value By Hour").FirstOrDefault().data;
            else
                MaxValueSerie = series.Where(w => w.label == "Limit").FirstOrDefault().data;


            decimal? MaxValue = 0;
            for (int i = 0; i < MaxValueSerie.Length; i++)
            {
                decimal? thisNum = MaxValueSerie[i];
                if (!MaxValue.HasValue || thisNum != 0)
                {
                    if (thisNum > MaxValue.Value)
                    {
                        MaxValue = thisNum;
                    }

                }
            }

            MaxValue = MaxValue * 1.62m;
            return Json(new { series, MaxValue }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHourValuesGlobalChartData(int EnergySensorID, int EnergySensorFamilyID, DateTime? StartDate, DateTime? EndDate)
        {
            var result = MNT_EnergySensorsService.GetDataForConsumptionGlobalChart(EnergySensorID, EnergySensorFamilyID, StartDate, EndDate, BaseGenericRequest);
            var dataset = JsonConvert.SerializeObject(result.Tables[0], Newtonsoft.Json.Formatting.Indented);
            var labels = JsonConvert.SerializeObject(result.Tables[1], Newtonsoft.Json.Formatting.Indented);
            var color = JsonConvert.SerializeObject(result.Tables[2], Newtonsoft.Json.Formatting.Indented);


            return Json(new { dataset, labels, color }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAlarmConfiguration(int? EnergySensorID, List<EnergySensorValue> MaxValuesFourHours)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorValuesService.Insert(EnergySensorID, MaxValuesFourHours, BaseGenericRequest);
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

        public JsonResult GetGaugeValues()
        {
            List<EnergySensorFamiliesGauge> result = new List<EnergySensorFamiliesGauge>();
            try
            {
                var SensorDate = DateTime.Now;
                var SensorHour = SensorDate.Hour;
                result = MNT_EnergySensorsService.GetGaugeValues(SensorDate, SensorHour, BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalChartTotalFamilies()
        {
            ChartFamiliesGaugeViewModel model = new ChartFamiliesGaugeViewModel();
            string ViewPath = "~/Areas/MNT/Views/EnergyDashboard/_Mo_GaugeFamiliesChart.cshtml";

            return Json(new
            {
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHourValuesByFamiliesChartGauge(DateTime ChartDate)
        {
            var result = MNT_EnergySensorsService.GetHourValuesByFamiliesChartGauge(ChartDate, BaseGenericRequest);
            var series = result.SeriesList;
            return Json(new { series }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalExporToExcel(int EnergySensorID)
        {
            ChartFamiliesGaugeViewModel model = new ChartFamiliesGaugeViewModel();
            string ViewPath = "~/Areas/MNT/Views/EnergyDashboard/_Mo_ExportToExcel.cshtml";
            model.EnergySensorID = EnergySensorID;

            return Json(new
            {
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SensorsAlertsExportToExcel(int EnergySensorID, DateTime txt_StartDate, DateTime txt_EndDate)
        {
            DataSet ds = new DataSet();
            ds = MNT_EnergySensorsService.SensorsAlertsExportToExcel(EnergySensorID, txt_StartDate, txt_EndDate, BaseGenericRequest);
            return this.ExcelExport(ds, Resources.MNT.EnergySensors.title_ExcelReportAlert);
        }


    }
}