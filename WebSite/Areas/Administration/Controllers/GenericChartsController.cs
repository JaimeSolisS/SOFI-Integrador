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
    public class GenericChartsController : BaseController
    {
        // GET: Administration/GenericCharts
        public ActionResult Index()
        {
            var model = new DataChartAdminViewModel();
            try
            {
                //Cargar listado de Áreas
                model.ChartAreasList = new SelectList(vw_CatalogService.List("DataChartsAreas", BaseGenericRequest), "CatalogDetailID", "ValueID");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public JsonResult GetChartsByAreaList(int ChartAreaID)
        {
            IEnumerable<SelectListItem> ChartsOfAreaList = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                ChartsOfAreaList = new SelectList(GenericChartService.List(ChartAreaID, BaseGenericRequest), "GenericChartID", "ChartName");
                if (ChartsOfAreaList != null)
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
                ChartsOfAreaList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFieldsOfChart(int GenericChartID)
        {
            DataChartFieldsViewModel model = new DataChartFieldsViewModel();
            var viewPath = "~/Areas/Administration/Views/GenericCharts/_DataChartAdminDataFields.cshtml";
            return PartialView(viewPath, model);
        }

        public ActionResult Search(int GenericChartID, string ValuesFromFilters)
        {
            DataChartFieldsViewModel model = new DataChartFieldsViewModel();
            var viewPath = "~/Areas/Administration/Views/GenericCharts/_DataChartAdminDataFields.cshtml";

            try
            {
                var GenericChartDataList = GenericChartDataService.List(GenericChartID, ValuesFromFilters, BaseGenericRequest);
                model.GenericChartDataList = GenericChartDataList;
                model.DataFieldsList = GenericChartService.GetAxes_List(GenericChartID, BaseGenericRequest);
                model.GenericChartDataList = GenericChartDataList;
                model.ContainsData = GenericChartDataList.Count > 0 ? true : false;
                model.GenericChartHeaderDataID = GenericChartDataList.FirstOrDefault().GenericChartHeaderDataID;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public JsonResult GetModalChartPreview()
        {
            DataChartFieldsViewModel model = new DataChartFieldsViewModel();
            string ViewPath = "~/Areas/Administration/Views/GenericCharts/_Mo_ChartPreview.cshtml";
            return Json(new{ View = RenderRazorViewToString(ViewPath, model)}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDataChart(string FitlerInfo, int GenericChartID, List<GenericChartData> GenericChartDataEntityData)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = GenericChartDataService.Insert(FitlerInfo, GenericChartID, GenericChartDataEntityData, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int GenericChartID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartService.Delete(GenericChartID, BaseGenericRequest);
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

        public ActionResult LoadGenericChartsTable(int? AreaID, int? ChartID)
        {
            DataChartAdminViewModel model = new DataChartAdminViewModel();
            List<GenericChart> list = new List<GenericChart>();
            string viewPath = "~/Areas/Administration/Views/GenericChartsAdministration/_Tbl_GenericCharts.cshtml";

            try
            {
                list = GenericChartService.List(BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, list);
        }

        public JsonResult LoadFilterInfo(int CatalogID)
        {
            IEnumerable<SelectListItem> FilterElementsList = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                var CatalogTag = CatalogDetailService.Get(CatalogID, BaseGenericRequest).ValueID;
                FilterElementsList = new SelectList(vw_CatalogService.List(CatalogTag, BaseGenericRequest), "CatalogDetailID", "DisplayText").OrderBy(x => x.Value);

                if (FilterElementsList != null)
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
                FilterElementsList
            }, JsonRequestBehavior.AllowGet);
        }

        //DASHBOARD: Esta funcion dibuja los filtros
        public JsonResult LoadDynamicFilterInfo(int FilterID, int GenericChartID)
        {
            List<Catalog> FilterElementsList = new List<Catalog>();
            GenericReturn result = new GenericReturn();
            string DefaultValue = "";

            try
            {
                var ValueID = vw_CatalogService.Get(FilterID, BaseGenericRequest).ValueID;
                DefaultValue = GenericChartsFiltersService.GetDefault(GenericChartID, FilterID, BaseGenericRequest).DefaultValue;
                FilterElementsList = vw_CatalogService.List(ValueID, BaseGenericRequest);

                if (FilterElementsList != null)
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
                FilterElementsList,
                DefaultValue
            }, JsonRequestBehavior.AllowGet);
        }

        //DASHBOARD: Esta funcion dibuja las gráficas dinamicas
        public JsonResult GetChartInfoByArea(string AreaValueID)
        {
            DataChartsInfoViewModel model = new DataChartsInfoViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "/Views/Shared/_GenericChartTemplate.cshtml";

            try
            {
                string oError = "";
                var ChartAreaID = MiscellaneousService.Catalog_GetDetailID(VARG_FacilityID, "DataChartsAreas", AreaValueID, out oError);
                var ChartsOfSelectedArea = GenericChartService.List(ChartAreaID, BaseGenericRequest);

                List<GenericChartsFilters> GenericChartFiltersList = new List<GenericChartsFilters>();
                List<GenericChartsAxis> GenericChartsAxisList = new List<GenericChartsAxis>();
                List<GenericChartFilterData> GenericChartFilterDataServiceList = new List<GenericChartFilterData>();
                List<GenericChartData> GenericChartDataList = new List<GenericChartData>();
                List<GenericChartHeaderData> GenericChartHeaderDataIDs = new List<GenericChartHeaderData>();

                foreach (var chart in ChartsOfSelectedArea)
                {
                    int GenericChartID = Convert.ToInt32(chart.GenericChartID);
                    GenericChartFiltersList.AddRange(GenericChartsFiltersService.List(GenericChartID, BaseGenericRequest));
                    GenericChartsAxisList.AddRange(GenericChartService.GetAxes_List(GenericChartID, BaseGenericRequest));
                    //GenericChartDataList.AddRange(GenericChartDataService.List(GenericChartID, null, BaseGenericRequest));
                }

                model.GenericChartHeaderDataID = GenericChartHeaderDataIDs;
                model.GenericChartsList = ChartsOfSelectedArea;
                model.GenericChartFiltersList = GenericChartFiltersList;
                model.GenericChartsAxisList = GenericChartsAxisList;
                model.GenericChartFilterDataList = new SelectList(GenericChartFilterDataServiceList, "GenericChartFilterID", "FilterValue");
                model.GenericChartDataList = GenericChartDataList;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        //DASHBOARD: Esta funcion filtra los datos las gráficas dinamicas
        public JsonResult GetFilteredData(int GenericChartID, string FilterData)
        {
            //DataChartsInfoViewModel model = new DataChartsInfoViewModel();
            List<GenericChartData> listData = new List<GenericChartData>();
            List<GenericChartsAxis> listAxis = new List<GenericChartsAxis>();
            GenericReturn result = new GenericReturn();

            try
            {
                listData = GenericChartDataService.List(GenericChartID, FilterData, BaseGenericRequest);
                listAxis = GenericChartService.GetAxes_List(GenericChartID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                listData,
                listAxis,
            }, JsonRequestBehavior.AllowGet);
        }

    }
}