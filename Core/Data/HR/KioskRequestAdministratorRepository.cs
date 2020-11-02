using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class KioskRequestAdministratorRepository : GenericRepository
    {
        public GenericReturn Insert(int? RequestTypeID, string RequestUserID, string RequesterUserName, string RequestDescription,
            int? RequestStatusID, int? DepartmentID, int? ShiftID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestTypeID", DbType.Int32, RequestTypeID);
                //db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestUserID);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iRequestedName", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, RequestDescription);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, RequestStatusID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable List(int? RequestID, string RequestNumber, string RequestTypeIDs, string RequestUserID, string RequesterUserName, string email,
            string RequestDescription, DateTime? StartDate, DateTime? EndDate, int? RequestCategoryID, string RequestStatusIDs,
            string RequestResponsible, string DepartmentIDs, string ShiftIDs, string FacilityIDs, int? ShiftID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iRequestNumber", DbType.String, RequestNumber);
                db.AddInParameter(dbCommand, "@iRequestTypeIDs", DbType.String, RequestTypeIDs);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestUserID);
                db.AddInParameter(dbCommand, "@iRequestedName", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iRequestedEmail", DbType.String, email);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, RequestDescription);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, RequestCategoryID);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, RequestStatusIDs);
                db.AddInParameter(dbCommand, "@iResponsibleName", DbType.String, RequestResponsible);
                db.AddInParameter(dbCommand, "@iDepartmentIDs", DbType.String, DepartmentIDs);
                db.AddInParameter(dbCommand, "@iFacilityIDs", DbType.String, FacilityIDs);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftIDs);
                //db.AddInParameter(dbCommand, "@iPageNumber", DbType.Int32, PageNumber);
                //db.AddInParameter(dbCommand, "@iRowspPage", DbType.Int32, RowspPage);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }
        public DataSet ListDataSet(int? RequestID, string RequestTypeIDs, string RequestUserID, string RequesterUserName, string email,
                             string RequestDescription, DateTime? StartDate, DateTime? EndDate, int? RequestCategoryID, string RequestStatusIDs,
                             int? RequestResponsibleID, string DepartmentIDs, int? ShiftID, GenericRequest request)
        {

            DataSet ds = new DataSet();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_ExcelReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iRequestTypeIDs", DbType.String, RequestTypeIDs);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestUserID);
                db.AddInParameter(dbCommand, "@iRequestedName", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iRequestedEmail", DbType.String, email);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, RequestDescription);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, RequestCategoryID);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, RequestStatusIDs);
                db.AddInParameter(dbCommand, "@iResponsibleID", DbType.Int32, RequestResponsibleID);
                db.AddInParameter(dbCommand, "@iDepartmentIDs", DbType.String, DepartmentIDs);
                //db.AddInParameter(dbCommand, "@iPageNumber", DbType.Int32, PageNumber);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                ds = db.ExecuteDataSet(dbCommand);

            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataSet ListDataSetFilter(int? RequestID, string RequestTypeIDs, string RequestUserID, string RequesterUserName, string email,
                                    string RequestDescription, DateTime? StartDate, DateTime? EndDate, int? RequestCategoryID, string RequestStatusIDs,
                                    string RequestResponsibleName, string RequestNumber, string DepartmentIDs, int? ShiftID, string ShiftIDs, string FacilityIDs, GenericRequest request)
        {

            DataSet ds = new DataSet();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_ExcelReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iRequestTypeIDs", DbType.String, RequestTypeIDs);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestUserID);
                db.AddInParameter(dbCommand, "@iRequestNumber", DbType.String, RequestNumber);
                db.AddInParameter(dbCommand, "@iRequestedName", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iRequestedEmail", DbType.String, email);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, RequestDescription);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, RequestCategoryID);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, RequestStatusIDs);
                db.AddInParameter(dbCommand, "@iResponsibleName", DbType.String, RequestResponsibleName);
                db.AddInParameter(dbCommand, "@iDepartmentIDs", DbType.String, DepartmentIDs);
                db.AddInParameter(dbCommand, "@iFacilityIDs", DbType.String, FacilityIDs);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftIDs);
                //db.AddInParameter(dbCommand, "@iPageNumber", DbType.Int32, PageNumber);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                ds = db.ExecuteDataSet(dbCommand);

            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataTable SimpleList(string RequestNumber, string RequestedByID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Requests_SimpleList]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestNumber", DbType.String, RequestNumber);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestedByID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public DataTable GetRequestNumbers(string RequestNumber, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Requests_GetRequestNumbers]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestNumber", DbType.String, RequestNumber);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public GenericReturn Update(string RequestIDs, int? RequestTypeID, string RequestUserID, string RequesterUserName, string email, string RequestDescription,
                                    int? RequestCategoryID, string RequestStatus, int? RequestResponsibleID, int? DepartmentID, int? ShiftID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestIDs", DbType.String, RequestIDs);
                db.AddInParameter(dbCommand, "@iRequestTypeID", DbType.Int32, RequestTypeID);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestUserID);
                db.AddInParameter(dbCommand, "@iRequestedName", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iRequestedEmail", DbType.String, email);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, RequestDescription);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, RequestCategoryID);
                db.AddInParameter(dbCommand, "@iRequestStatus", DbType.String, RequestStatus);
                db.AddInParameter(dbCommand, "@iResponsibleID", DbType.Int32, RequestResponsibleID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public GenericReturn Update(string RequestIDs, int? RequestTypeID, string RequestUserID, string RequesterUserName, string email, string RequestDescription,
                                  int? RequestCategoryID, string RequestStatus, int? RequestResponsibleID, int? DepartmentID, int? ShiftID, string Comments, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestIDs", DbType.String, RequestIDs);
                db.AddInParameter(dbCommand, "@iRequestTypeID", DbType.Int32, RequestTypeID);
                db.AddInParameter(dbCommand, "@iRequestedByID", DbType.String, RequestUserID);
                db.AddInParameter(dbCommand, "@iRequestedName", DbType.String, RequesterUserName);
                db.AddInParameter(dbCommand, "@iRequestedEmail", DbType.String, email);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, RequestDescription);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, RequestCategoryID);
                db.AddInParameter(dbCommand, "@iRequestStatus", DbType.String, RequestStatus);
                db.AddInParameter(dbCommand, "@iResponsibleID", DbType.Int32, RequestResponsibleID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public DataTable RequestsLog_List(int? RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("HR.RequestsLog_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public DataTable Notifications_List(GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("HR.RequestsLog_Notifications_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public DataTable Notifications_List4ID(int requestsLogID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("HR.RequestsLog_Notifications_List4ID");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestsLogNotificationsID", DbType.Int32, requestsLogID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }
        public GenericReturn Notifications_Update(string RequestLogNotificationIDs, int? RequestLogID, int? IsReaded, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[RequestsLog_Notifications_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestLogNotificationIDs", DbType.String, RequestLogNotificationIDs);
                db.AddInParameter(dbCommand, "@iRequestLogID", DbType.Int32, RequestLogID);
                db.AddInParameter(dbCommand, "@iIsReaded", DbType.Int32, IsReaded);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable GetUserAccessToRequestOptions(GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[GetUserAccessToRequestOptions]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public DataTable GetResponsiblesByFilter(string SearchUserInfo, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[GetResponsiblesByFilter]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iSearchUserInfo", DbType.String, SearchUserInfo);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.String, request.FacilityID);
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public DataTable GetRequestResponsibles(string UserText, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Request_ResponsiblesList]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserText", DbType.String, UserText);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }
    }
}
