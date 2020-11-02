using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class OperationTaskRepository : GenericRepository
    {
        #region CRUD Methods
        public DataTable List(int? OperationTaskID, int? OperationSetupParameterID, int? OperationRecordID, string ResponsibleName, int? MachineID,
            int? ShiftID, int? DateType, DateTime? StartDate, DateTime? EndDate, int? StatusID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationTasks_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationTaskID", DbType.Int32, OperationTaskID);
                db.AddInParameter(dbCommand, "@iOperationSetupParameterID", DbType.Int32, OperationSetupParameterID);
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iResponsibleName", DbType.String, ResponsibleName);
                db.AddInParameter(dbCommand, "@iMachineID", DbType.Int32, MachineID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iDateType", DbType.Int32, DateType);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public GenericReturn Update(int? OperationTaskID, int? OperationSetupParameterID, string ProblemDescription, int? ResponsibleID, string SuggestedAction, string AttendantUserName, DateTime? TargetDate, DateTime? ClosedDate, int? StatusID, int? ChangedBy, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationTask_Update");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationTaskID", DbType.Int32, OperationTaskID);
                db.AddInParameter(dbCommand, "@iOperationSetupParameterID", DbType.Int32, OperationSetupParameterID);
                db.AddInParameter(dbCommand, "@iProblemDescription", DbType.String, ProblemDescription);
                db.AddInParameter(dbCommand, "@iResponsibleID", DbType.Int32, ResponsibleID);
                db.AddInParameter(dbCommand, "@iSuggestedAction", DbType.String, SuggestedAction);
                db.AddInParameter(dbCommand, "@iAttendantUserName", DbType.String, AttendantUserName);
                db.AddInParameter(dbCommand, "@iTargetDate", DbType.DateTime, TargetDate);
                db.AddInParameter(dbCommand, "@iClosedDate", DbType.DateTime, ClosedDate);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, ChangedBy);
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

        #endregion

        #region Reports
        public DataSet OperationTasks_Report(string MachineIDs, string MachineSetupIDs, string MaterialIDs, string ProcessIDs, string ShiftIDs, string StatusIDs, string ResponsibleIDs, string Attendant, int? DateType, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationTasks_Report]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineIDs", DbType.String, MachineIDs);
                db.AddInParameter(dbCommand, "@iMachineSetupIDs", DbType.String, MachineSetupIDs);
                db.AddInParameter(dbCommand, "@iMaterialIDs", DbType.String, MaterialIDs);
                db.AddInParameter(dbCommand, "@iProcessIDs", DbType.String, ProcessIDs);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, StatusIDs);
                db.AddInParameter(dbCommand, "@iResponsibleIDs", DbType.String, ResponsibleIDs);
                db.AddInParameter(dbCommand, "@iAttendant", DbType.String, Attendant);
                db.AddInParameter(dbCommand, "@iDateType", DbType.Int32, DateType);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }

            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        #endregion
    }
}
