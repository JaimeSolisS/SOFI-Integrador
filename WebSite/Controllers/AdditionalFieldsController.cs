using Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebSite.Filters;
using WebSite.Models.ViewModels;
using WebSite.Utilities;
using static WebSite.Models.StaticModels;

namespace WebSite.Controllers
{
    [SessionExpire]
    public class AdditionalFieldsController : BaseController
    {
        public ActionResult Get(int ReferenceID, string ModuleName, bool ViewReadOnly)
        {
            try
            {
                List<TableAdditionalFields> result = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result = Core.Services.TableAdditionalFieldService.List(ReferenceID, ModuleName, BaseGenericRequest);

                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.CatalogJSON))
                        {
                            item.CatalogContext = serializer.Deserialize<List<Catalog>>(item.CatalogJSON);
                            item.CatalogContext.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                    }
                }

                var model = new AdditionalFieldsViewModel
                {
                    CollectionAdditionalFields = result,
                    ViewReadOnly = ViewReadOnly,
                    ReferenceID = ReferenceID
                };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AdditionalFieldsView", model);
                }
                return PartialView("_AdditionalFieldsView", model);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = e.Message,
                    Title = "",
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
                throw;
            }

        }
        public ActionResult GetConfiguration(int ReferenceID, int FormatID, string ModuleName, bool ViewReadOnly)
        {
            try
            {
                List<TableAdditionalFields> result = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result = Core.Services.TableAdditionalFieldService.ListConfiguration(ReferenceID, FormatID, ModuleName, BaseGenericRequest);

                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.CatalogJSON))
                        {
                            item.CatalogContext = serializer.Deserialize<List<Catalog>>(item.CatalogJSON);
                            item.CatalogContext.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                    }
                }

                var model = new AdditionalFieldsViewModel
                {
                    CollectionAdditionalFields = result,
                    ViewReadOnly = ViewReadOnly,
                    ReferenceID = ReferenceID
                };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AdditionalFieldsView", model);
                }
                return PartialView("_AdditionalFieldsView", model);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = e.Message,
                    Title = "",
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
                throw;
            }

        }

        public ActionResult GetListIndex(int ReferenceID, int FormatID, string ModuleName, bool ViewReadOnly)
        {
            try
            {
                List<TableAdditionalFields> result = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result = Core.Services.TableAdditionalFieldService.ListConfiguration(ReferenceID, FormatID, ModuleName, BaseGenericRequest);

                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.CatalogJSON))
                        {
                            item.CatalogContext = serializer.Deserialize<List<Catalog>>(item.CatalogJSON);
                            item.CatalogContext.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                    }
                }

                var model = new AdditionalFieldsViewModel
                {
                    CollectionAdditionalFields = result,
                    ViewReadOnly = ViewReadOnly,
                    ReferenceID = ReferenceID
                };
                
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AdditionalFieldsIndexView", model);
                }
                return PartialView("_AdditionalFieldsIndexView", model);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = e.Message,
                    Title = "",
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
                throw;
            }

        }

        public ActionResult GetColumn(int ReferenceID, string ModuleName, string ColumnName, bool ViewReadOnly)
        {
            try
            {
                List<TableAdditionalFields> result = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result = Core.Services.TableAdditionalFieldService.List4Column(ReferenceID, ModuleName, ColumnName, BaseGenericRequest);

                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.CatalogJSON))
                        {
                            item.CatalogContext = serializer.Deserialize<List<Catalog>>(item.CatalogJSON);
                            item.CatalogContext.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                    }
                }

                var model = new AdditionalFieldsViewModel
                {
                    CollectionAdditionalFields = result,
                    ViewReadOnly = ViewReadOnly,
                    ReferenceID = ReferenceID
                };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AdditionalFieldView", model);
                }
                return PartialView("_AdditionalFieldView", model);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = e.Message,
                    Title = "",
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
                throw;
            }

        }
    }
}