using Core.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.Data
{
    public class MNT_EnergySensorValuesRepository : GenericRepository
    {
        public GenericReturn Insert(int? EnergySensorID, DataTable AlarmConfiguration, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorValues_Upsert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                //db.AddInParameter(dbCommand, "@iValueHour", DbType.Int32, ValueHour);

                SqlParameter p = new SqlParameter("@iAlarmConfiguration", AlarmConfiguration)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);

                //db.AddInParameter(dbCommand, "@iMaxValue", DbType.Decimal, MaxValue);
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

        public DataTable List(int? EnergySensorID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorValues_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
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

    }
}
