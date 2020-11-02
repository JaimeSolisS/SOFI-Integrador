using Core.Entities;
using Core.Service;
using Core.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebSite.Areas.HR.Models.ViewModels.KioskEmployee;
using WebSite.Models;
using ZKFPEngXControl;
using static WebSite.Models.StaticModels;
// using ZKFPEngXControl;

namespace WebSite.Areas.HR.Controllers
{
    public class KioskEmployeeController : BaseController
    {
        public ActionResult Login(string UserCode)
        {
            LoginUserViewModel model = new LoginUserViewModel();
            try
            {
                var UserPointsMovementList = KioskEmployeeService.GetPointsMovements(UserCode, BaseGenericRequest);
                var AvailablePoints = UserPointsMovementList.Where(p => p.Tipo == 0).Sum(s => s.Puntos);
                var PetitionPoints = UserPointsMovementList.Where(p => p.Tipo == 1).Sum(s => s.Puntos);
                var EmployeeMovements = KioskEmployeeService.GetEmployeeMovements(UserCode, BaseGenericRequest).FirstOrDefault();
                var BackgroundImage = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_BackgroundImage", "");
                var WorkedHours = KioskEmployeeService.GetPrePayroll(UserCode).Sum(s => Convert.ToInt32(s.HorasLaborales));
                var ExtraHours = KioskEmployeeService.GetPrePayroll(UserCode).Sum(s => Convert.ToInt32(s.HorasExtras));

                EmployeeMovements.CoursesAvailable = KioskEmployeeService.GetCoursesAvailable(UserCode, BaseGenericRequest).Count();

                model.EmployeeID = UserCode;
                model.NotificationsUnreaded = KioskEmployeeService.GetNotificationsUnreaded(UserCode, BaseGenericRequest).Count();
                model.UserInfoList = KioskEmployeeService.GetUserData(UserCode);
                model.EmployeMovements = EmployeeMovements;
                model.BackgroundImage = string.Format("{0}/{1}", VARG_HostName, BackgroundImage);
                model.FacilityName = FacilityService.List4Select(BaseGenericRequest, false).Where(x => x.FacilityID == VARG_FacilityID).FirstOrDefault().FacilityName;
                model.SessionTime = Convert.ToInt32(MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_CloseSessionAfter", "15"));
                model.SessionTimeWarning = Convert.ToInt32(MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_SessionTimeWarning", "15"));
                model.PrePayRollTotalHours = WorkedHours + ExtraHours;
                model.PaymentReceiptsList = KioskEmployeeService.GetPaymentReceipts(UserCode).Count();
                model.AvailablePoints = AvailablePoints + PetitionPoints;
                model.UserAccessID = VARG_UserID;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
            }

            return View(model);
        }

        public JsonResult LoginValidation(string UserCode)
        {
            bool IsValid = false;
            try
            {
                if (UserCode != null)
                {
                    if (KioskEmployeeService.GetUserData(UserCode).Count() > 0)
                        IsValid = true;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
            }

            return Json(new
            {
                IsValid
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoginWithActiveDirectory(string Domain, string User, string Password)
        {
            bool isValid = false;
            string UserCode = "";
            try
            {
                var LDAPDomain = string.Format("{0}://{1}", "LDAP", Domain);
                var SearcherFilter = "samAccountName=" + User + "";
                DirectoryEntry Entry = new DirectoryEntry(LDAPDomain, User, Password);
                Entry.RefreshCache();
                DirectorySearcher Searcher = new DirectorySearcher(Entry, SearcherFilter);
                SearchResult results = Searcher.FindOne();

                var NTUser = results.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                UserCode = UserService.GetInfo(NTUser, BaseGenericRequest).EmployeeNumber;
                if (NTUser != null)
                {
                    isValid = true;
                }

            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                isValid = false;
            }

            return Json(new
            {
                isValid,
                UserCode
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSuggestionModal()
        {
            var ViewPath = "~/Areas/HR/Views/KioskEmployee/_Suggestions.cshtml";
            var model = new SelectList(new List<SelectListItem>());
            try
            {
                model = new SelectList(vw_CatalogService.List4Select("KioskCommentCategories", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetPrivateRequestsModal(string EmployeeID)
        {
            var ViewPath = "~/Areas/HR/Views/KioskEmployee/_PrivateRequests.cshtml";
            List<KioskUserRequest> model = new List<KioskUserRequest>();
            try
            {
                model = KioskRequestAdministratorService.SimpleList(null, EmployeeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetPrivateRequestsTable(string EmployeeID, string RequestNumber)
        {
            var ViewPath = "~/Areas/HR/Views/KioskEmployee/_Tbl_PrivateRequests.cshtml";
            List<KioskUserRequest> model = new List<KioskUserRequest>();
            try
            {
                model = KioskRequestAdministratorService.SimpleList(RequestNumber, EmployeeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetPrivateSuggestionsModal(string EmployeeID)
        {
            var ViewPath = "~/Areas/HR/Views/KioskEmployee/_PrivateSuggestions.cshtml";
            List<KioskEmployeeSuggestion> model = new List<KioskEmployeeSuggestion>();
            try
            {
                model = KioskSuggestionsAdministratorService.List(EmployeeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetPrePayRollModal(string EmployeeNumber)
        {
            List<KioskPrePayroll> List = new List<KioskPrePayroll>();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_PrePayroll.cshtml";
            try
            {
                List = KioskEmployeeService.GetPrePayroll(EmployeeNumber);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, List);
        }

        public ActionResult GetPaymentReceiptsModal(string EmployeeNumber)
        {
            PaymentReceiptsViewModel model = new PaymentReceiptsViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_PaymentReceipts.cshtml";
            try
            {
                var PaymentReceiptsList = KioskEmployeeService.GetPaymentReceipts(EmployeeNumber);
                model.PaymentReceiptsList = PaymentReceiptsList;
                model.EmployeeNumber = EmployeeNumber;

                //foreach (var Receipts in PaymentReceiptsList)
                //{
                //    var Path = System.Web.HttpContext.Current.Server.MapPath("~/Files/HR/Kiosk/Receipts/" + EmployeeNumber);
                //    var FullPathPDF = System.Web.HttpContext.Current.Server.MapPath(Receipts.FilePDF.Replace("{0}", EmployeeNumber));
                //    var FullPathXML = System.Web.HttpContext.Current.Server.MapPath(Receipts.FileXML.Replace("{0}", EmployeeNumber));

                //    if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                //    if (!System.IO.File.Exists(FullPathPDF)) System.IO.File.WriteAllBytes(FullPathPDF, Receipts.BytePDF);
                //    if (!System.IO.File.Exists(FullPathXML)) System.IO.File.WriteAllBytes(FullPathXML, Receipts.ByteXML);

                //}
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public JsonResult InscribeUserToCourse(int? CourseID, string EmployeeNumber, string Name)
        {
            GenericReturn result = new GenericReturn();
            KioskCoursesViewModel model = new KioskCoursesViewModel();
            string AvailablePath = "~/Areas/HR/Views/KioskEmployee/_Tbl_AvailableCourses.cshtml";
            string InscribePath = "~/Areas/HR/Views/KioskEmployee/_Tbl_InscribedCourses.cshtml";

            try
            {
                result = KioskEmployeeService.InscribeUserToCourse(CourseID, EmployeeNumber, Name, BaseGenericRequest);
                model.InscribePermit = KioskEmployeeService.GetUserCoursesPermit(EmployeeNumber, BaseGenericRequest);
                model.KioskCoursesList = KioskEmployeeService.GetCoursesAvailable(EmployeeNumber, BaseGenericRequest);
                model.KioskCoursesInscribedList = KioskEmployeeService.GetCoursesInscribed(EmployeeNumber, BaseGenericRequest);
                model.AvailableCoursesCount = model.KioskCoursesList.Count();
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
                AvailableTable = RenderRazorViewToString(AvailablePath, model),
                InscribedTable = RenderRazorViewToString(InscribePath, model),
                model.AvailableCoursesCount,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserPointsMovementModal(string EmployeeNumber, string RFC)
        {
            EmployeePointsMovementsViewModel model = new EmployeePointsMovementsViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_PointsExchange.cshtml";
            try
            {
                var UserPointsMovementList = KioskEmployeeService.GetPointsMovements(EmployeeNumber, BaseGenericRequest);
                model.ItemsList = KioskEmployeeService.GetAvailableItems(EmployeeNumber, BaseGenericRequest);
                model.UserPointsMovementList = UserPointsMovementList;
                model.InterchangedTicketsList = KioskEmployeeService.GetPendingPetitions(EmployeeNumber, RFC, BaseGenericRequest);
                model.AvailablePoints = UserPointsMovementList.Where(p => p.Tipo == 0).Sum(s => s.Puntos);
                model.PetitionPoints = UserPointsMovementList.Where(p => p.Tipo == 1).Sum(s => s.Puntos);

                DateTime date = DateTime.Now;
                var startDate = new DateTime(date.Year, date.Month, 20);
                var endDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
                var currentDate = MiscellaneousService.Facility_GetDate(VARG_FacilityID);

                if (currentDate >= startDate && currentDate <= endDate)
                {
                    model.EnableExchangePetitions = true;
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetCoursesModal(string EmployeeNumber)
        {
            KioskCoursesViewModel model = new KioskCoursesViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_Courses.cshtml";
            try
            {
                model.InscribePermit = KioskEmployeeService.GetUserCoursesPermit(EmployeeNumber, BaseGenericRequest);
                model.KioskCoursesList = KioskEmployeeService.GetCoursesAvailable(EmployeeNumber, BaseGenericRequest);
                model.KioskCoursesInscribedList = KioskEmployeeService.GetCoursesInscribed(EmployeeNumber, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetPrivateSuggestionsTable(string EmployeeID)
        {
            var ViewPath = "~/Areas/HR/Views/KioskEmployee/_Tbl_PrivateSuggestions.cshtml";
            List<KioskEmployeeSuggestion> model = new List<KioskEmployeeSuggestion>();
            try
            {
                model = KioskSuggestionsAdministratorService.List(EmployeeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }



        [HttpPost]
        public JsonResult SaveSuggestions(int KioskCommentCategoryID, string Comment, string EmployeeID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = KioskEmployeeService.Comments_Insert(KioskCommentCategoryID, Comment, EmployeeID, BaseGenericRequest);
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

        public JsonResult GetWizardModal(bool? IsPublic)
        {
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_RequestWizard.cshtml";
            WizardRequestViewModel model = new WizardRequestViewModel();
            try
            {
                model.DepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "DepartmentID", "DepartmentName");
                model.ShiftsList = new SelectList(ShiftService.List4Select(BaseGenericRequest, Resources.Common.chsn_SelectOption, true), "ShiftID", "ShiftDescription");
                model.RequestTypesList = vw_CatalogService.List4Select("HR_RequestTypes", BaseGenericRequest, false, Resources.Common.chsn_SelectOption);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
            }

            return Json(new
            {
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModalRequisitions(bool IsPublic, string ClaveEmpleado)
        {
            Models.ViewModels.KioskRequestAdministrator.IndexViewModel model = new Models.ViewModels.KioskRequestAdministrator.IndexViewModel();
            string ViewPath = "~/Areas/HR/Views/Kiosk/_Mo_Requests.cshtml";
            try
            {
                if (!IsPublic)
                {
                    ViewPath = "~/Areas/HR/Views/KioskEmployee/_MyRequisitions.cshtml";
                    model.ClaveEmpleado = ClaveEmpleado;
                }
                model.RequestTypesList = new SelectList(vw_CatalogService.List4Select("HR_RequestTypes", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText", 0);
                model.RequestStatusList = new SelectList(vw_CatalogService.List4Select("HR_RequestStatus", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText", 0);
                model.RequestDepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest, false, Resources.Common.TagAll), "DepartmentID", "DepartmentName");
                model.CurrentDate = MiscellaneousService.Facility_GetDate(VARG_FacilityID);
                model.RequestHistoryDays = Convert.ToInt32(MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Request_History_Days", "60"));
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult SearchRequestData(string RequestUserID, string RequestNumber, string RequestTypeIDs, string RequestStatusIDs, DateTime? StartDate, DateTime? EndDate, string DepartmentIDs,
                                       string EmployeeInfo, string RequestDescription, int? PageNumber, bool? OnlyRead)
        {
            Models.ViewModels.KioskRequestAdministrator.IndexViewModel model = new Models.ViewModels.KioskRequestAdministrator.IndexViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Tbl_Requests.cshtml";
            try
            {
                if (PageNumber != null)
                {
                    model.RequestsList = KioskRequestAdministratorService.ListFiltered(RequestNumber, RequestTypeIDs, RequestUserID, RequestStatusIDs, null,
                    StartDate, EndDate, DepartmentIDs, EmployeeInfo, RequestDescription, null, null, BaseGenericRequest);
                }
                model.OnlyRead = OnlyRead;

                if (model.RequestsList.Count > 0)
                {
                    var divitionOnPages = Convert.ToDouble(Decimal.Divide(model.RequestsList.FirstOrDefault().PageCount, 10));
                    model.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(divitionOnPages)));
                }

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult SearchRequestPrivateData(string RequestUserID, string RequestNumber, string RequestTypeIDs, string RequestStatusIDs, DateTime? StartDate, DateTime? EndDate, string DepartmentIDs,
                               string EmployeeInfo, string RequestDescription, int? PageNumber)
        {
            Models.ViewModels.KioskRequestAdministrator.IndexViewModel model = new Models.ViewModels.KioskRequestAdministrator.IndexViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Tbl_Requests.cshtml";
            try
            {
                model.RequestsList = KioskRequestAdministratorService.ListFiltered(RequestTypeIDs, RequestNumber, RequestUserID, RequestStatusIDs, VARG_UserID.ToString(),
                StartDate, EndDate, DepartmentIDs, EmployeeInfo, RequestDescription, null, null, BaseGenericRequest);

                if (model.RequestsList.Count > 0)
                {
                    var divitionOnPages = Convert.ToDouble(Decimal.Divide(model.RequestsList.FirstOrDefault().PageCount, 10));
                    model.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(divitionOnPages)));
                }

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult ReloadSuggestionsTable(string EmployeeID)
        {
            var ViewPath = "~/Areas/HR/Views/KioskSuggestionsAdministrator/_Tbl_KioskSuggestions.cshtml";
            List<KioskEmployeeSuggestion> model = new List<KioskEmployeeSuggestion>();
            try
            {
                model = KioskSuggestionsAdministratorService.List(EmployeeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult ReloadSuggestionsPrivateTable(string EmployeeID)
        {
            var ViewPath = "~/Areas/HR/Views/KioskEmployee/_Tbl_PrivateSuggestions.cshtml";
            List<KioskEmployeeSuggestion> model = new List<KioskEmployeeSuggestion>();
            try
            {
                model = KioskSuggestionsAdministratorService.List(EmployeeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
            }

            return PartialView(ViewPath, model);
        }

        public void TestFinger()
        {

            ZKFPEngX fp = new ZKFPEngX();
            fp.SensorIndex = 0;
            fp.InitEngine();

            System.Threading.Thread.Sleep(5000);


            object imgdata = new object();
            bool b = fp.GetFingerImage(ref imgdata);

            var template = fp.GetTemplate();
            var templateString = fp.GetTemplateAsString();

            var templateStringFormated = Convert.FromBase64String(templateString);

            var user = "mspkWYOKjhNxAQjnF2PBCeogSYEF3aJ7gQR6K2NBBgCsZ4EHdDFmwQgANH+BCAs4WgEJbblPggngOlyBCei7eUEJgzx5gQgQP2gBDw3AMAID0cBXwRDqx1iCGOtJQQIK0UpaQiALy4QBCCnMeYEMLU00AQpF0CCBC0fTSsEGztVygQs81VnCFFFYhgIKMFh/wQo82WPCDlDaLkEONto9QVnOXDxCYp5dWAIRVV03gUIKUZQUF8SkFg8EEV5gYGFjZmltcncDBQcJBBFbXV5gYmVpbnR3BAYICgQRYGFhYmVoa25ydgEEBggEEVdZW15gY2hudAIGCQ0PBRFhYmJlaWxucnV3AwQHBBFXWFlcXmJob3YECg8SFAURYWJkZmpsb3F0dgIDBgMRVVdXWFtdYGlxAQgPFBcYBhFkZ2lrbG9xdHYBAwQDEVRVVlZYW2BocgQOFRgZGgcRaWlqam5xc3V3AgMDEVNTVFRWWV9odQoWGxwdHQgQa2tqbXBydHV3AxFTUVBPUlZdZ3cVICQkIyMJD2xpa21wcnQDEUtLSkpNUlljdyArLSsoKAoNamtsbgMRR0ZERUhPVVlOMjQzMC4tAAD/AxFEQ0BARVBUVEtAOzk2MjAAAP8DEUJBOztKWFdTTkdCPzs3NQAA/wMRPjkzK2lhW1VSTUhDQD05BBAuKyFxZF1WVFFLRkI+";

            var result = fp.VerFingerFromStr(templateString, user, false, false);

            var HashCode = fp.GetHashCode();


            fp.EndEngine();
        }

        public ActionResult GetModalOpportunitiesProgram(string EmployeeID)
        {
            List<OpportunitiesProgram> list = new List<OpportunitiesProgram>();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_PrivateOpportunitiesProgram.cshtml";
            try
            {
                list = OpportunitiesProgramService.SimpleList(null, EmployeeID, BaseGenericRequest);
                var result = KioskNotificationsDetailService.Update(null, null, "01054", true, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, list);
        }

        public ActionResult GetOpportunitiesProgramRecords(string OpportunityNumber, string EmployeeNumber)
        {
            List<OpportunitiesProgram> list = new List<OpportunitiesProgram>();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Tbl_PrivateOpportunitiesProgram.cshtml";
            try
            {
                list = OpportunitiesProgramService.SimpleList(OpportunityNumber, EmployeeNumber, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, list);
        }

        public ActionResult GetOpportunitiesProgramDetailsRecords(int OpportunityProgramID, string EmployeeID)
        {
            List<KioskNotification> list = new List<KioskNotification>();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Tbl_PrivateOpportunitiesProgramLog.cshtml";
            string ErrorMessage = "";
            try
            {
                var CatalogDetailID = MiscellaneousService.Catalog_GetDetailID(VARG_FacilityID, "KioskModulesReferences", "KMR_OpportunitiesProgramModule", out ErrorMessage);
                list = KioskNotificationsService.List(null, EmployeeID, OpportunityProgramID, CatalogDetailID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, list);
        }

        public ActionResult GetOpportunityProgramMedia(int OpportunityProgramID, string AttachmentType)
        {
            List<IAttachment> list = new List<IAttachment>();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_OpportunityProgramMedia.cshtml";
            try
            {
                int ReferenceType = 0;
                var Reference = vw_CatalogService.List("SystemReferenceTypes", BaseGenericRequest).FirstOrDefault(f => f.ValueID.ToUpper() == AttachmentType.ToUpper());
                if (Reference != null)
                {
                    ReferenceType = Reference.CatalogDetailID;
                }
                list = AttachmentService.List(null, AttachmentType, OpportunityProgramID, ReferenceType, null, null, null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, list);
        }

        public ActionResult GetOpportunityProgramText(int OpportunityProgramID)
        {
            OpportunitiesProgram model = new OpportunitiesProgram();
            string ViewPath = "~/Areas/HR/Views/KioskEmployee/_Mo_OpportunityProgramText.cshtml";
            try
            {
                model = OpportunitiesProgramService.List(OpportunityProgramID, BaseGenericRequest).FirstOrDefault();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public JsonResult ApplyOpportunnity(int OpportunityProgramID, string CandidateID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = OpportunitiesProgramCandidatesService.Insert(OpportunityProgramID, CandidateID, "", false, BaseGenericRequest);
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

        public JsonResult SendAttachmentsReceipt(string EmployeeNumber, string Invoices, string ReceiptNumber, string eMailAddressTo)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                string InvoiceName = null; //solo va a aplicar para cuando es un solo recibo
                string PathAttachments = "";
                var receipts = KioskEmployeeService.GetReceiptsAttachmentsByIDs(EmployeeNumber, Invoices, BaseGenericRequest);

                foreach (var receipt in receipts)
                {
                    var Path = System.Web.HttpContext.Current.Server.MapPath("~/Files/Temp/HR/Kiosk/Receipts/" + EmployeeNumber);
                    var FullPathPDF = System.Web.HttpContext.Current.Server.MapPath(receipt.FilePDF.Replace("{0}", EmployeeNumber));
                    var FullPathXML = System.Web.HttpContext.Current.Server.MapPath(receipt.FileXML.Replace("{0}", EmployeeNumber));

                    if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                    if (!System.IO.File.Exists(FullPathPDF)) System.IO.File.WriteAllBytes(FullPathPDF, receipt.BytePDF);
                    if (!System.IO.File.Exists(FullPathXML)) System.IO.File.WriteAllBytes(FullPathXML, receipt.ByteXML);

                    PathAttachments += FullPathPDF + "|" + FullPathXML + "|";
                }


                //Por motivos de seguridad del server linkeado, tuve que traerme los datos hasta aca y volverlos a mandar,
                //No me dejó hacerlo directamente en el server
                var FullDataReceipts = KioskEmployeeService.GetPaymentReceipts(EmployeeNumber);
                var SplitInvoices = Invoices.Split(',');
                var ListForYearAndWeek = from r in FullDataReceipts join s in SplitInvoices on r.FolioSobre equals s.ToString() select new { r.sFiscal, r.aFiscal };
                string HTMLBodyInfo = "";
                foreach (var data in ListForYearAndWeek)
                {
                    HTMLBodyInfo += "<li>" + Resources.Common.lbl_Year + ":" + data.aFiscal + " - " + Resources.Common.lbl_Week + ":" + data.sFiscal + "</li> ";
                }

                if (receipts.Count() == 1)
                {
                    InvoiceName = receipts.FirstOrDefault().FolioSobre;
                }
                result = KioskEmployeeService.Receipts_Notifications(EmployeeNumber, ReceiptNumber, InvoiceName, HTMLBodyInfo, eMailAddressTo, PathAttachments, BaseGenericRequest);

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

        [HttpPost]
        public JsonResult GeneratePointsPetition(string clave, string rfc, List<KioskExchangeItemsForList> listaArticulos)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = KioskEmployeeService.Insert_PointsExchangePetitions(clave, rfc, listaArticulos, BaseGenericRequest);
                result.ErrorMessage = Resources.HR.Kiosk.msg_RequestSuccesfullyRegistered;
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

        public JsonResult ReleaseUselessFiles(string EmployeeNumber, string Invoice)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                var Path = System.Web.HttpContext.Current.Server.MapPath("~/Files/Temp/HR/Kiosk/Receipts/" + EmployeeNumber);
                var InvoiceName = System.IO.Path.GetFileName(Invoice);

                if (System.IO.File.Exists(Path + "/" + InvoiceName))
                {
                    System.IO.File.Delete(Path + "/" + InvoiceName);
                    System.IO.File.Delete(Path + "/" + InvoiceName.Replace(".pdf", ".xml"));
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
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAttachmentOfReceipt(string EmployeeNumber, string Folio)
        {
            GenericReturn result = new GenericReturn();
            string AttachmentPath = "";
            try
            {
                var receipts = KioskEmployeeService.GetReceiptsAttachmentsByIDs(EmployeeNumber, Folio, BaseGenericRequest);
                foreach (var receipt in receipts)
                {
                    AttachmentPath = "/Files/Temp/HR/Kiosk/Receipts/" + EmployeeNumber + "/" + receipt.FolioSobre + ".pdf";
                    var Path = System.Web.HttpContext.Current.Server.MapPath("~/Files/Temp/HR/Kiosk/Receipts/" + EmployeeNumber);
                    var FullPathPDF = System.Web.HttpContext.Current.Server.MapPath(receipt.FilePDF.Replace("{0}", EmployeeNumber));
                    var FullPathXML = System.Web.HttpContext.Current.Server.MapPath(receipt.FileXML.Replace("{0}", EmployeeNumber));

                    if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                    if (!System.IO.File.Exists(FullPathPDF)) System.IO.File.WriteAllBytes(FullPathPDF, receipt.BytePDF);
                    if (!System.IO.File.Exists(FullPathXML)) System.IO.File.WriteAllBytes(FullPathXML, receipt.ByteXML);
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
                AttachmentPath,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AutocompleteUserInfo(string UserCode)
        {
            int? DepartmentID = 0;
            int? ShiftID = 0;
            string UserFullName = "";
            bool IsValidUser = false;
            try
            {
                var userData = KioskEmployeeService.AutocompleteUserInfo(UserCode, BaseGenericRequest);
                if (userData.Count > 0)
                {
                    DepartmentID = userData.FirstOrDefault().DepartmentID;
                    ShiftID = userData.FirstOrDefault().ShiftID;
                    UserFullName = userData.FirstOrDefault().nombre;
                    IsValidUser = true;
                }
            }
            catch (Exception ex)
            {
                DepartmentID = 0;
                ShiftID = 0;
            }

            return Json(new
            {
                DepartmentID,
                IsValidUser,
                UserFullName,
                ShiftID
            }, JsonRequestBehavior.AllowGet);

        }

    }
}