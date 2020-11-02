using Core.Entities;
using Core.Service;
using Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Filters;
using WebSite.Models.ViewModels.Attachments;
using WebSite.Utilities;
using static WebSite.Models.StaticModels;

namespace WebSite.Controllers
{
    [SessionExpire]
    public class AttachmentsController : BaseController
    {
        public ActionResult SaveDropzoneJsUploadedFiles(int ReferenceID, string AttachmentType, int? CompanyID)
        {
            int? ReferenceType = 0;
            string RelativePath = "";
            var Reference = vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
            if (Reference != null)
            {
                ReferenceType = Reference.CatalogDetailID;
            }
            string fName = "";
            string AttachmentDirectory = "";
            string pathString = "";
            GenericReturn result = new GenericReturn();
            string[] filesnames = new string[Request.Files.Count];
            string[] filespath = new string[Request.Files.Count];
            int index = 0;
            try
            {
                switch (AttachmentType.ToUpper())
                {
                    case "RECEIPTID":
                        {
                            AttachmentDirectory = "Receiving";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "PARTS":
                        {
                            AttachmentDirectory = "Parts";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "SHIPMENTID":
                        {
                            AttachmentDirectory = "Shipments";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "SHIPPINGORDERID":
                        {
                            AttachmentDirectory = "ShippingsOrders";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "INVOICEID":
                        {
                            AttachmentDirectory = "Invoices";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "CERFILEPATH":
                    case "KEYFILEPATH":
                        {
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Companies\\" + ReferenceID.ToString() + "\\CFDI", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), "");
                            break;
                        }
                    case "LOGOURL":
                        {
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Companies\\" + ReferenceID.ToString() + "\\Logo", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), "");
                            break;
                        }
                    case "PURCHASEORDERID":
                        {
                            AttachmentDirectory = "PurchaseOrders";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "SALESORDERID":
                        {
                            AttachmentDirectory = "SalesOrders";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "TEMPID":
                    case "TEMPKIOSKBACKGROUND":
                        {
                            AttachmentDirectory = "TempFiles";
                            RelativePath = "\\Files\\" + AttachmentDirectory + "\\" + ReferenceID.ToString() + "\\";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "TMSORDERID":
                        {
                            AttachmentDirectory = "TMS_Orders";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "ENERGYSENSORID":
                        {
                            AttachmentDirectory = "EnergySensorsFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensors\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "ENERGYSENSORFAMILYID":
                        {
                            AttachmentDirectory = "EnergySensorFamiliesFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensorFamilies\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "OPPORTUNITYPROGRAMMEDIA":
                        {
                            AttachmentDirectory = "OpportunitiesProgramFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\OpportunitiesProgramFiles\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
					case "IDENTIFICATIONTYPES":
                        {
                            AttachmentDirectory = "IdentificationTypesFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\IdentificationTypesFiles\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "TOOLSIMG":
                        {
                            AttachmentDirectory = "ToolsImagesFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\ToolsImagesFiles\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
 					case "EREQUESTMEDIA":
                        {
                            AttachmentDirectory = "eRequestFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\eREQ\\eRequestFiles\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }                    default:
                        break;
                }
                
                foreach (string fileName in Request.Files)
                {
                    
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        //var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\"+ AttachmentDirectory + "\\", Server.MapPath(@"\")));                        
                        //pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                       
                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        filesnames[index] = fName;
                        filespath[index] = path;

                        result = AttachmentService.Insert(ReferenceID, AttachmentType, ReferenceType, CompanyID, 0, Extensions.GetVirtualPath4File(path), BaseGenericRequest);
                        if (result.ErrorCode == 0)
                        {
                            file.SaveAs(path);
                        }


                    }

                    index++;
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
                filesnames,
                filespath,
                RelativePath
            }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Get(int ReferenceID, string AttachmentType, int? CompanyID, bool? EnableFileType = true, bool? EnableDelete = true)
        {
            try
            {
                IEnumerable<IAttachment> AttachmentList = Enumerable.Empty<IAttachment>();
                List<Catalog> FileTypeList = new List<Catalog>();
                int? ReferenceType = 0;
                var Reference = vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
                if (Reference != null)
                {
                    ReferenceType = Reference.CatalogDetailID;
                }

                switch (AttachmentType.ToUpper())
                {
                    case "RECEIPTID":
                        {
                            FileTypeList = Core.Service.vw_CatalogService.List4Select("WMS_AttachmentsFileTypes", BaseGenericRequest);
                            break;
                        }
                    case "PARTS":
                        {
                            FileTypeList = Core.Service.vw_CatalogService.List4Select("FileTypesPartsNumber", BaseGenericRequest);
                            break;
                        }
                    case "SHIPMENTID":
                        {
                            FileTypeList = Core.Service.vw_CatalogService.List4Select("FileTypeShipments", BaseGenericRequest);
                            break;
                        }
                    case "SHIPPINGORDERID":
                        {
                            FileTypeList = Core.Service.vw_CatalogService.List4Select("FileTypeShippingOrders", BaseGenericRequest);
                            break;
                        }
                    case "INVOICEID":
                        {
                            FileTypeList = Core.Service.vw_CatalogService.List4Select("AR_AttachmentsFileTypes", BaseGenericRequest);
                            break;
                        }
                    case "PURCHASEORDERID":
                        {
                            FileTypeList = vw_CatalogService.List4Select("FileTypesPO", BaseGenericRequest);
                            break;
                        }
                    case "SALESORDERID":
                        {
                            FileTypeList = vw_CatalogService.List4Select("FileTypesSD", BaseGenericRequest);
                            break;
                        }
                    case "TEMPID":
                    case "TMSORDERID":
                    case "TEMPKIOSKBACKGROUND":
                        {
                            FileTypeList = vw_CatalogService.List4Select("AttachmentTypes", BaseGenericRequest);
                            break;
                        }
                    case "ENERGYSENSORID":
                        {
                            FileTypeList = vw_CatalogService.List4Select("AttachmentTypes", BaseGenericRequest);
                            break;
                        }
                    case "ENERGYSENSORFAMILYID":
                        {
                            FileTypeList = vw_CatalogService.List4Select("AttachmentTypes", BaseGenericRequest);
                            break;
                        }
                    case "OPPORTUNITYPROGRAMMEDIA":
                        {
                            FileTypeList = vw_CatalogService.List4Select("AttachmentTypes", BaseGenericRequest);
                            break;
                        }
                    case "IDENTIFICATIONTYPES":
                        {
                            FileTypeList = vw_CatalogService.List4Select("AttachmentTypes", BaseGenericRequest);
                            break;
                        }
                    case "TOOLSIMG":
                        {
                            FileTypeList = vw_CatalogService.List4Select("AttachmentTypes", BaseGenericRequest);
                            break;
                        }
                    default:
                        break;
                }
                AttachmentList = AttachmentService.List(null, AttachmentType, ReferenceID, ReferenceType, CompanyID, null, null, BaseGenericRequest);
                var model = new AttachmentViewModel
                {
                    Attachments = AttachmentList,
                    FileTypeList = FileTypeList,
                    EnableFileType = EnableFileType,
                    EnableDelete = EnableDelete
                };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AttachmentsTable", model);
                }
                return View("_AttachmentsTable", model);
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

        public ActionResult Download(int FileId, string AttachmentType)
        {
            IEnumerable<IAttachment> model = Enumerable.Empty<IAttachment>();
            int? ReferenceType = 0;
            var Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
            if (Reference != null)
            {
                ReferenceType = Reference.CatalogDetailID;
            }

            model = AttachmentService.List(FileId, AttachmentType, null, ReferenceType, null, null, null, BaseGenericRequest);
            IAttachment Attachment;

            string JSCorefunction = "";
            if (model.ToList().Count != 0)
            {
                Attachment = model.FirstOrDefault();

                string ResourcePathLocal = Uri.EscapeUriString(Attachment.FilePathName);
                string ResourcePath = ResourcePathLocal.Replace("#", Uri.EscapeDataString("#").ToString());

                switch (Extensions.GetAttachmentFile(Attachment.FileName))
                {
                    case AttachmentFile.Image:
                        string Orientation = Extensions.GetOrientationImage(Attachment.FilePathName);
                        JSCorefunction = "ViewResourceImage('" + FileId + "','" + ResourcePath + "','" + Attachment.FileName + "','" + Attachment.UserFullName + "','" + Attachment.DateAdded.ToString("yyyy-MM-dd HH:mm") + "','" + Orientation + "');";
                        break;
                    case AttachmentFile.File:
                        JSCorefunction = "DownloadFile('" + ResourcePath + "');";
                        break;
                    case AttachmentFile.Other:
                        JSCorefunction = "OpenResourceFile('" + ResourcePath + "');";
                        break;
                }
                return Json(new
                {
                    ErrorCode = 0,
                    ErrorMessage = "",
                    Title = "",
                    notifyType = NotifyType.success.ToString(),
                    JSCorefunction

                }, JsonRequestBehavior.AllowGet);

            }

            return Json(new
            {
                ErrorCode = 99,
                ErrorMessage = Resources.Common.ntf_Nofile,
                Title = "",
                notifyType = NotifyType.error.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImage(int ReferenceID, string AttachmentType)
        {
            IEnumerable<IAttachment> model = Enumerable.Empty<IAttachment>();
            int? ReferenceType = 0;
            var Reference = vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
            if (Reference != null)
            {
                ReferenceType = Reference.CatalogDetailID;
            }

            model = AttachmentService.List(null, AttachmentType, ReferenceID, ReferenceType, null, null, null, BaseGenericRequest).OrderByDescending(o => o.FileID);
            IAttachment Attachment;


            if (model.ToList().Count != 0)
            {
                Attachment = model.FirstOrDefault();

                string ResourcePathLocal = Uri.EscapeUriString(Attachment.FilePathName);
                string ResourcePath = ResourcePathLocal.Replace("#", Uri.EscapeDataString("#").ToString());

                return Json(new
                {
                    ErrorCode = 0,
                    ErrorMessage = "",
                    Title = "",
                    notifyType = NotifyType.success.ToString(),
                    ImgSrc = ResourcePath

                }, JsonRequestBehavior.AllowGet);

            }

            return Json(new
            {
                ErrorCode = 99,
                ErrorMessage = Resources.Common.ntf_Nofile,
                Title = "",
                notifyType = NotifyType.error.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int FileId, string AttachmentType, int FileTypeId)
        {
            GenericReturn result = new GenericReturn();
            int? ReferenceType = 0;
            var Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
            if (Reference != null)
            {
                ReferenceType = Reference.CatalogDetailID;
            }

            result = AttachmentService.QuickUpdate(FileId, AttachmentType, null, ReferenceType, null, FileTypeId, null, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(int FileId, string AttachmentType)
        {
            int? ReferenceType = 0;
            var Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
            if (Reference != null)
            {
                ReferenceType = Reference.CatalogDetailID;
            }

            //var model = Core.Service.AttachmentService.List(FileId, AttachmentType, null, ReferenceType, null, null, null, BaseGenericRequest);
            //if (model != null)
            //{
            //    if (System.IO.File.Exists(model.FirstOrDefault().FilePathName))
            //    {
            //        System.IO.File.Delete(model.FirstOrDefault().FilePathName);
            //    }
            //}
            var result = AttachmentService.Delete(FileId, AttachmentType, BaseGenericRequest);

            // Si se borro el registro


            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Copy(int ReferenceID, string AttachmentType, int NewReferenceID, string NewAttachmentType)
        {
            //se mueve la funcionalidad a basecontroler para poder utilizarla en todos los controladores
            return this.CopyAttachments(ReferenceID, AttachmentType, NewReferenceID, NewAttachmentType);
        }

        public ActionResult SaveDropzoneJsUploadedFiles_FT(int ReferenceID, string AttachmentType, int? CompanyID, int FileType)
        {
            int? ReferenceType = 0;
            var Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
            if (Reference != null)
            {
                ReferenceType = Reference.CatalogDetailID;
            }
            string fName = "";
            string AttachmentDirectory = "";
            string pathString = "";
            GenericReturn result = new GenericReturn();
            try
            {
                switch (AttachmentType.ToUpper())
                {
                    case "RECEIPTID":
                        {
                            AttachmentDirectory = "Receiving";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "PARTS":
                        {
                            AttachmentDirectory = "Parts";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "SHIPMENTID":
                        {
                            AttachmentDirectory = "Shipments";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "SHIPPINGORDERID":
                        {
                            AttachmentDirectory = "ShippingsOrders";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "INVOICEID":
                        {
                            AttachmentDirectory = "Invoices";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "CERFILEPATH":
                    case "KEYFILEPATH":
                        {
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Companies\\" + ReferenceID.ToString() + "\\CFDI", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), "");
                            break;
                        }
                    case "LOGOURL":
                        {
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Companies\\" + ReferenceID.ToString() + "\\Logo", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), "");
                            break;
                        }
                    case "PURCHASEORDERID":
                        {
                            AttachmentDirectory = "PurchaseOrders";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "SALESORDERID":
                        {
                            AttachmentDirectory = "SalesOrders";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "TEMPID":
                    case "TEMPKIOSKBACKGROUND":
                        {
                            AttachmentDirectory = "TempFiles";
                            var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "TMSORDERID":
                        {
                            AttachmentDirectory = "TMS_Orders";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "ENERGYSENSORID":
                        {
                            AttachmentDirectory = "EnergySensorsFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensors\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "ENERGYSENSORFAMILYID":
                        {
                            AttachmentDirectory = "EnergySensorFamiliesFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensorFamilies\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    case "OPPORTUNITYPROGRAMMEDIA":
                        {
                            AttachmentDirectory = "OpportunitiesProgramFiles";
                            var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\OpportunitiesProgramFiles\\", Server.MapPath(@"\")));
                            pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                            break;
                        }
                    default:
                        break;
                }

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        //var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\"+ AttachmentDirectory + "\\", Server.MapPath(@"\")));                        
                        //pathString = Path.Combine(originalDirectory.ToString(), ReferenceID.ToString());

                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);

                        result = AttachmentService.Insert(ReferenceID, AttachmentType, ReferenceType, CompanyID, FileType, Extensions.GetVirtualPath4File(path), BaseGenericRequest);
                        if (result.ErrorCode == 0)
                        {
                            file.SaveAs(path);
                        }


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
                result.ID
            }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Properties_QuickUpsert(int? FileId, string PropertyName, string PropertyValue, int? PropertyTypeID, GenericRequest request)
        {
            Catalog Reference = new Catalog();
            GenericReturn result = new GenericReturn();
            try
            {
                result = AttachmentService.Properties_QuickUpdate(FileId, PropertyName, PropertyValue, PropertyTypeID, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

    }
}