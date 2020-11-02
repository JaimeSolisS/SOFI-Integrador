using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class OperationTaskDetailsRepository : GenericRepository
    {
        public DataTable List(int? OperationTaskDetailID, int? OperationTaskID, string Comments, int? rogress, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationTaskDetails_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationTaskDetailID", DbType.Int32, OperationTaskDetailID);
                db.AddInParameter(dbCommand, "@iOperationTaskID", DbType.Int32, OperationTaskID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
                db.AddInParameter(dbCommand, "@iProgress", DbType.Int32, rogress);
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

        public GenericReturn Insert(int? OperationTaskID, string Comments, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationTaskDetails_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationTaskID", DbType.Int32, OperationTaskID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
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
                result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
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
