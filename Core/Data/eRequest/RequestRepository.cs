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
    public class RequestRepository : GenericRepository
    {
        public DataTable GetFacility(GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Facilities_ListUserAccess]");
            try
            {
                // Parameters
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

        public DataTable GetFormat4User(GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_ListAccess]");
            try
            {
                // Parameters
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

        public DataTable GetFormat4ID(int FormatID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_4ID]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
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

        public DataTable RequestList(int? RequestID, string FormatIDs, int? Folio, string DepartmentIDs, string StatusIDs, string FacilityIDs, int? DateTypeID, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Requests_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.String, FormatIDs);
                db.AddInParameter(dbCommand, "@iFolio", DbType.Int32, Folio);
                db.AddInParameter(dbCommand, "@iDepartmentIDs", DbType.String, DepartmentIDs);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, StatusIDs);
                db.AddInParameter(dbCommand, "@iFacilityIDs", DbType.String, FacilityIDs);
                db.AddInParameter(dbCommand, "@iDateTypeID", DbType.Int32, DateTypeID);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
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

        public DataTable RequestList(int? RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Requests_List_4Edit]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
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
        public DataTable RequestsGenericDetailList(int RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestsGenericDetail_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
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
        public DataTable RequestLoopsFlowList(int RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestsLoopsFlow_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
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
        public DataTable RequestsGenericDetailListTable(int RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[TableDescriptionFields_List4Table]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
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
        public DataTable RequestsApprovalLogList(int RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestsApprovalLog_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
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

        public GenericReturn Create(DataTable dtGenericDetail, DataTable dtAdditionalFields, int FacilityID, string RequestDate, int DepeartmentID, int FormatID, string Concept, string Specification, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Requests_Create]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iFolio", DbType.String, null);
                db.AddInParameter(dbCommand, "@iRequestDate", DbType.Date, RequestDate);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iConcept", DbType.String, Concept);
                db.AddInParameter(dbCommand, "@iDepartmetID", DbType.Int32, DepeartmentID);
                db.AddInParameter(dbCommand, "@iSpecification", DbType.String, Specification);
                db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oRequestID", DbType.Int32, 0);
                //Parametro tipo tabla
                SqlParameter p = new SqlParameter("@iT_Detail", dtGenericDetail)
                {
                    SqlDbType = SqlDbType.Structured
                };
                SqlParameter a = new SqlParameter("@iT_AdditionalFields", dtAdditionalFields)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                dbCommand.Parameters.Add(a);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oRequestID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn Edit(int RequestID, DataTable dtGenericDetail, DataTable dtAdditionalFields, int FacilityID, string RequestDate, int DepeartmentID, int FormatID, string Concept, string Specification, string FilesToDelete, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Requests_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iFolio", DbType.String, null);
                db.AddInParameter(dbCommand, "@iRequestDate", DbType.Date, RequestDate);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iConcept", DbType.String, Concept);
                db.AddInParameter(dbCommand, "@iFilesToDelete", DbType.String, FilesToDelete);
                db.AddInParameter(dbCommand, "@iDepartmetID", DbType.Int32, DepeartmentID);
                db.AddInParameter(dbCommand, "@iSpecification", DbType.String, Specification);
                db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oRequestID", DbType.Int32, 0);
                //Parametro tipo tabla
                SqlParameter p = new SqlParameter("@iT_Detail", dtGenericDetail)
                {
                    SqlDbType = SqlDbType.Structured
                };
                SqlParameter a = new SqlParameter("@iT_AdditionalFields", dtAdditionalFields)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                dbCommand.Parameters.Add(a);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oRequestID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn SeqValidate(int RequestLoopFlowID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestLoopFlow_SeqValidate]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestLoopFlowID", DbType.Int32, RequestLoopFlowID);
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

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn ApprovalFlowUser(int RequestLoopFlowID, int UserID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestLoopFlow_Validate]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestLoopFlowID", DbType.Int32, RequestLoopFlowID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oUse2FA", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oUse2FA");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public GenericReturn SendCodeValidation(string UserID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Users_GenerateDynamicCode]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserIDorEmail", DbType.String, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public GenericReturn CheckCodeValidation(int UserID, string FAToken, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[User_ValidateCodeVerification]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@i2FAToken", DbType.String, FAToken);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oSignature", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oSignature");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn CheckSignature(int UserID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[User_ValidateSignature]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn UpdateStatus(int RequestID, int StatusID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Requests_UpdateStatus]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
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
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn RequestsLoopsFlowStatusUpdate(int RequestLoopFlowID, string Status, string Comments, int UserID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestsLoopsFlow_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestLoopFlowID", DbType.Int32, RequestLoopFlowID);
                db.AddInParameter(dbCommand, "@iStatus", DbType.String, Status);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public bool ViewReadOnly(int? RequestID, GenericRequest request)
        {
            bool ViewOnly;
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestsApprovalLog_ViewOnly]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oViewOnly", DbType.Boolean, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                ViewOnly = (bool)db.GetParameterValue(dbCommand, "@oViewOnly");
            }
            catch (Exception ex)
            {
                ViewOnly = true;
            }
            finally
            { dbCommand.Dispose(); }
            return ViewOnly;
        }
        public GenericReturn SaveSignature(Byte[] IMG, int UserID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Users_Signature_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iSignature", DbType.Binary, IMG);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable MediaList(int RequestID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestMedia_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { dbCommand.Dispose(); }

        }

        public DataTable GetSignatureIMG(GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Users_Signature]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { dbCommand.Dispose(); }

        }
        public DataTable GetUserAccessToeRequest(GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[GetUserAccessToeRequest]");
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

        public DataTable FieldsConfigurationList(int EntityID, int EntityIndicator, string SystemModuleTag, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("EntityFieldsConfiguration_GetSystemModuleFields");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iEntityID", DbType.Int32, EntityID);
                db.AddInParameter(dbCommand, "@iEntityIndicator", DbType.Int32, EntityIndicator);
                db.AddInParameter(dbCommand, "@iSystemModuleTag", DbType.String, SystemModuleTag);
                //db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }
    }
}
