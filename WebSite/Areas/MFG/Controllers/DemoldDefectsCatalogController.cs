using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.DemoldDefectsCatalog;
using WebSite.Models;

namespace WebSite.Areas.MFG.Controllers
{
    public class DemoldDefectsCatalogController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var VATList = MFG_ProductionVATsService.List(BaseGenericRequest);
                VATList.Insert(0, new ProductionVAT() { VATID = 0, VATName = Resources.Common.TagAll });
                model.VATList = new SelectList(VATList, "VATID", "VatName");

                var LinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                LinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
                model.LinesList = new SelectList(LinesList, "ProductionLineID", "LineNumber");

                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription");

                model.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
                model.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
                model.DemoldDefectDetailList = DemoldDefectDetailsService.List(null, null, null, null, DateTime.Now, DateTime.Now, null, null,null, BaseGenericRequest);

                var defects = DemoldDefectsService.GetCategoriesAndData("DemoldDefectsTypes", null, BaseGenericRequest);
                defects.Insert(0, new Catalog() {  Param1 = Resources.Common.TagAll });
                model.DefectsList= new SelectList(defects, "Param1", "Param1", Resources.Common.TagAll);

                var designs = vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, true, Resources.Common.TagAll);
                model.DesignList = new SelectList(designs, "CatalogDetailID", "DisplayText", 0);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int DemoldDefectDetailID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = DemoldDefectDetailsService.Delete(DemoldDefectDetailID, BaseGenericRequest);
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

        public JsonResult Search(int? ShiftID, int? ProductionLineID, int? VATID, string InspectorName,
            DateTime? StartDate, DateTime? EndDate,string DefectCat,int? DefectType,int? DesignID)
        {
            List<DemoldDefectDetail> list = new List<DemoldDefectDetail>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/DemoldDefectsCatalog/_Tbl_DemoldDefectDetails.cshtml";

            try
            {
                DefectCat = DefectCat == Resources.Common.TagAll ? null : DefectCat;
                list = DemoldDefectDetailsService.List(ShiftID, ProductionLineID, VATID, InspectorName, StartDate, EndDate, DefectCat, DefectType, DesignID, BaseGenericRequest);
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
                View = RenderRazorViewToString(ViewPath, list)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(int? DemoldDefectDetailID, int? DemoldDefectID, int? ProductID, int? MoldFamilyID, int? BaseID, int? AdditionID, int? SideEyeID, int? Quantity, int? DemoldDefectTypeID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = DemoldDefectDetailsService.Update(DemoldDefectDetailID, DemoldDefectID, ProductID, MoldFamilyID, BaseID, AdditionID, SideEyeID, Quantity, DemoldDefectTypeID, BaseGenericRequest);
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

        public ActionResult DemoldDefectsAlertsExportToExcel(int? ddl_Shifts, int? ddl_ProductionLines, int? VatNumber, string txt_Inspector,
            DateTime? txt_StartDateExcel, DateTime? txt_EndDateExcel, string ddl_DefectCat, int? ddl_DefectType,int? ddl_design)
        {
            DataSet ds = new DataSet();
            ddl_DefectCat = ddl_DefectCat == Resources.Common.TagAll ? null : ddl_DefectCat;        
            ds = DemoldDefectDetailsService.DemoldDefectsExportToExcel(ddl_Shifts, ddl_ProductionLines, VatNumber, txt_Inspector,
                txt_StartDateExcel, txt_EndDateExcel, ddl_DefectCat, ddl_DefectType, ddl_design, BaseGenericRequest);
            return this.ExcelExport(ds, Resources.MFG.DemoldDefectsCatalog.title_DemoldDefectsExcelReport);
        }

        public JsonResult GetModalAlerts()
        {
            AlertsViewModel model = new AlertsViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/DemoldDefectsCatalog/_Mo_Alerts.cshtml";
            try
            {
                var LinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                LinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
                LinesList.Insert(0, new ProductionLine() { ProductionLineID = null, LineNumber = Resources.Common.chsn_SelectOption });
                model.LinesList = new SelectList(LinesList, "ProductionLineID", "LineNumber");

                var DefectsCategoriesList = DemoldDefectsService.GetCategoriesAndData("DemoldDefectsTypes", null, BaseGenericRequest);
                DefectsCategoriesList.Insert(0, new Catalog() { CatalogDetailID = 0, Param1 = Resources.Common.TagAll });
                model.DefectsCategoriesList = new SelectList(DefectsCategoriesList, "CatalogDetailID", "Param1");

                model.FamiliesList = new SelectList(vw_CatalogService.List4Select("LensMoldFamilies", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription");
                model.AlertsList = DemoldDefectAlertsService.List(BaseGenericRequest);

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

        [HttpPost]
        public JsonResult AlertsUpsert(List<DemoldDefectAlert> DemoldDefectAlerts)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = DemoldDefectAlertsService.Upsert(DemoldDefectAlerts, BaseGenericRequest);
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
    }
}