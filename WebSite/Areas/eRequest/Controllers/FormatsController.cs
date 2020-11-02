using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.eRequest.Models.ViewModels.Formats;
using WebSite.Models;
using WebSite.Utilities;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.eRequest.Controllers
{
    public class FormatsController : BaseController
    {
        // GET: eRequest/Formats
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            try
            {
                model.FormatList = FormatsService.FormatsList("", "", BaseGenericRequest);
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
        public ActionResult SearchFormat(string FormatName, string Approver)
        {
            IndexViewModel model = new IndexViewModel();
            string viewPath = "~/Areas/eRequest/Views/Formats/_Tbl_Formats.cshtml";

            try
            {
                var UserAccess = RequestService.GetUserAccessToeRequest(BaseGenericRequest);
                model.FormatList = FormatsService.FormatsList(FormatName, Approver, BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            return PartialView(viewPath, model.FormatList);
        }
        public ActionResult Create(int? FormatID)
        {
            var model = new CreateViewModel();
            Guid TransactionID = Guid.NewGuid();
            GenericReturn result = new GenericReturn();
            model.TransactionID = TransactionID;
            string ErrorMessage;
            try
            {
                var eReqFieldConf = vw_CatalogService.List4Select("eReq_FieldConfList", BaseGenericRequest, true);
                if (FormatID != null)
                {
                    model.FormatID = FormatID.ToInt();
                    model.EntityFormat = RequestService.GetFormat4ID(FormatID.ToInt(), BaseGenericRequest);
                    model._UserList = FormatsService.UsersFacilitiesPermission(BaseGenericRequest.FacilityID.ToString(), BaseGenericRequest);
					int CatalogID = MiscellaneousService.Catalog_GetID(VARG_FacilityID, "FlowPhase", out ErrorMessage);                    result = FormatsService.FormatGenericDetailTempInsert(FormatID.ToInt(), TransactionID, BaseGenericRequest);
                    model.FacilityList = new SelectList(RequestService.GetFacility(BaseGenericRequest), "FacilityID", "FacilityName", BaseGenericRequest.FacilityID);

                    PDFFile PDFData = PDFFileService.PDFFiles_GetPDFDetail(FormatID.ToInt(), BaseGenericRequest);

                    if (PDFData != null)
                    {
                        model.PDFFile = PDFData;
                        model._PDFAdditionalFields = FormatsService.PDFFilesDetail_TEMP_List4Header(TransactionID, PDFData.FileID, BaseGenericRequest);
                        model._PDFDetailFields = FormatsService.PDFFilesDetail_TEMP_List4Detail(TransactionID, PDFData.FileID, BaseGenericRequest);
                        model._PDFConfigurationSignature = FormatsService.PDFFilesDetail_TEMP_List4Signatures(TransactionID, PDFData.FileID, BaseGenericRequest);
                        model._FontList = new SelectList(vw_CatalogService.List("eReq_Fonts", BaseGenericRequest), "ValueID", "DisplayText");
                        model._FontListFE = vw_CatalogService.List("eReq_Fonts", BaseGenericRequest);
                        model._FontColorList = vw_CatalogService.List("eReq_FontColor", BaseGenericRequest);
                        model._ValueToShow = vw_CatalogService.List("PDF_Value_To_Show", BaseGenericRequest);
                        model._ShowWhen = vw_CatalogService.List("PDF_Show_When", BaseGenericRequest);
                    }
                    if (result.ErrorCode == 0)
                    {
                        model._FormatGenericDetail = FormatsService.FormatGenericDetailList(FormatID.ToInt(), TransactionID, BaseGenericRequest);
                        model._CatalogPhase = CatalogDetailService.List4FormatPhase(CatalogID, FormatID.ToString(), TransactionID, BaseGenericRequest);
                        model._FormatAccessTemp = FormatsService.FormatAccessTemp_List4Transaction(FormatID.ToInt(), TransactionID, BaseGenericRequest);
                        model._FormatsLoopsRulesTemp = FormatsService.FormatsLoopsRulesTemp_List(TransactionID, BaseGenericRequest.FacilityID, BaseGenericRequest);

                    }
                    if (eReqFieldConf != null)
                    {
                        if (eReqFieldConf.Count == 1)
                        {
                            var SystemModuleTag = eReqFieldConf[0].ValueID;
                            var EntityIndicator = MiscellaneousService.Catalog_GetDetailID(VARG_FacilityID, "SystemEntities", "eReqFormat", out ErrorMessage);
                            model._CustomFieldsList = RequestService.FieldsConfigurationList(FormatID.ToInt(), EntityIndicator, SystemModuleTag, BaseGenericRequest);
                        }
                    }
                }
                else
                {

                }

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
        public ActionResult TableFormatDetail(int FormatID, Guid TransactionID)
        {
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_DetailFields.cshtml";
            var model = new List<FormatGenericDetailTemp>();
            try
            {
                model = FormatsService.FormatGenericDetailList(FormatID.ToInt(), TransactionID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public JsonResult GetModalNewField()
        {
            ModalAddFieldViewModal model = new ModalAddFieldViewModal();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_NewField.cshtml";
            model.Title = @Resources.Request.lbl_AddFormatFields;

            try
            {
                model.ParameterList = new SelectList(vw_CatalogService.List4Select("AdditionalFields_DataType", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.CatalogsList = new SelectList(vw_CatalogService.List4Formats(BaseGenericRequest, true), "CatalogTag", "CatalogName");
                result.ErrorCode = 0;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTranslation(string Tag, string DescriptionEN, string DescriptionES)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.TranslationDetaulUpdate(Tag, DescriptionEN, DescriptionES, BaseGenericRequest);
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

        public JsonResult FormatAccessTempInsert(int FormatID, Guid TransactionID, string FacilityID, string UserListID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatAccessTempInsert(FormatID, TransactionID, FacilityID, UserListID, BaseGenericRequest);
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
        public JsonResult FormatAccessTempDelete(int FormatAccessTempID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatAccess_TEMP_Delete(FormatAccessTempID, BaseGenericRequest);
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
        public JsonResult PDFFilesDetail_TEMP_Delete(int FileDetailTempID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = PDFFileService.PDFFilesDetail_TEMP_Delete(FileDetailTempID, BaseGenericRequest);
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
        public JsonResult FormatsLoopsRulesDetail_TEMP_Delete(int FormatLoopRuleDetailTempID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatsLoopsRulesDetail_TEMP_Delete(FormatLoopRuleDetailTempID, BaseGenericRequest);
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

        public JsonResult FormatLoopRuleDetailDelete(int FormatLoopRuleTempID, int Seq, Guid TransactionID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatsLoopsRulesDetail_TEMP_Delete4Edit(FormatLoopRuleTempID, Seq, TransactionID, BaseGenericRequest);
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
        public JsonResult FormatPhaseTempDelete(int CatalogDetailTempID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatPhase_CatalogDetail_TEMP_Delete(CatalogDetailTempID, BaseGenericRequest);
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
        public JsonResult FormatLoopApproverTempDelete(int FormatLoopApproverTempID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatsLoopsApprovers_TEMP_Delete(FormatLoopApproverTempID, BaseGenericRequest);
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
        public JsonResult FormatsLoopsFlowTempDelete(int FormatLoopFlowTempID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatsLoopsFlow_TEMP_Delete(FormatLoopFlowTempID, BaseGenericRequest);
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
        public JsonResult GetModalNewDetail(int? FormatGenericDetailTempID)
        {
            ModalAddDetailFieldViewModal model = new ModalAddDetailFieldViewModal();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_NewDetailField.cshtml";
            string Title = @Resources.Request.lbl_AddDetailField;
            int CatalogParameterSelect = 0;
            string ParameterValueID = "";
            try
            {
                if (FormatGenericDetailTempID != null)
                {
                    model.FormatDetail = FormatsService.FormatGenericDetailList4FormatGenericDetail(FormatGenericDetailTempID.ToInt(), BaseGenericRequest);
                    model.EnglishName = TranslationDetailService.GetTranslation4Tag(model.FormatDetail.TranslationTag, "EN-US", BaseGenericRequest).Description;
                    model.SpanishName = TranslationDetailService.GetTranslation4Tag(model.FormatDetail.TranslationTag, "ES-MX", BaseGenericRequest).Description;
                    CatalogParameterSelect = model.FormatDetail.DataTypeID;
                    ParameterValueID = model.FormatDetail.DataTypeName;
                    Title = @Resources.Request.lbl_EditDetailField;
                }
                model.ParameterList = new SelectList(vw_CatalogService.List4Select("AdditionalFields_DataType", BaseGenericRequest, true), "CatalogDetailID", "DisplayText", CatalogParameterSelect);
                model.CatalogsList = new SelectList(vw_CatalogService.List4Formats(BaseGenericRequest, true), "CatalogTag", "CatalogName");
                model.ParameterValueID = ParameterValueID;
                result.ErrorCode = 0;
                model.Title = Title;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveNewField(int EntityID, int IsVisible, int IsMandatory, string ValueID,
            string Param1, string Param2, string Param3, string CatalogTagID, int DataTypeID, int FieldLength, int FieldPrecission)
        {

            GenericReturn result = new GenericReturn();
            try
            {
                int EntityIndicator = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemEntities", "eReqFormat", out string ErrorMessage);

                int SystemModule = vw_CatalogService.List("eReq_GenericFields", BaseGenericRequest).FirstOrDefault().CatalogID;

                result = FormatsService.EntityFieldsConfiguration_Insert(EntityID, EntityIndicator, SystemModule, IsVisible, IsMandatory, ValueID, Param1, Param2, Param3, CatalogTagID, DataTypeID, FieldLength, FieldPrecission, BaseGenericRequest);
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
        public JsonResult SaveFormat(int? FormatID, string FormatName, int Use2FA, int DirectApproval, int HasDetail, int HasAttachment, Guid TransactionID,
                                     string FormatPath, int? PositionYInitial, int? MaxDetail, int? Interline, List<EntityField> Fieldslist)
        {
            if (PositionYInitial == null) { PositionYInitial = 0; }
            if (MaxDetail == null) { MaxDetail = 0; }
            if (Interline == null) { Interline = 0; }
            GenericReturn result = new GenericReturn();
            //string ErrorMessage = "";
            try
            {
                result = FormatsService.Upsert(FormatID, FormatName, Use2FA, DirectApproval, HasDetail, HasAttachment, TransactionID, FormatPath, PositionYInitial.ToInt(), MaxDetail.ToInt(), Interline.ToInt(), Fieldslist, BaseGenericRequest);
                //if (result.ErrorCode == 0)
                //{
                //    var EntityIndicator = MiscellaneousService.Catalog_GetDetailID(BaseGenericRequest.FacilityID, "SystemEntities", "eReqFormat", out ErrorMessage);
                //    int SystemModule = vw_CatalogService.List("eReq_GenericFields", BaseGenericRequest).FirstOrDefault().CatalogID;
                //    foreach (EntityField i in Fieldslist)
                //    {
                //        EntityField field = new EntityField()
                //        {
                //            FacilityID = BaseGenericRequest.FacilityID,
                //            EntityID = i.EntityID,
                //            EntityIndicator = EntityIndicator,
                //            SystemModule = SystemModule,
                //            SystemModuleTag = i.SystemModuleTag,
                //            FieldID = i.FieldID,
                //            CustomTranslation = i.CustomTranslation,
                //            IsVisible = i.IsVisible,
                //            IsMandatory = i.IsMandatory
                //        };

                //        result = FormatsService.EntityFieldUpsert(field, BaseGenericRequest);
                //    }
                //}

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
        public JsonResult SaveNewRule(string RuleName, string RulesID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                // result = FormatsService.Upsert(FormatID, FormatName, Use2FA, DirectApproval, HasDetail, HasAttachment, FileID, BaseGenericRequest);                
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
        public JsonResult SetPDFFilesTempQuickUpdate(int FileDetailTempID, string ColumnName, string Value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = PDFFileService.PDFFilesDetail_TEMP_QuickUpdate(FileDetailTempID, ColumnName, Value, BaseGenericRequest);                
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
        public JsonResult CreateNewFormat(string FormatName, int Use2FA, int DirectApproval, int HasDetail, int HasAttachment, int? FileID, int Enabled)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.CreateNewFormat(FormatName, Use2FA, DirectApproval, HasDetail, HasAttachment, FileID, Enabled, BaseGenericRequest);
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
        public JsonResult SaveNewDetail(int? FormatGenericDetailTempID, int FormatID, string NameES, string NameEN, string Description, int DataTypeID, int FieldLength, int FieldPrecision,
            string CatalogTag, int IsMandatory, int Enabled, Guid TransactionID)
        {

            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatGenericDetailUpsert(FormatGenericDetailTempID, FormatID, NameES, NameEN, Description, DataTypeID, FieldLength, FieldPrecision, CatalogTag, IsMandatory, Enabled, TransactionID, BaseGenericRequest);
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
        public JsonResult DeleteDetail(int FormatID, string ColumnName, Guid TransactionID)
        {

            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatGenericDetailTempDelete(FormatID, ColumnName, TransactionID, BaseGenericRequest);
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
        public JsonResult FormatsLoopsRulesDelete(int FormatLoopRuleTempID)
        {

            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatsLoopsRules_TEMP_Delete(FormatLoopRuleTempID, BaseGenericRequest);
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



        public JsonResult CreateCatalogDetail(string ValueID, string Param1, Guid Transaction)
        {
            GenericReturn result = new GenericReturn();
            string ErrorMessage = "";
            try
            {
                int CatalogID = MiscellaneousService.Catalog_GetID(BaseGenericRequest.FacilityID, "FlowPhase", out ErrorMessage); 
                    //vw_CatalogService.List("FlowPhase", BaseGenericRequest).FirstOrDefault().CatalogID;

                CatalogDetail catalogDetail = new CatalogDetail()
                {
                    CatalogID = CatalogID,
                    ValueID = ValueID,
                    Param1 = Param1,
                };

                result = FormatsService.AddNewFlowPhase(catalogDetail, Transaction, BaseGenericRequest);
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

        public ActionResult GetCatalogsDetail(string FormatID, Guid TransactionID)
        {
            List<CatalogDetailTemp> model = new List<CatalogDetailTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_CatalogsDetailTable.cshtml";
            string ErrorMessage = "";
            try
            {
                int CatalogID = MiscellaneousService.Catalog_GetID(VARG_FacilityID, "FlowPhase", out ErrorMessage);
                    //vw_CatalogService.List("FlowPhase", BaseGenericRequest).FirstOrDefault().CatalogID;
                // Listados para los combos
                model = CatalogDetailService.List4FormatPhase(CatalogID, FormatID.ToString(), TransactionID, BaseGenericRequest);
            }
            catch (Exception)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }
        public ActionResult GetAccessFormat(string FormatID, Guid TransactionID)
        {
            List<FormatAccessTemp> model = new List<FormatAccessTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatAccess.cshtml";
            try
            {
                model = FormatsService.GetFormatAccessTemp_List(TransactionID, BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }
        public ActionResult GetPDFFieldConfiguration(int FormatID, Guid TransactionID)
        {
            var model = new CreateViewModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_PDFFieldConfiguration.cshtml";
            int FileID = 0;
            try
            {
                PDFFile PDFData = PDFFileService.PDFFiles_GetPDFDetail(FormatID, BaseGenericRequest);
                model.PDFFile = PDFData;
                if (PDFData != null)
                {
                    FileID = PDFData.FileID;
                }
                model._PDFAdditionalFields = FormatsService.PDFFilesDetail_TEMP_List4Header(TransactionID, FileID, BaseGenericRequest);
                model._FontList = new SelectList(vw_CatalogService.List("eReq_Fonts", BaseGenericRequest), "ValueID", "DisplayText");
                model._FontListFE = vw_CatalogService.List("eReq_Fonts", BaseGenericRequest);
                model._FontColorList = vw_CatalogService.List("eReq_FontColor", BaseGenericRequest);
                model._ValueToShow = vw_CatalogService.List("PDF_Value_To_Show", BaseGenericRequest);
                model._ShowWhen = vw_CatalogService.List("PDF_Show_When", BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }
        public ActionResult GetPDFFieldDetailConfiguration(int FormatID, Guid TransactionID)
        {
            var model = new CreateViewModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_PDFFieldDetail.cshtml";
            int FileID = 0;
            try
            {
                PDFFile PDFData = PDFFileService.PDFFiles_GetPDFDetail(FormatID, BaseGenericRequest);
                model.PDFFile = PDFData;
                if (PDFData != null)
                {
                    FileID = PDFData.FileID;
                }
                model._PDFDetailFields = FormatsService.PDFFilesDetail_TEMP_List4Detail(TransactionID, FileID, BaseGenericRequest);
                model._FontList = new SelectList(vw_CatalogService.List("eReq_Fonts", BaseGenericRequest), "ValueID", "DisplayText");
                model._FontListFE = vw_CatalogService.List("eReq_Fonts", BaseGenericRequest);
                model._FontColorList = vw_CatalogService.List("eReq_FontColor", BaseGenericRequest);
                model._ValueToShow = vw_CatalogService.List("PDF_Value_To_Show", BaseGenericRequest);
                model._ShowWhen = vw_CatalogService.List("PDF_Show_When", BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }
        public ActionResult GetPDFFieldConfigurationSignature(int FormatID, Guid TransactionID)
        {
            var model = new CreateViewModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_PDFConfigurationSignature.cshtml";
            int FileID = 0;
            try
            {
                PDFFile PDFData = PDFFileService.PDFFiles_GetPDFDetail(FormatID, BaseGenericRequest);
                model.PDFFile = PDFData;
                if (PDFData != null)
                {
                    FileID = PDFData.FileID;
                }
                model._PDFConfigurationSignature = FormatsService.PDFFilesDetail_TEMP_List4Signatures(TransactionID, PDFData.FileID, BaseGenericRequest);
                model._FontList = new SelectList(vw_CatalogService.List("eReq_Fonts", BaseGenericRequest), "ValueID", "DisplayText");
                model._FontListFE = vw_CatalogService.List("eReq_Fonts", BaseGenericRequest);
                model._FontColorList = vw_CatalogService.List("eReq_FontColor", BaseGenericRequest);
                model._ValueToShow = vw_CatalogService.List("PDF_Value_To_Show", BaseGenericRequest);
                model._ShowWhen = vw_CatalogService.List("PDF_Show_When", BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }
        public ActionResult GetAdditionalFieldTable(int FormatID)
        {
            List<EntityField> model = new List<EntityField>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsFields.cshtml";
            try
            {
                var eReqFieldConf = vw_CatalogService.List4Select("eReq_FieldConfList", BaseGenericRequest, true);
                if (eReqFieldConf.Count == 1)
                {
                    string ErrorMessage;
                    var SystemModuleTag = eReqFieldConf[0].ValueID;
                    var EntityIndicator = MiscellaneousService.Catalog_GetDetailID(VARG_FacilityID, "SystemEntities", "eReqFormat", out ErrorMessage);
                    model = RequestService.FieldsConfigurationList(FormatID.ToInt(), EntityIndicator, SystemModuleTag, BaseGenericRequest);
                }
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }

        public ActionResult UpdateTblFormatLoopRules(int FacilitySelectID, Guid TransactionID)
        {
            List<FormatsLoopsRulesTemp> model = new List<FormatsLoopsRulesTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsLoopRules.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsRulesTemp_List(TransactionID, FacilitySelectID, BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }
        public ActionResult GetTableFormatsLoopRules(int FacilityID, Guid TransactionID)
        {
            List<FormatsLoopsRulesTemp> model = new List<FormatsLoopsRulesTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsLoopRules.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsRulesTemp_List(TransactionID, FacilityID, BaseGenericRequest);
            }
            catch (Exception e)
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(ViewPath, model);
            }
            return View(ViewPath, model);
        }

        public ActionResult GetCatalogsDetailCultures(int CatalogDetailTempID, int CatalogDetailID)
        {
            List<TranslationDetail> model = new List<TranslationDetail>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_CatalogsDetailsCultureTable.cshtml";
            try
            {
                // Listados para los combos
                model = FormatsService.GetTranslationList(CatalogDetailTempID, CatalogDetailID, BaseGenericRequest);
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

        public ActionResult GetAddUserModal()
        {
            Administration.Models.ViewModels.Users.CreateViewModel model = new Administration.Models.ViewModels.Users.CreateViewModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_AddNewUser.cshtml";
            try
            {
                // Listados para los combos
                var Profiles = ProfileService.List(new Profile { }, BaseGenericRequest, true);
                var Departments = DepartmentService.List4Select(BaseGenericRequest);
                var Shifts = ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll);
                var Cultures = vw_CatalogService.List4Select("SystemCultures", BaseGenericRequest, true);

                model.ProfilesList = new SelectList(Profiles, "ProfileID", "ProfileName");
                model.DepartmentsList = new SelectList(Departments, "DepartmentID", "DepartmentName");
                model.ShiftsList = new SelectList(Shifts, "ShiftID", "shiftDescription");
                model.CulturesList = new SelectList(Cultures, "ValueID", "DisplayText");
                model.IsModal = true;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetAddNewRuleModal(int? FormatLoopRuleTempID, string FormatID, Guid TransactionID)
        {
            MoNewRuleModel model = new MoNewRuleModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_New_Rule.cshtml";
            string ErrorMessage = "";
            try
            {
                int CatalogID = MiscellaneousService.Catalog_GetID(VARG_FacilityID, "FlowPhase", out ErrorMessage); 
                // vw_CatalogService.List("FlowPhase", BaseGenericRequest).FirstOrDefault().CatalogID;
                if (FormatLoopRuleTempID != null)
                {
                    model._CatalogDetailTemp = CatalogDetailService.List4FormatPhaseEdit(FormatLoopRuleTempID, CatalogID, FormatID, TransactionID, BaseGenericRequest);
                    model.IsEdit = true;
                    model.FormatLoopRuleTempID = FormatLoopRuleTempID.ToInt();
                    model.RuleName = FormatsService.FormatsLoopsRules_TEMP_List(FormatLoopRuleTempID.ToInt(), BaseGenericRequest).FirstOrDefault().Description;
                }
                else
                {
                    model._CatalogDetailTemp = CatalogDetailService.List4FormatPhase(CatalogID, FormatID, TransactionID, BaseGenericRequest);
                }

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

         public ActionResult GetAddNewPDFDetailConfigureModal(Guid TransactionID)
        {
            List<FormatGenericDetailTemp> model = new List<FormatGenericDetailTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_New_PDFFieldDetailConfigure.cshtml";           
            try
            {
                model = FormatsService.FormatGenericDetailTemp_List4PDFConfiguration(TransactionID,BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetAddNewPDFSignatureConfigureModal(Guid TransactionID)
        {
            List<CatalogDetailTemp> model = new List<CatalogDetailTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_New_PDFFieldSignatures.cshtml";           
            try
            {
                model = FormatsService.CatalogsDetailTemp_List4ConfigurationPDF(TransactionID,BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetCreateRuleStepsModal(int FormatLoopRuleTempID, int FormatID, Guid TransactionID)
        {
            AddNewRuleModel model = new AddNewRuleModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_Create_Rule_Step1.cshtml";
            try
            {
                model._ComparatorType = new SelectList(vw_CatalogService.List4Select("eReq_Coparison_OP", BaseGenericRequest, true), "CatalogDetailID", "ValueID");
                model._DatePart = new SelectList(vw_CatalogService.List4Select("eReq_Date_Part", BaseGenericRequest, true), "ValueID", "DisplayText");
                model.FormatLoopRuleTempID = FormatLoopRuleTempID;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetAddNewApprover(int FormatLoopFlowTempID, int? FormatLoopFlowApproverTempID, int SelectedFacility, string FormatID, Guid TransactionID)
        {
            MoNewApproverModel model = new MoNewApproverModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_New_Approver.cshtml";
            bool IsEdit = false;
            var DataUser = new User();
            FormatsLoopsApprovers_TEMP Data = new FormatsLoopsApprovers_TEMP();
            try
            {
                var DepartmentList = DepartmentService.List4Facility(SelectedFacility, BaseGenericRequest);
                if (DepartmentList.Count > 1)
                {
                    DepartmentList.Insert(0, new Department { DepartmentID = 0, DepartmentName = Resources.Request.lbl_AllDepartments });
                }
                if (FormatLoopFlowApproverTempID != null)
                {
                    Data = FormatsService.FormatsLoopsAprovers_TEMP_List4ID(FormatLoopFlowTempID, BaseGenericRequest);
                    DataUser = UserService.GetInfo(Data.ApproverID.ToString(), BaseGenericRequest);
                    model._SelectListConfiguracion = new SelectList(vw_CatalogService.List4Select("eReq_ApproverMenu", BaseGenericRequest, true), "ValueID", "DisplayText");
                    model._SelectListDepartments = new SelectList(DepartmentList, "DepartmentID", "DepartmentName", Data.DepartmentID);
                    model._SelectListPosition = new SelectList(vw_CatalogService.List4Select("eReq_JobsPositions", BaseGenericRequest, true), "CatalogDetailID", "DisplayText", Data.JobPositionID);
                    model._SelectListUoM = new SelectList(vw_CatalogService.List4Select("UoMeRequest", BaseGenericRequest, true), "CatalogDetailID", "DisplayText", Data.ToleranceUoM);
                    model.FormatLoopFlowApproverTempID = FormatLoopFlowApproverTempID.ToInt();

                    IsEdit = true;
                }
                else
                {
                    model._SelectListConfiguracion = new SelectList(vw_CatalogService.List4Select("eReq_ApproverMenu", BaseGenericRequest, true), "ValueID", "DisplayText");
                    model._SelectListDepartments = new SelectList(DepartmentList, "DepartmentID", "DepartmentName");
                    model._SelectListPosition = new SelectList(vw_CatalogService.List4Select("eReq_JobsPositions", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                    model._SelectListUoM = new SelectList(vw_CatalogService.List4Select("UoMeRequest", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                }
                model.IsEdit = IsEdit;
                model.FormatLoopFlowTempID = FormatLoopFlowTempID;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetNewUserAccessModal()
        {
            IEnumerable<SelectListItem> model = new SelectList(new List<Facility>());
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_NewAccessFormat.cshtml";
            try
            {
                model = new SelectList(RequestService.GetFacility(BaseGenericRequest), "FacilityID", "FacilityName");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetUserList(string FacilitiesList)
        {
            List<User> model = new List<User>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_List_UsersAccess.cshtml";
            try
            {
                model = FormatsService.UsersFacilitiesPermission(FacilitiesList, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult CreateUser(string UserAccountID, string eMail, string EmployeeNumber,
                                  string FirstName, string LastName, string ProfileID, int DepartmentID, int ShiftID, string DefaultCultureID, string ProfileArrayID)
        {
            int ErrorCode = 0, ID = 0;
            string ErrorMessage = "";

            if (!string.IsNullOrEmpty(ProfileArrayID))
            {
                ProfileArrayID = ProfileArrayID.TrimEnd(',');
            }
            User user = new User()
            {
                eMail = eMail,
                UserAccountID = UserAccountID,
                EmployeeNumber = EmployeeNumber,
                FirstName = FirstName,
                LastName = LastName,
                Enabled = true,
                DepartmentID = DepartmentID,
                ShiftID = ShiftID,
                DefaultCultureID = DefaultCultureID,
                BaseFacilityID = VARG_FacilityID,
                FacilityID = VARG_FacilityID,
                ChangedBy = VARG_UserID,
                CultureID = VARG_CultureID,
                ProfileArrayIds = ProfileArrayID
            };

            var result = UserService.Add(user);

            ID = result.ID;
            ErrorCode = result.ErrorCode;
            ErrorMessage = result.ErrorMessage;

            return Json(new
            {
                ErrorCode,
                ErrorMessage,
                Title = "",
                notifyType = ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateNewRule(List<t_FormatsLoopsFlow> FormatsLoopsFlow, int FacilitySelectedID, Guid TransactionID, int FormatID, string Description)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.CreateRule(FormatsLoopsFlow, FacilitySelectedID, TransactionID, FormatID, Description, BaseGenericRequest);
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
        public JsonResult CreateNewRuleDescription(Guid TransactionID, int FormatLoopRuleTempID, int FieldID, int IsAdditionalField, string DatePartArgument, int ComparisonOperator, string RuleDetailType, decimal? Seq, string ValuesArray, int FacilityID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.FormatsLoopsRulesDetail_TEMP_Create(TransactionID, FormatLoopRuleTempID, FieldID, IsAdditionalField, DatePartArgument, ComparisonOperator, RuleDetailType, Seq, ValuesArray, FacilityID, BaseGenericRequest);
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
        public JsonResult GetFormats_GenericFieldData(int CatalogDetailID, int FacilityID)
        {
            string DataTypeValue = "";
            int IsAdditionalField = 0;
            string CatalogTag = "";
            int TableAdditionalFieldID = 0;
            try
            {
                FormatsService.Formats_GenericFieldData(CatalogDetailID, FacilityID, out DataTypeValue, out IsAdditionalField, out CatalogTag, out TableAdditionalFieldID);
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                DataTypeValue,
                IsAdditionalField,
                CatalogTag,
                TableAdditionalFieldID
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFormats_GetFieldData4ValueID(string ValueID)
        {
            string DataTypeValue = "";
            int IsAdditionalField = 0;
            string CatalogTag = "";
            int TableAdditionalFieldID = 0;
            try
            {
                FormatsService.Formats_GetFieldData4ValueID(ValueID, BaseGenericRequest.FacilityID, BaseGenericRequest.CultureID, out DataTypeValue, out IsAdditionalField, out CatalogTag, out TableAdditionalFieldID);
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                DataTypeValue,
                IsAdditionalField,
                CatalogTag,
                TableAdditionalFieldID
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCatalogList(string CatalogTag)
        {
            List<Catalog> SelectList = new List<Catalog>();
            try
            {
                SelectList = vw_CatalogService.List(CatalogTag, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                SelectList
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditRule(int FormatLoopRuleTempID, List<t_FormatsLoopsFlow> FormatsLoopsFlow, int FacilitySelectedID, Guid TransactionID, int FormatID, string Description)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.EditRules(FormatLoopRuleTempID, FormatsLoopsFlow, FacilitySelectedID, TransactionID, FormatID, Description, BaseGenericRequest);
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
        public JsonResult PDFFilesDetail_TEMP_Add(Guid TransactionID, int FormatID, string FieldNames, string FieldType)
        {
            GenericReturn result = new GenericReturn(); 
            int FileID = 0;
            try
            {
                PDFFile PDFData = PDFFileService.PDFFiles_GetPDFDetail(FormatID, BaseGenericRequest);
                if (PDFData != null)
                {
                    FileID = PDFData.FileID;
                }
                result = FormatsService.PDFFilesDetail_TEMP_Add(TransactionID, FileID, FieldNames, FieldType, BaseGenericRequest);
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
        public JsonResult PDFFilesDetail_TEMP_HeaderInsert(Guid TransactionID, int FormatID, string FieldName, string FieldType)
        {
            GenericReturn result = new GenericReturn();
            int FileID = 0;
            try
            {
                PDFFile PDFData = PDFFileService.PDFFiles_GetPDFDetail(FormatID, BaseGenericRequest);
                if (PDFData != null)
                {
                    FileID = PDFData.FileID;
                }
                result = PDFFileService.PDFFilesDetail_TEMP_Insert(TransactionID, FileID, FieldName, FieldType, BaseGenericRequest);
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
        public ActionResult GetFlowTable(int FormatLoopRuleTempID)
        {
            List<FormatsLoopsFlow_TEMP> model = new List<FormatsLoopsFlow_TEMP>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsLoopsFlow.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsFlow_TEMP_List(FormatLoopRuleTempID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetFormatsLoopsRulesDetailTable(int FormatLoopRuleTempID, Guid TransactionID, int FormatID)
        {
            Tbl_Format_LoopRuleDetailModel model = new Tbl_Format_LoopRuleDetailModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_Format_LoopRuleDetail.cshtml";
            try
            {
                model._ListFormatLoopsRulesDetail_Temp = FormatsService.FormatsLoopsRulesDetail_TEMP_List4Edit(FormatLoopRuleTempID, TransactionID, FormatID, BaseGenericRequest);
                model.TransactionID = TransactionID;
                model.FormatID = FormatID;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetFormatsLoopsRulesDetailTableException(int FormatLoopRuleTempID, Guid TransactionID, int FormatID, Decimal Seq)
        {
            List<FormatsLoopsRulesDetail_TEMP> model = new List<FormatsLoopsRulesDetail_TEMP>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/Ul_FormatsLoopsRulesDetail_TEMP_List4EditExceptions.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsRulesDetail_TEMP_List4EditExceptions(FormatLoopRuleTempID, TransactionID, FormatID, Seq, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetApproversTable(int FormatLoopFlowTempID)
        {
            List<FormatsLoopsApprovers_TEMP> model = new List<FormatsLoopsApprovers_TEMP>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsLoopsApprovers.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsAprovers_TEMP_List(FormatLoopFlowTempID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetSearchUserModal(string Search)
        {
            Administration.Models.ViewModels.Users.CreateViewModel model = new Administration.Models.ViewModels.Users.CreateViewModel();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_SearchUser.cshtml";
            try
            {
                // Listados para los combos
                var Profiles = ProfileService.List(new Profile { }, BaseGenericRequest, true);
                var Departments = DepartmentService.List4Select(BaseGenericRequest);
                var Shifts = ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll);
                var Cultures = vw_CatalogService.List4Select("SystemCultures", BaseGenericRequest, true);

                model.ProfilesList = new SelectList(Profiles, "ProfileID", "ProfileName");
                model.DepartmentsList = new SelectList(Departments, "DepartmentID", "DepartmentName");
                model.ShiftsList = new SelectList(Shifts, "ShiftID", "shiftDescription");
                model.CulturesList = new SelectList(Cultures, "ValueID", "DisplayText");
                model.IsModal = true;
                model.Search = Search;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public JsonResult SearchADUsers(string UserText)
        {
            List<User> UserList = new List<User>();
            string viewPath = "~/Areas/eRequest/Views/Formats/_Tbl_ResponsiblesResult.cshtml";
            try
            {
                Utilities.Security security = new Utilities.Security();
                var DefaultCultureID = MiscellaneousService.Param_GetValue(0, "DefaultCultureID", "EN-US");

                var FullUserInfo = security.GetCurrentUser();
                var UserMachineInfo = FullUserInfo.Split('\\');
                var MachineDomainName = UserMachineInfo[0];
                var MachineUserName = UserMachineInfo[1];

                try
                {
                    // Variables a utilizar
                    string URI = string.Format("{0}://{1}", "LDAP", MachineDomainName);
                    string ldapFilter = "";
                    DirectoryEntry Entry = new DirectoryEntry(URI);
                    DirectorySearcher Searcher = new DirectorySearcher(Entry, ldapFilter);

                    //Consultar por correo
                    ldapFilter = "(userprincipalname=*" + UserText.Replace(" ", "*") + "*)";
                    Entry.RefreshCache();
                    Searcher = new DirectorySearcher(Entry, ldapFilter);

                    foreach (SearchResult result in Searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetDirectoryEntry();
                        //Si el registro no está en la lista, agregarlo
                        var User = new User
                        {
                            UserAccountID = de.Properties["samAccountName"].Value.ToString(),
                            FirstName = de.Properties["name"].Value.ToString(),
                            eMail = de.Properties["userprincipalname"].Value.ToString(),
                            ChangedBy = VARG_UserID,
                            CultureID = DefaultCultureID
                        };
                        UserList.Add(User);
                    }

                    //Consultar por name
                    ldapFilter = "(name=*" + UserText.Replace(" ", "*") + "*)";
                    Entry.RefreshCache();
                    Searcher = new DirectorySearcher(Entry, ldapFilter);

                    foreach (SearchResult result in Searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetDirectoryEntry();

                        var matchList = UserList.Select(u => u.UserAccountID == de.Properties["samAccountName"].Value.ToString()).ToList();
                        //Si el registro no está en la lista, agregarlo
                        if (matchList.Count() == 0)
                        {
                            var User = new User
                            {
                                UserAccountID = de.Properties["samAccountName"].Value.ToString(),
                                FirstName = de.Properties["name"].Value.ToString(),
                                eMail = de.Properties["userprincipalname"].Value.ToString(),
                                ChangedBy = VARG_UserID,
                                CultureID = DefaultCultureID
                            };
                            UserList.Add(User);
                        }
                    }

                    // Si se tecleo usuario y apellido, invertirlos y realizar la busqueda
                    if (UserText.Contains(" "))
                    {
                        string TextSearch = "";
                        foreach (var item in UserText.Split(' '))
                        { TextSearch = item + "*" + TextSearch; }

                        if (!string.IsNullOrEmpty(TextSearch))
                        {
                            ldapFilter = "(name=*" + TextSearch + "*)";
                            Entry.RefreshCache();
                            Searcher = new DirectorySearcher(Entry, ldapFilter);

                            foreach (SearchResult result in Searcher.FindAll())
                            {
                                DirectoryEntry de = result.GetDirectoryEntry();
                                var matchList = UserList.Select(u => u.UserAccountID == de.Properties["samAccountName"].Value.ToString()).ToList();
                                //Si el registro no está en la lista, agregarlo
                                if (matchList.Count() == 0)
                                {
                                    var User = new User
                                    {
                                        UserAccountID = de.Properties["samAccountName"].Value.ToString(),
                                        FirstName = de.Properties["name"].Value.ToString(),
                                        eMail = de.Properties["userprincipalname"].Value.ToString(),
                                        ChangedBy = VARG_UserID,
                                        CultureID = DefaultCultureID
                                    };
                                    UserList.Add(User);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }

            return Json(new
            {
                View = RenderRazorViewToString(viewPath, UserList)
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddResponsibleAccount(User User)
        {
            GenericReturn result = new GenericReturn();
            var DefaultCultureID = MiscellaneousService.Param_GetValue(0, "DefaultCultureID", "EN-US");
            var ExistUser = UserService.GetUserID(User.UserAccountID);
            try
            {
                if (ExistUser == 0)
                {
                    result = UserService.AddNewUSerAccount(User, new Core.Entities.GenericRequest() { FacilityID = 0, CultureID = DefaultCultureID });
                }
                else if (ExistUser > 0)
                {
                    result.ErrorCode = 0;
                    result.ErrorMessage = Resources.Common.ntf_ValidUser;
                    result.ID = ExistUser;
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveNewApprover(int FormatLoopFlowTempID, Guid TransactionID, int DepartmentOriginID, int JobPositionID, int DepartmentID, string ApproverIDs, int AddTolerance, int Tolerance, int ToleranceUoM)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = FormatsService.AddFormatsLoopsApprovers(FormatLoopFlowTempID, TransactionID, DepartmentOriginID, JobPositionID, DepartmentID, ApproverIDs, AddTolerance, Tolerance, ToleranceUoM, BaseGenericRequest);
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
        public ActionResult GetNewFormatModal()
        {
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Mo_New_Format.cshtml";
            //try
            //{

            //}
            //catch (Exception e)
            //{
            //    ViewBag.ErrorMessage = e.Message;
            //}
            return PartialView(ViewPath);
        }

        public ActionResult GetTblFormatsLoopsRulesDetail(int FormatID, int FormatLoopRuleTempID, Guid TransactionID, decimal Seq, int FacilityID)
        {
            List<FormatsLoopsRulesDetail_TEMP> model = new List<FormatsLoopsRulesDetail_TEMP>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsLoopsRulesDetail_4EditRules.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsRulesDetail_TEMP_List4EditRules(FormatID, FormatLoopRuleTempID, TransactionID, Seq, FacilityID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetTblFormatsLoopsRulesDetailExceptions(int FormatID, int FormatLoopRuleTempID, Guid TransactionID, decimal Seq, int FacilityID)
        {
            List<FormatsLoopsRulesDetail_TEMP> model = new List<FormatsLoopsRulesDetail_TEMP>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsLoopsRulesDetail_TEMP_4Exceptions.cshtml";
            try
            {
                model = FormatsService.FormatsLoopsRulesDetail_TEMP_List4EditRulesExceptions(FormatID, FormatLoopRuleTempID, TransactionID, Seq, FacilityID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetFormatMenu(int FormatID)
        {
            int model = 0;
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatMenu.cshtml";
            try
            {
                model = FormatID;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult UploadFilesTemp(Guid TransactionID)
        {
            string fName = "";
            string AttachmentDirectory = "";
            string pathString = "";
            string pathReturn = "";
            GenericReturn result = new GenericReturn();
            try
            {

                AttachmentDirectory = "TempFiles\\Formats";
                var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                pathString = Path.Combine(originalDirectory.ToString(), TransactionID.ToString("N"));

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        pathReturn = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(pathReturn);
                        pathReturn = Wrappers.GetVirtualPath4File(pathReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message.ToString();
            }
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID,
                pathReturn,
                fName,
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFormatsFieldsTable(int FormatID)
        {
            List<EntityField> model = new List<EntityField>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatsFieldsOnlyRead.cshtml";
            try
            {
                model = FormatsService.FormatsListGeneralFields(FormatID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }        
        public ActionResult GetFormatsDetailFieldsTable(int FormatID)
        {
            List<FormatGenericDetailTemp> model = new List<FormatGenericDetailTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_DetailFieldsReadOnly.cshtml";
            try
            {
                model = FormatsService.FormatGenericDetailField_List(FormatID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetListPhasesTable(int FormatID)
        {
            List<CatalogDetailTemp> model = new List<CatalogDetailTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_CatalogsDetailReadOnly.cshtml";
            try
            {
                model = FormatsService.Formats_ListPhases(FormatID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        } 
        public ActionResult GetFormatAccessListTable(int FormatID)
        {
            List<FormatAccessTemp> model = new List<FormatAccessTemp>();
            string ViewPath = "~/Areas/eRequest/Views/Formats/_Tbl_FormatAccessReadOnly.cshtml";
            try
            {
                model = FormatsService.FormatAccess_List(FormatID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
    }

}