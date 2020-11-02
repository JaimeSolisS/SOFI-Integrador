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
    public class OpportunitiesProgramRepository : GenericRepository
    {
        public DataTable List(int? OpportunityProgramID, string OpportunityNumber, int? DateTypeID, int? DepartmentID, string ShiftIDs, string Grades, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("HR.OpportunitiesProgram_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iOpportunityNumber", DbType.String, OpportunityNumber);
                db.AddInParameter(dbCommand, "@iDateTypeID", DbType.String, DateTypeID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iGrades", DbType.String, Grades);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
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

        public DataTable SimpleList(string OpportunityNumber, string EmployeeNumber, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_SimpleList]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityNumber", DbType.String, OpportunityNumber);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
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
        public GenericReturn Insert(string Name, string Description, DataTable t_OpportunitiesProgram_Candidates, int? DescriptionTypeID, 
            int? DepartmentID, int? ShiftID, int? GradeID, int? DdlFacilityID, DateTime? ExpirationDate, bool? Enabled, int? CreatedBy, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iName", DbType.String, Name);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, Description);

                SqlParameter p = new SqlParameter("@iResponsibles", t_OpportunitiesProgram_Candidates)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);

                db.AddInParameter(dbCommand, "@iDescriptionTypeID", DbType.Int32, DescriptionTypeID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iGradeID", DbType.Int32, GradeID);
                db.AddInParameter(dbCommand, "@iExpirationDate", DbType.DateTime, ExpirationDate);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iCreatedBy", DbType.Int32, CreatedBy);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, DdlFacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oOpportunityProgramID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oOpportunityProgramID");
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

        public GenericReturn Update(int? OpportunityProgramID, string OpportunityNumber, string Name, string Description, DataTable t_OpportunitiesProgram_Candidates, 
            int? DescriptionTypeID, int? DepartmentID, int? ShiftID, int? GradeID, int? DdlFacilityID, DateTime? ExpirationDate, bool? Enabled, int NotificationTypeID, string FilesToDelete, int? CreatedBy, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iOpportunityNumber", DbType.String, OpportunityNumber);
                db.AddInParameter(dbCommand, "@iName", DbType.String, Name);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, Description);
                SqlParameter p = new SqlParameter("@iResponsibles", t_OpportunitiesProgram_Candidates)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iDescriptionTypeID", DbType.Int32, DescriptionTypeID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iGradeID", DbType.Int32, GradeID);
                db.AddInParameter(dbCommand, "@iExpirationDate", DbType.DateTime, ExpirationDate);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iNotificationTypeID", DbType.Int32, NotificationTypeID);
                db.AddInParameter(dbCommand, "@iFilesToDelete", DbType.String, FilesToDelete);
                db.AddInParameter(dbCommand, "@iCreatedBy", DbType.Int32, CreatedBy);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, DdlFacilityID);
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
        public GenericReturn SetEnableDisable(int? OpportunityProgramID, bool? Enabled, int NotificationTypeID, string StatusName, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_EnableDisable]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iNotificationTypeID", DbType.Int32, NotificationTypeID);
                db.AddInParameter(dbCommand, "@iStatusName", DbType.String, StatusName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
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

        public DataSet ListDataSet(string NumVacant, int DateTypeID, string Departments, string ShiftIDs, string GradeIDs, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {



            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_ExcelReport]");
            try
            {
                // Parameters
                //db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iOpportunityNumber", DbType.String, NumVacant);
                db.AddInParameter(dbCommand, "@iDateTypeID", DbType.Int32, DateTypeID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, Departments);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iGradeIDs", DbType.String, GradeIDs);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                using (DataSet ds = db.ExecuteDataSet(dbCommand))
                {
                    return ds;
                }
            }
            finally
            { dbCommand.Dispose(); }
        }

        public GenericReturn SendNotifications(int? OpportunityProgramID, string Comment, string CandidateID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Candidates_Notifications]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iComment", DbType.String, Comment);
                db.AddInParameter(dbCommand, "@iCandidateID", DbType.String, CandidateID);
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

        public DataTable GetUserAccessToOProgramOptions(GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[GetUserAccessToOProgramOptions]");
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

        public DataTable MediaList(int OpportunityProgramID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgramMedia_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
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

    }
}
