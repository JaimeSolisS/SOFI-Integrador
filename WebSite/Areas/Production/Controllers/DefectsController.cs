using Core.Entities;
using Core.Entities.Production;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.Production.Models.Defects;
using WebSite.Models;

namespace WebSite.Areas.Production.Controllers
{
    public class DefectsController : BaseController
    {
        #region CRUD
        // GET: Production/Defects
        public ActionResult Index()
        {
            IndexViewModel Model = new IndexViewModel();
            try
            {
                Model = InitializeControls();
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(Model);
            }
            return View(Model);
        }

        #endregion

        #region Methods

        private IndexViewModel InitializeControls()
        {
            IndexViewModel result = new IndexViewModel();
            Defect DefectInfo = new Defect();
            Catalog ProductionProcessInfo = null;
            bool ApplyLines = false;
            DefectInfo.DefectID = null;
            DefectInfo.Enabled = true;

            result.DefectsList = DefectService.List(DefectInfo, new DefectProcess(), BaseGenericRequest, false);
            var defectlist = new List<Defect>();
            defectlist.Insert(0, new Defect() { DefectID = 0, DefectName = "" });
            defectlist.AddRange(result.DefectsList);
            result.DefectsListCatalog = new SelectList(defectlist, "DefectID", "DefectName");

            /*Production Process*/
            List<ProductionLine> productionLineList = null;
            var productionList = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
            productionList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.ProductionProcessList = new SelectList(productionList, "CatalogDetailID", "DisplayText", 0);

            /*Production Line*/
            if (productionList.Count > 0)
            {
                ProductionProcessInfo = productionList.FirstOrDefault();

                ApplyLines = MiscellaneousService.ProductionProcess_ApplyLines(ProductionProcessInfo.CatalogDetailID, false);
                if (ApplyLines)
                {
                    result.ClassProcessLine = string.Empty;
                    
                }
            }

            productionLineList = ProductionLineService.List(new ProductionLine { Enabled = true }, BaseGenericRequest);
            productionLineList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
            result.UserProcessesLine = new SelectList(productionLineList, "ProductionLineID", "LineNumber", 0);
            /*VA*/
            var VAList = vw_CatalogService.List4Select("ProductionVA", BaseGenericRequest, false);
            VAList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.VAList = new SelectList(VAList, "CatalogDetailID", "DisplayText",0);

            /*Design*/
            var DesignList = vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, false);
            DesignList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.DesignList = new SelectList(DesignList, "CatalogDetailID", "DisplayText", 0);

            /*Shift*/
            var ShiftList = ShiftService.List(null, true, BaseGenericRequest);
            ShiftList.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
            result.ShiftList = new SelectList(ShiftList, "ShiftID", "ShiftDescription", 0);
            return result;
        }

        public ActionResult DefectInfo_List(int ProductionProcessID, int ProductionLineID, int VAID, int DesignID, int ShiftID)
        {
            List<Defect> model;
            Defect defect = new Defect();
            DefectProcess defectProcess = new DefectProcess();

            if (ProductionProcessID > 0) { defectProcess.ProductionProcessID = ProductionProcessID; }
            if (ProductionLineID > 0) { defectProcess.ProductionLineID = ProductionLineID; }
            if (VAID > 0) { defectProcess.VAID = VAID; }
            if (DesignID > 0) { defectProcess.DesignID = DesignID; }
            if (ShiftID > 0) { defectProcess.ShiftID = ShiftID; }


            defect.Enabled = true;

            model = DefectService.List(defect, defectProcess, BaseGenericRequest, false);

            return PartialView("~/Areas/Production/Views/Defects/_DefectTable.cshtml", model);
        }

        public ActionResult Defect_AddDetails(DefectProcess entity)
        {
            IndexViewModel model = new IndexViewModel();
            model = InitializeControls();
            model.DefectProcessInfo.DefectProcessID = 0;
            model.ModalHeader_AddDetailTag = Resources.Defects.lbl_AD_ViewHeaderTag;
            model.Modal_SaveButtonTag = Resources.Defects.lbl_AD_AddDefectDetailsTag;
            return PartialView("~/Areas/Production/Views/Defects/_Mo_AddDefectDetail.cshtml", model);
        }

        public ActionResult Defect_EditDetails(DefectProcess entity)
        {
            IndexViewModel model = new IndexViewModel();
            Defect DefectInfo = new Defect();
            Catalog ProductionProcessInfo = null;
            bool ApplyLines = false;
            DefectInfo.DefectID = null;
            DefectInfo.Enabled = true;

            entity.ProductionProcessID = entity.ProductionProcessID.SelectedValue();
            entity.ProductionLineID = entity.ProductionLineID.SelectedValue();
            entity.VAID = entity.VAID.SelectedValue();
            entity.DesignID = entity.DesignID.SelectedValue();
            entity.ShiftID = entity.ShiftID.SelectedValue();
            entity.DefectProcessID = entity.DefectProcessID.SelectedValue();

            model.DefectProcessInfo = DefectProcessService.Find(entity, BaseGenericRequest);

            var defectlist = DefectService.List(DefectInfo, new DefectProcess(), BaseGenericRequest, false);
            defectlist.Insert(0, new Defect() { DefectID = 0, DefectName = "" });
            defectlist.AddRange(model.DefectsList);
            model.DefectsListCatalog = new SelectList(defectlist, "DefectID", "DefectName", model.DefectProcessInfo.DefectID);

            /*Production Process*/
            List<ProductionLine> productionLineList = null;
            var productionList = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
            productionList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.ProductionProcessList = new SelectList(productionList, "CatalogDetailID", "DisplayText", model.DefectProcessInfo.ProductionProcessID);

            /*Production Line*/
            if (productionList.Count > 0)
            {
                ProductionProcessInfo = productionList.Find(p=>p.CatalogDetailID == model.DefectProcessInfo.ProductionProcessID);

                ApplyLines = MiscellaneousService.ProductionProcess_ApplyLines(ProductionProcessInfo.CatalogDetailID, false);
                if (ApplyLines) { model.ClassProcessLine = string.Empty; }
            }
            /*Production Line*/
            productionLineList = ProductionLineService.List(new ProductionLine { Enabled = true }, BaseGenericRequest);
            productionLineList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
            model.UserProcessesLine = new SelectList(productionLineList, "ProductionLineID", "LineNumber", model.DefectProcessInfo.ProductionLineID);

            /*VA*/
            var VAList = vw_CatalogService.List4Select("ProductionVA", BaseGenericRequest, false);
            VAList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.VAList = new SelectList(VAList, "CatalogDetailID", "DisplayText", model.DefectProcessInfo.VAID);

            /*Design*/
            var DesignList = vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, false);
            DesignList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.DesignList = new SelectList(DesignList, "CatalogDetailID", "DisplayText", model.DefectProcessInfo.DesignID);

            /*Shift*/
            var ShiftList = ShiftService.List(null, true, BaseGenericRequest);
            ShiftList.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
            model.ShiftList = new SelectList(ShiftList, "ShiftID", "ShiftDescription", model.DefectProcessInfo.ShiftID);

            model.ModalHeader_AddDetailTag = Resources.Defects.lbl_AD_EditViewHeaderTag;
            model.Modal_SaveButtonTag = Resources.Defects.lbl_EditDefectDetailsTag;


            return PartialView("~/Areas/Production/Views/Defects/_Mo_AddDefectDetail.cshtml", model);
        }

        public ActionResult DefectProcessInfo_List(DefectProcess defect)
        {
            defect.ProductionProcessID = defect.ProductionProcessID.SelectedValue();
            defect.ProductionLineID = defect.ProductionLineID.SelectedValue();
            defect.VAID = defect.VAID.SelectedValue();
            defect.DesignID = defect.DesignID.SelectedValue();
            defect.ShiftID = defect.ShiftID.SelectedValue();
            defect.DefectProcessID = defect.DefectProcessID.SelectedValue();
            var model = DefectProcessService.List(defect, BaseGenericRequest);
            return PartialView("~/Areas/Production/Views/Defects/_DefectProcessTable.cshtml", model);
        }


        [HttpPost]
        public ActionResult ProductionProcessApplyLines(int CatalogDetailID, int ParamIndex)
        {
            var result = MiscellaneousService.ProductionProcess_ApplyLines(CatalogDetailID, false);
            var response = new { ApplyLines = result };
            return Json(response);
        }

        [HttpPost]
        public ActionResult DefectProcess_Add(DefectProcess process)
        {
            try
            {
                var response = DefectProcessService.Upsert(process, BaseGenericRequest);

                return Json(new
                {
                    response.ErrorCode,
                    response.ErrorMessage,
                    notifyType = response.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = ex.Message,
                    notifyType = StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DefectProcess_Delete(int DefectProcessID)
        {
            try
            {
                var response = DefectProcessService.Delete(DefectProcessID, BaseGenericRequest);

                return Json(new
                {
                    response.ErrorCode,
                    response.ErrorMessage,
                    notifyType = response.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = ex.Message,
                    notifyType = StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult DefectProcess_DeleteAllDetails(int DefectID)
        {
            try
            {
                var response = DefectProcessService.DeleteAllDetail(DefectID, BaseGenericRequest);

                return Json(new
                {
                    response.ErrorCode,
                    response.ErrorMessage,
                    notifyType = response.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = ex.Message,
                    notifyType = StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}