using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class DownTimeRepository : GenericRepository
    {
        public DataTable List(int? DownTimeID, int? ReferenceID, int? RefenceTypeID, DateTime? StartDate, DateTime? EndDate, int? DepartmentID, int? ReasonID, string Comments, int? DownTimeTypeID, int? StatusID, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DownTimes_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDownTimeID", DbType.Int32, DownTimeID);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);               
                db.AddInParameter(dbCommand, "@iRefenceTypeID", DbType.Int32, RefenceTypeID);                
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iReasonID", DbType.Int32, ReasonID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
                db.AddInParameter(dbCommand, "@iDownTimeTypeID", DbType.Int32, DownTimeTypeID);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
        public GenericReturn Insert(int? OperationRecordID, string StartTime, string Endtime, int? DepartmentID, int? ReasonID, string Comments, int? DownTimeTypeID, int? StatusID, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DownTimes_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iStartTime", DbType.String, StartTime);
                db.AddInParameter(dbCommand, "@iEndTime", DbType.String, Endtime);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iReasonID", DbType.Int32, ReasonID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
                db.AddInParameter(dbCommand, "@iDownTimeTypeID", DbType.Int32, DownTimeTypeID);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
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
        public GenericReturn Upsert(int? DownTimeID, int? ReferenceID, int? ReferenceTypeID, string StartTime, string EndTime, int? DepartmentID, int? ReasonID, string Comments,bool CloseTime, int? DownTimeTypeID, int? StatusID, int? ChangeInserts, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DownTimes_Upsert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDownTimeID", DbType.Int32, DownTimeID);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceTypeID", DbType.Int32, ReferenceTypeID);
                db.AddInParameter(dbCommand, "@iStartTime", DbType.String, StartTime);
                db.AddInParameter(dbCommand, "@iEndTime", DbType.String, EndTime);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iReasonID", DbType.Int32, ReasonID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);         
                db.AddInParameter(dbCommand, "@iCloseTime", DbType.Boolean, CloseTime);
                db.AddInParameter(dbCommand, "@iDownTimeTypeID", DbType.Int32, DownTimeTypeID);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iChangeInserts", DbType.Int32, ChangeInserts);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
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
        public GenericReturn Delete(int? DownTimeID, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DownTimes_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDownTimeID", DbType.Int32, DownTimeID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
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

    }
}
