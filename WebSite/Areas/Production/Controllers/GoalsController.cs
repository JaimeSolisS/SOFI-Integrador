using Core.Entities;
using Core.Entities.Production;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.Production.Models.Goals;
using WebSite.Models;

namespace WebSite.Areas.Production.Controllers
{
    public class GoalsController : BaseController
    {
        // GET: Production/Goals
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

        #region CRUD
        [HttpPost]
        public ActionResult ProductionProcessApplyLines(int CatalogDetailID)
        {
            var result = MiscellaneousService.ProductionProcess_ApplyLines(CatalogDetailID, false);
            var response = new { ApplyLines = result };
            return Json(response);
        }

        #endregion

        #region Methods
        private IndexViewModel InitializeControls()
        {
            IndexViewModel result = new IndexViewModel();
            ProductionGoal ProductionGoalInfo = new ProductionGoal();
            Catalog ProductionProcessInfo = null;
            bool ApplyLines = false;

            result.ProductionGoalsList = ProductionGoalService.List(ProductionGoalInfo, BaseGenericRequest);

            /*Production Goal*/
            var productionGoals = vw_CatalogService.List4Select("ProductionGoals", BaseGenericRequest, false);
            productionGoals.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.CatalogGoalList = new SelectList(productionGoals, "CatalogDetailID", "DisplayText", 0);


            /*Production Process*/
            List<ProductionLine> productionLineList = null;
            var productionList = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
            productionList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.ProductionProcessList = new SelectList(productionList, "CatalogDetailID", "DisplayText");

            /*Production Line*/
            if (productionList.Count > 0)
            {
                ProductionProcessInfo = productionList.FirstOrDefault();

                ApplyLines = MiscellaneousService.ProductionProcess_ApplyLines(ProductionProcessInfo.CatalogDetailID, false);
                if (ApplyLines)
                {
                    result.ClassProcessLine = string.Empty;
                    productionLineList = ProductionLineService.List(new ProductionLine { Enabled = true }, BaseGenericRequest);
                    productionLineList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
                    result.UserProcessesLine = new SelectList(productionLineList, "ProductionLineID", "LineNumber", 0);
                }
            }

            /*VA*/
            var VAList = vw_CatalogService.List4Select("ProductionVA", BaseGenericRequest, false);
            VAList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.VAList = new SelectList(VAList, "CatalogDetailID", "DisplayText");

            /*Design*/
            var DesignList = vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, false);
            DesignList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            result.DesignList = new SelectList(DesignList, "CatalogDetailID", "DisplayText");

            /*Shift*/
            var ShiftList = ShiftService.List(null, true, BaseGenericRequest);
            ShiftList.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
            result.ShiftList = new SelectList(ShiftList, "ShiftID", "ShiftDescription");

            return result;
        }

        public ActionResult ProductionGoals_List(int ProductionProcessID, int ProductionLineID, int VAID, int DesignID, int ShiftID, int CatalogGoalID)
        {
            List<ProductionGoal> model;
            ProductionGoal entity = new ProductionGoal();

            if (ProductionProcessID > 0) { entity.ProductionProcessID = ProductionProcessID; }
            if (ProductionLineID > 0) { entity.ProductionLineID = ProductionLineID; }
            if (VAID > 0) { entity.VAID = VAID; }
            if (DesignID > 0) { entity.DesignID = DesignID; }
            if (ShiftID > 0) { entity.ShiftID = ShiftID; }
            if (CatalogGoalID > 0) { entity.GoalNameID = CatalogGoalID; }

            model = ProductionGoalService.List(entity, BaseGenericRequest);

            return PartialView("~/Areas/Production/Views/Goals/_ProductionGoalsMasterTable.cshtml", model);
        }

        public ActionResult ProductionGoal_LoadNewGoalInfo()
        {
            IndexViewModel model = new IndexViewModel();
            model = InitializeControls();
            var list = vw_CatalogService.List4Select("ProductionGoals", BaseGenericRequest, true);
            model.GoalsNameList = new SelectList(list, "CatalogDetailID", "DisplayText");
            model.ModalGoalHeader = Resources.ProductionGoal.lbl_AD_ViewHeaderTag;
            model.ModalGoalAcceptButton = Resources.Common.btn_Accept;
            model.ProductionGoalInfo = new ProductionGoal();
            model.ProductionGoalInfo.GoalID = 0;
            return PartialView("~/Areas/Production/Views/Goals/_Mo_NewProductionGoalMaster.cshtml", model);
        }

        public ActionResult ProductionGoalDetail_List(ProductionGoal entity)
        {
            TableDetailViewModel model = new TableDetailViewModel();
            if (entity.ProductionProcessID == 0)
                entity.ProductionProcessID = null;
            if (entity.ProductionLineID == 0)
                entity.ProductionLineID = null;
            if (entity.VAID == 0)
                entity.VAID = null;
            if (entity.DesignID == 0)
                entity.DesignID = null;
            if (entity.ShiftID == 0)
                entity.ShiftID = null;
            model.ProductionGoalsDetails = ProductionGoalDetailService.List(entity, BaseGenericRequest);
            model.TableID = "tbl_ProductionGoalDetails_" + entity.GoalID.ToString();
            model.GoalID = entity.GoalID.ToInt();
         
            return PartialView("~/Areas/Production/Views/Goals/_ProductionGoalsDetailTable.cshtml", model);
        }

        public ActionResult ProductionGoal_Edit(int GoalID)
        {
            IndexViewModel model = new IndexViewModel();

            Catalog ProductionProcessInfo = null;
            bool ApplyLines = false;

            model.ProductionGoalInfo = ProductionGoalService.Find(new ProductionGoal { GoalID = GoalID }, BaseGenericRequest);

            /*Production Goal*/
            var productionGoals = vw_CatalogService.List4Select("ProductionGoals", BaseGenericRequest, false);
            productionGoals.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.CatalogGoalList = new SelectList(productionGoals, "CatalogDetailID", "DisplayText", model.ProductionGoalInfo.GoalNameID);



            /*Production Process*/
            List<ProductionLine> productionLineList = null;
            var productionList = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
            productionList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.ProductionProcessList = new SelectList(productionList, "CatalogDetailID", "DisplayText", model.ProductionGoalInfo.ProductionProcessID);

            /*Production Line*/
            if (productionList.Count > 0)
            {
                ProductionProcessInfo = productionList.Find(p => p.CatalogDetailID == model.ProductionGoalInfo.ProductionProcessID);

                ApplyLines = MiscellaneousService.ProductionProcess_ApplyLines(ProductionProcessInfo.CatalogDetailID, false);
                if (ApplyLines) { model.ClassProcessLine = string.Empty; }
            }
            /*Production Line*/
            productionLineList = ProductionLineService.List(new ProductionLine { Enabled = true }, BaseGenericRequest);
            productionLineList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
            model.UserProcessesLine = new SelectList(productionLineList, "ProductionLineID", "LineNumber", model.ProductionGoalInfo.ProductionLineID);

            /*VA*/
            var VAList = vw_CatalogService.List4Select("ProductionVA", BaseGenericRequest, false);
            VAList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.VAList = new SelectList(VAList, "CatalogDetailID", "DisplayText", model.ProductionGoalInfo.VAID);

            /*Design*/
            var DesignList = vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest, false);
            DesignList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });
            model.DesignList = new SelectList(DesignList, "CatalogDetailID", "DisplayText", model.ProductionGoalInfo.DesignID);

            /*Shift*/
            var ShiftList = ShiftService.List(null, true, BaseGenericRequest);
            ShiftList.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
            model.ShiftList = new SelectList(ShiftList, "ShiftID", "ShiftDescription", model.ProductionGoalInfo.ShiftID);

            model.ModalGoalHeader = Resources.ProductionGoal.lbl_AD_EditViewHeaderTag;
            model.ModalGoalAcceptButton = Resources.ProductionGoal.btn_AD_UpdateGoal;

            return PartialView("~/Areas/Production/Views/Goals/_Mo_NewProductionGoalMaster.cshtml", model);
        }

        [HttpPost]
        public ActionResult ProductionGoal_Add(ProductionGoal entity)
        {
            try
            {
                var response = ProductionGoalService.Add(entity, BaseGenericRequest);

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
        public ActionResult ProductionGoal_Upsert(ProductionGoal entity)
        {
            try
            {
                var response = ProductionGoalService.Upsert(entity, BaseGenericRequest);

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
        public ActionResult ProductionGoal_Delete(int GoalID)
        {
            try
            {
                var response = ProductionGoalService.Delete(GoalID, BaseGenericRequest);

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
        public ActionResult ProductionGoalDetail_BulkUpsert(List<Core.Entities.SQL_DataType.t_ProductionGoalsDetail> list, int GoalID)
        {
            ProductionGoalsDetail entity = new ProductionGoalsDetail();
            try
            {
                entity.GoalID = GoalID;
                var response = ProductionGoalDetailService.BulkUpsert(list, entity, BaseGenericRequest);

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