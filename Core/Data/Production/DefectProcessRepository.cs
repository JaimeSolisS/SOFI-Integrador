using Core.Entities;
using Core.Entities.Production;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DefectProcessRepository : GenericRepository
    {
        #region CRUD
        public GenericReturn Upsert(DefectProcess defect, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DefectsProcesses_Upsert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDefectProcessID", DbType.Int32, defect.DefectProcessID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, defect.ProductionProcessID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, defect.ProductionLineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, defect.VAID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, defect.DesignID);
                db.AddInParameter(dbCommand, "@iDefectID", DbType.Int32, defect.DefectID);
                db.AddInParameter(dbCommand, "@iColor", DbType.String, defect.Color);
                db.AddInParameter(dbCommand, "@iFontColor", DbType.String, defect.FontColor);
                db.AddInParameter(dbCommand, "@iGoalValue", DbType.Decimal, defect.GoalValue);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, defect.ShiftID);

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

        public DataTable List(DefectProcess defect, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DefectsProcesses_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDefectProcessID", DbType.Int32, defect.DefectProcessID);
                db.AddInParameter(dbCommand, "@iDefectID", DbType.Int32, defect.DefectID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, defect.ProductionProcessID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, defect.ProductionLineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, defect.VAID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, defect.DesignID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, defect.ShiftID);

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

        public GenericReturn Delete(int DefectProcessID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DefectsProcesses_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDefectProcessID", DbType.Int32, DefectProcessID);
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

        public GenericReturn DeleteAllDetail(int DefectID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DefectsProcesses_DeleteAll");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDefectID", DbType.Int32, DefectID);
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
