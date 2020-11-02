using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class GuardsRepository : GenericRepository
    {
        public DataTable List(int? GuardID, string GuardName, string UniqueNumber, string StatusIDs, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Guards_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGuardID", DbType.Int32, GuardID);
                db.AddInParameter(dbCommand, "@iGuardName", DbType.String, GuardName);
                db.AddInParameter(dbCommand, "@iUniqueNumber", DbType.String, UniqueNumber);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, StatusIDs);
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

        public GenericReturn Insert(string GuardName, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Guards_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGuardName", DbType.String, GuardName);
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

        public GenericReturn Update(int? GuardID, string GuardName, string UniqueNumber, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Guards_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGuardID", DbType.Int32, GuardID);
                db.AddInParameter(dbCommand, "@iGuardName", DbType.String, GuardName);
                db.AddInParameter(dbCommand, "@iUniqueNumber", DbType.String, UniqueNumber);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public bool ValidateGuardCode(string UniqueNumber)
        {

            bool ParamValue = false;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT [SG].[fn_GuardValidateCode](@iUniqueNumber)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUniqueNumber", DbType.String, UniqueNumber);

                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToBoolean();
            }
            catch(Exception ex)
            {
                ParamValue = false;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return ParamValue;
        }
    }
}
