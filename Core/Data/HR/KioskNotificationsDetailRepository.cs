using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class KioskNotificationsDetailRepository : GenericRepository
    {
        public GenericReturn Update(int? KioskNotificationDetailID, int? KioskNotificationID, string EmployeeUserID, bool? IsReaded, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskNotificationsDetail_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskNotificationDetailID", DbType.Int32, KioskNotificationDetailID);
                db.AddInParameter(dbCommand, "@iKioskNotificationID", DbType.Int32, KioskNotificationID);
                db.AddInParameter(dbCommand, "@iEmployeeUserID", DbType.String, EmployeeUserID);
                db.AddInParameter(dbCommand, "@iIsReaded", DbType.Boolean, IsReaded);
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
    }
}
