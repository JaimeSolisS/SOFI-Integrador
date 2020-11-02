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
    public class SecurityGuardToolsRepository : GenericRepository
    {
        public DataTable List(string AccessCode, int? SecurityGuardLogID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardTools_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
                db.AddInParameter(dbCommand, "@iSecurityGuardLogID", DbType.String, SecurityGuardLogID);
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

        public GenericReturn Insert(SecurityGuardTool SecurityTool, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardTools_Insert]");
            try
            {
                // Parameters
                //SqlParameter p = new SqlParameter("@iSecurityToolsList", SecurityToolsList)
                //{
                //    SqlDbType = SqlDbType.Structured
                //};
                //dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iSecurityGuardLogID", DbType.Int32, SecurityTool.SecurityGuardLogID);
                db.AddInParameter(dbCommand, "@iToolName", DbType.String, SecurityTool.ToolName);
                db.AddInParameter(dbCommand, "@iToolImgPath", DbType.String, SecurityTool.ToolImgPath);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oID", DbType.Int32, 0);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
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

        public GenericReturn UpdateImgPath(int? SecurityGuardToolID, string ToolImgPath, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardTools_UpdateImgPath]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iSecurityGuardToolID", DbType.Int32, SecurityGuardToolID);
                db.AddInParameter(dbCommand, "@iToolImgPath", DbType.String, ToolImgPath);
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

        public DataTable ListByCheckIn(string AccessCode, int? TempAttachmentID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardTool_ListByCheckIn]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
                db.AddInParameter(dbCommand, "@iTempAttachmentID", DbType.Int32, TempAttachmentID);
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

        public DataTable OldNewToolsList(int? TempAttachmentID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardTools_OldNewToolsList]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTempAttachmentID", DbType.Int32, TempAttachmentID);
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

        public GenericReturn UpdateTempData(int? OldSecurityGuardLogID, int? NewSecurityGuardLogID, string ToolsToDisable, DataTable ToolsToAble, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardTools_UpdateTempData]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOldSecurityGuardLogID", DbType.Int32, OldSecurityGuardLogID);
                db.AddInParameter(dbCommand, "@iNewSecurityGuardLogID", DbType.Int32, NewSecurityGuardLogID);
                db.AddInParameter(dbCommand, "@iToolsToDisable", DbType.String, ToolsToDisable);
                SqlParameter p = new SqlParameter("@iToolsToAble", ToolsToAble)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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

        public DataTable GetAvailableToolsByUser(int? TempAttachmentID, string AccessCode, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[GetAvailableToolsByUser]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTempAttachmentID", DbType.Int32, TempAttachmentID);
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
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
