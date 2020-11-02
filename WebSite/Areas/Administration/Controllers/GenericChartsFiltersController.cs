using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.Administration.Models.ViewModels.GenericCharts;
using WebSite.Models;

namespace WebSite.Areas.Administration.Controllers
{
    public class GenericChartsFiltersController : BaseController
    {

        public ActionResult GetFiltersByChart(int GenericChartID, int? GenericChartHeaderDataID)
        {
            DataChartFiltersViewModel model = new DataChartFiltersViewModel();
            var viewPath = "~/Areas/Administration/Views/GenericCharts/_DataChartAdminFilters.cshtml";

            try
            {
                var GenericChartList = GenericChartService.List(GenericChartID, null, BaseGenericRequest);
                var GenericChartFilterList = GenericChartsFiltersService.List(GenericChartID, BaseGenericRequest);
                model.FilterList = GenericChartFilterList;
                model.ChartType = GenericChartList.FirstOrDefault().ChartTypeText.ToLower();
                model.ChartName = GenericChartList.FirstOrDefault().ChartName;
                model.ChartTitle = GenericChartList.FirstOrDefault().ChartTitle;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        [HttpPost]
        public JsonResult SaveNewFilter(GenericChartsFilters GenericChartsFilterEntity)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsFiltersService.Insert(
                    GenericChartsFilterEntity.GenericChartID
                    , GenericChartsFilterEntity.FilterName
                    , GenericChartsFilterEntity.FilterTypeID
                    , GenericChartsFilterEntity.FilterListID
                    , GenericChartsFilterEntity.DefaultValue
                    , GenericChartsFilterEntity.DefaultValueFormula
                    , GenericChartsFilterEntity.Enabled
                    , BaseGenericRequest);
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
        public JsonResult SaveEditedFilter(GenericChartsFilters GenericChartsFilterEntity)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsFiltersService.Update(
                    GenericChartsFilterEntity.GenericChartFilterID
                    , GenericChartsFilterEntity.GenericChartID
                    , GenericChartsFilterEntity.FilterName
                    , GenericChartsFilterEntity.FilterTypeID
                    , GenericChartsFilterEntity.FilterListID
                    , GenericChartsFilterEntity.DefaultValue
                    , GenericChartsFilterEntity.DefaultValueFormula
                    , GenericChartsFilterEntity.Enabled
                    , BaseGenericRequest);
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
        public JsonResult DeleteFilter(int GenericChartFilterID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsFiltersService.Delete(GenericChartFilterID, BaseGenericRequest);
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


        public ActionResult FiltersTableReload(int GenericChartID)
        {
            List<GenericChartsFilters> model = new List<GenericChartsFilters>();
            var viewPath = "~/Areas/Administration/Views/GenericChartsAdministration/_Tbl_GenericChartsFilters.cshtml";

            try
            {
                model = GenericChartsFiltersService.List(GenericChartID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public JsonResult GetListOfLists(string ValueID)
        {
            IEnumerable<SelectListItem> ListOfLists = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                ListOfLists = new SelectList(vw_CatalogService.List(ValueID, BaseGenericRequest), "CatalogDetailID", "DisplayText");
                if (ListOfLists != null)
                {
                    result.ErrorCode = 0;
                }
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
                ListOfLists
            }, JsonRequestBehavior.AllowGet);
        }

    }
}