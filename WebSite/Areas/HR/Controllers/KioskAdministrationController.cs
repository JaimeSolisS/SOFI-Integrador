using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.HR.Models.ViewModels.KioskAdministration;
using WebSite.Models;
using WebSite.Utilities;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.HR.Controllers
{
    public class KioskAdministrationController : BaseController
    {
        // GET: HR/KioskAdministration
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            try
            {
                var KioskAreaList = KioskAreaService.List(true, BaseGenericRequest);


                if (KioskAreaList != null && KioskAreaList.Count > 0)
                    model.KioskAreaList = KioskAreaList.FindAll(p => p.KioskAreaID > 0).ToList();

                KioskAreaList.Insert(0, new KioskArea { KioskAreaID = 0, Title = Resources.Common.TagAll });

                model.ListAreas = new SelectList(KioskAreaList, "KioskAreaID", "Title");
                model.FileTypeList = new SelectList(vw_CatalogService.List4Select("HRK_FilesTypes", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                int DFT_View = Int32.Parse(MiscellaneousService.Param_GetValue(VARG_FacilityID, "CI_Admin_DFT_View", ""));
                model.DFT_View = DFT_View;
                model.ViewsList = new SelectList(vw_CatalogService.List4Select("CI_SectionViews", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", DFT_View);

            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
            }
            return View(model);
        }

        public ActionResult KioskAreaList(int KioskAreaID, int FileTypeID, int ViewType, bool? IsRoot)
        {
            List<KioskArea> model = new List<KioskArea>();
            List<KioskAreaNode> modelNodes = new List<KioskAreaNode>();
            int? KioskAreaID2 = null;
            int? FileTypeID2 = null;

            if (KioskAreaID > 0) { KioskAreaID2 = KioskAreaID; }
            if (FileTypeID > 0) { FileTypeID2 = FileTypeID; }

            var view = vw_CatalogService.Get(ViewType, BaseGenericRequest);
            if (view != null && view.ValueID == "N")
            {
                modelNodes = KioskAreaDetailService.NodeList(null, null, KioskAreaID2, true, FileTypeID2, null, BaseGenericRequest);
                return PartialView("~/Areas/HR/Views/KioskAdministration/_KioskAreaNodeTable.cshtml", modelNodes);
            }
            else if (view != null && view.ValueID == "S")
            {
                model = KioskAreaService.List(KioskAreaID2, true, FileTypeID2, IsRoot, BaseGenericRequest, false);
                return PartialView("~/Areas/HR/Views/KioskAdministration/_KioskAreaTable.cshtml", model);
            }
            return PartialView("~/Areas/HR/Views/KioskAdministration/_KioskAreaNodeTable.cshtml", model);
        }

        [HttpPost]
        public ActionResult KioskArea_Delete(int KioskAreaID)
        {
            var result = KioskAreaService.Delete(KioskAreaID, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult KioskAreaDetailList(int KioskAreaID, int FileTypeID)
        {
            List<KioskAreaDetail> model;
            int? KioskAreaID2 = null;
            int? FileTypeID2 = null;

            if (KioskAreaID > 0) { KioskAreaID2 = KioskAreaID; }
            if (FileTypeID > 0) { FileTypeID2 = FileTypeID; }

            model = KioskAreaDetailService.List(null, null, KioskAreaID2, true, FileTypeID2, null, BaseGenericRequest);

            return PartialView("~/Areas/HR/Views/KioskAdministration/_KioskAreaDetailTable.cshtml", model);
        }
        public ActionResult KioskAreaNodeList(int KioskAreaID, int FileTypeID, int KioskAreaDetailID)
        {
            List<KioskAreaNode> model = new List<KioskAreaNode>();
            model = KioskAreaDetailService.NodeList(null, KioskAreaDetailID, null, true, FileTypeID, null, BaseGenericRequest);
            return PartialView("~/Areas/HR/Views/KioskAdministration/_KioskAreaNodeDivTable.cshtml", model);
        }

        public ActionResult KioskArea_GeneralSettings()
        {
            GeneralSettingsViewModel model = new GeneralSettingsViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskAdministration/_GeneralSettingsManagement.cshtml";
            try
            {
                int KioskCarouselMediaID = KioskMediaService.GetKioskCarouselID(BaseGenericRequest);
                model.SessionTime = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_KIOSK_CLOSESESSIONAFTER", "0");
                model.TransitionTime = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_KIOSK_CAROUSELTRANSITIONTIME", "0");
                model.imgURL = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_KIOSK_BACKGROUNDIMAGE", "0");
                model.KioskCarouselMediaID = KioskCarouselMediaID;
                model.CarouselMediaList = KioskMediaService.TempList(KioskCarouselMediaID, model.TempAttachmentID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
            }

            return PartialView(ViewPath, model);
        }
        public JsonResult GetCarruselMediaTemp(int KioskCarouselMediaID, int TempAttachmentID)
        {
            List<KioskCarouselMediaTmp> list = new List<KioskCarouselMediaTmp>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/HR/Views/KioskAdministration/_GeneralSettingsAttachmentsTable.cshtml";
            try
            {
                list = KioskMediaService.TempList(KioskCarouselMediaID, TempAttachmentID, BaseGenericRequest);

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
                View = RenderRazorViewToString(ViewPath, list)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KioskAreaDetail_EditInit(int KioskAreaID, int KioskAreaDetailID, bool? IsRoot)
        {
            KioskAreaDetailUpsertViewModel model = new KioskAreaDetailUpsertViewModel();

            // catalogs
            model.SourcePathHidden = "";
            model.KioskAreaDetailInfo = KioskAreaDetailService.Find(KioskAreaDetailID, BaseGenericRequest);
            model.HeaderModal = Resources.CI.DashboardManagement.lbl_DashboardAreaNodeHeaderUpdateTag;
            model.ButtonAcceptModal = Resources.CI.DashboardManagement.btn_UpdateSectionDetailsTag;
            model.FileTypeList = vw_CatalogService.List4Select("HRK_FilesTypes", BaseGenericRequest, true);
            model.FileTypeList[0].DisplayText = Resources.Common.chsn_SelectOption;
            model.KioskSizeList = new SelectList(vw_CatalogService.List4Select("CID_Sizes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.KioskAreaDetailInfo.SizeID);
            model.KioskIconList = new SelectList(vw_CatalogService.List4Select("CID_Icons", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.KioskAreaDetailInfo.IconID);
            model.KioskDataEffectList = new SelectList(vw_CatalogService.List4Select("CID_DataEffect", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.KioskAreaDetailInfo.DataEffectID);
            model.KioskAreaDetailInfo.KioskAreaID = KioskAreaID;
            model.KioskAreaDetailInfo.KioskAreaDetailID = KioskAreaDetailID;
            model.KioskNameTranslate = KioskAreaDetailService.GetNameInfo(KioskAreaDetailID, BaseGenericRequest);
            model.BackgroundColor = model.KioskAreaDetailInfo.BackgroundColor;
            model.FontColor = model.KioskAreaDetailInfo.FontColor;

            if (model.KioskAreaDetailInfo != null && model.KioskAreaDetailInfo.ParentKioskAreaDetailID == 0)
            {
                IsRoot = true;
            }


            if (IsRoot != null)
            {
                if (Convert.ToBoolean(IsRoot))
                {
                    model.HeaderModal = Resources.HR.Kiosk.title_CreateRootNode;
                    model.IsRoot = IsRoot.ToBoolean();
                    model.SizeClass = "col-xs-12";
                }
                else
                {
                    model.HeaderModal = Resources.HR.Kiosk.title_EditRootNode;
                }
            }

            if (model.KioskAreaDetailInfo.FileTypeValueID.ToUpper() == "G")
            {
                model.FileTypeGaleryCss = "";
                model.FileTypeMediaOrFileCss = "";
            }
            else if (model.KioskAreaDetailInfo.FileTypeValueID.ToUpper() == "L")
            {
                model.FileTypeLinkCss = "";
            }
            else
            {
                model.FileTypeMediaOrFileCss = "";
            }

            if (!string.IsNullOrEmpty(model.KioskAreaDetailInfo.BackgroundImage))
            {
                string BackgroundImageTmp = "";
                BackgroundImageTmp = Server.MapPath(model.KioskAreaDetailInfo.BackgroundImage);
                string tmpBackgroundImageTmp = new DirectoryInfo(string.Format("{0}Files\\Temp\\HR\\{1}\\{2}", Server.MapPath(@"\"), model.TransactionID, model.KioskAreaDetailInfo.FileTypeValueID.ToUpper())).ToString();

                if (!Directory.Exists(tmpBackgroundImageTmp))
                    Directory.CreateDirectory(tmpBackgroundImageTmp);

                if (System.IO.File.Exists(BackgroundImageTmp))
                {
                    System.IO.File.Copy(BackgroundImageTmp, tmpBackgroundImageTmp + "\\" + Path.GetFileName(BackgroundImageTmp));
                    model.KioskAreaDetailInfo.BackgroundImage = Wrappers.GetVirtualPath4File(tmpBackgroundImageTmp + "\\" + Path.GetFileName(BackgroundImageTmp));
                }
            }

            //////////////calculo de combos de parents y secciones
            List<KioskArea> sections = new List<KioskArea>();
            List<KioskAreaNode> parents = new List<KioskAreaNode>();
            var ParentOptionChoosen = KioskAreaDetailService.Get(KioskAreaDetailID, BaseGenericRequest).ParentKioskAreaDetailID;

            //model.ParentsNodesList = new SelectList(parents, "KioskAreaDetailID", "Name");
            var area = KioskAreaService.Get(KioskAreaID, IsRoot, BaseGenericRequest);
            if (area != null && area.ParentID != null)
            {
                model.KioskAreaDetailInfo.ParentKioskAreaDetailID = area.ParentID;
                //lista las secciones del padre
                sections = KioskAreaService.List(null, true, null, IsRoot, BaseGenericRequest).Where(w => w.ParentID == area.ParentID).ToList();
                model.SectionsList = new SelectList(sections, "KioskAreaID", "Title", KioskAreaID);

                //listado de los nodos padre y el padre seleccionado por default
                var ParentNode = KioskAreaDetailService.Get(area.ParentID, BaseGenericRequest);
                var ListParentsNodes = KioskAreaDetailService.List(null, null, ParentNode.KioskAreaID, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(ListParentsNodes, "KioskAreaDetailID", "Name", area.ParentID);
            }
            else
            {
                //sacar el parent node del KioskAreaID
                var areaslist = KioskAreaService.List(KioskAreaID, true, null, IsRoot, BaseGenericRequest);
                model.SectionsList = new SelectList(areaslist, "KioskAreaID", "Title", KioskAreaID);
                //sino tiene padre es nodo RAIZ y se muestran todos lo nodos padres en el listado, pero no esta seleccionado ninguno
                parents = KioskAreaDetailService.NodeList(null, null, null, null, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(parents, "KioskAreaDetailID", "Name", ParentOptionChoosen);
            }
            //////////////calculo de combos de parents y secciones

            return PartialView("~/Areas/HR/Views/KioskAdministration/_Mo_KioskAreaDetailUpsert.cshtml", model);
        }

        public ActionResult UploadKioskDetails(string TransactionDetailID)
        {
            GenericReturn result = new GenericReturn();
            string pathString = "";
            string path = "";
            try
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Temp\\HR\\{1}\\Kiosk\\BackgroundImages", Server.MapPath(@"\"), TransactionDetailID));
                pathString = Path.Combine(originalDirectory.ToString(), "");

                // Limpia el directorio cada que cargue un tipo de archivo
                try
                {
                    if (Directory.Exists(pathString))
                    {
                        string[] filePaths = Directory.GetFiles(pathString);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                catch { }

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);
                    }
                    result.ErrorMessage = string.Format(Resources.Common.nft_FileUploaded, file.FileName);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                FilePath = Wrappers.GetVirtualPath4File(path),
                FileName = Path.GetFileName(path)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateQuickEditable(string name, int pk, string value)
        {
            var result = KioskAreaService.QuickUpdate(pk, name, value, VARG_FacilityID, VARG_UserID, BaseGenericRequest.CultureID);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SizeList()
        {
            var list = new SelectList(vw_CatalogService.List4Select("CIA_Sizes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");

            return Json(list.Select(s => new { value = s.Value, text = s.Text }), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult KioskAreaDetail_Delete(int ID)
        {
            var result = KioskAreaDetailService.Delete(ID, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KioskAreaDetail_Upsert(KioskAreaDetail entity, List<t_GenericItem> list, string TransactionDetailID)
        {
            string BackGroundImageTmp = "";
            string SourcePath = "";
            if (!string.IsNullOrEmpty(entity.BackgroundImage))
            {
                BackGroundImageTmp = Server.MapPath(entity.BackgroundImage);
                entity.BackgroundImage = "/Files/HR/{0}/BackgroundImages/" + Path.GetFileName(entity.BackgroundImage);
            }
            if (!string.IsNullOrEmpty(entity.SourcePath) && !string.IsNullOrEmpty(entity.FileTypeValueID))
            {
                SourcePath = Server.MapPath(entity.SourcePath);
                if (entity.FileTypeValueID.ToUpper() != "G")
                {
                    entity.SourcePath = "/Files/HR/{0}/" + entity.FileTypeValueID + "/" + Path.GetFileName(entity.SourcePath);
                }
                else
                {
                    entity.SourcePath = "/Files/HR/{0}/" + entity.FileTypeValueID + "/";
                }
            }

            var result = KioskAreaDetailService.Upsert(entity, list, BaseGenericRequest);
            if (result.ErrorCode == 0)
            {
                var baseDirectory = new DirectoryInfo(string.Format("{0}Files\\HR\\{1}", Server.MapPath(@"\"), result.ID));
                string backgroundImagesPath = Path.Combine(baseDirectory.ToString(), "BackgroundImages");
                string fileTypePath = Path.Combine(baseDirectory.ToString(), entity.FileTypeValueID.ToUpper());


                if (!string.IsNullOrEmpty(BackGroundImageTmp) && System.IO.File.Exists(BackGroundImageTmp))
                {
                    if (!Directory.Exists(backgroundImagesPath))
                        Directory.CreateDirectory(backgroundImagesPath);


                    System.IO.File.Copy(BackGroundImageTmp, backgroundImagesPath + "\\" + Path.GetFileName(BackGroundImageTmp), true);
                }

                if (entity.KioskAreaDetailID == 0)
                {
                    if (!string.IsNullOrEmpty(SourcePath) && !string.IsNullOrEmpty(entity.FileTypeValueID))
                    {
                        if (entity.FileTypeValueID.ToUpper() != "G" && System.IO.File.Exists(SourcePath))
                        {
                            if (!Directory.Exists(fileTypePath))
                                Directory.CreateDirectory(fileTypePath);
                            System.IO.File.Copy(SourcePath, fileTypePath + "\\" + Path.GetFileName(SourcePath), true);
                        }
                        else
                        {
                            if (!Directory.Exists(fileTypePath))
                                Directory.CreateDirectory(fileTypePath);

                            string[] filePaths = Directory.GetFiles(Path.GetDirectoryName(SourcePath));
                            foreach (string filePath in filePaths)
                            {
                                System.IO.File.Copy(filePath, fileTypePath + "\\" + Path.GetFileName(filePath), true);
                            }
                        }
                    }
                }
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

        public ActionResult KioskArea_UpdateGeneralSettings(string ImageURL, int SessionTime, string CarouselTransitionTime, int ReferenceID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                string TempFilePath = Server.MapPath(ImageURL).Replace("%20", " ");
                string FinalDirectory = "";
                string NewFilePath = "";
                int KioskMediaID = 0;

                MiscellaneousService.Params_SetValue("HR_Kiosk_CloseSessionAfter", SessionTime.ToString(), BaseGenericRequest);
                MiscellaneousService.Params_SetValue("HR_Kiosk_CarouselTransitionTime", CarouselTransitionTime.ToString(), BaseGenericRequest);
                KioskMediaID = KioskMediaService.Insert(BaseGenericRequest).ID;

                if (ImageURL == "Nothing")
                {
                    result = MiscellaneousService.Params_SetValue("HR_Kiosk_BackgroundImage", "", BaseGenericRequest);
                }
                else if (!String.IsNullOrEmpty(ImageURL))
                {

                    result = MiscellaneousService.Params_SetValue("HR_Kiosk_BackgroundImage", "/Files/HR/Kiosk/BackgroundImage/" + Path.GetFileName(ImageURL), BaseGenericRequest);
                    if (result.ErrorCode == 0)
                    {
                        // Guardar fisicamente la imagen a utilizar BACKGROUND
                        FinalDirectory = new DirectoryInfo(string.Format("{0}Files\\HR\\Kiosk\\BackgroundImage", Server.MapPath(@"\"))).ToString();
                        NewFilePath = Path.Combine(FinalDirectory, Path.GetFileName(TempFilePath));

                        if (System.IO.File.Exists(TempFilePath))
                        {
                            if (!Directory.Exists(FinalDirectory))
                                Directory.CreateDirectory(FinalDirectory);

                            System.IO.File.Copy(TempFilePath, NewFilePath, true);
                        }
                    }
                }

                return Json(new
                {
                    result.ErrorCode,
                    result.ErrorMessage,
                    Title = "",
                    notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                    result.ID,
                    KioskMediaID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                return Json(new
                {
                    result.ErrorCode,
                    ex.Message,
                    Title = "",
                    notifyType = NotifyType.error.ToString(),
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult KioskCarouselMedia_Delete(int ID)
        {
            var result = KioskMediaService.Delete(ID, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KioskArea_AddKioskAreaInit()
        {
            KioskAreaUpsertViewModel model = new KioskAreaUpsertViewModel();
            model.CulturesList = vw_CatalogService.List("SystemCultures", BaseGenericRequest);
            model.SizeList = new SelectList(vw_CatalogService.List4Select("CIA_Sizes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
            model.Sequence = 0;
            return PartialView("~/Areas/HR/Views/KioskAdministration/_Mo_KioskAreaUpsert.cshtml", model);
        }

        public ActionResult KioskArea_Upsert(int SizeID, int Sequence, int? ParentID, List<t_GenericItem> list)
        {

            //agregar el parent
            var result = KioskAreaService.Upsert(SizeID, Sequence, ParentID, list, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KioskAreaDetail_Init(int? KioskAreaID, int? KioskAreaDetailID, bool? IsRoot)
        {
            KioskAreaDetailUpsertViewModel model = new KioskAreaDetailUpsertViewModel();

            // catalogs
            model.FileTypeDisabled = "";
            model.SourcePathHidden = "";
            model.ButtonAcceptModal = Resources.CI.DashboardManagement.btn_SectionDetailsTag;
            model.FileTypeList = vw_CatalogService.List4Select("HRK_FilesTypes", BaseGenericRequest, true);
            model.FileTypeList[0].DisplayText = Resources.Common.chsn_SelectOption;
            model.KioskSizeList = new SelectList(vw_CatalogService.List4Select("CID_Sizes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.KioskIconList = new SelectList(vw_CatalogService.List4Select("CID_Icons", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.KioskFontColorList = new SelectList(vw_CatalogService.List4Select("CID_FontColor", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.KioskBackColorList = new SelectList(vw_CatalogService.List4Select("CID_BackColor", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.KioskNameTranslate = vw_CatalogService.List("SystemCultures", BaseGenericRequest);
            model.KioskDataEffectList = new SelectList(vw_CatalogService.List4Select("CID_DataEffect", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.KioskAreaDetailInfo.KioskAreaID = KioskAreaID;
            model.KioskAreaDetailInfo.KioskAreaDetailID = 0;

            if (IsRoot != null)
            {
                if (Convert.ToBoolean(IsRoot))
                {
                    model.HeaderModal = Resources.HR.Kiosk.title_CreateRootNode;
                    model.IsRoot = Convert.ToBoolean(IsRoot);
                    model.SizeClass = "col-xs-12";
                }
            }

            //////////////calculo de combos de parents y secciones
            List<KioskArea> sections = new List<KioskArea>();
            List<KioskAreaNode> parents = new List<KioskAreaNode>();

            var area = KioskAreaService.Get(KioskAreaID, IsRoot, BaseGenericRequest);
            if (area != null && area.ParentID != null)
            {
                model.KioskAreaDetailInfo.ParentKioskAreaDetailID = area.ParentID;
                //lista las secciones del padre
                if (KioskAreaDetailID != null)
                {
                    sections = KioskAreaService.List(null, true, null, null, BaseGenericRequest).Where(w => w.ParentID == KioskAreaDetailID).ToList();
                    //old
                    model.SectionsList = new SelectList(sections, "KioskAreaID", "Title", KioskAreaDetailID);

                    //beta
                    model.SectionsList = new SelectList(sections, "KioskAreaID", "Title", sections.FirstOrDefault().KioskAreaID);
                }
                //listado de los nodos padre y el padre seleccionado por default
                //los nodos son los hermanos del nodo actual , y el nodo actual es el seleccionado
                var ListParentsNodes = KioskAreaDetailService.List(null, null, KioskAreaID, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(ListParentsNodes, "KioskAreaDetailID", "Name", KioskAreaDetailID);
            }
            else
            {
                //es nodo raiz, el listado de parensts son raices y esta seleccionado el nodo actual como padre
                parents = KioskAreaDetailService.NodeList(null, null, null, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(parents, "KioskAreaDetailID", "Name", KioskAreaDetailID);

                //sections = KioskAreaService.List(null, true, null, IsRoot, BaseGenericRequest).Where(w => w.ParentID == KioskAreaDetailID).ToList(); ;
                sections = KioskAreaService.List(null, true, null, null, BaseGenericRequest).Where(w => w.ParentID == KioskAreaDetailID).ToList();
                model.SectionsList = new SelectList(sections, "KioskAreaID", "Title");
            }
            //////////////calculo de combos de parents y secciones

            return PartialView("~/Areas/HR/Views/KioskAdministration/_Mo_KioskAreaDetailUpsert.cshtml", model);
        }

        public ActionResult GetReorderSectionsModal(int? KioskAreaDetailID)
        {
            ModalReorderSectionsViewModel model = new ModalReorderSectionsViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/HR/Views/KioskAdministration/_Mo_ReorderSections.cshtml";
            try
            {
                var sections = KioskAreaService.List(null, BaseGenericRequest);
                model.SectionList = sections.Where(w => w.ParentID == KioskAreaDetailID).ToList();
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

        public ActionResult GetNodeSections(int? KioskAreaDetailID)
        {
            List<KioskArea> model = new List<KioskArea>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/HR/Views/KioskAdministration/_Tbl_NodeSections.cshtml";
            try
            {
                var sections = KioskAreaService.List(null, true, null, null, BaseGenericRequest);
                model = sections.Where(w => w.ParentID == KioskAreaDetailID).ToList();
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


        public JsonResult GetParentNodeSections(int? ParentID)
        {
            GenericReturn result = new GenericReturn();
            IEnumerable<SelectListItem> List = new SelectList(new List<SelectListItem>());
            try
            {
                var sections = KioskAreaService.List(null, true, null, null, BaseGenericRequest);
                List = new SelectList(sections.Where(w => w.ParentID == ParentID).ToList(), "KioskAreaID", "Title");
                //List = new SelectList(sections, "KioskAreaID", "Title");

                if (List != null)
                {
                    result.ErrorCode = 0;
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
                List,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KioskArea_QuickTranslateUpdate(int KioskAreaID, List<t_GenericItem> list)
        {
            var result = KioskAreaService.QuickTranslateUpdate(KioskAreaID, list, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KioskAreasDetail_UpdateQuickEditable(string name, int pk, string value)
        {
            var result = KioskAreaDetailService.QuickUpdate(pk, name, value, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSeqNumber(int KioskAreaDetailID)
        {
            GenericReturn result = new GenericReturn();
            string SeqNumber = "";
            try
            {
                SeqNumber = KioskAreaDetailService.GetSeqNumber(KioskAreaDetailID);
                if (SeqNumber != "")
                {
                    result.ErrorCode = 0;
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
                SeqNumber,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
