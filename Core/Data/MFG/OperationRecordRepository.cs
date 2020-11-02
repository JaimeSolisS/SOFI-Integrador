using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class OperationRecordRepository : GenericRepository
    {
        public DataTable List(int? OperationRecordID, int? MachineID, int? ShiftID, DateTime? StartDate, DateTime? EndDate, int? StatusID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationRecords_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iMachineID", DbType.Int32, MachineID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
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
        public GenericReturn Insert(int? MachineID, int? ShiftID, DateTime? OperationDate, int? MachineSetupID, int? MaterialID, string OperatorNumber, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationRecords_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineID", DbType.Int32, MachineID);
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iOperationDate", DbType.DateTime, OperationDate);
                db.AddInParameter(dbCommand, "@iOperatorNumber", DbType.String, OperatorNumber);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oOperationRecordID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oOperationRecordID");
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
        public GenericReturn Close(int? OperationRecordID, int OperationProductionID, int? CurrentShotNumber, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationRecords_Close]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iCurrentShotNumber", DbType.Int32, CurrentShotNumber);
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
        public GenericReturn CloseShift(int? OperationRecordID, int OperationProductionID, int? CurrentShotNumber, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationRecords_CloseShift]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iCurrentShotNumber", DbType.Int32, CurrentShotNumber);
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

        public string GetJulianDay(DateTime Date, string DefaultValue, GenericRequest request)
        {
            string ParamValue = DefaultValue;
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT [MFG].[GetJulianDay] (@iDate,@iFacilityID,@iUserID,@iCultureID)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDate", DbType.DateTime, Date);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToString();
            }
            catch
            {
                ParamValue = DefaultValue;
            }
            finally
            {
                dbCommand.Dispose();
            }
            return ParamValue;
        }

        #region Reports
        public DataSet Gaskets_EER_Yield_Report(int? Shift, int? MachineID, DateTime? Startdate, DateTime? Enddate, DateTime? StartDateExcel, DateTime? EndDateExcel, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[Gaskets_EER_Yield_Report]");
            dbCommand.CommandTimeout = 500;
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iShift", DbType.Int32, Shift);
                db.AddInParameter(dbCommand, "@iMachineID", DbType.Int32, MachineID);
                db.AddInParameter(dbCommand, "@iStartdate", DbType.DateTime, Startdate);
                db.AddInParameter(dbCommand, "@iEnddate", DbType.DateTime, Enddate);
                db.AddInParameter(dbCommand, "@iStartDateExcel", DbType.DateTime, StartDateExcel);
                db.AddInParameter(dbCommand, "@iEndDateExcel", DbType.DateTime, EndDateExcel);
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
        public DataSet Gaskets_EER_Yield_Details_Report(DateTime? StartDateExcel, DateTime? EndDateExcel, string Shifts, string Machines, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[Gaskets_EER_Yield_Details_Report]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iStartDateExcel", DbType.DateTime, StartDateExcel);
                db.AddInParameter(dbCommand, "@iEndDateExcel", DbType.DateTime, EndDateExcel);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, Shifts);
                db.AddInParameter(dbCommand, "@iMachineIDs", DbType.String, Machines);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }catch(Exception ex)
            {
                var error = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataSet OperationRecords_Parameters_Report(string MachineParameterID, string MachimesIDs, string ProductionProcessID, 
            string ShiftID, string MaterialID, bool? IsGoodAnswer, DateTime? StartDatetime, DateTime? EndDatetime, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationRecords_Parameters_Report]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineParameterIDs", DbType.String, MachineParameterID);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachimesIDs);
                db.AddInParameter(dbCommand, "@iProductionProcessIDs", DbType.String, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftID);
                db.AddInParameter(dbCommand, "@iMaterialIDs", DbType.String, MaterialID);
                db.AddInParameter(dbCommand, "@iIsGoodAnswer", DbType.Boolean, IsGoodAnswer);
                db.AddInParameter(dbCommand, "@iStartDatetime", DbType.DateTime, StartDatetime);
                db.AddInParameter(dbCommand, "@iEndDatetime", DbType.DateTime, EndDatetime);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }catch(Exception ex)
            {
                var error = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataSet OperationRecords_GasketDiameters_Report(string GasketID, string MachimesIDs, string Cavity, string ProductionProcessIDs, 
            string ShiftIDs, string MaterialIDs, DateTime? StartDatetime, DateTime? EndDatetime, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationRecords_GasketDiameters_Report]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGasketID", DbType.String, GasketID);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachimesIDs);
                db.AddInParameter(dbCommand, "@iCavity", DbType.String, Cavity);
                db.AddInParameter(dbCommand, "@iProductionProcessIDs", DbType.String, ProductionProcessIDs);
                db.AddInParameter(dbCommand, "@iShiftIDs", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iMaterialIDs", DbType.String, MaterialIDs);
                db.AddInParameter(dbCommand, "@iStartDatetime", DbType.DateTime, StartDatetime);
                db.AddInParameter(dbCommand, "@iEndDatetime", DbType.DateTime, EndDatetime);
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



        public DataSet DashboardGasket_YieldDefectsReport(DateTime? ChartDate, string MachinesIDs, int? ShiftID, int? ProductionProcessID, int? MaterialID, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_YieldDefectsReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataSet DashboardGasket_DowntimesReport(DateTime? ChartDate, string MachinesIDs, int? ShiftID, int? ProductionProcessID, int? MaterialID, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_DowntimesReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, null);
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

        public DataSet DashboardGasket_ProductionReport(DateTime? ChartDate, string MachinesIDs, int? ShiftID, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_ProductionReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iMachineIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
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

        public DataSet DashboardChartsAllReports(
            DateTime ChartDate
            , int? ShiftID
            //Datos para Yield
            , string YieldMachines
            , int? YieldProcessID
            , int? YieldMaterialID
            //Datos para Downtimes
            , string DownTimesMachines
            , int? DownTimesProcessID
            , int? DownTimesMaterialID
            //Datos de Production
            , string ProductionMachines
            , GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardChartsAllReports]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);

                //Yield Parameters
                db.AddInParameter(dbCommand, "@iYieldMachines", DbType.String, YieldMachines);
                db.AddInParameter(dbCommand, "@iYieldProcessID", DbType.Int32, YieldProcessID);
                db.AddInParameter(dbCommand, "@iYieldMaterialID", DbType.Int32, YieldMaterialID);
                //Downtimes Parameters
                db.AddInParameter(dbCommand, "@iDownTimesMachines", DbType.String, DownTimesMachines);
                db.AddInParameter(dbCommand, "@iDownTimesProcessID", DbType.Int32, DownTimesProcessID);
                db.AddInParameter(dbCommand, "@iDownTimesMaterialID", DbType.Int32, DownTimesMaterialID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, null);
                //Production Parameters
                db.AddInParameter(dbCommand, "@iProductionMachines", DbType.String, ProductionMachines);

                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        #endregion


        #region Charts   

        public DataSet YieldDefectsChart(DateTime? ChartDate, string MachinesIDs, int? ProductionProcessID, int? MaterialID, int? ShiftID, int? Top, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_YieldDefectsChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, Top);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);

            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataSet EERDowntimesChart(DateTime? ChartDate, string MachinesIDs, int? ProductionProcessID, int? MaterialID, int? ShiftID, int? Top, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_EERDowntimesChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, Top);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataTable Dashboard_ProductionChart(DateTime? ChartDate, string MachinesIDs, int? ShiftID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_ProductionChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationDate", DbType.Date, ChartDate);
                db.AddInParameter(dbCommand, "@iMachineIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
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
        public DataSet DowntimesChart(DateTime? ChartDate, string MachinesIDs, int? ProductionProcessID, int? MaterialID, int? ShiftID, int? Top, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DashboardGasket_DowntimesChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iMachinesIDs", DbType.String, MachinesIDs);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, Top);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        #endregion
    }
}
