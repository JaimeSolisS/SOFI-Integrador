using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebSite.Areas.eRequest.Models.ViewModels.Request;
using WebSite.Filters;
using WebSite.Models;
using WebSite.Utilities;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.eRequest.Controllers
{
    public class RequestController : BaseController
    {
        // GET: eRequest/Request
        [SecurityFilter]
        public ActionResult Index(int? RequestID)
        {
            var model = new IndexViewModel();

            try
            {
                // Listados para los combos
                var UserAccess = RequestService.GetUserAccessToeRequest(BaseGenericRequest);
                model.AllowFullAccess = UserAccess.FirstOrDefault().AllowFullAccess;
                model.AllowEdit = UserAccess.FirstOrDefault().AllowEdit;
                model.AllowCancel = UserAccess.FirstOrDefault().AllowCancel;
                model.AllowCreateReq = UserAccess.FirstOrDefault().AllowCreateReq;
                model.AllowApproveReq = UserAccess.FirstOrDefault().AllowApproveReq;
                model.FacilityList = new SelectList(RequestService.GetFacility(BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "FacilityID", "FacilityName");
                model.FormatList = new SelectList(RequestService.GetFormat4User(BaseGenericRequest), "FormatID", "FormatName");
                model.DepartmentList = new SelectList(DepartmentService.List(null, null, true, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID, false), "DepartmentID", "DepartmentName");
                model.DateTypeList = new SelectList(vw_CatalogService.List("eReq_DateType", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                model.StatusList = new SelectList(vw_CatalogService.List("eReq_RequestStatus", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                model.RequestDefaultStatus = MiscellaneousService.Param_GetValue(VARG_FacilityID, "eReq_RequestDefaultStatus", "0");
                model.UserFacilitiesList = new SelectList(UserService.GetFacilities(VARG_UserID, VARG_FacilityID, VARG_CultureID), "FacilityID", "FacilityName");
                if (RequestID != null)
                {
                    model.RequestList = RequestService.RequestList(RequestID, null, null, null, null, null, null, null, null, BaseGenericRequest);
                }
                else
                {
                    model.RequestList = RequestService.RequestList(null, null, null, null, null, null, null, null, null, BaseGenericRequest);
                }
                //model.ApprovalStatusID = vw_CatalogService.List("eReq_RequestStatus", BaseGenericRequest).Where(v => v.ValueID == "Approval").FirstOrDefault().CatalogDetailID;
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

        public ActionResult SearchRequest(string FormatIDs, int? Folio, string DepartmentIDs, string StatusIDs, string FacilityIDs, int? DateTypeID, DateTime? StartDate, DateTime? EndDate)
        {
            IndexViewModel model = new IndexViewModel();
            string viewPath = "~/Areas/eRequest/Views/Request/_Tbl_Requests.cshtml";

            try
            {
                var UserAccess = RequestService.GetUserAccessToeRequest(BaseGenericRequest);
                model.AllowFullAccess = UserAccess.FirstOrDefault().AllowFullAccess;
                model.AllowEdit = UserAccess.FirstOrDefault().AllowEdit;
                model.AllowCancel = UserAccess.FirstOrDefault().AllowCancel;
                model.AllowPdf = UserAccess.FirstOrDefault().AllowViewPdf;
                model.AllowViewLog = UserAccess.FirstOrDefault().AllowViewLog;
                model.RequestList = RequestService.RequestList(null, FormatIDs, Folio, DepartmentIDs, StatusIDs, FacilityIDs, DateTypeID, StartDate, EndDate, BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            return PartialView(viewPath, model);
        }

        public JsonResult GetRequestByFacility(int FacilityID)
        {
            var list = RequestService.GetFormat4User(new GenericRequest() { FacilityID = FacilityID, UserID = BaseGenericRequest.UserID, CultureID = BaseGenericRequest.CultureID });
            list.Insert(0, new FormatsEntity() { FormatID = 0, FormatName = Resources.Common.chsn_SelectOption });
            return Json(list.Select(s => new { value = s.FormatID, text = s.FormatName }), JsonRequestBehavior.AllowGet);
        }

        #region List

        public ActionResult GetDetail(int RequestID, int FormatID, bool ViewReadOnly)
        {
            try
            {
                List<TableAdditionalFields> result = null;
                List<RequestGenericDetail_tb> resultvalues = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                result = Core.Services.TableAdditionalFieldService.ListDetail(RequestID, FormatID, BaseGenericRequest);
                resultvalues = RequestService.RequestsGenericDetailListTable(RequestID, BaseGenericRequest);

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
                if (resultvalues != null && resultvalues.Count > 0)
                {
                    foreach (var item in resultvalues)
                    {
                        if (!string.IsNullOrEmpty(item.CatalogJSONConcept))
                        {
                            item.CatalogContextConcept = serializer.Deserialize<List<Catalog>>(item.CatalogJSONConcept);
                            item.CatalogContextConcept.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                        if (!string.IsNullOrEmpty(item.CatalogJSONReference1))
                        {
                            item.CatalogContextReference1 = serializer.Deserialize<List<Catalog>>(item.CatalogJSONReference1);
                            item.CatalogContextReference1.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                        if (!string.IsNullOrEmpty(item.CatalogJSONReference2))
                        {
                            item.CatalogContextReference2 = serializer.Deserialize<List<Catalog>>(item.CatalogJSONReference2);
                            item.CatalogContextReference2.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                        if (!string.IsNullOrEmpty(item.CatalogJSONReference3))
                        {
                            item.CatalogContextReference3 = serializer.Deserialize<List<Catalog>>(item.CatalogJSONReference3);
                            item.CatalogContextReference3.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                        if (!string.IsNullOrEmpty(item.CatalogJSONReference4))
                        {
                            item.CatalogContextReference4 = serializer.Deserialize<List<Catalog>>(item.CatalogJSONReference4);
                            item.CatalogContextReference4.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                        if (!string.IsNullOrEmpty(item.CatalogJSONReference5))
                        {
                            item.CatalogContextReference5 = serializer.Deserialize<List<Catalog>>(item.CatalogJSONReference5);
                            item.CatalogContextReference5.Insert(0, new Catalog { CatalogDetailID = 0, DisplayText = "" });
                        }
                    }
                }
                var model = new AdditionalFieldsDescriptionViewModel
                {
                    CollectionAdditionalFields = result,
                    CollectionAdditionalFieldsTable = resultvalues,
                    ViewReadOnly = ViewReadOnly,
                    ReferenceID = RequestID
                };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Areas/eRequest/Views/Request/_AdditionalFieldsDescriptionView.cshtml", model);
                }
                return PartialView("~/Areas/eRequest/Views/Request/_AdditionalFieldsDescriptionView.cshtml", model);
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

        public JsonResult NextSeq(int RequestLoopFlowID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = RequestService.SeqValidate(RequestLoopFlowID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ApprovalFlowUser(int RequestLoopFlowID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = RequestService.ApprovalFlowUser(RequestLoopFlowID, null, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovalFlowUserLogin(int RequestLoopFlowID, int UserID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = RequestService.ApprovalFlowUser(RequestLoopFlowID, UserID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendCodeValidation(string UserID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = RequestService.SendCodeValidation(UserID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckSignature(int? UserID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                if (UserID == null)
                {
                    UserID = BaseGenericRequest.UserID;
                }
                result = RequestService.CheckSignature(UserID.ToInt(), BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCodeValidation(int? UserID, string FAToken)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                if (UserID == null)
                {
                    UserID = BaseGenericRequest.UserID;
                }
                result = RequestService.CheckCodeValidation(UserID.ToInt(), FAToken, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSignature(string Img64, int? UserID)
        {
            GenericReturn result = new GenericReturn();
            Byte[] bitmapData = new Byte[Img64.Length];
            bitmapData = Convert.FromBase64String(FixBase64ForImage(Img64));
            try
            {
                result = RequestService.SaveSignature(bitmapData, UserID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateStatus(int RequestID, string Status)
        {
            GenericReturn result = new GenericReturn();
            int StatusID = 0;
            string Error = "";
            try
            {
                StatusID = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "eReq_RequestStatus", Status, out Error);
                if (Error != "")
                {
                    result.ErrorCode = 99;
                    result.ErrorMessage = Error;
                }
                else
                {
                    result = RequestService.UpdateStatus(RequestID, StatusID, BaseGenericRequest);
                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RequestsLoopsFlowStatusUpdate(int RequestLoopFlowID, string Status, string Comment, int? UserID)
        {
            GenericReturn result = new GenericReturn();
            if (UserID == null)
            {
                UserID = BaseGenericRequest.UserID;
            }
            try
            {
                result = RequestService.RequestsLoopsFlowStatusUpdate(RequestLoopFlowID, Status, Comment, UserID.ToInt(), BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestDetail(int RequestID)
        {
            List<RequestsGenericDetail> model = new List<RequestsGenericDetail>();
            string ViewPath = "~/Areas/eRequest/Views/Request/_tbl_RequestDetail.cshtml";
            try
            {
                model = RequestService.RequestsGenericDetailList(RequestID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetRequestLoopFlow(int RequestID)
        {
            List<RequestLoopFlow> model = new List<RequestLoopFlow>();
            string ViewPath = "~/Areas/eRequest/Views/Request/_ApprovalSeq.cshtml";
            try
            {

                model = RequestService.RequestLoopsFlowList(RequestID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }


        public ActionResult GetApprovalLog(int RequestID)
        {
            List<RequestApprovalLog> model = new List<RequestApprovalLog>();
            string ViewPath = "~/Areas/eRequest/Views/Request/_tbl_RequestApprovalLog.cshtml";
            try
            {
                model = RequestService.RequestsApprovalLogList(RequestID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public JsonResult GetSignature()
        {
            string img = "";
            string base64String = "";
            User signature;
            try
            {
                signature = RequestService.GetSignatureIMG(BaseGenericRequest);
                if (signature.Signature != null)
                {
                    if (signature.Signature.Length > 0)
                    {
                        base64String = Convert.ToBase64String(signature.Signature, 0, signature.Signature.Length);
                        img = "data:image/png;base64," + base64String;
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return Json(new
            {
                img
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRequestMedia(int RequestID)
        {
            List<FormatsMedia> model = new List<FormatsMedia>();
            string ViewPath = "~/Areas/eRequest/Views/Request/_AttachmentsTableDetail.cshtml";
            try
            {
                model = RequestService.MediaList(RequestID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return PartialView(ViewPath, model);
        }

        public ActionResult PrinteRequest(int RequestID, int FormatID, string CompanyLogo)
        {            
            PDFFile FileInfo = PDFFileService.PDFFiles_GetPDFDetail(FormatID, BaseGenericRequest);
            string PDFResult = "";
            bool ResultDig = false;
            if (FileInfo != null)
            {
                PDFResult = Tools.GeneratePDFReport(FormatID, FileInfo.FullPath,
                                        "[eReq].[GetPaseSalida]", FileInfo.FileID.ToString(),
                                        "~/Files/eRequest/Reports/", FileInfo.FileName + RequestID,
                                        RequestID, "",
                                        VARG_CultureID, VARG_FacilityID, VARG_UserID,
                                        CompanyLogo,
                                        out bool Result);
                ResultDig = Result;

            }

            if (ResultDig && !String.IsNullOrEmpty(PDFResult))
            {
                var pdfPath = RequestController.GetVirtualPath4File(PDFResult);
                return Json(new { ErrorCode = 0, ResultDig, pdfPath }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = PDFResult,
                    Title = "",
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public static string GetVirtualPath4File(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                path = path.Substring((System.Web.HttpContext.Current.Server.MapPath("\\")).Length);
                path = Uri.EscapeDataString(path);
                //path = HttpUtility.UrlEncode(path);
                path = "/" + path.Replace("\\", "/");
            }

            return path;
        }
        public static string FixBase64ForImage(string image)
        {
            StringBuilder sbText = new StringBuilder(image, image.Length);
            sbText.Replace("\r\n", String.Empty);
            sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }


        #endregion

        #region Create/Edit

        public ActionResult Create(int FacilityID, int FormatID, int? RequestID)
        {
            var model = new CreateViewModel();
            var EntityIndicator = MiscellaneousService.Catalog_GetDetailID(FacilityID, "SystemEntities", "eReqFormat", out string ErrorMessage);
            bool ViewOnly = false;

            try
            {
                var RequestEntity = new Request();
                if (RequestID != null)
                {
                    RequestEntity = RequestService.RequestList(RequestID, BaseGenericRequest).FirstOrDefault();
                    model.FacilityList = new SelectList(RequestService.GetFacility(BaseGenericRequest), "FacilityID", "FacilityName", RequestEntity.FacilityID);
                    model.FormatList = new SelectList(RequestService.GetFormat4User(BaseGenericRequest), "FormatID", "FormatName", RequestEntity.FormatID);
                    model.EntityFields = MiscellaneousService.EntityFieldsConfiguration(RequestEntity.FacilityID, RequestEntity.FormatID, EntityIndicator, "eReq_GenericFields", BaseGenericRequest.CultureID);
                    model.RequestID = RequestID;
                    model.EntityFormat = RequestService.GetFormat4ID(RequestEntity.FormatID, BaseGenericRequest);
                    model.RequestDate = RequestEntity.RequestDate.ToString("yyyy-MM-dd");
                    model.Btn_Save_ID = "btn-edit";
                    model.ConceptValue = RequestEntity.Concept;
                    model.SpecificationValue = RequestEntity.Specification;
                    model.FormatMediaList = RequestService.MediaList(RequestEntity.RequestID, BaseGenericRequest);
                    model.Folio = RequestEntity.Folio;
                    ViewOnly = RequestService.ViewReadOnly(RequestID, BaseGenericRequest);
                }
                else
                {
                    model.FacilityList = new SelectList(RequestService.GetFacility(BaseGenericRequest), "FacilityID", "FacilityName", BaseGenericRequest.FacilityID);
                    model.FormatList = new SelectList(RequestService.GetFormat4User(BaseGenericRequest), "FormatID", "FormatName", FormatID);
                    model.EntityFields = MiscellaneousService.EntityFieldsConfiguration(FacilityID, FormatID, EntityIndicator, "eReq_GenericFields", BaseGenericRequest.CultureID);
                    model.EntityFormat = RequestService.GetFormat4ID(FormatID, BaseGenericRequest);
                    model.RequestDate = MiscellaneousService.Facility_GetDate(BaseGenericRequest.FacilityID).ToString("yyyy-MM-dd");
                    model.Btn_Save_ID = "btn-create";
                }
                /* (RequestID != null) ? RequestEntity.DepartmetID : 0;*/
                var DepartmentID = UserService.Get(VARG_UserID, BaseGenericRequest).DepartmentID != null ? UserService.Get(VARG_UserID, BaseGenericRequest).DepartmentID : 0;
                model.FormatID = FormatID;
                model.DepartmentList = new SelectList(DepartmentService.List(null, null, true, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID, true), "DepartmentID", "DepartmentName", DepartmentID);
                model.ViewReadOnly = ViewOnly;
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public JsonResult CreateNewRequest(List<t_RequestGenericDetail> GenericDetail, List<t_AdditionalFields> AdditionalFields, int FacilityID, string RequestDate, int DepeartmentID, int FormatID, string Concept, string Specification)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = RequestService.Create(GenericDetail, AdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditRequest(int RequestID, List<t_RequestGenericDetail> GenericDetail, List<t_AdditionalFields> AdditionalFields, int FacilityID, string RequestDate, int DepeartmentID, int FormatID, string Concept, string Specification, string FilesToDelete)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = RequestService.Edit(RequestID, GenericDetail, AdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, FilesToDelete, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReturnLoadModal(int RequestID)
        {
            MaterialReturnViewModel model = new MaterialReturnViewModel();
            try
            {
                var RequestEntity = RequestService.RequestList(RequestID, BaseGenericRequest);
                if (RequestEntity.Count > 0)
                {
                    model.RequestEntity = RequestEntity.FirstOrDefault();
                }
                model.RequestDetailList = RequestService.RequestsGenericDetailList(RequestID, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView("~/Areas/eRequest/Views/Request/_Mo_ReturnMaterial.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateRequestDetailReturnDate(int[] RequestGenericDetailIDs, string Comments)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                if (RequestGenericDetailIDs != null)
                {
                    foreach (var RequestGenericDetailID in RequestGenericDetailIDs)
                    {
                        result = RequestsGenericDetailService.Update(new RequestsGenericDetail() { RequestGenericDetailID = RequestGenericDetailID, DateReturn = DateTime.Now }, Comments, BaseGenericRequest);
                    }
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
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMaterialReturnTable(int RequestID)
        {
            List<RequestsGenericDetail> model = new List<RequestsGenericDetail>();
            try
            {
                model = RequestService.RequestsGenericDetailList(RequestID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView("~/Areas/eRequest/Views/Request/_Tbl_ReturnMaterial.cshtml", model);
        }

        #endregion

    }
}