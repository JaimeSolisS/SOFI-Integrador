using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class AppEventProfileRepository : GenericRepository
    {
        #region MISC Methods

        #endregion

        #region Methods

        public bool IsValid(int EventID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            bool IsValid = false;
            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.AppEventsProfiles_isValid");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEventID", DbType.Int32, EventID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oIsValid", DbType.Boolean, 0);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                IsValid = (bool)db.GetParameterValue(dbCommand, "@oIsValid");
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

            return IsValid;
        }

        public GenericReturn UpdateAccess(int EventID, int ProfileID, bool Access, int? AccessTypeID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("dbo.AppEventsProfiles_UpdateAccess");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEventID", DbType.Int32, EventID);
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iAccess", DbType.Boolean, Access);
                db.AddInParameter(dbCommand, "@iAccessTypeID", DbType.Int32, AccessTypeID);
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
