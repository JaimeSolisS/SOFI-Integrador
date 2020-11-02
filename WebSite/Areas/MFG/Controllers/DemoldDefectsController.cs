using Core.Service;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.DemoldDefects;
using WebSite.Models;

namespace WebSite.Areas.MFG.Controllers
{
    public class DemoldDefectsController : BaseController
    {
        public ActionResult Index(int? ProductionProcessID, int? ProductionLineID, int? VATID, string InspectorName)
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                model.DayCode = OperationRecordService.GetJulianDay(DateTime.Now, "", BaseGenericRequest);
                var ShiftList = ShiftService.List4Select(BaseGenericRequest, Resources.Common.chsn_SelectOption, true);
                model.CurrentShiftID = DashboardService.GetCurrentShiftID(1, BaseGenericRequest);
                model.ShiftsList = new SelectList(ShiftList, "ShiftID", "ShiftDescription", model.CurrentShiftID);

                var ProductionLinesList = new List<ProductionLine>();
                ProductionLinesList.Insert(0, new ProductionLine() { ProductionLineID = 0, LineNumber = Resources.Common.chsn_SelectOption });
                model.LinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber");

                if (ProductionProcessID != null && ProductionProcessID != 0)
                {
                    model.ProductionProcessID = ProductionProcessID;
                    model.ProductionLineID = ProductionLineID;
                    model.VATID = VATID;
                    model.InspectorName = InspectorName;

                    ProductionLinesList = ProductionLineService.ListbyProdProcess(ProductionProcessID, BaseGenericRequest);
                    model.LinesList = new SelectList(ProductionLinesList, "ProductionLineID", "LineNumber", ProductionLineID);
                    var vats = MFG_ProductionVATsService.List(model.CurrentShiftID.ToString(), ProductionLineID.ToString(), BaseGenericRequest);
                    model.VATList = new SelectList(vats, "VATID", "VATName", VATID);
                }
                model.ProductionProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText", ProductionProcessID);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }

        public ActionResult CaptureWizard(int ShiftID, string ShiftName, int ProductionProcessID, int ProductionLineID, string ProductionLineName, string InspectorName, string VATID, string VATName)
        {
            CaptureWizardViewModel model = new CaptureWizardViewModel();
            GenericReturn result = new GenericReturn();
            string viewPath = "~/Areas/MFG/Views/DemoldDefects/CaptureWizard.cshtml";

            try
            {
                model.DemoldDefectEntity = new DemoldDefects()
                {
                    ShiftID = ShiftID,
                    ShiftName = ShiftName,
                    ProductionLineID = ProductionLineID,
                    ProductionLineName = ProductionLineName,
                    InspectorName = InspectorName,
                    VATID = VATID,
                    VATName = VATName,
                    ProductionProcessID = ProductionProcessID
                };

                //model.ExceptionMessage = DemoldDefectsService.ValidateToNextFase(0, "", "", "", "", VATName, ProductionProcessID, ProductionLineName, BaseGenericRequest).ErrorMessage;
                model.FamiliesList = vw_CatalogService.List("LensMoldFamilies", BaseGenericRequest).OrderBy(p => Convert.ToInt32(p.Param1)).ToList();
                //model.FamiliesList = DemoldDefectsService.GetMoldFamiliesByFacility(VATName, ProductionProcessID, ProductionLineName, BaseGenericRequest);
                model.BaseCategoriesList = DemoldDefectsService.GetCategoriesAndData("DemoldDefectBases", null, BaseGenericRequest);
                model.AditionsCategoriesList = DemoldDefectsService.GetCategoriesAndData("DemoldDefectAditions", null, BaseGenericRequest);
                model.DefectTypes = DemoldDefectsService.GetCategoriesAndData("DemoldDefectsTypes", null, BaseGenericRequest);
                model.SidesList = vw_CatalogService.List("DemoldDefectSides", BaseGenericRequest);
                model.DayCode = OperationRecordService.GetJulianDay(DateTime.Now, "", BaseGenericRequest);
                model.DefectsList = DefectService.SelectList(null, null, null, true, BaseGenericRequest);
                //model.DemoldDefectDetailsList = DemoldDefectDetailsService.List(BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);
        }

        public ActionResult ReloadCaptureWizard(int DemoldDefectID)
        {
            CaptureWizardViewModel model = new CaptureWizardViewModel();
            //GenericReturn result = new GenericReturn();
            string viewPath = "~/Areas/MFG/Views/DemoldDefects/CaptureWizard.cshtml";

            try
            {
                model.DemoldDefectEntity = DemoldDefectsService.List(DemoldDefectID, BaseGenericRequest).FirstOrDefault();

                model.FamiliesList = vw_CatalogService.List("LensMoldFamilies", BaseGenericRequest).OrderBy(p => Convert.ToInt32(p.Param1)).ToList();
                model.BaseCategoriesList = DemoldDefectsService.GetCategoriesAndData("DemoldDefectBases", null, BaseGenericRequest);
                model.AditionsCategoriesList = DemoldDefectsService.GetCategoriesAndData("DemoldDefectAditions", null, BaseGenericRequest);
                model.DefectTypes = DemoldDefectsService.GetCategoriesAndData("DemoldDefectsTypes", null, BaseGenericRequest);
                model.SidesList = vw_CatalogService.List("DemoldDefectSides", BaseGenericRequest);
                model.DayCode = OperationRecordService.GetJulianDay(DateTime.Now, "", BaseGenericRequest);
                model.DefectsList = DefectService.SelectList(null, null, null, true, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);
        }

        public JsonResult FillDefectTable(int? ProductionLineID, int? VATID, string InspectorName, int ProductID)
        {
            List<DemoldDefectDetail> list = new List<DemoldDefectDetail>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = DemoldDefectDetailsService.ListByProduct(ProductionLineID, VATID, InspectorName, ProductID, null, BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVATList(int[] ShiftIDs, int[] ProductionLineIDs)
        {
            List<ProductionVAT> list = new List<ProductionVAT>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = MFG_ProductionVATsService.List(ShiftIDs, ProductionLineIDs, BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLensTypesList(int MoldFamilyID, int?[] MoldLensTypeIDs)
        {
            List<LenType> list = new List<LenType>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = MoldFamilyLensTypesService.ListOfLens(MoldFamilyID, MoldLensTypeIDs, BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLinesList(int ProductionProcessID)
        {
            List<ProductionLine> list = new List<ProductionLine>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = ProductionLineService.ListbyProdProcess(ProductionProcessID, BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllFamiliesList()
        {
            List<Catalog> list = new List<Catalog>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = vw_CatalogService.List("LensMoldFamilies", BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionFamiliesList(string VATName, int ProductionProcessID, string ProductionLineName)
        {
            List<Catalog> list = new List<Catalog>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = DemoldDefectsService.GetMoldFamiliesByFacility(VATName, ProductionProcessID, ProductionLineName, BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckProduductList()
        {
            GenericReturn result = new GenericReturn();
            string Message = "";

            try
            {

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
                Message
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataOfCategory(string CatalogTag, string Category)
        {
            List<Catalog> list = new List<Catalog>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = DemoldDefectsService.GetCategoriesAndData(CatalogTag, Category, BaseGenericRequest);

                if (list != null)
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateToNextFase(int? CaseControl, string LenType, string Base, string Addition, string Side, string VAT, int ProductionProcessID, string Line)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = DemoldDefectsService.ValidateToNextFase(CaseControl, LenType, Base, Addition, Side, VAT, ProductionProcessID, Line, BaseGenericRequest);
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
        public JsonResult Save(DemoldDefects DemoldDefectEntity, List<DemoldDefectDetail> DefectMoldDetailsEntities, DateTime? DefectDate, int ProductID, int LensGross)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = DemoldDefectsService.Insert(DemoldDefectEntity, DefectMoldDetailsEntities, DefectDate, ProductID, LensGross, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult xEditableVATList(int[] ShiftID, int[] ProductionLineID)
        {
            var list = MFG_ProductionVATsService.List(BaseGenericRequest).OrderBy(v => v.LineNumber);
            return Json(list.Select(s => new { value = s.VATID, text = s.VATName }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult xEditableProductionLinesList(int ProductionProcessID)
        {
            var list = ProductionLineService.ListbyProdProcess(ProductionProcessID, BaseGenericRequest);
            return Json(list.Select(s => new { value = s.ProductionLineID, text = s.LineNumber }), JsonRequestBehavior.AllowGet);
        }
    }
}