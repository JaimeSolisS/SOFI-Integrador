using System;
using WebSite.Areas.MFG.Models.ViewModels.DemoldDefectsCharts;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Service;
using Core.Entities;
using WebSite.Models;

namespace WebSite.Areas.MFG.Controllers
{
    public class DemoldDefectsChartsController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var DemoldDefectsCategoriesList = DemoldDefectsService.GetCategoriesAndData("DemoldDefectsTypes", null, BaseGenericRequest);
                DemoldDefectsCategoriesList.Insert(0, new Catalog() { CatalogDetailID = 0, Param1 = Resources.Common.TagAll });
                model.DemoldDefectsCategoriesList = new SelectList(DemoldDefectsCategoriesList, "CatalogDetailID", "Param1");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }
        public ActionResult PieChartPanel(string DemoldDefectCategory)
        {
            IndexViewModel model = new IndexViewModel();
            string ViewPath = "/Areas/MFG/Views/DemoldDefectsCharts/_DemoldDefectsPieChart.cshtml";
            try
            {
                var ProductionLinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription", 0);
                model.ProductionLinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber", 0);
                model.FamiliesList = new SelectList(vw_CatalogService.List4Select("LensMoldFamilies", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText", 0);
                model.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
                model.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
                model.DesignList = new SelectList(vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText", 0);
                ViewBag.DemoldDefectCategory = DemoldDefectCategory;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);
        }
        public ActionResult PercGrossChartPanel(string DemoldDefectCategory)
        {
            IndexViewModel model = new IndexViewModel();
            string ViewPath = "/Areas/MFG/Views/DemoldDefectsCharts/_DemoldDefectsPercGrossChart.cshtml";

            try
            {
                var ProductionLinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription", 0);
                model.ProductionLinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber", 0);
                model.FamiliesList = new SelectList(vw_CatalogService.List4Select("LensMoldFamilies", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText", 0);
                model.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
                model.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
                model.DesignList = new SelectList(vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText", 0);
                ViewBag.DemoldDefectCategory = DemoldDefectCategory;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);

        }

        public JsonResult GetPieChartData(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs,
            DateTime? StartDate, DateTime? EndDate, string DefectType, int? DesignID)
        {
            var result = DemoldDefectChartsService.GetPieChartData(ProductionLineIDs, MoldFamilyIDs, ShiftIDs, StartDate, EndDate, DefectType, DesignID, BaseGenericRequest);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPercGrossChartData(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs,
            DateTime? StartDate, DateTime? EndDate, string DefectType, int? DesignID)
        {
            var result = DemoldDefectChartsService.GetBarChartData(ProductionLineIDs, MoldFamilyIDs, ShiftIDs, StartDate, EndDate, DefectType, DesignID, BaseGenericRequest);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDemodDefectChartsHelpModal()
        {
            string ViewPath = "~/Areas/MFG/Views/DemoldDefectsCharts/_Mo_DemoldDefectsChartsHelp.cshtml";
            try
            {

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath);
        }

    }
}