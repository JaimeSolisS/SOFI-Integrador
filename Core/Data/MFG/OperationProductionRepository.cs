using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class OperationProductionRepository : GenericRepository
    {
        public DataTable List(int? OperationProductionID, int? OperationRecordID, decimal? CycleTime, int? CavitiesNumber, int? ProducedQty, int? InitialShotNumber, int? FinalShotNumber, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationProduction_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iCycleTime", DbType.Decimal, CycleTime);
                db.AddInParameter(dbCommand, "@iCavitiesNumber", DbType.Int32, CavitiesNumber);
                db.AddInParameter(dbCommand, "@iProducedQty", DbType.Int32, ProducedQty);
                db.AddInParameter(dbCommand, "@iInitialShotNumber", DbType.Int32, InitialShotNumber);
                db.AddInParameter(dbCommand, "@iFinalShotNumber", DbType.Int32, FinalShotNumber);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
        public GenericReturn Upsert(int? OperationProductionID, int? OperationRecordID, decimal? CycleTime, int? CavitiesNumber, int? ProducedQty, int? InitialShotNumber, int? FinalShotNumber, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationProduction_Upsert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iCycleTime", DbType.Decimal, CycleTime);
                db.AddInParameter(dbCommand, "@iCavitiesNumber", DbType.Int32, CavitiesNumber);
                db.AddInParameter(dbCommand, "@iProducedQty", DbType.Int32, ProducedQty);
                db.AddInParameter(dbCommand, "@iInitialShotNumber", DbType.Int32, InitialShotNumber);
                db.AddInParameter(dbCommand, "@iFinalShotNumber", DbType.Int32, FinalShotNumber);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
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
        public DataTable Details_List(int? OperationProductionDetailID, int? OperationProductionID, int? Hour, int? ShotNumber, int? ProducedQty, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationProductionDetails_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionDetailID", DbType.Int32, OperationProductionDetailID);
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iHour", DbType.Int32, Hour);
                db.AddInParameter(dbCommand, "@iShotNumber", DbType.Int32, ShotNumber);
                db.AddInParameter(dbCommand, "@iProducedQty", DbType.Int32, ProducedQty);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
        public GenericReturn Details_Upsert(int? OperationProductionDetailID, int? OperationProductionID, int? Hour, int? ShotNumber, int? ProducedQty, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationProductionDetails_Upsert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionDetailID", DbType.Int32, OperationProductionDetailID);
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iHour", DbType.Int32, Hour);
                db.AddInParameter(dbCommand, "@iShotNumber", DbType.Int32, ShotNumber);
                db.AddInParameter(dbCommand, "@iProducedQty", DbType.Int32, ProducedQty);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
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

        public DataTable DetailsLogs_List(int? OperationProductionDetailLogID, int? OperationProductionID, int? CurrentShotNumber, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationProductionDetailsLogs_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionDetailLogID", DbType.Int32, OperationProductionDetailLogID);
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iCurrentShotNumber", DbType.Int32, CurrentShotNumber);
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

        public GenericReturn DetailsLogs_Insert(int? OperationProductionID, int? CurrentShotNumber,bool RecalculateProductionHours, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationProductionDetailsLogs_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iCurrentShotNumber", DbType.Int32, CurrentShotNumber);                
                db.AddInParameter(dbCommand, "@iRecalculateProductionHours", DbType.Boolean, RecalculateProductionHours);
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



        public DataTable ShiftHours(int? OperationProductionID, int? OperationRecordID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[mfg].[OperationProduction_ShiftHours]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
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

        public int GetHour(int OperationProductionID)
        {
            int Hour = 0;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT  [MFG].[fn_OperationProduction_GetHour](@iOperationProductionID)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);

                // Execute Query
                Hour = Convert.ToInt32(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return Hour;
        }
        public bool IsCapturedHour(int OperationProductionID,int Hour)
        {
            bool result = false;
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT  [MFG].[fn_OperationProduction_IsCapturedHour](@iOperationProductionID,@iHour)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationProductionID", DbType.Int32, OperationProductionID);
                db.AddInParameter(dbCommand, "@iHour", DbType.Int32, Hour);
                // Execute Query
                result = Convert.ToBoolean(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }
    }
}
