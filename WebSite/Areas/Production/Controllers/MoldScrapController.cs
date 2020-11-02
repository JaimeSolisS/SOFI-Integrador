using Core.Entities;
using Core.Entities.Production;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Areas.Production.Models.MoldScrap;
using WebSite.Models;

namespace WebSite.Areas.Production.Controllers
{
    public class MoldScrapController : BaseController
    {

        #region CRUD
        // GET: Production/MoldScrap
        public ActionResult Index()
        {
            IndexViewModel Model = new IndexViewModel();
            int ApplyLines = 0;
            try
            {
                var MoldScrapInfo = new MoldScraps();
                Catalog ProductionProcessInfo = null;
                List<UsersProcessLine> productionLineList = null;
                var productionList = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, false);
                Model.ProductionProcessList = new SelectList(productionList, "CatalogDetailID", "DisplayText");
                // Anexar fila vacia al principio

                Model.ScrapDateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

                if (productionList.Count > 0)
                {
                    ProductionProcessInfo = productionList.FirstOrDefault();

                    ApplyLines = MiscellaneousService.Param_GetValue(ProductionProcessInfo.CatalogDetailID, 1, "0").ToInt();
                    if (ApplyLines == 1)
                    {
                        Model.ClassProcessLine = string.Empty;
                        productionLineList = UsersProcessesLinesService.AccessList(ProductionProcessInfo.CatalogDetailID, BaseGenericRequest, false);
                        productionLineList.Insert(0, new UsersProcessLine() { ProductionLineID = 0, ProductionLineName = Resources.Common.TagAll });
                        Model.UserProcessesLine = new SelectList(productionLineList, "ProductionLineID", "ProductionLineName");
                    }
                }
                MoldScrapInfo.ScrapDate = DateTime.Now;
                if (ProductionProcessInfo != null)
                {
                    MoldScrapInfo.ProductionProcessID = ProductionProcessInfo.CatalogDetailID;
                }
                if(productionLineList != null && productionLineList.Count == 1)
                {
                    MoldScrapInfo.ProductionProcessID = productionLineList[0].ProductionLineID;
                }
                Model.MoldScrapsList = MoldScrapService.List(MoldScrapInfo, BaseGenericRequest);

                /*Shift*/
                var ShiftList = ShiftService.List(null, true, BaseGenericRequest);
                ShiftList.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = Resources.Common.TagAll });
                Model.ShiftList = new SelectList(ShiftList, "ShiftID", "ShiftDescription");

                Model.DesignList  = new SelectList(vw_CatalogService.List4Select("ProductionDesigns", BaseGenericRequest), "CatalogDetailID", "DisplayText");

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

        #region Get

        [HttpPost]
        public ActionResult ProductionProcessApplyLines(int CatalogDetailID, int ParamIndex)
        {
            var result = MiscellaneousService.ProductionProcess_ApplyLines(CatalogDetailID, false);
            var response = new { ApplyLines = result };
            return Json(response);
        }

        [HttpPost]
        public ActionResult UsersProcessesLines_GetAccessList(int CatalogDetailID)
        {
            List<UsersProcessLine> response;
            response = UsersProcessesLinesService.AccessList(CatalogDetailID, BaseGenericRequest, false);
            if (response == null)
            {
                response = new List<UsersProcessLine>();
            }
            else
            {
                response.Insert(0, new UsersProcessLine() { ProductionLineID = 0, ProductionLineName = Resources.Common.TagAll });
            }
            return Json(response);
        }

        public ActionResult MoldScrap_GetShiftList(DateTime ScrapDate, int ProductionProcessID, int ProductionLineID, int ShiftID, int DesignID)
        {
            List<MoldScraps> model;
            MoldScraps entity = new MoldScraps();
            entity.ScrapDate = ScrapDate;

            if (ProductionProcessID > 0)
            {
                entity.ProductionProcessID = ProductionProcessID;
            }
            if (ProductionLineID > 0)
            {
                entity.ProductionLineID = ProductionLineID;
            }
            if (ShiftID > 0)
            {
                entity.ShiftID = ShiftID;
            }
            if (DesignID > 0)
            {
                entity.DesignID = DesignID;
            }
            model = MoldScrapService.List(entity, BaseGenericRequest);

            return PartialView("~/Areas/Production/Views/MoldScrap/_MoldScrapTable.cshtml", model);

        }

        [HttpPost]
        public ActionResult MoldScrap_BulkUpsert(List<Core.Entities.SQL_DataType.t_MoldScrap> list, DateTime ScrapDate, int ProductionProcessID, int ProductionLineID, int ShiftID,int DesignID)
        {
            MoldScraps entity = new MoldScraps();
            try
            {
                entity.ScrapDate = ScrapDate;

                if (ProductionProcessID > 0)
                {
                    entity.ProductionProcessID = ProductionProcessID;
                }
                if (ProductionLineID > 0)
                {
                    entity.ProductionLineID = ProductionLineID;
                }
                if (ShiftID > 0)
                {
                    entity.ShiftID = ShiftID;
                }
                if (DesignID > 0)
                {
                    entity.DesignID = DesignID;
                }
                var response = MoldScrapService.BulkUpsert(list, entity, BaseGenericRequest);

                return Json(new
                {
                    response.ErrorCode,
                    response.ErrorMessage,
                    notifyType = response.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
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