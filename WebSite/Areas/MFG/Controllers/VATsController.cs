using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.VATs;
using WebSite.Models;

namespace WebSite.Areas.MFG.Controllers
{
    public class VATsController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            ProductionLine ProductionLine = new ProductionLine();
            try
            {
                //Cargar listado de lineas de los procesos
                var ProductionLinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });
                model.ProductionLinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber");

                //Cargar listado de turnos
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription");

                //Cargar listado de procesos
                model.ProductionProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");

                //Cargar listado de VATs
                model.ProductionVATsList = MFG_ProductionVATsService.List(BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            return View(model);
        }

        public JsonResult Search(string VATName, int[] ShiftIDs, int? ProductionProcessID, int[] ProductionLineIDs)
        {
            List<ProductionVAT> list = new List<ProductionVAT>();
            string ViewPath = "~/Areas/MFG/Views/VATs/_Tbl_VATsRecords.cshtml";

            try
            {
                list = MFG_ProductionVATsService.List(VATName, ShiftIDs, ProductionProcessID, ProductionLineIDs, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(ViewPath, list)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New()
        {
            EditViewModel model = new EditViewModel();
            string viewPath = "~/Areas/MFG/Views/VATs/_Mo_NewEdit.cshtml";

            try
            {
                model.Title = Resources.MFG.VatsCatalog.title_NewVAT;
                model.ProductionVATObj.Enabled = true;

                //Cargar listado de lineas de los procesos
                var ProductionLinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.chsn_SelectOption });
                model.ProductionLinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber");


                //Cargar listado de turnos
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.chsn_SelectOption, true), "ShiftID", "ShiftDescription");

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(viewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int VATID)
        {
            EditViewModel model = new EditViewModel();
            string viewPath = "~/Areas/MFG/Views/VATs/_Mo_NewEdit.cshtml";

            try
            {
                model.Title = Resources.MFG.VatsCatalog.title_EditVAT;
                model.IsEdit = true;
                model.ProductionVATObj = MFG_ProductionVATsService.List(VATID, BaseGenericRequest).FirstOrDefault();

                //Cargar listado de lineas de los procesos
                var ProductionLinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.chsn_SelectOption });
                model.ProductionLinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber");

                //Listado de turnos
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll, true), "ShiftID", "ShiftDescription");


            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(viewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int VATID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MFG_ProductionVATsService.Delete(VATID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(int? VATID, string VATName, int? ShiftID, int? ProductionLineID, bool Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                if (VATID == null)
                    result = MFG_ProductionVATsService.Insert(VATName, ShiftID, ProductionLineID, Enabled, BaseGenericRequest);
                else
                    result = MFG_ProductionVATsService.Update(VATID, VATName, ShiftID, ProductionLineID, Enabled, BaseGenericRequest);
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

        public JsonResult GetProductionLinesList(int ProductionProcessID)
        {
            IEnumerable<SelectListItem> list = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                var ProductionLinesList = ProductionLineService.List(VARG_FacilityID, BaseGenericRequest);
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.TagAll });

                if (ProductionProcessID == 0)
                    list = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber");
                else
                    list = new SelectList(ProductionLinesList.Where(x => x.ProductionProcessID == ProductionProcessID || x.ProductionLineID == 0), "ProductionLineID", "LineNumber");


                if (list != null)
                {
                    if (list.Count() == 1)
                    {
                        var AuxList = new List<ProductionLine>();
                        AuxList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.noneSelected });
                        list = new SelectList(AuxList, "ProductionLineID", "LineNumber");
                    }

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
                list
            }, JsonRequestBehavior.AllowGet);
        }

    }
}