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
    public class GenericChartsAdministrationController : BaseController
    {
        // GET: Administration/GenericChartsAdministration
        public ActionResult Index()
        {
            var model = new DataChartAdminViewModel();
            try
            {
                //Cargar listado de Áreas
                var ChartAreasList = vw_CatalogService.List("DataChartsAreas", BaseGenericRequest);
                ChartAreasList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
                model.ChartAreasList = new SelectList(ChartAreasList, "CatalogDetailID", "ValueID");
                model.GenericChartsList = GenericChartService.List(BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            return View(model);
        }

        public ActionResult New()
        {
            EditDataChartViewModel model = new EditDataChartViewModel();
            string viewPath = "~/Areas/Administration/Views/GenericChartsAdministration/Edit.cshtml";

            try
            {
                //Cargar listado de Áreas
                var ChartAreasList = vw_CatalogService.List("DataChartsAreas", BaseGenericRequest);
                ChartAreasList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.ChartAreasList = new SelectList(ChartAreasList, "CatalogDetailID", "DisplayText");

                //Cargar listado de Tipos de gráficas
                var ChartTypesList = vw_CatalogService.List("DataChartsTypes", BaseGenericRequest);
                ChartTypesList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.ChartTypesList = new SelectList(ChartTypesList, "CatalogDetailID", "DisplayText");
                model.NewEditTitle = Resources.GenericCharts.title_NewChart;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);
        }

        public ActionResult Edit(int GenericChartID)
        {
            EditDataChartViewModel model = new EditDataChartViewModel();

            try
            {
                var GenericChartListInfo = GenericChartService.List(GenericChartID, null, BaseGenericRequest);
                model.GenericChartID = GenericChartID;
                model.GenericChartsList = GenericChartListInfo;
                model.ChartName = GenericChartListInfo.FirstOrDefault().ChartName;
                model.ChartTitle = GenericChartListInfo.FirstOrDefault().ChartTitle;
                model.Enabled = GenericChartListInfo.FirstOrDefault().Enabled;
                model.GenericChartsFiltersList = GenericChartsFiltersService.List(GenericChartID, BaseGenericRequest);
                model.GenericChartsAxisList = GenericChartService.GetAxes_List(GenericChartID, BaseGenericRequest);
                model.SelectedAreaID = GenericChartListInfo.FirstOrDefault().ChartAreaID;
                model.SelectedChartTypeID = GenericChartListInfo.FirstOrDefault().ChartTypeID;

                //Cargar listado de Áreas
                var ChartAreasList = vw_CatalogService.List("DataChartsAreas", BaseGenericRequest);
                ChartAreasList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.ChartAreasList = new SelectList(ChartAreasList, "CatalogDetailID", "DisplayText");

                //Cargar listado de Tipos de gráficas
                var ChartTypesList = vw_CatalogService.List("DataChartsTypes", BaseGenericRequest);
                ChartTypesList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.ChartTypesList = new SelectList(ChartTypesList, "CatalogDetailID", "DisplayText");

                model.NewEditTitle = Resources.GenericCharts.title_EditChart;
                model.GenericChartHeaderDataID = GenericChartDataService.GET_GenericChartHeaderDataID(GenericChartID, model.GenericChartsFiltersList.FirstOrDefault().GenericChartFilterID, BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            return View(model);
        }

        public ActionResult Search(int? ChartAreaID, int? GenericChartID)
        {
            List<GenericChart> model = new List<GenericChart>();
            var viewPath = "~/Areas/Administration/Views/GenericChartsAdministration/_Tbl_GenericCharts.cshtml";

            try
            {
                model = GenericChartService.List(ChartAreaID, GenericChartID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public JsonResult GetModalNewEditFilter(bool IsEdit)
        {
            GenericReturn result = new GenericReturn();
            NewEditDataChartFilterViewModel model = new NewEditDataChartFilterViewModel();
            string ViewPath = "~/Areas/Administration/Views/GenericChartsAdministration/_Mo_NewEditFilter.cshtml";

            try
            {
                //Cargar listado de tipos de filtros
                var FilterTypes_List = vw_CatalogService.List("DataChartsFiltersDataTypes", BaseGenericRequest);
                FilterTypes_List.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.FilterTypes_List = new SelectList(FilterTypes_List, "CatalogDetailID", "DisplayText");

                //Cargar listado de tipos de valores default
                var DefaultValue_List = vw_CatalogService.List("DataChartsFiltersDefaultValues", BaseGenericRequest);
                //DefaultValue_List.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.DefaultValue_List = DefaultValue_List;// new SelectList(DefaultValue_List, "CatalogDetailID", "DisplayText");

                var ListsForFilterTypes_List = vw_CatalogService.List("DataChartsCatalogsForFilters", BaseGenericRequest);
                //ListsForFilterTypes_List.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.noneSelected });
                model.ListsForFilterTypes_List = new SelectList(ListsForFilterTypes_List, "CatalogDetailID", "DisplayText");

                //model.ListsForFilterTypes_List = new SelectList(vw_CatalogService.List("DataChartsCatalogsForFilters", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                model.FiltersFormulas_List = new SelectList(vw_CatalogService.List("", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                model.FiltersDefault_List = new SelectList(vw_CatalogService.List("", BaseGenericRequest), "CatalogDetailID", "DisplayText");




                if (IsEdit)
                {
                    model.IsEdit = true;
                    model.NewEditTitle = Resources.GenericCharts.title_EditFilter;
                }
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


        public ActionResult GetModalNewEditAxis()
        {
            NewEditDataChartAxisViewModel model = new NewEditDataChartAxisViewModel();
            string ViewPath = "~/Areas/Administration/Views/GenericChartsAdministration/_Mo_NewEditAxis.cshtml";
            try
            {
                //Cargar listado de categorias
                model.AxisCategories_List = new SelectList(vw_CatalogService.List4Select("DataChartsAxes", BaseGenericRequest, true, Resources.Common.noneSelected), "CatalogDetailID", "DisplayText");
                //Cargar listado de tipos de graficas                
                model.DataChartsTypes_List = new SelectList(vw_CatalogService.List4Select("DataChartsTypes", BaseGenericRequest, true, Resources.Common.noneSelected), "CatalogDetailID", "DisplayText");
                //Cargar listado de tipos de datos                
                model.DataTypes_List = new SelectList(vw_CatalogService.List4Select("DataChartAxisDataTypes", BaseGenericRequest, true, Resources.Common.noneSelected), "CatalogDetailID", "DisplayText");
                //Cargar listado de formatos
                model.DataFormat_List = vw_CatalogService.List4Select("DataFormat", BaseGenericRequest, true, Resources.Common.noneSelected);

                //if (IsEdit)
                //{
                //    model.IsEdit = true;
                //    model.NewEditTitle = Resources.GenericCharts.title_EditAxis;
                //}
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public JsonResult InsertChart(int? GenericChartHeaderDataID, int? GenericChartID, List<GenericChart> GenericChartDataEntities, List<GenericChartsFilters> GenericChartsFilterEntities, List<GenericChartsAxis> GenericChartsAxisEntities)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                if (GenericChartsFilterEntities == null)
                {
                    result.ErrorCode = 99;
                    result.ErrorMessage = Resources.GenericCharts.msg_ChartFiltersRequired;
                }
                else if (GenericChartsAxisEntities == null)
                {
                    result.ErrorCode = 99;
                    result.ErrorMessage = Resources.GenericCharts.msg_ChartAxesRequired;
                }
                else
                {
                    result = GenericChartService.Insert(GenericChartHeaderDataID, GenericChartID, GenericChartDataEntities, GenericChartsFilterEntities, GenericChartsAxisEntities, BaseGenericRequest);
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
                result.ID
            }, JsonRequestBehavior.AllowGet);

        }

    }
}