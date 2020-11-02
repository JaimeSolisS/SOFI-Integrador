using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class KioskNotificationsRepository : GenericRepository
    {
        public DataTable List(int? KioskNotificationID, string EmployeeID, int? ReferenceID, int? ReferenceTypeID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskNotifications_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskNotificationID", DbType.Int32, KioskNotificationID);
                db.AddInParameter(dbCommand, "@iEmployeeUserID", DbType.String, EmployeeID);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceTypeID", DbType.Int32, ReferenceTypeID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

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
