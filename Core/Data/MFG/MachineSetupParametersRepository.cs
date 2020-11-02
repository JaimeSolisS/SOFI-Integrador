using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class MachineSetupParametersRepository : GenericRepository
    {
        public DataTable List(int? MachineSetupID, int? ParameterSectionID, int? MachineParameterID, string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, int? ParameterListID, bool? UseReference, string ReferenceName, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MachineSetupParameters_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineParameterID", DbType.Int32, MachineParameterID);
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
                db.AddInParameter(dbCommand, "@iParameterSectionID", DbType.Int32, ParameterSectionID);
                db.AddInParameter(dbCommand, "@iParameterName", DbType.String, ParameterName);
                db.AddInParameter(dbCommand, "@iParameterTypeID", DbType.Int32, ParameterTypeID);
                db.AddInParameter(dbCommand, "@iParameterLength", DbType.Int32, ParameterLength);
                db.AddInParameter(dbCommand, "@iParameterPrecision", DbType.Int32, ParameterPrecision);
                db.AddInParameter(dbCommand, "@iParameterListID", DbType.Int32, ParameterListID);
                db.AddInParameter(dbCommand, "@iUseReference", DbType.Boolean, UseReference);
                db.AddInParameter(dbCommand, "@iReferenceName", DbType.String, ReferenceName);
                db.AddInParameter(dbCommand, "@iIsCavity", DbType.Boolean, IsCavity);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Delete(int? MachineSetupID, int? ParameterSectionID, int? MachineSetupParameterID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MachineSetupParameters_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
                db.AddInParameter(dbCommand, "@iParameterSectionID", DbType.Int32, ParameterSectionID);
                db.AddInParameter(dbCommand, "@iMachineSetupParameterID", DbType.Int32, MachineSetupParameterID);
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
