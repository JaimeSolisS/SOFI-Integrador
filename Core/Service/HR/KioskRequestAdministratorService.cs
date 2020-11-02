using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class KioskRequestAdministratorService
    {
        private static KioskRequestAdministratorRepository _rep;

        static KioskRequestAdministratorService()
        {
            _rep = new KioskRequestAdministratorRepository();
        }

        public static GenericReturn Insert(int? RequestTypeID, string RequestUserID, string RequesterUserName, string RequestDescription,
                                            int? RequestStatusID, int? DepartmentID, int? ShiftID, GenericRequest request)
        {
            return _rep.Insert(RequestTypeID, RequestUserID, RequesterUserName, RequestDescription, RequestStatusID, DepartmentID, ShiftID, request);
        }

        public static List<KioskUserRequest> List(int? RequestID, string RequestNumber, string RequestTypeIDs, string RequestUserID, string RequesterUserName, string email, string RequestDescription,
                                                    DateTime? StartDate, DateTime? EndDate, int? RequestCategoryID, string RequestStatusIDs, string RequestResponsibleID,
                                                    string DepartmentIDs, string FacilityIDs, int? ShiftID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(RequestID, RequestNumber, RequestTypeIDs, RequestUserID, RequesterUserName, email, RequestDescription, StartDate, EndDate, RequestCategoryID,
                RequestStatusIDs, RequestResponsibleID, DepartmentIDs, null, FacilityIDs, ShiftID, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }
        }

        public static List<KioskUserRequest> ListFiltered(string RequestNumber, string RequestTypeIDs, string RequestUserID, string RequestStatusIDs, string RequestResponsible, DateTime? StartDate, DateTime? EndDate, string DepartmentIDs,
                                               string EmployeeInfo, string RequestDescription, string ShiftIDs, string FacilityIDs, GenericRequest request)
        {
            if (FacilityIDs == null)
            {
                var Facilities = UserService.GetFacilities(request.UserID, request.FacilityID, request.CultureID);
                if (Facilities != null)
                {
                    FacilityIDs = string.Join(",", Facilities.Select(i => i.FacilityID));
                }
            }

            using (DataTable dt = _rep.List(null, RequestNumber, RequestTypeIDs, RequestUserID, EmployeeInfo, EmployeeInfo, RequestDescription, StartDate, EndDate, null,
                RequestStatusIDs, RequestResponsible, DepartmentIDs, ShiftIDs, FacilityIDs, null, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }
        }

        public static List<KioskUserRequest> List(int? RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(RequestID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }
        }

        public static List<KioskUserRequest> List(string RequestUserID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, RequestUserID, null, null, null, null, null, null, null, null, null, null, null, null, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }
        }


        public static List<KioskUserRequest> List(string RequestUserID, string RequestNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, RequestNumber, null, RequestUserID, null, null, null, null, null, null, null, null, null, null, null, null, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }
        }


        public static DataSet ListFilteredDataset(string[] ddl_RequestTypes, string[] ddl_RequestStatus, DateTime? StartDate, DateTime? EndDate, string[] ddl_RequestDepartment,
                                                string RequestEmployee, string RequestDescription, GenericRequest request)
        {

            var RequestTypes = ddl_RequestTypes != null ? string.Join<string>(",", ddl_RequestTypes) : null;
            var RequestStatus = ddl_RequestStatus != null ? string.Join<string>(",", ddl_RequestStatus) : null;
            var RequestDepartment = ddl_RequestDepartment != null ? string.Join<string>(",", ddl_RequestDepartment) : null;


            using (DataSet ds = _rep.ListDataSet(null, RequestTypes, null, RequestEmployee, RequestEmployee, RequestDescription, StartDate, EndDate, null,
                RequestStatus, null, RequestDepartment, null, request))
            {
                return ds;
            }
        }

        public static DataSet ListFilteredDatasetExport(string[] ddl_RequestTypes, string[] ddl_RequestStatus, DateTime? StartDate, DateTime? EndDate, string[] ddl_RequestDepartment,
                                        string RequestResponsibleName, string RequestEmployee, string RequestNumber, string RequestDescription, string[] ddl_ShiftsList, string[] ddl_UserFacilities, GenericRequest request)
        {

            var RequestTypes = ddl_RequestTypes != null ? string.Join<string>(",", ddl_RequestTypes) : null;
            var RequestStatus = ddl_RequestStatus != null ? string.Join<string>(",", ddl_RequestStatus) : null;
            var RequestDepartment = ddl_RequestDepartment != null ? string.Join<string>(",", ddl_RequestDepartment) : null;
            var ShiftIDs = ddl_ShiftsList != null ? string.Join<string>(",", ddl_ShiftsList) : null;
            var FacilityIDs = ddl_UserFacilities != null ? string.Join<string>(",", ddl_UserFacilities) : null;


            using (DataSet ds = _rep.ListDataSetFilter(null, RequestTypes, null, RequestEmployee, RequestEmployee, RequestDescription, StartDate, EndDate, null,
                RequestStatus, RequestResponsibleName, RequestNumber, RequestDepartment, null, ShiftIDs, FacilityIDs, request))
            {
                return ds;
            }
        }

        public static List<KioskUserRequest> SimpleList(string RequestNumber, string RequestedByID, GenericRequest request)
        {
            using (DataTable dt = _rep.SimpleList(RequestNumber, RequestedByID, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }

        }
        public static List<KioskUserRequest> GetRequestNumbers(string RequestNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.GetRequestNumbers(RequestNumber, request))
            {
                List<KioskUserRequest> _list = dt.ConvertToList<KioskUserRequest>();
                return _list;
            }
        }


        public static GenericReturn Update(string RequestIDs, int? RequestTypeID, string RequestUserID, string RequesterUserName, string email, string RequestDescription,
                                                int? RequestCategoryID, string RequestStatus, int? RequestResponsibleID, int? DepartmentID, int? ShiftID, GenericRequest request)
        {
            return _rep.Update(RequestIDs, RequestTypeID, RequestUserID, RequesterUserName, email, RequestDescription, RequestCategoryID, RequestStatus, RequestResponsibleID, DepartmentID, ShiftID, request);
        }

        public static GenericReturn UpdateStatus(string RequestIDs, string RequestStatus, int? RequestResponsibleID, string Comments, GenericRequest request)
        {
            return _rep.Update(RequestIDs, null, null, null, null, null, null, RequestStatus, RequestResponsibleID, null, null, Comments, request);
        }

        public static List<KioskUserRequestLog> RequestsLog_List(int? RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestsLog_List(RequestID, request))
            {
                List<KioskUserRequestLog> _list = dt.ConvertToList<KioskUserRequestLog>();
                return _list;
            }
        }

        public static List<KioskRequestNotification> Notifications_List(GenericRequest request)
        {
            using (DataTable dt = _rep.Notifications_List(request))
            {
                List<KioskRequestNotification> _list = dt.ConvertToList<KioskRequestNotification>();
                return _list;
            }
        }

        public static List<KioskRequestNotification> Notifications_List4ID(int requestsLogID, GenericRequest request)
        {
            using (DataTable dt = _rep.Notifications_List4ID(requestsLogID, request))
            {
                List<KioskRequestNotification> _list = dt.ConvertToList<KioskRequestNotification>();
                return _list;
            }
        }

        public static GenericReturn Notifications_Update(string RequestLogNotificationIDs, int? RequestLogID, int? IsReaded, GenericRequest request)
        {
            return _rep.Notifications_Update(RequestLogNotificationIDs, RequestLogID, IsReaded, request);
        }

        public static GenericReturn Notifications_SetReadedStatus(string RequestLogNotificationIDs, GenericRequest request)
        {
            return _rep.Notifications_Update(RequestLogNotificationIDs, null, 1, request);
        }

        public static List<HRUserAccess> GetUserAccessToRequestOptions(GenericRequest request)
        {
            using (DataTable dt = _rep.GetUserAccessToRequestOptions(request))
            {
                List<HRUserAccess> _list = dt.ConvertToList<HRUserAccess>();
                return _list;
            }
        }

        public static List<User> GetResponsiblesByFilter(string SearchUserInfo, GenericRequest request, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.GetResponsiblesByFilter(SearchUserInfo, request))
            {
                List<User> _list = dt.ConvertToList<User>();
                if (_list != null && EmptyFirst)
                {
                    _list.Insert(0, new User() { ID = 0, FirstName = "", LastName = "", eMail = "" });
                }
                return _list;
            }
        }

        public static List<User> GetResponsiblesList(string UserText, GenericRequest request)
        {
            using (DataTable dt = _rep.GetRequestResponsibles(UserText, request))
            {
                List<User> _list = dt.ConvertToList<User>();
                return _list;
            }
        }

    }
}
