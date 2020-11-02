using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MFG_ProductionVATsRepository : GenericRepository
    {
        public DataTable List(int? VATID, string VATName, int? ShiftID, int? ProductionLineID, bool? Enabled, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[ProductionVATs_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iVATName", DbType.String, VATName);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
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

        public DataTable List(int? VATID, string VATName, string ShiftID, int? ProductionProcessID, string ProductionLineID, bool? Enabled, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[ProductionVATs_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iVATName", DbType.String, VATName);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.String, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.String, ProductionLineID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
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

        public GenericReturn Delete(int? VATID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[ProductionVATs_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
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

        public GenericReturn Insert(string VATName, int? ShiftID, int? ProductionLineID, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[ProductionVATs_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVATName", DbType.String, VATName);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
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

        public GenericReturn Update(int? VATID, string VATName, int? ShiftID, int? ProductionLineID, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[ProductionVATs_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iVATName", DbType.String, VATName);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
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
    }
}
