using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class ErrorLogsRepository :GenericRepository
    {
        #region Methods
        public GenericReturn Insert(string ApplicationName, int ErrorLogId, string ErrorMessage, int FacilityID, string ProcessName,
         string Reference1,
         string Reference2,
         string Reference3,
         string Reference4,
         string Reference5,
         int UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("ErrorLogs_Insert");
            try
            {
                // Parameters                
                db.AddInParameter(dbCommand, "@iApplicationName", DbType.String, ApplicationName);
                db.AddInParameter(dbCommand, "@iCultureId", DbType.String, CultureID);
                db.AddInParameter(dbCommand, "@iErrorLogId", DbType.Int32, ErrorLogId);
                db.AddInParameter(dbCommand, "@iErrorMessage", DbType.String, ErrorMessage);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iProcessName", DbType.String, ProcessName);
                db.AddInParameter(dbCommand, "@iReference1", DbType.String, Reference1);
                db.AddInParameter(dbCommand, "@iReference2", DbType.String, Reference2);
                db.AddInParameter(dbCommand, "@iReference3", DbType.String, Reference3);
                db.AddInParameter(dbCommand, "@iReference4", DbType.String, Reference4);
                db.AddInParameter(dbCommand, "@iReference5", DbType.String, Reference5);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oErrorLogId", DbType.Int32, 0);


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
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }
        #endregion
    }
}
