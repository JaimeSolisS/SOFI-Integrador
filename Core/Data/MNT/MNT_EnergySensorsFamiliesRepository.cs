using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MNT_EnergySensorsFamiliesRepository : GenericRepository
    {
        public GenericReturn Insert(string FamilyName, decimal? MaxValueperHour, string ImagePath, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorFamilies_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFamilyName", DbType.String, FamilyName);
                db.AddInParameter(dbCommand, "@iMaxValueperHour", DbType.Decimal, MaxValueperHour);
                db.AddInParameter(dbCommand, "@iImagePath", DbType.String, ImagePath);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oSensorFamilyID", DbType.Int32, 0);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oSensorFamilyID");
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

        public DataTable List(int? EnergySensorFamilyID, string FamilyName, DateTime? Date, int? CurrentHour, GenericRequest request)
        {
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorFamilies_List]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
                db.AddInParameter(dbCommand, "@iFamilyName", DbType.String, FamilyName);
                db.AddInParameter(dbCommand, "@iDate", DbType.Date, Date);
                db.AddInParameter(dbCommand, "@iCurrentHour", DbType.Int32, CurrentHour);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }

        public DataTable Dashboard_List(DateTime? Date, int? CurrentHour, GenericRequest request)
        {
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergyDashboardFamilies_List]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iDate", DbType.Date, Date);
                db.AddInParameter(dbCommand, "@iCurrentHour", DbType.Int32, CurrentHour);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }


        public GenericReturn Delete(int? EnergySensorFamilyID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MNT.EnergySensorFamilies_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
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

        public GenericReturn Update(int? EnergySensorFamilyID, string FamilyName, decimal? MaxValueperHour, string ImagePath, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorFamilies_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
                db.AddInParameter(dbCommand, "@iFamilyName", DbType.String, FamilyName);
                db.AddInParameter(dbCommand, "@iMaxValueperHour", DbType.Decimal, MaxValueperHour);
                db.AddInParameter(dbCommand, "@iImagePath", DbType.String, ImagePath);
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

        public GenericReturn CopyAs(int? EnergySensorID, string SensorName, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MNT.EnergySensors_CopyAs");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iSensorName", DbType.String, SensorName);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oEnergySensorID", DbType.Int32, 0);
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
