using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.CI.Models.ViewModels.Administration;
using WebSite.Utilities;
using static WebSite.Models.StaticModels;
using System.IO;
using Core.Entities.SQL_DataType;
using WebSite.Models;

namespace WebSite.Areas.CI.Controllers
{
    public class AdministrationController : BaseController
    {
        // GET: CI/DashboardManagement
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            try
            {
                var dashboardAreaList = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest, true);

                if (dashboardAreaList != null && dashboardAreaList.Count > 0)
                {
                    model.DashboardAreaList = dashboardAreaList.FindAll(p => p.DashboardAreaID > 0).ToList();
                }
                model.ListAreas = new SelectList(dashboardAreaList, "DashboardAreaID", "Title");
                model.FileTypeList = new SelectList(vw_CatalogService.List4Select("CID_FileTypes", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
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

        public ActionResult DashboardAreaList(int DashboardAreaID, int FileTypeID, int ViewType)
        {
            List<DashboardArea> model = new List<DashboardArea>();
            List<DashboardAreaNode> modelNodes = new List<DashboardAreaNode>();
            int? DashboardAreaID2 = null;
            int? FileTypeID2 = null;

            if (DashboardAreaID > 0) { DashboardAreaID2 = DashboardAreaID; }
            if (FileTypeID > 0) { FileTypeID2 = FileTypeID; }

            var view = vw_CatalogService.Get(ViewType, BaseGenericRequest);
            if (view != null && view.ValueID == "N")
            {
                modelNodes = CI_DashboardAreaDetailService.NodeList(null, null, DashboardAreaID2, true, FileTypeID2, null, BaseGenericRequest);
                return PartialView("~/Areas/CI/Views/Administration/_DashboardAreaNodeTable.cshtml", modelNodes);
            }
            else if (view != null && view.ValueID == "S")
            {
                model = CI_DashboardAreaService.List(DashboardAreaID2, true, FileTypeID2, BaseGenericRequest, false);
                return PartialView("~/Areas/CI/Views/Administration/_DashboardAreaTable.cshtml", model);
            }
            return PartialView("~/Areas/CI/Views/Administration/_DashboardAreaNodeTable.cshtml", model);
        }

        public ActionResult DashboardAreaDetailList(int DashboardAreaID, int FileTypeID)
        {
            List<DashboardAreaDetail> model;
            int? DashboardAreaID2 = null;
            int? FileTypeID2 = null;

            if (DashboardAreaID > 0) { DashboardAreaID2 = DashboardAreaID; }
            if (FileTypeID > 0) { FileTypeID2 = FileTypeID; }

            model = CI_DashboardAreaDetailService.List(null, null, DashboardAreaID2, true, FileTypeID2, null, BaseGenericRequest);

            return PartialView("~/Areas/CI/Views/Administration/_DashboardAreaDetailTable.cshtml", model);
        }
        public ActionResult DashboardAreaNodeList(int DashboardAreaID, int FileTypeID, int DashboardAreaDetailID)
        {
            List<DashboardAreaNode> model = new List<DashboardAreaNode>();
            model = CI_DashboardAreaDetailService.NodeList(null, DashboardAreaDetailID, null, true, FileTypeID, null, BaseGenericRequest);
            return PartialView("~/Areas/CI/Views/Administration/_DashboardAreaNodeDivTable.cshtml", model);
        }

        public ActionResult SizeList()
        {
            var list = new SelectList(vw_CatalogService.List4Select("CIA_Sizes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");

            return Json(list.Select(s => new { value = s.Value, text = s.Text }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DashboardArea_Delete(int ID)
        {
            var result = CI_DashboardAreaService.Delete(ID, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DashboardArea_GeneralSettings()
        {
            GeneralSettingsViewModel model = new GeneralSettingsViewModel();
            model.TransactionID = Guid.NewGuid().ToString();
            var result = CI_DashboardAreaService.GetGenericSettings(BaseGenericRequest);
            string imageStream = "data:image/png;base64,";
            string imagePath = "";
            string fullpath = "";
            byte[] imgBuffer;
            if (result != null && result.Count > 0)
            {
                foreach (GenericItem item in result)
                {
                    if (item.Key.ToUpper() == "CI_DASHBOARD_BACKGROUNDIMAGE")
                    {
                        model.imgURL = item.Name.Replace("\\", "/");
                        imagePath = "../../" + item.Name.Replace("\\", "/");

                        fullpath = string.Format("{0}{1}", System.Web.HttpContext.Current.Server.MapPath("\\"), item.Name.Replace("/", "\\"));

                        if (System.IO.File.Exists(fullpath))
                        {
                            imgBuffer = System.IO.File.ReadAllBytes(fullpath);
                            if (imgBuffer.Length > 0)
                            {
                                imageStream += Convert.ToBase64String(imgBuffer);
                                string Orientation = Wrappers.GetOrientationImage(fullpath);
                                model.imgOrientation = Orientation;
                                model.imgTitle = Path.GetFileName(imagePath);
                                model.imgSource = imageStream;
                            }
                        }
                    }
                    else if (item.Key.ToUpper() == "CI_DASHBOARD_SCREENSAVERINTERVAL")
                    {
                        model.ScreenSaverInterval = item.Name;
                    }
                    else if (item.Key.ToUpper() == "CI_DASHBOARD_SCREENSAVERVIDEO")
                    {
                        model.videoURL = item.Name;
                        model.videoSrc = item.Name.Replace("/", "\\");
                    }
                    else if (item.Key.ToUpper() == "CI_DASHBOARD_CLOSEDWINDOWAFTER")
                    {
                        model.CloseWindowAfter = item.Name;
                    }
                }
            }
            GenericReturn responseCreateSettings = CI_DashboardCarouselVideosTmpService.CreateSettingsOnTmp(model.TransactionID.ToString(), BaseGenericRequest);

            if (responseCreateSettings.ErrorCode == 0)
            {
                model.CarouselVideosList = CI_DashboardCarouselVideosTmpService.List(model.TransactionID.ToString(), BaseGenericRequest);
                foreach (var item in model.CarouselVideosList)
                { item.Path = string.Format("{0}/{1}", VARG_HostName, item.Path); }

                // Create File
                var FinalDirectory = new DirectoryInfo(string.Format("{0}Files\\CI\\Carousel\\", Server.MapPath(@"\"))).ToString();
                var TmpFileDirectory = string.Format("{0}Files\\Temp\\DashboardSetting\\{1}\\", Server.MapPath(@"\"), model.TransactionID);
                if (!Directory.Exists(TmpFileDirectory))
                    Directory.CreateDirectory(TmpFileDirectory);

                if (!Directory.Exists(FinalDirectory))
                    Directory.CreateDirectory(FinalDirectory);

                // Como medida de manteninimiento, borrar los datos que existan previamente cargados
                string[] filePaths = Directory.GetFiles(string.Format("{0}Files\\Temp\\DashboardSetting\\", Server.MapPath(@"\")), "*.*", SearchOption.AllDirectories);
                foreach (var file in filePaths)
                {
                    if (System.IO.File.Exists(file))
                    { System.IO.File.Delete(file); }
                }
            }
            return PartialView("~/Areas/CI/Views/Administration/_GeneralSettingsManagement.cshtml", model);
        }

        public ActionResult DashboardArea_AddDashboardAreaInit()
        {
            DashboardAreaUpsertViewModel model = new DashboardAreaUpsertViewModel();
            model.CulturesList = vw_CatalogService.List("SystemCultures", BaseGenericRequest);
            model.SizeList = new SelectList(vw_CatalogService.List4Select("CIA_Sizes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
            model.Sequence = 0;
            return PartialView("~/Areas/CI/Views/Administration/_Mo_DashboardAreaUpsert.cshtml", model);
        }

        [HttpPost]
        public void DashboardArea_UploadFile(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string FileFolder = context.Request.QueryString["FileFolder"].ToString();

            string[] files;
            int numFiles;

            string dirFullPath = System.Web.HttpContext.Current.Server.MapPath("~/" + FileFolder + "/");

            if (!Directory.Exists(dirFullPath))
            { System.IO.Directory.CreateDirectory(dirFullPath); }

            files = System.IO.Directory.GetFiles(dirFullPath);
            numFiles = files.Length;
            numFiles = numFiles + 1;

            string str_image = "";
            string pathToSave = "";

            foreach (string s in context.Request.Files)
            {
                HttpPostedFile file = context.Request.Files[s];
                string fileName = file.FileName;
                string fileExtension = file.ContentType;

                if (!string.IsNullOrEmpty(fileName))
                {
                    fileExtension = Path.GetExtension(fileName);
                    // F.Vera 26 Oct 2016 se reemplaza caracter ^ por -,-,- agregando los caracteres que fueron eliminados en el Guid para que mantenga la misma lóngitud,
                    // Esto con el fin de que al agregar una tarea se pueda visualizar
                    str_image = fileName.Replace(fileExtension, "") + "-,-,-MyFile_" + numFiles.ToString() + Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                    pathToSave = System.Web.HttpContext.Current.Server.MapPath("~/" + FileFolder + "/") + str_image;
                    file.SaveAs(pathToSave);
                }
            }
            context.Response.Write(pathToSave);
        }


        [HttpPost]
        public ActionResult DashboardArea_Upsert(int SizeID, int Sequence, int? ParentID, List<t_GenericItem> list)
        {

            //agregar el parent
            var result = CI_DashboardAreaService.Upsert(SizeID, Sequence, ParentID, list, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        #region QuickUpdateEdit

        public ActionResult DashboardArea_CultureListInit(int DashboarAreaID, string FieldTitle)
        {
            GenericCultureViewModel model = new GenericCultureViewModel();
            model.ID = DashboarAreaID;
            model.ModelHeader = FieldTitle;
            model.FieldTranslates = CI_DashboardAreaService.GetTitleInfo(DashboarAreaID, BaseGenericRequest);
            return PartialView("~/Areas/CI/Views/Administration/_Mo_CultureList.cshtml", model);
        }

        [HttpPost]
        public ActionResult DashboardArea_QuickTranslateUpdate(int DashboardAreaID, List<t_GenericItem> list)
        {
            var result = CI_DashboardAreaService.QuickTranslateUpdate(DashboardAreaID, list, BaseGenericRequest);
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
        public ActionResult UpdateQuickEditable(string name, int pk, string value)
        {
            var result = CI_DashboardAreaService.QuickUpdate(pk, name, value, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DashboardAreaDetails

        public ActionResult DashboardAreaDetail_Init(int DashboarAreaID, int? DashboardAreaDetailID)
        {
            DashboardAreaDetailUpsertViewModel model = new DashboardAreaDetailUpsertViewModel();

            // catalogs
            model.FileTypeDisabled = "";
            model.SourcePathHidden = "";
            model.HeaderModal = Resources.CI.DashboardManagement.lbl_DashboardAreaNodeHeaderTag;
            model.ButtonAcceptModal = Resources.CI.DashboardManagement.btn_SectionDetailsTag;
            model.FileTypeList = vw_CatalogService.List4Select("CID_FileTypes", BaseGenericRequest, true);
            model.FileTypeList[0].DisplayText = Resources.Common.chsn_SelectOption;
            model.DashboardSizeList = new SelectList(vw_CatalogService.List4Select("CID_Sizes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.DashboardIconList = new SelectList(vw_CatalogService.List4Select("CID_Icons", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.DashboardFontColorList = new SelectList(vw_CatalogService.List4Select("CID_FontColor", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.DashboardBackColorList = new SelectList(vw_CatalogService.List4Select("CID_BackColor", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.DashboardNameTranslate = vw_CatalogService.List("SystemCultures", BaseGenericRequest);
            model.DashboardDataEffectList = new SelectList(vw_CatalogService.List4Select("CID_DataEffect", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            model.DashboardAreaDetailInfo.DashboardAreaID = DashboarAreaID;
            model.DashboardAreaDetailInfo.DashboardAreaDetailID = 0;

            //////////////calculo de combos de parents y secciones
            List<DashboardArea> sections = new List<DashboardArea>();
            List<DashboardAreaNode> parents = new List<DashboardAreaNode>();

            var area = CI_DashboardAreaService.Get(DashboarAreaID, BaseGenericRequest);
            if (area != null && area.ParentID != null)
            {
                model.DashboardAreaDetailInfo.ParentDashboardAreaDetailID = area.ParentID;
                //lista las secciones del padre
                if (DashboardAreaDetailID != null)
                {
                    sections = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest).Where(w => w.ParentID == DashboardAreaDetailID).ToList();
                    model.SectionsList = new SelectList(sections, "DashboardAreaID", "Title", DashboardAreaDetailID);
                }
                //listado de los nodos padre y el padre seleccionado por default
                //los nodos son los hermanos del nodo actual , y el nodo actual es el seleccionado
                var ListParentsNodes = CI_DashboardAreaDetailService.List(null, null, DashboarAreaID, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(ListParentsNodes, "DashboardAreaDetailID", "Name", DashboardAreaDetailID);
            }
            else
            {
                //es nodo raiz, el listado de parensts son raices y esta seleccionado el nodo actual como padre
                parents = CI_DashboardAreaDetailService.NodeList(null, null, null, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(parents, "DashboardAreaDetailID", "Name", DashboardAreaDetailID);
                sections = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest).Where(w => w.ParentID == DashboardAreaDetailID).ToList();
                model.SectionsList = new SelectList(sections, "DashboardAreaID", "Title");
            }
            //////////////calculo de combos de parents y secciones

            return PartialView("~/Areas/CI/Views/Administration/_Mo_DashboardAreaDetailUpsert.cshtml", model);
        }

        public ActionResult DashboardAreaDetail_EditInit(int DashboarAreaID, int DashboardAreaDetailID)
        {
            DashboardAreaDetailUpsertViewModel model = new DashboardAreaDetailUpsertViewModel();

            // catalogs
            model.DashboardAreaDetailInfo = CI_DashboardAreaDetailService.Find(DashboardAreaDetailID, BaseGenericRequest);
            model.HeaderModal = Resources.CI.DashboardManagement.lbl_DashboardAreaNodeHeaderUpdateTag;
            model.ButtonAcceptModal = Resources.CI.DashboardManagement.btn_UpdateSectionDetailsTag;
            model.FileTypeList = vw_CatalogService.List4Select("CID_FileTypes", BaseGenericRequest, true);
            model.FileTypeList[0].DisplayText = Resources.Common.chsn_SelectOption;
            model.DashboardSizeList = new SelectList(vw_CatalogService.List4Select("CID_Sizes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.DashboardAreaDetailInfo.SizeID);
            model.DashboardIconList = new SelectList(vw_CatalogService.List4Select("CID_Icons", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.DashboardAreaDetailInfo.IconID);
            model.DashboardFontColorList = new SelectList(vw_CatalogService.List4Select("CID_FontColor", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.DashboardAreaDetailInfo.FontColorID);
            model.DashboardBackColorList = new SelectList(vw_CatalogService.List4Select("CID_BackColor", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.DashboardAreaDetailInfo.BackColorID);
            model.DashboardDataEffectList = new SelectList(vw_CatalogService.List4Select("CID_DataEffect", BaseGenericRequest, false), "CatalogDetailID", "DisplayText", model.DashboardAreaDetailInfo.DataEffectID);
            model.DashboardAreaDetailInfo.DashboardAreaID = DashboarAreaID;
            model.DashboardAreaDetailInfo.DashboardAreaDetailID = DashboardAreaDetailID;
            model.DashboardNameTranslate = CI_DashboardAreaDetailService.GetNameInfo(DashboardAreaDetailID, BaseGenericRequest);

            if (model.DashboardAreaDetailInfo.FileTypeValueID.ToUpper() == "G")
            {
                model.FileTypeGaleryCss = "";
                model.FileTypeMediaOrFileCss = "";
            }
            else if (model.DashboardAreaDetailInfo.FileTypeValueID.ToUpper() == "L")
            {
                model.FileTypeLinkCss = "";
            }
            else
            {
                model.FileTypeMediaOrFileCss = "";
            }

            if (!string.IsNullOrEmpty(model.DashboardAreaDetailInfo.BackgroundImage))
            {
                string BackgroundImageTmp = "";
                BackgroundImageTmp = Server.MapPath(model.DashboardAreaDetailInfo.BackgroundImage);
                string tmpBackgroundImageTmp = new DirectoryInfo(string.Format("{0}Files\\Temp\\CI\\{1}\\{2}", Server.MapPath(@"\"), model.TransactionID, model.DashboardAreaDetailInfo.FileTypeValueID.ToUpper())).ToString();

                if (!Directory.Exists(tmpBackgroundImageTmp))
                    Directory.CreateDirectory(tmpBackgroundImageTmp);

                if (System.IO.File.Exists(BackgroundImageTmp))
                {
                    System.IO.File.Copy(BackgroundImageTmp, tmpBackgroundImageTmp + "\\" + Path.GetFileName(BackgroundImageTmp));
                    model.DashboardAreaDetailInfo.BackgroundImage = Wrappers.GetVirtualPath4File(tmpBackgroundImageTmp + "\\" + Path.GetFileName(BackgroundImageTmp));
                }
            }

            //////////////calculo de combos de parents y secciones
            List<DashboardArea> sections = new List<DashboardArea>();
            List<DashboardAreaNode> parents = new List<DashboardAreaNode>();

            //model.ParentsNodesList = new SelectList(parents, "DashboardAreaDetailID", "Name");
            var area = CI_DashboardAreaService.Get(DashboarAreaID, BaseGenericRequest);
            if (area != null && area.ParentID != null)
            {
                model.DashboardAreaDetailInfo.ParentDashboardAreaDetailID = area.ParentID;
                //lista las secciones del padre
                sections = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest).Where(w => w.ParentID == area.ParentID).ToList();
                model.SectionsList = new SelectList(sections, "DashboardAreaID", "Title", DashboarAreaID);

                //listado de los nodos padre y el padre seleccionado por default
                var ParentNode = CI_DashboardAreaDetailService.Get(area.ParentID, BaseGenericRequest);
                var ListParentsNodes = CI_DashboardAreaDetailService.List(null, null, ParentNode.DashboardAreaID, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(ListParentsNodes, "DashboardAreaDetailID", "Name", area.ParentID);
            }
            else
            {
                //sacar el parent node del DashboarAreaID
                var areaslist = CI_DashboardAreaService.List(DashboarAreaID, true, null, BaseGenericRequest);
                model.SectionsList = new SelectList(areaslist, "DashboardAreaID", "Title", DashboarAreaID);
                //sino tiene padre es nodo RAIZ y se muestran todos lo nodos padres en el listado, pero no esta seleccionado ninguno
                parents = CI_DashboardAreaDetailService.NodeList(null, null, null, true, null, null, BaseGenericRequest);
                model.ParentsNodesList = new SelectList(parents, "DashboardAreaDetailID", "Name");
            }
            //////////////calculo de combos de parents y secciones

            return PartialView("~/Areas/CI/Views/Administration/_Mo_DashboardAreaDetailUpsert.cshtml", model);
        }

        [HttpPost]
        public ActionResult DashboardAreaDetail_Upsert(DashboardAreaDetail entity, List<t_GenericItem> list, string TransactionDetailID)
        {
            string BackGroundImageTmp = "";
            string SourcePath = "";
            if (!string.IsNullOrEmpty(entity.BackgroundImage))
            {
                BackGroundImageTmp = Server.MapPath(entity.BackgroundImage);
                entity.BackgroundImage = "/Files/CI/{0}/BackgroundImages/" + Path.GetFileName(entity.BackgroundImage);
            }
            if (!string.IsNullOrEmpty(entity.SourcePath) && !string.IsNullOrEmpty(entity.FileTypeValueID))
            {
                SourcePath = Server.MapPath(entity.SourcePath);
                if (entity.FileTypeValueID.ToUpper() != "G")
                {
                    entity.SourcePath = "/Files/CI/{0}/" + entity.FileTypeValueID + "/" + Path.GetFileName(entity.SourcePath);
                }
                else
                {
                    entity.SourcePath = "/Files/CI/{0}/" + entity.FileTypeValueID + "/";
                }
            }

            var result = CI_DashboardAreaDetailService.Upsert(entity, list, BaseGenericRequest);
            if (result.ErrorCode == 0)
            {
                var baseDirectory = new DirectoryInfo(string.Format("{0}Files\\CI\\{1}", Server.MapPath(@"\"), result.ID));
                string backgroundImagesPath = Path.Combine(baseDirectory.ToString(), "BackgroundImages");
                string fileTypePath = Path.Combine(baseDirectory.ToString(), entity.FileTypeValueID.ToUpper());


                if (!string.IsNullOrEmpty(BackGroundImageTmp) && System.IO.File.Exists(BackGroundImageTmp))
                {
                    if (!Directory.Exists(backgroundImagesPath))
                        Directory.CreateDirectory(backgroundImagesPath);


                    System.IO.File.Copy(BackGroundImageTmp, backgroundImagesPath + "\\" + Path.GetFileName(BackGroundImageTmp), true);
                }

                if (entity.DashboardAreaDetailID == 0)
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

        public ActionResult UploadDashboardFileType(string TransactionDetailID, string hdnFileTypeValueID, string hdnFileExtensions)
        {
            GenericReturn result = new GenericReturn();
            string pathString = "";
            string path = "";
            string filenames = "";
            bool IsValidExtension = false;
            try
            {
                if (hdnFileTypeValueID.ToUpper() != "G" && Request.Files.Count > 1)
                {
                    result.ErrorCode = 99;
                    result.ErrorMessage = Resources.CI.DashboardManagement.lbl_OnlyOnceFileTag;
                    return Json(new
                    {
                        result.ErrorCode,
                        result.ErrorMessage,
                        notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                        FilePath = Wrappers.GetVirtualPath4File(path),
                        FileName = Path.GetFileName(path)
                    }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(hdnFileExtensions))
                {
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[fileName];
                        if (file != null && file.ContentLength > 0)
                        {
                            IsValidExtension = false;

                            foreach (var extension in hdnFileExtensions.Split(','))
                            {
                                if (extension.Trim().ToUpper() == Path.GetExtension(file.FileName).ToUpper())
                                {
                                    IsValidExtension = true;
                                    break;
                                }
                            }

                            if (!IsValidExtension)
                            {
                                result.ErrorCode = 99;
                                result.ErrorMessage = string.Format(Resources.CI.DashboardManagement.lbl_FileNotValidTag, Path.GetFileName(file.FileName));
                                return Json(new
                                {
                                    result.ErrorCode,
                                    result.ErrorMessage,
                                    notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                                    FilePath = Wrappers.GetVirtualPath4File(path),
                                    FileName = Path.GetFileName(path)
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }

                var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Temp\\CI\\{1}\\{2}", Server.MapPath(@"\"), TransactionDetailID, hdnFileTypeValueID));
                pathString = Path.Combine(originalDirectory.ToString(), "");

                // Limpia el directorio cada que cargue un tipo de archivo
                try
                {
                    if (Directory.Exists(pathString) && hdnFileTypeValueID.ToUpper() != "G")
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
                        filenames += Path.GetFileName(path) + ", ";
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
                FileName = filenames.TrimEnd(' ').TrimEnd(',')
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadDashboardDetails(string TransactionDetailID)
        {
            GenericReturn result = new GenericReturn();
            string pathString = "";
            string path = "";
            try
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Temp\\CI\\{1}\\Dashboard\\BackgroundImages", Server.MapPath(@"\"), TransactionDetailID));
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

        [HttpPost]
        public ActionResult DashboardAreasDetail_UpdateQuickEditable(string name, int pk, string value)
        {
            var result = CI_DashboardAreaDetailService.QuickUpdate(pk, name, value, BaseGenericRequest);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DashboardSettings
        public ActionResult UploadDashboardSetting(string TransactionSettings)
        {
            GenericReturn result = new GenericReturn();
            string pathString = "";
            string path = "";
            try
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Temp\\DashboardSetting\\{1}", Server.MapPath(@"\"), TransactionSettings));
                pathString = Path.Combine(originalDirectory.ToString(), "");
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
                FilePath = Wrappers.GetVirtualPath4File(path)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadCarouselVideo(string TransactionSettings)
        {
            GenericReturn result = new GenericReturn();
            string pathString = "";
            string path = "";
            try
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\Temp\\DashboardSetting\\{1}", Server.MapPath(@"\"), TransactionSettings));
                List<t_Files> list = new List<t_Files>();
                pathString = Path.Combine(originalDirectory.ToString(), "");
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                        if (System.IO.File.Exists(path))
                        {

                            list.Add(new t_Files
                            {
                                FileName = Path.GetFileName(path)
                                ,
                                FilePathName = string.Format("{0}/{1}/{2}/{3}", VARG_HostName, "Files/Temp/DashboardSetting", TransactionSettings, Path.GetFileName(path))
                            });


                        }
                    }
                    result.ErrorMessage = string.Format(Resources.Common.nft_FileUploaded, file.FileName);
                }

                if (list.Count > 0)
                {
                    result = CI_DashboardCarouselVideosTmpService.Insert(TransactionSettings, list, BaseGenericRequest);
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
                FilePath = Wrappers.GetVirtualPath4File(path)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DashboardCarouselVideos_List(string TransactionID)
        {
            List<DashboardCarouselVideosTmp> model = null;
            model = CI_DashboardCarouselVideosTmpService.List(TransactionID, BaseGenericRequest);

            return PartialView("~/Areas/CI/Views/Administration/_GeneralSettingsAttachmentsTable.cshtml", model);
        }

        [HttpPost]
        public ActionResult DashboardCarouselVideos_Delete(int ID)
        {
            var result = CI_DashboardCarouselVideosTmpService.Delete(ID, BaseGenericRequest);
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
        public ActionResult DashboardCarouselVideo_SetSortable(string TransactionID, List<t_GenericItem> list)
        {
            var result = CI_DashboardCarouselVideosTmpService.SetSort(TransactionID, list, BaseGenericRequest);
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
        public ActionResult DashboardAreaDetail_Delete(int ID)
        {
            var result = CI_DashboardAreaDetailService.Delete(ID, BaseGenericRequest);
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
        public ActionResult DashboardArea_UpdateGeneralSettings(string ImageURL, string VideoURL, int ScreenSaverInterval, int ClosedWindowAfter, string TransactionID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                string TempFilePath = Server.MapPath(ImageURL);
                string TempFileVideoPath = Server.MapPath(VideoURL);
                string OldFilePath = "";
                ImageURL = "/Files/CI/Dashboard/BackgroundImages/" + Path.GetFileName(ImageURL);
                VideoURL = "/Files/CI/Videos/" + Path.GetFileName(VideoURL);
                result = CI_DashboardAreaService.GeneralSettingsUpdate(ImageURL, VideoURL, ScreenSaverInterval, ClosedWindowAfter, TransactionID, VARG_HostName, BaseGenericRequest);

                if (result.ErrorCode == 0)
                {
                    // Guardar fisicamente la imagen a utilizar
                    string FinalDirectory = new DirectoryInfo(string.Format("{0}Files\\CI\\Dashboard\\BackgroundImages", Server.MapPath(@"\"))).ToString();
                    string NewFilePath = Path.Combine(FinalDirectory, Path.GetFileName(TempFilePath));

                    if (System.IO.File.Exists(TempFilePath) && !System.IO.File.Exists(FinalDirectory + '\\' + Path.GetFileName(ImageURL)))
                    {
                        if (!Directory.Exists(FinalDirectory))
                            Directory.CreateDirectory(FinalDirectory);

                        System.IO.File.Copy(TempFilePath, NewFilePath, true);
                    }

                    // Guardar fisicamente el video a utilizar
                    FinalDirectory = new DirectoryInfo(string.Format("{0}Files\\CI\\Videos\\", Server.MapPath(@"\"))).ToString();
                    NewFilePath = Path.Combine(FinalDirectory, Path.GetFileName(TempFileVideoPath));

                    if (System.IO.File.Exists(TempFileVideoPath) && !System.IO.File.Exists(FinalDirectory + '\\' + Path.GetFileName(VideoURL)))
                    {
                        if (!Directory.Exists(FinalDirectory))
                            Directory.CreateDirectory(FinalDirectory);

                        System.IO.File.Copy(TempFileVideoPath, NewFilePath, true);
                    }

                    FinalDirectory = new DirectoryInfo(string.Format("{0}Files\\CI\\Carousel\\", Server.MapPath(@"\"))).ToString();
                    var list = CI_DashboardCarouselVideosService.List(null, BaseGenericRequest);
                    if (list != null && list.Count > 0)
                    {
                        if (!Directory.Exists(FinalDirectory))
                        {
                            Directory.CreateDirectory(FinalDirectory);
                        }

                        foreach (var item in list)
                        {
                            OldFilePath = string.Format("{0}Files\\Temp\\DashboardSetting\\{1}\\{2}", Server.MapPath(@"\"), TransactionID, item.FileName);
                            NewFilePath = Path.Combine(FinalDirectory, item.FileName);
                            if (System.IO.File.Exists(OldFilePath))
                            {
                                System.IO.File.Copy(OldFilePath, NewFilePath, true);
                                System.IO.File.Delete(OldFilePath);
                            }
                        }

                    }

                    string[] filePaths = Directory.GetFiles(FinalDirectory, "*.*", SearchOption.AllDirectories);
                    foreach (string file in filePaths)
                    {
                        var fileList = list.Find(x => x.FileName.ToLower() == Path.GetFileName(file).ToLower());

                        if (System.IO.File.Exists(file) && fileList == null)
                        { System.IO.File.Delete(file); }
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
        #endregion

        public ActionResult GetReorderSectionsModal(int DashboardAreaDetailID)
        {
            ModalReorderSectionsViewModel model = new ModalReorderSectionsViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/CI/Views/Administration/_Mo_ReorderSections.cshtml";
            try
            {
                var sections = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest);
                model.SectionList = sections.Where(w => w.ParentID == DashboardAreaDetailID).ToList();
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
        public ActionResult GetNodeSections(int DashboardAreaDetailID)
        {
            List<DashboardArea> model = new List<DashboardArea>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/CI/Views/Administration/_Tbl_NodeSections.cshtml";
            try
            {
                var sections = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest);
                model = sections.Where(w => w.ParentID == DashboardAreaDetailID).ToList();
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
        public JsonResult GetParentNodeSections(int ParentID)
        {
            GenericReturn result = new GenericReturn();
            IEnumerable<SelectListItem> List = new SelectList(new List<SelectListItem>());
            try
            {
                var sections = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest);
                List = new SelectList(sections.Where(w => w.ParentID == ParentID).ToList(), "DashboardAreaID", "Title");
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
        public JsonResult GetSeqNumber(int DashboardAreaDetailID)
        {
            GenericReturn result = new GenericReturn();
            string SeqNumber = "";
            try
            {
                SeqNumber = CI_DashboardAreaDetailService.GetSeqNumber(DashboardAreaDetailID);
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