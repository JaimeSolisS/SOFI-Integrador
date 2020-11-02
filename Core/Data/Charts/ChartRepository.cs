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
    class ChartRepository : GenericRepository
    {
        public DataTable Charts_List(int? ChartID, int? ChartType, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartID", DbType.Int32, ChartID);
                db.AddInParameter(dbCommand, "@iChartType", DbType.Int32, ChartType);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable ChartOptions_List(int? ChartOptionID, int? ChartID, int? OptionID, string OptionValue, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("ChartOptions_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartOptionID", DbType.Int32, ChartOptionID);
                db.AddInParameter(dbCommand, "@iChartID", DbType.Int32, ChartID);
                db.AddInParameter(dbCommand, "@iOptionID", DbType.Int32, OptionID);
                db.AddInParameter(dbCommand, "@iOptionValue", DbType.String, OptionValue);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Charts_ComputerSettings_List(int? Chart_SettingID, string ComputerName, int? ChartID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_ComputerSettings_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChart_SettingID", DbType.Int32, Chart_SettingID);
                db.AddInParameter(dbCommand, "@iComputerName", DbType.String, ComputerName);
                db.AddInParameter(dbCommand, "@iChartID", DbType.Int32, ChartID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Charts_ComputerSettingsDetail_List(int? Chart_SetttingDetailID, int? Chart_SettingID, int? OptionID, string OptionValue, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_ComputerSettingsDetail_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChart_SetttingDetailID", DbType.Int32, Chart_SetttingDetailID);
                db.AddInParameter(dbCommand, "@iChart_SettingID", DbType.Int32, Chart_SettingID);
                db.AddInParameter(dbCommand, "@iOptionID", DbType.Int32, OptionID);
                db.AddInParameter(dbCommand, "@iOptionValue", DbType.String, OptionValue);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }


        public DataTable GetChartSliderData(int? Chart_SettingID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("GetChartSliderData");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChart_SettingID", DbType.Int32, Chart_SettingID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public GenericReturn Charts_ComputerSettings_Insert(string ComputerName, int? ChartID, int ShiftID, int? FacilityID, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_ComputerSettings_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iComputerName", DbType.String, ComputerName);
                db.AddInParameter(dbCommand, "@iChartID", DbType.Int32, ChartID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oChart_SettingID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oChart_SettingID");
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
        public GenericReturn Charts_ComputerSettings_Delete(int? Chart_SettingID, int? FacilityID, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_ComputerSettings_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChart_SettingID", DbType.Int32, Chart_SettingID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn Charts_ComputerSettings_QuickUpdate(int? Chart_SettingID, string ColumnName, string Value, int? FacilityID, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_ComputerSettings_QuickUpdate");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChart_SettingID", DbType.Int32, Chart_SettingID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn Charts_ComputerSettingsDetail_QuickUpdate(int? Chart_SetttingDetailID, string ColumnName, string Value, int? FacilityID, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Charts_ComputerSettingsDetail_QuickUpdate");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChart_SetttingDetailID", DbType.Int32, Chart_SetttingDetailID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn Charts_ComputerSettings_UpdateConfiguration(DataTable Charts_ComputerSettings, DataTable Charts_ComputerSettingsDetail, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Charts_ComputerSettings_UpdateConfiguration]");
            try
            {
                // Parameters
                SqlParameter p1 = new SqlParameter("@iCharts_ComputerSettings", Charts_ComputerSettings)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter("@iCharts_ComputerSettingsDetail", Charts_ComputerSettingsDetail)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p2);
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
