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
    public class KioskEmployeeRepository : GenericRepository
    {
        public GenericReturn Comments_Insert(int KioskCommentCategoryID, string Comment, string EmployeeID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskEmployeeSuggestions_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskCommentCategoryID", DbType.Int32, KioskCommentCategoryID);
                db.AddInParameter(dbCommand, "@iEmployeeID", DbType.String, EmployeeID);
                db.AddInParameter(dbCommand, "@iComment", DbType.String, Comment);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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

        public DataTable GetUserData(string serCode)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetUserData]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserCode", DbType.String, serCode);
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

        public DataTable GetEmployeeMovements(string UserPrivateInfo, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetEmployeeMovements]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@UserPrivateInfo", DbType.String, UserPrivateInfo);
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

        public DataTable GetNotifications(int? KioskNotificationDetailID, int? KioskNotificationID, string EmployeeUserID, bool? IsReaded, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskNotificationsDetail_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskNotificationDetailID", DbType.Int32, KioskNotificationDetailID);
                db.AddInParameter(dbCommand, "@iKioskNotificationID", DbType.Int32, KioskNotificationID);
                db.AddInParameter(dbCommand, "@iEmployeeUserID", DbType.String, EmployeeUserID);
                db.AddInParameter(dbCommand, "@iIsReaded", DbType.Boolean, IsReaded);
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

        public DataSet GetPrePayroll(string ClaveEmp)
        {
            // Get DbCommand to Execute the Update Procedure
            DataSet ds = new DataSet();
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetPrePayroll]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, ClaveEmp);
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataSet GetPaymentReceipts(string ClaveEmp)
        {
            // Get DbCommand to Execute the Update Procedure
            DataSet ds = new DataSet();
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetPaymentReceipts]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserCode", DbType.String, ClaveEmp);
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public GenericReturn Receipts_Notifications(string EmployeeNumber, string ReceiptNumber, string InvoiceName,
            string HTMLBodyInfo, string eMailAddressTo, string eMailAttachments, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskEmployeeReceipts_Notifications]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
                db.AddInParameter(dbCommand, "@iInvoiceName", DbType.String, InvoiceName); //Solo aplica cuando se envía un solo recibo
                db.AddInParameter(dbCommand, "@iReceiptNumber", DbType.String, ReceiptNumber);
                db.AddInParameter(dbCommand, "@iHTMLBodyInfo", DbType.String, HTMLBodyInfo);
                db.AddInParameter(dbCommand, "@ieMailAddressTo", DbType.String, eMailAddressTo);
                db.AddInParameter(dbCommand, "@ieMailAttachments", DbType.String, eMailAttachments);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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

        public DataTable GetUserCoursesPermit(string UserCode, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetUserCoursesPermit]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserCode", DbType.String, UserCode);
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

        public DataTable GetCoursesAvailable(string clave, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetCoursesAvailable]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iclave", DbType.String, clave);
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

        public DataTable GetCoursesInscribed(string clave, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_GetCoursesInscribed]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iclave", DbType.String, clave);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.String, request.FacilityID);
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

        #region PointsSystems
        public DataTable Exec_PointSystemQueriesByEmployee(string EmployeeNumber, string QueryType, string RFC, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Exec_PointSystemQueriesByEmployee]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
                db.AddInParameter(dbCommand, "@iRFC", DbType.String, RFC);
                db.AddInParameter(dbCommand, "@iQueryType", DbType.String, QueryType);
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
        #endregion

        public GenericReturn Insert_PointsExchangePetitions(string ClaveEmp, string RFC, DataTable listaArticulos, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Insert_PointsExchangePetitions]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iClaveEmp", DbType.String, ClaveEmp);
                db.AddInParameter(dbCommand, "@iRFC", DbType.String, RFC);
                SqlParameter p = new SqlParameter("@ilistaArticulos", listaArticulos)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                //result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                //result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
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

        public GenericReturn InscribeUserToCourse(int? CourseID, string EmployeeNumber, string Name, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[InscribeUserToCourse]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCourseID", DbType.Int32, CourseID);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
                db.AddInParameter(dbCommand, "@iName", DbType.String, Name);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                //result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                //result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
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

        public DataSet GetReceiptsAttachmentsByIDs(string EmployeeID, string Invoices, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            DataSet ds = new DataSet();
            dbCommand = db.GetStoredProcCommand("[HR].[GetReceiptsAttachmentsByIDs]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEmployeeID", DbType.String, EmployeeID);
                db.AddInParameter(dbCommand, "@iInvoices", DbType.String, Invoices);
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataTable AutocompleteUserInfo(string UserCode, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[Kiosk_AutocompleteUserInfo]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserCode", DbType.String, UserCode);
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
    }
}
