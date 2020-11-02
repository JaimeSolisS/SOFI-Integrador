using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DemoldDefectAlertsRepository : GenericRepository
    {
        public DataTable List(int? DemoldDefectAlertID, int? ShiftID, int? ProductionLineID, int? MoldFamilyID, int? Gross, int? DefectCategory, int? HourInterval, bool? Enabled, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefectAlerts_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDemoldDefectAlertID", DbType.Int32, DemoldDefectAlertID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
                db.AddInParameter(dbCommand, "@iMoldFamilyID", DbType.Int32, MoldFamilyID);
                db.AddInParameter(dbCommand, "@iGross", DbType.Int32, Gross);
                db.AddInParameter(dbCommand, "@iDefectCategory", DbType.Int32, DefectCategory);
                db.AddInParameter(dbCommand, "@iHourInterval", DbType.Int32, HourInterval);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Upsert(DataTable DemoldDefectAlerts, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefectAlerts_Upsert]");
            try
            {
                // Parameters
                SqlParameter p = new SqlParameter("@iDemoldDefectAlerts", DemoldDefectAlerts)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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
