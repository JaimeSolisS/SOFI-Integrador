using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.HR.Models.ViewModels.KioskRequestAdministrator;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.HR.Controllers
{
    public class KioskRequestAdministratorController : BaseController
    {
        //Este RequestNumber solo se usa cuando eres redireccionado de un correo, te envía directamente a la solicitud sin mas, sin filtros
        public ActionResult Index(string PostedRequestNumber)
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var UserAccess = KioskRequestAdministratorService.GetUserAccessToRequestOptions(BaseGenericRequest);
                model.DefaultStatus = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_RequestStatus_DefaultStatus", "Assigned,Open");
                model.AllowViewAllRequests = UserAccess.FirstOrDefault().AllowFullAccess;
                model.AllowAssign = UserAccess.FirstOrDefault().AllowAssign;
                model.AllowMarkDone = UserAccess.FirstOrDefault().AllowMarkDone;
                model.AllowCancel = UserAccess.FirstOrDefault().AllowCancel;
                model.AllowClose = UserAccess.FirstOrDefault().AllowClose;
                model.AllowReOpen = UserAccess.FirstOrDefault().AllowReOpen;

                model.UserFacilitiesList = new SelectList(UserService.GetFacilities(VARG_UserID, VARG_FacilityID, VARG_CultureID), "FacilityID", "FacilityName");
                model.RequestTypesList = new SelectList(vw_CatalogService.List4Select("HR_RequestTypes", BaseGenericRequest, false, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                model.RequestStatusNormalList = vw_CatalogService.List4Select("HR_RequestStatus", BaseGenericRequest, false, Resources.Common.TagAll);
                model.RequestDepartmentsList = new SelectList(DepartmentService.List4Select(BaseGenericRequest, true, Resources.Common.TagAll), "DepartmentID", "DepartmentName");
                model.RequestHistoryDays = Convert.ToInt32(MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Request_History_Days", "60"));
                model.ShiftsList = new SelectList(ShiftService.GetShiftsByAvailableFacilities4Select(BaseGenericRequest, "", false), "ShiftID", "ShiftDescription");
                model.PostedRequestNumber = PostedRequestNumber;

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return View(model);
        }

        public ActionResult SearchRequestData(string RequestUserID, string RequestNumber, string RequestTypeIDs, string RequestStatusIDs, DateTime? StartDate, DateTime? EndDate,
                                               string DepartmentIDs, string EmployeeInfo, string RequestDescription, string ShiftIDs, string FacilityIDs, string RequestResponsible)
        {
            IndexViewModel model = new IndexViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Tbl_Requests.cshtml";
            try
            {

                model.RequestsList = KioskRequestAdministratorService.ListFiltered(RequestNumber, RequestTypeIDs, RequestUserID, RequestStatusIDs, RequestResponsible,
                StartDate, EndDate, DepartmentIDs, EmployeeInfo, RequestDescription, ShiftIDs, FacilityIDs, BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public JsonResult SaveRequest(int? RequestTypeID, string RequestUserID, string RequesterUserName, string RequestDescription,
                                            int? RequestStatusID, int? DepartmentID, int? ShiftID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = KioskRequestAdministratorService.Insert(RequestTypeID, RequestUserID, RequesterUserName, RequestDescription, RequestStatusID, DepartmentID, ShiftID, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetViewRequestModal(int RequestID)
        {
            RequestViewModel model = new RequestViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Mo_RequestView.cshtml";
            try
            {
                var Request = KioskRequestAdministratorService.List(RequestID, BaseGenericRequest).FirstOrDefault();
                model.Employee = Request.RequestedName;
                model.Department = Request.DepartmentName;
                model.Type = Request.RequestTypeName;
                model.OpenDate = Request.DateAdded;
                model.Description = Request.Description;
                model.Shift = Request.ShiftDescription;
                model.Status = Request.RequestStatusName;

            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetAddUserModal()
        {
            Administration.Models.ViewModels.Users.CreateViewModel model = new Administration.Models.ViewModels.Users.CreateViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Mo_AddNewUser.cshtml";
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

        public ActionResult RequestsExportToExcel(string[] ddl_RequestTypes, string[] ddl_RequestStatus, DateTime? StartDate, DateTime? EndDate, string[] ddl_RequestDepartment,
                             string txt_RequestEmployee, string txt_RequestDescription, string txt_RequestNumber, string txt_Responsible, string[] ddl_ShiftsList, string[] ddl_UserFacilities)
        {
            using (DataSet ds = KioskRequestAdministratorService.ListFilteredDatasetExport(ddl_RequestTypes, ddl_RequestStatus, StartDate, EndDate, ddl_RequestDepartment,
                txt_Responsible, txt_RequestEmployee, txt_RequestNumber, txt_RequestDescription, ddl_ShiftsList, ddl_UserFacilities, BaseGenericRequest))
            {
                return this.ExcelExport(ds, Resources.HR.Kiosk.title_Requests);
            }

        }

        [HttpPost]
        public JsonResult ChangeStatusRequest(string RequestIDs, string NewStatus, int? RequestResponsibleID, string Comments)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = KioskRequestAdministratorService.UpdateStatus(RequestIDs, NewStatus, RequestResponsibleID, Comments, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssignResponsibleModal()
        {
            List<User> model = new List<User>();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Mo_AssignResponsible.cshtml";
            try
            {
                model = UserService.List(null, null, null, null, null, null, VARG_FacilityID, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetRequestLog(int RequestID)
        {
            List<KioskUserRequestLog> model = new List<KioskUserRequestLog>();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Tbl_RequestsLog.cshtml";
            try
            {
                model = KioskRequestAdministratorService.RequestsLog_List(RequestID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetUserNotifications()
        {
            List<KioskRequestNotification> model = new List<KioskRequestNotification>();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_UnreadedNotifications.cshtml";
            try
            {
                model = KioskRequestAdministratorService.Notifications_List(BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public ActionResult SetUserNotificationsReaded(string RequestNotificationIDs)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = KioskRequestAdministratorService.Notifications_SetReadedStatus(RequestNotificationIDs, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterResponsible(string SearchUserInfo)
        {
            List<User> result = new List<User>();
            try
            {
                var ResponsiblesList = KioskRequestAdministratorService.GetResponsiblesByFilter(SearchUserInfo, BaseGenericRequest);
                ResponsiblesList.Insert(1, new Core.Entities.User { UserID = 0, FirstName = "-- " + Resources.Common.tt_AddNew, LastName = " --" });
                result = ResponsiblesList;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }

            return Json(new
            {
                ResponsiblesList = result
            }, JsonRequestBehavior.AllowGet);
        }
        //Rolando
        public ActionResult GetRequestLogNotificationID(int RequestLogNotificationID)
        {
            List<KioskRequestNotification> model = new List<KioskRequestNotification>();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Mo_RequestLogNotifications.cshtml";
            try
            {
                model = KioskRequestAdministratorService.Notifications_List4ID(RequestLogNotificationID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }
        public ActionResult GetRequestStatusChangeModal(string StatusTypeChange, int? RequestID)
        {
            RequestStatusUpdateViewModel model = new RequestStatusUpdateViewModel();
            string ViewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Mo_RequestStatusChange.cshtml";
            try
            {
                model.RequestID = RequestID;
                model.StatusTypeChange = StatusTypeChange;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public JsonResult GetRequestNumbers(string RequestNumber)
        {
            List<KioskUserRequest> list = new List<KioskUserRequest>();
            try
            {
                list = KioskRequestAdministratorService.GetRequestNumbers(RequestNumber, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }

            return Json(new
            {
                RequestNumbersList = list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestResponsibles(string UserText)
        {
            List<User> list = new List<User>();
            try
            {
                list = KioskRequestAdministratorService.GetResponsiblesList(UserText, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }

            return Json(new
            {
                RequestResponsiblesList = list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchADUsers(string UserText)
        {
            List<User> UserList = new List<User>();
            string viewPath = "~/Areas/HR/Views/KioskRequestAdministrator/_Tbl_ResponsiblesResult.cshtml";
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

            try
            {
                result = UserService.AddNewUSerAccount(User, new Core.Entities.GenericRequest() { FacilityID = 0, CultureID = DefaultCultureID });
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
    }
}