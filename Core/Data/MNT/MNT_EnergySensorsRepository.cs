using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MNT_EnergySensorsRepository : GenericRepository
    {
        public GenericReturn Insert(int? EnergySensorID, string SensorName, int? EnergySensorFamilyID, int? SensorUseID, int? UnitID, int? IndexKey, string Deviceid, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensors_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iSensorName", DbType.String, SensorName);
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
                db.AddInParameter(dbCommand, "@iSensorUseID", DbType.Int32, SensorUseID);
                db.AddInParameter(dbCommand, "@iUnitID", DbType.Int32, UnitID);
                db.AddInParameter(dbCommand, "@iIndexKey", DbType.Int32, IndexKey);
                db.AddInParameter(dbCommand, "@iDeviceid", DbType.String, Deviceid);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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
                result.ID = (int)db.GetParameterValue(dbCommand, "@oEnergySensorID");
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable List(int? EnergySensorID, string SensorFamiliesIDs, string SensorNames, string SensorUsesIDs, GenericRequest request)
        {
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensors_List]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iSensorFamilies", DbType.String, SensorFamiliesIDs);
                db.AddInParameter(dbCommand, "@iSensorNames", DbType.String, SensorNames);
                db.AddInParameter(dbCommand, "@iSensorUses", DbType.String, SensorUsesIDs);
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

        public DataTable DashboardList(int? EnergySensorFamilyID, DateTime SensorDate, int SensorHour, GenericRequest request)
        {
            dbCommand = db.GetStoredProcCommand("[MNT].[DashboardEnergySensors_List]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
                db.AddInParameter(dbCommand, "@iSensorDate", DbType.DateTime, SensorDate);
                db.AddInParameter(dbCommand, "@iSensorHour", DbType.Int32, SensorHour);
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

        public DataTable GetGaugeValues(DateTime? SensorDate, int? SensorHour, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MNT.GetGaugeValues");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iSensorDate", DbType.DateTime, SensorDate);
                db.AddInParameter(dbCommand, "@iSensorHour", DbType.Int32, SensorHour);
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

        public GenericReturn Delete(int? EnergySensorID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensors_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
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

        public GenericReturn Update(int? EnergySensorID, string SensorName, int? EnergySensorFamilyID, int? SensorUseID, int? UnitID, int? IndexKey, string Deviceid, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensors_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iSensorName", DbType.String, SensorName);
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
                db.AddInParameter(dbCommand, "@iSensorUseID", DbType.Int32, SensorUseID);
                db.AddInParameter(dbCommand, "@iUnitID", DbType.Int32, UnitID);
                db.AddInParameter(dbCommand, "@iIndexKey", DbType.Int32, IndexKey);
                db.AddInParameter(dbCommand, "@iDeviceid", DbType.String, Deviceid);
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

        public DataSet GetDataForConsumptionDayChart(int? EnergySensorID, DateTime? Date, GenericRequest request)
        {
            DataSet ds = new DataSet();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorConsumptionbyDayChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iDate", DbType.Date, Date);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
                //dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataSet GetDataForConsumptionGlobalChart(int? EnergySensorID, int? EnergySensorFamilyID, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            DataSet ds = new DataSet();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorsGlobalConsumptionChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iEnergySensorFamilyID", DbType.Int32, EnergySensorFamilyID);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
                //dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataTable GetUoMListForEnergySensors(string CatalogName, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MNT.GetUoMListForEnergySensors");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogName", DbType.String, CatalogName);
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

        public DataSet GetHourValuesByFamiliesChartGauge(DateTime ChartDate, GenericRequest request)
        {
            DataSet ds = new DataSet();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergySensorFamiliesGaugeChartValues_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.Date, ChartDate);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
                //dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataSet SensorsAlertsExportToExcel(int EnergySensorID, DateTime StartDate, DateTime EndDate, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MNT].[EnergyDashboardSensorsAlertsReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEnergySensorID", DbType.Int32, EnergySensorID);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
    }
}
