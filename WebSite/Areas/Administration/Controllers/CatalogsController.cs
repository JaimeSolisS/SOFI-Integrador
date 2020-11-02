using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Entities;
using WebSite.Areas.Administration.Models.ViewModels.Catalogs;
using Core.Service;
using Core.Entities.Utilities;
using WebSite.Models;

namespace WebSite.Areas.Administration.Controllers
{
    public class CatalogsController : BaseController
    {
        #region Index
        // GET: Administration/Catalogs
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            try
            {
                var EventID = MiscellaneousService.Param_GetValue(VARG_FacilityID, "Event_AddCatalog", "0").ToInt();
                model.AllowCreate = MiscellaneousService.Event_ValidUser(EventID, VARG_UserID);
                EventID = MiscellaneousService.Param_GetValue(VARG_FacilityID, "Event_CatalogsEdit", "0").ToInt();
                model.AllowEdit = MiscellaneousService.Event_ValidUser(EventID, VARG_UserID);
                // Listados para los combos
                model.OrganizationsList = new SelectList(OrganizationService.List4Config(BaseGenericRequest), "OrganizationID", "OrganizationName");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult Search(int CatalogID)
        {
            Catalog model = new Catalog();
            string ViewPath = "~/Areas/Administration/Views/Catalogs/CatalogsDetailTable.cshtml";
            try
            {
                // Listados para los combos
                model = vw_CatalogService.Get(CatalogID, BaseGenericRequest);
            }
            catch (Exception )
            {

                throw;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }

        //public ActionResult Get(int CatalogID)
        //{
        //    bool AllowEditCatalog = false;
        //    string OrganizationName = "", FacilityName = "", CatalogDescription = "";

        //    return Json(new
        //    {
        //        AllowEditCatalog = AllowEditCatalog,
        //        OrganizationName = OrganizationName,
        //        FacilityName = FacilityName,
        //        CatalogDescription = CatalogDescription
        //    }, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Create

        //public ActionResult Create()
        //{
        //    // Listados para los combos
        //    var IsSystmeValueList = vw_CatalogService.List4Select("YesNo", BaseGenericRequest);

        //    ViewBag.IsSystemValueList = new SelectList(IsSystmeValueList, "ValueID", "DisplayText");
        //    ViewBag.OrganizationsList = new SelectList(OrganizationService.List4Select(BaseGenericRequest), "OrganizationID", "OrganizationName");
        //    ViewBag.FacilitiesList = new SelectList(IsSystmeValueList, "FacilityID", "FacilityName");

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView();
        //    }
        //    return View();
        //}


        [HttpPost]
        public JsonResult GetFacilities(int OrganizationID)
        {
            // Listados para los combos
            var facilities = FacilityService.List4Config(OrganizationID, null, BaseGenericRequest, false)
                .Select(c => new Facility() { FacilityID = c.FacilityID, FacilityName = c.FacilityName }).Distinct().ToList();
            return Json(facilities, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAccess(int CatalogID)
        //{
        //    var access = UserFacilityService.Access(CatalogID, VARG_FacilityID, VARG_UserID, VARG_CultureID);
        //    var list = access.Select(c => new UserFacility() { OrganizationID = c.OrganizationID, CompanyID = c.CompanyID, FacilityID = c.FacilityID }).Distinct().ToList();
        //    return Json(list);
        //}

        [HttpPost]
        public ActionResult CreateNewCatalog(string CatalogTag, string CatalogName, string CatalogDescription, int? OrganizationID,
                                   int? FacilityID, string hf_IsSystemValue)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                // Validaciones
                Catalog catalog = new Catalog()
                {
                    CatalogTag = CatalogTag,
                    CatalogName = CatalogName,
                    CatalogDescription = CatalogDescription,
                    OrganizationID = OrganizationID,
                    FacilityID = FacilityID,
                    IsSystemValue = hf_IsSystemValue.ToBoolean(),
                    Enabled = true
                };

                result = vw_CatalogService.Add(catalog, BaseGenericRequest);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatalogsDetail(int CatalogID)
        {
            List<CatalogDetail> model = new List<CatalogDetail>();
            string ViewPath = "~/Areas/Administration/Views/Catalogs/_CatalogsDetailTable.cshtml";
            try
            {
                // Listados para los combos
                model = CatalogDetailService.List(CatalogID, BaseGenericRequest);
            }
            catch (Exception )
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }

        public ActionResult GetCatalogsParameters(int CatalogID)
        {
            List<CatalogParameter> model = new List<CatalogParameter>();
            string ViewPath = "~/Areas/Administration/Views/Catalogs/_CatalogsParametersTable.cshtml";
            try
            {
                // Listados para los combos
                model = CatalogParameterService.List(CatalogID, BaseGenericRequest);
            }
            catch (Exception )
            {

                throw;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }

        public JsonResult InsertCatalogsParameters(List<CatalogParameter> Parameterslist)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                foreach (CatalogParameter i in Parameterslist)
                {
                    CatalogParameter parameter = new CatalogParameter()
                    {
                        CatalogID = i.CatalogID,
                        ParamID = i.ParamID,
                        ParamName = i.ParamName,
                        Description = i.Description,
                        Configured = i.Configured
                    };
                    result = CatalogParameterService.Upsert(parameter, new GenericRequest() { UserID = VARG_UserID, FacilityID = VARG_FacilityID, CultureID = VARG_CultureID });
                }
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;                
            }

            return Json(new
            {
                ErrorCode = 0,
                ErrorMessage = Resources.Catalog.ParamsUpdated + result.ErrorMessage,
                Title = "",
                notifyType = StaticModels.NotifyType.success.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParametersConfiguration(int CatalogID)
        {
            // Listados para los combos
            var Parameters = CatalogParameterService.List(CatalogID, BaseGenericRequest)
                .Select(c => new CatalogParameter() { ParamID = c.ParamID, ParamName = c.ParamName }).Distinct().ToList();
            return Json(Parameters, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateCatalogDetail(int hf_ADF_CatalogID, string ValueID, string Param1, string Param2,
                                string Param3, string Param4)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                // Validaciones
                if (String.IsNullOrEmpty(ValueID))
                {
                    result.ErrorCode = 2;
                    result.ErrorMessage = Resources.Catalog.MissingValueID;
                }
                else
                {
                    CatalogDetail catalogDetail = new CatalogDetail()
                    {
                        CatalogID = hf_ADF_CatalogID,
                        ValueID = ValueID,
                        Param1 = Param1,
                        Param2 = Param2,
                        Param3 = Param3,
                        Param4 = Param4
                    };

                    result = CatalogDetailService.Add(catalogDetail, new GenericRequest() { UserID = VARG_UserID, FacilityID = VARG_FacilityID, CultureID = VARG_CultureID });
                }
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        [HttpPost]
        public JsonResult UpdateEditable(string name, int pk, string value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = CatalogDetailService.QuickUpdate(pk, name, value, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditCatalog(int hf_E_C_CatalogID, string E_C_CatalogName, string E_C_CatalogDescription, int? E_C_OrganizationID,
                                 int? E_C_FacilityID, string hf_E_C_IsSystemValue)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                // Validaciones
                Catalog catalog = new Catalog()
                {
                    CatalogID = hf_E_C_CatalogID,
                    CatalogName = E_C_CatalogName,
                    CatalogDescription = E_C_CatalogDescription,
                    OrganizationID = E_C_OrganizationID,
                    FacilityID = E_C_FacilityID,
                    IsSystemValue = hf_E_C_IsSystemValue.ToBoolean(),
                    Enabled = true
                };

                result = vw_CatalogService.Update(catalog, BaseGenericRequest);

            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult DeleteCatalogDetail(int CatalogDetailID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = CatalogDetailService.Delete(CatalogDetailID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;                
            }
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        #region EditTranslations
        [HttpPost]
        public JsonResult CulturesList()
        {
            var list = vw_CatalogService.List4Select("SystemCultures", BaseGenericRequest).Select(s => new { value = s.ValueID, text = s.DisplayText });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatalogsDetailCultures(int CatalogDetailID)
        {
            List<TranslationDetail> model = new List<TranslationDetail>();
            string ViewPath = "~/Areas/Administration/Views/Catalogs/_CatalogsDetailsCultureTable.cshtml";
            try
            {
                // Listados para los combos
                model = TranslationDetailService.List(CatalogDetailID, BaseGenericRequest);
            }
            catch (Exception)
            {
                throw;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }


        [HttpPost]
        public JsonResult Translation_UpdateEditable(string pk2, string name, int pk, string value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = TranslationDetailService.QuickUpdate(pk, pk2, name, value, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateCatalogDetailCulture(string hf_EDC_CatalogDetailID, string CultureID, string TranslationDescription)
        {

            GenericReturn result = new GenericReturn();
            try
            {
                // Validaciones
                if (String.IsNullOrEmpty(TranslationDescription))
                {
                    result.ErrorCode = 2;
                    result.ErrorMessage = Resources.Catalog.MissingDescription;
                }
                else
                {
                    TranslationDetail translationDetail = new TranslationDetail()
                    {
                        Tag = "Cat_" + hf_EDC_CatalogDetailID,
                        CultureID = CultureID,
                        Description = TranslationDescription
                    };
                    result = TranslationDetailService.Add(translationDetail, BaseGenericRequest);
                }
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult GetCatalogInfo(int CatalogID)
        {
            Catalog StorageLocation = new Catalog();
            StorageLocation = vw_CatalogService.GetInfo(CatalogID, BaseGenericRequest);
            return Json(StorageLocation == null? new Catalog(): StorageLocation, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList(string Catalog)
        {
            //para su uso en listados para xeditable
            var list = vw_CatalogService.List4Select(Catalog, BaseGenericRequest, false).OrderBy(o => o.DisplayText).Select(s => new { value = s.CatalogDetailID, text = s.DisplayText });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCatalogs()
        {
            // Listados para los combos
            var catalogs = vw_CatalogService.List4Config(BaseGenericRequest,false)
                .Select(c => new Catalog() { CatalogID = c.CatalogID, CatalogName = c.CatalogName })
                .Distinct().ToList().OrderBy(c => c.CatalogName);
            return Json(catalogs, JsonRequestBehavior.AllowGet);
        }
    }
}