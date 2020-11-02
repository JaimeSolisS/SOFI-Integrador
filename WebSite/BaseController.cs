using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Utilities;
using System.Linq;
using static WebSite.Models.StaticModels;
using System.Data;
using ClosedXML.Excel;

namespace WebSite
{
    public class BaseController : Controller
    {
        #region Properties

        public const string SOFI_COOKIE = "SOFI_UserInfo";
        public const string SESSION_KEY_CULTURE = "CultureID";
        public const string USER_ID = "UserID";
        public const string USER_NAME = "UserName";
        public const string FIRST_NAME = "Firstname";
        public const string LAST_NAME = "Lastname";
        public const string USER_FULLNAME = "UserFullName";
        public const string FACILITY_ID = "BaseFacilityId";
        public const string COMPANY_ID = "CompanyId";
        public const string OPERATOR_NUMBER = "OperatorNumber";

        public string VARG_HostName
        {
            get
            {
                return Request.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            }
        }
        public string VARG_CultureID
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                {
                    return Request.Cookies[SOFI_COOKIE][SESSION_KEY_CULTURE];
                    //return Request.Cookies["culture"];
                }
                else
                {
                    string Currentculture = CultureInfo.CurrentCulture.ToString();
                    return MiscellaneousService.Param_GetValue(0, "DefaultCultureId", Currentculture);
                }
            }
        }

        public int VARG_UserID
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                {
                    return Request.Cookies[SOFI_COOKIE][USER_ID].ToInt();
                }
                else { return 0; }
            }
        }

        public string VARG_UserNameID
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                { return Request.Cookies[SOFI_COOKIE][USER_NAME]; }
                else { return ""; }
            }
        }

        public string VARG_FirstName
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                { return Request.Cookies[SOFI_COOKIE][FIRST_NAME]; }
                else { return ""; }
            }
        }

        public string VARG_LastName
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                { return Request.Cookies[SOFI_COOKIE][LAST_NAME]; }
                else { return ""; }
            }
        }

        public int VARG_FacilityID
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                { return Request.Cookies[SOFI_COOKIE][FACILITY_ID].ToInt(); }
                else { return 0; }
            }
        }

        public int VARG_CompanyID
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                { return Request.Cookies[SOFI_COOKIE][COMPANY_ID].ToInt(); }
                else { return 0; }
            }
        }

        public string VARG_ComputerName
        {
            get
            {
                //
                return Request.UserHostAddress.ToString();
            }
        }

        public string VARG_OperatorNumber
        {
            get
            {
                if (Request.Cookies[SOFI_COOKIE] != null)
                { return Request.Cookies[SOFI_COOKIE][OPERATOR_NUMBER]; }
                else { return ""; }
            }
        }
        public static string ReverseLookup(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return ip;

            try
            {
                return Dns.GetHostEntry(ip).HostName ?? ip;
            }
            catch (SocketException) { return ip; }
        }

        public GenericRequest BaseGenericRequest = new GenericRequest();
        #endregion

        #region Methods

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];

            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = LanguageMang.GetDefaultLanguage();
                }
            }
            new LanguageMang().SetLanguage(lang);
            Session.Add(SESSION_KEY_CULTURE, lang);

            BaseGenericRequest = new GenericRequest()
            {
                FacilityID = VARG_FacilityID,
                UserID = VARG_UserID,
                //CultureID = VARG_CultureID,
                CultureID = lang,
                CompanyID = VARG_CompanyID,
                OperatorNumber = VARG_OperatorNumber
            };
            return base.BeginExecuteCore(callback, state);
        }

        public PartialViewResult GetFacilities()
        {
            var model = UserService.GetFacilities(VARG_UserID, VARG_FacilityID, VARG_CultureID);
            return PartialView("~/Views/Shared/_Facilities.cshtml", model);
        }
        public ActionResult ChangeFacility(int FacilityID)
        {
            Session.Add(FACILITY_ID, FacilityID);
            var a = BaseGenericRequest;

            if (Request.Cookies[SOFI_COOKIE] != null)
            {
                Response.Cookies[SOFI_COOKIE].Values[FACILITY_ID] = FacilityID.ToString();
                Response.Cookies[SOFI_COOKIE].Values[USER_ID] = VARG_UserID.ToString();
            }

            var result = FacilityService.List(null, FacilityID, true, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            if (result.Count != 0)
            {
                Session.Add(COMPANY_ID, result.First().CompanyID);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageMang().SetLanguage(lang);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public void MessageBoxOK(NotifyType notifyType, string title, string message)
        {
            //title = title.CleanInvalidChar();
            //message = message.CleanInvalidChar();
            string btnOKText = Resources.Common.BtnOK_Messages.ToString();
            TempData["MessageBoxOKTitle"] = title;
            TempData["MessageBoxOKMessage"] = message;
            TempData["MessageBoxOKType"] = notifyType;
            TempData["MessageBoxbtnOKText"] = btnOKText;
        }

        public void Notification(NotifyType notifyType, string title, string message)
        {
            //title = title.CleanInvalidChar();
            //message = message.CleanInvalidChar();
            TempData["NotificationTitle"] = title;
            TempData["NotificationMessage"] = message;
            TempData["NotificationType"] = notifyType;
        }
        public ActionResult ExcelExport(DataSet ds, string fileName)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.Columns[0].ColumnName.Contains("TableName"))
                    {
                        if (dt.Rows.Count > 0)
                        { dt.TableName = dt.Rows[0]["TableName"].ToString(); }

                        dt.Columns.RemoveAt(0);
                    }
                    wb.Worksheets.Add(dt);
                    //formato de ajuste de celdas
                    //wb.Worksheet(1).Columns().AdjustToContents();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
                }
            }
        }

        public ActionResult ExcelReportWithTabName(DataSet ds, string fileName, string[] TabName)
        {

            using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
            {
                int index = 0;
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.Columns[0].ColumnName.Contains("TableName"))
                    {
                        if (dt.Rows.Count > 0)
                        { dt.TableName = dt.Rows[0]["TableName"].ToString(); }

                        dt.Columns.RemoveAt(0);
                    }

                    dt.TableName = TabName[index];
                    wb.Worksheets.Add(dt);
                    index++;
                }

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
                }
            }
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult DataTableLang()
        {
            var nodes = new dataTablesLang
            {
                sProcessing = Resources.Common.dtatbl_sProcessing,
                sZeroRecords = Resources.Common.dtatbl_sZeroRecords,
                sLengthMenu = Resources.Common.dtatbl_LengthMenu,
                sEmptyTable = Resources.Common.dtatbl_sEmptyTable,
                sInfo = Resources.Common.dtatbl_info,
                sInfoEmpty = Resources.Common.dtatbl_sInfoEmpty,
                sInfoFiltered = Resources.Common.dtatbl_sInfoFiltered,
                sInfoPostFix = "",
                sSearch = Resources.Common.dtatbl_sSearch,
                sUrl = "",
                sInfoThousands = ",",
                sLoadingRecords = Resources.Common.dtatbl_sLoadingRecords,
                oPaginate = new dataTablesLang.dataTablesLangoPaginate
                {
                    sFirst = Resources.Common.dtatbl_sFirst,
                    sLast = Resources.Common.dtatbl_sLast,
                    sNext = Resources.Common.dtatbl_sNext,
                    sPrevious = Resources.Common.dtatbl_sPrevious
                },
                oAria = new dataTablesLang.dataTablesLangoAria
                {
                    sSortAscending = Resources.Common.dtatbl_sSortAsc,
                    sSortDescending = Resources.Common.dtatbl_sSortDesc
                }
            };
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult CopyAttachments(int ReferenceID, string AttachmentType, int NewReferenceID, string NewAttachmentType)
        {
            int? ReferenceType = 0;
            int? NewReferenceType = 0;
            var NewSrcPath = "";
            Catalog Reference = new Catalog();
            GenericReturn result = new GenericReturn();
            try
            {
                Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
                if (Reference != null)
                {
                    ReferenceType = Reference.CatalogDetailID;
                }
                Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == NewAttachmentType.ToUpper());
                if (Reference != null)
                {
                    NewReferenceType = Reference.CatalogDetailID;
                }
                var AttachmentList = Core.Services.AttachmentService.List(null, AttachmentType, ReferenceID, ReferenceType, VARG_CompanyID, null, null, BaseGenericRequest);

                if (AttachmentList != null && AttachmentList.Any())
                {
                    string AttachmentDirectory;
                    string pathString = "";
                    switch (NewAttachmentType.ToUpper())
                    {
                        case "PARTS":
                            {
                                AttachmentDirectory = "Parts";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "TEMPID":
                            {
                                AttachmentDirectory = "TempFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "APINVOICEID":
                            {
                                AttachmentDirectory = "AP_Invoices";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "APPAYMENTID":
                            {
                                AttachmentDirectory = "AP_Payments";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "TMSORDERID":
                            {
                                AttachmentDirectory = "TMS_Orders";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "ENERGYSENSORID":
                            {
                                AttachmentDirectory = "EnergySensorsFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensors\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "ENERGYSENSORFAMILYID":
                            {
                                AttachmentDirectory = "EnergySensorFamiliesFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensorFamilies\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "MACHINEID":
                            {
                                AttachmentDirectory = "MachinesFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MFG\\Machines\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "KIOSKCAROUSELMEDIA":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\HR\\Carousel\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString());
                                break;
                            }
                        case "OPPORTUNITYPROGRAMMEDIA":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\HR\\OpportunitiesProgram\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString());
                                break;
                            }
                        case "IDENTIFICATIONTYPES":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\IdentificationTypesFiles\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "TOOLSIMG":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\ToolsImagesFiles\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                NewSrcPath = "\\Files\\SG\\ToolsImagesFiles\\" + NewReferenceID.ToString() + "\\";
                                break;
                            }
                        case "EREQUESTMEDIA":
                            {
                                AttachmentDirectory = "eRequestFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\eREQ\\eRequestFiles\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                                break;
                            }
                        default:
                            break;
                    }


                    if (!Directory.Exists(pathString))
                        Directory.CreateDirectory(pathString);


                    foreach (var item in AttachmentList)
                    {
                        var NewFilePathName = string.Format("{0}\\{1}", pathString, item.FileName);
                        switch (NewAttachmentType.ToUpper())
                        {
                            case "IDENTIFICATIONTYPES":
                                NewSrcPath += item.FileName;
                                break;

                        }
                        var originalDirectory = Server.MapPath(item.FilePathName);
                        System.IO.File.Copy(originalDirectory, NewFilePathName, true);

                        Core.Services.AttachmentService.QuickUpdate(item.FileID, AttachmentType, NewReferenceID, NewReferenceType, null, null, Utilities.Extensions.GetVirtualPath4File(NewFilePathName), BaseGenericRequest);
                    }
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
                NewSrcPath,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReplaceAttachments(int ReferenceID, string AttachmentType, int NewReferenceID, string NewAttachmentType)
        {
            int? ReferenceType = 0;
            int? NewReferenceType = 0;
            Catalog Reference = new Catalog();
            GenericReturn result = new GenericReturn();
            try
            {
                Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
                if (Reference != null)
                {
                    ReferenceType = Reference.CatalogDetailID;
                }
                Reference = Core.Service.vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == NewAttachmentType.ToUpper());
                if (Reference != null)
                {
                    NewReferenceType = Reference.CatalogDetailID;
                }

                //los archivos origen
                //var AttachmentList = Core.Services.AttachmentService.List(null, NewAttachmentType, NewReferenceID, NewReferenceType, VARG_CompanyID, null, null, BaseGenericRequest);
                var AttachmentList = Core.Services.AttachmentService.List(null, NewAttachmentType, NewReferenceID, NewReferenceType, VARG_CompanyID, null, null, BaseGenericRequest);


                if (AttachmentList != null)
                {
                    string AttachmentDirectory;
                    string pathString = "";
                    switch (NewAttachmentType.ToUpper())
                    {
                        case "PARTS":
                            {
                                AttachmentDirectory = "Parts";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "TEMPID":
                            {
                                AttachmentDirectory = "TempFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "APINVOICEID":
                            {
                                AttachmentDirectory = "AP_Invoices";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "APPAYMENTID":
                            {
                                AttachmentDirectory = "AP_Payments";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "TMSORDERID":
                            {
                                AttachmentDirectory = "TMS_Orders";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\" + AttachmentDirectory + "\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "ENERGYSENSORID":
                            {
                                AttachmentDirectory = "EnergySensorsFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensors\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "ENERGYSENSORFAMILYID":
                            {
                                AttachmentDirectory = "EnergySensorFamiliesFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MNT\\EnergySensorFamilies\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "MACHINEID":
                            {
                                AttachmentDirectory = "MachinesFiles";
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\MFG\\Machines\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), NewReferenceID.ToString());
                                break;
                            }
                        case "KIOSKCAROUSELMEDIA":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\HR\\Carousel", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString());
                                break;
                            }
                        case "OPPORTUNITYPROGRAMMEDIA":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\HR\\OpportunitiesProgram", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString());
                                break;
                            }
                        case "IDENTIFICATIONTYPES":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\IdentificationTypesFiles\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                                break;
                            }
                        case "TOOLSIMG":
                            {
                                var destinationDirectory = new DirectoryInfo(string.Format("{0}Files\\SG\\ToolsImagesFiles\\", Server.MapPath(@"\")));
                                pathString = Path.Combine(destinationDirectory.ToString(), ReferenceID.ToString());
                                break;
                            }
                        default:
                            break;
                    }


                    if (!Directory.Exists(pathString))
                        Directory.CreateDirectory(pathString);


                    foreach (var item in AttachmentList)
                    {
                        //borra los archivos anteriores
                        Core.Services.AttachmentService.Delete(item.FileID, NewAttachmentType, BaseGenericRequest);
                    }

                    var NewAttachmentList = Core.Services.AttachmentService.List(null, AttachmentType, ReferenceID, ReferenceType, VARG_CompanyID, null, null, BaseGenericRequest);
                    foreach (var item in NewAttachmentList)
                    {
                        var NewFilePathName = string.Format("{0}\\{1}", pathString, item.FileName);
                        var originalDirectory = Server.MapPath(item.FilePathName);
                        if (originalDirectory != NewFilePathName)
                        {
                            System.IO.File.Copy(originalDirectory, NewFilePathName, true);
                        }

                        Core.Services.AttachmentService.QuickUpdate(item.FileID, NewAttachmentType, NewReferenceID, NewReferenceType, null, null,
                            Utilities.Extensions.GetVirtualPath4File(NewFilePathName), BaseGenericRequest);
                    }
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

    }
}