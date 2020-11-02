namespace Core.Data
{
    #region Namespaces

    using Entities;
    using System;
    using System.Data;

    #endregion

    public class NotificationRepository : GenericRepository
    {
        #region Methods

        public DataTable NotificationsPendingToSendList()
        {

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("NotificationsPendingToSendList");
            try
            {
                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }

        }

        public GenericReturn UpdateNotificationsStatus(int? Id, bool? Sent, string CultureId)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("UpdateNotificationsStatus");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iId", DbType.Int32, Id);
                db.AddInParameter(dbCommand, "@iSent", DbType.Boolean, Sent);
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

        public GenericReturn UpdateNotificationsTries(int? Id, string ErrorMessageToSave, string CultureId)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("UpdateNotificationsTries");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iId", DbType.Int32, Id);
                db.AddInParameter(dbCommand, "@iErrorMessageToSave", DbType.String, ErrorMessageToSave);
                db.AddInParameter(dbCommand, "@iCultureId", DbType.String, CultureId);
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
    }
}
