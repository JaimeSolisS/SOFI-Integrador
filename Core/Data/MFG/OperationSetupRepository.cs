using Core.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.Data
{
    class OperationSetupRepository : GenericRepository
    {
        public DataTable List(int? OperationSetupID, int? OperationRecordID, int? OperationRecordSeq, int? MaterialID, int? MachineSetupID, int? StatusID, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationSetups_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationSetupID", DbType.Int32, OperationSetupID);
                db.AddInParameter(dbCommand, "@iOperationRecordID", DbType.Int32, OperationRecordID);
                db.AddInParameter(dbCommand, "@iOperationRecordSeq", DbType.Int32, OperationRecordSeq);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
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

        public DataTable ParameterList(int? OperationSetupParameterID, int? OperationSetupID, int? ParameterSectionID, int? Seq, int? OperationParameterID, int? ParameterUoMID, bool? IsMandatory, bool? UseReference, string Reference, decimal? MinValue, decimal? MaxValue, string Value, string ValueList, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationSetupParameters_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationSetupParameterID", DbType.Int32, OperationSetupParameterID);
                db.AddInParameter(dbCommand, "@iOperationSetupID", DbType.Int32, OperationSetupID);
                db.AddInParameter(dbCommand, "@iParameterSectionID", DbType.Int32, ParameterSectionID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, Seq);
                db.AddInParameter(dbCommand, "@iOperationParameterID", DbType.Int32, OperationParameterID);
                db.AddInParameter(dbCommand, "@iParameterUoMID", DbType.Int32, ParameterUoMID);
                db.AddInParameter(dbCommand, "@iIsMandatory", DbType.Boolean, IsMandatory);
                db.AddInParameter(dbCommand, "@iUseReference", DbType.Boolean, UseReference);
                db.AddInParameter(dbCommand, "@iReference", DbType.String, Reference);
                db.AddInParameter(dbCommand, "@iMinValue", DbType.Decimal, MinValue);
                db.AddInParameter(dbCommand, "@iMaxValue", DbType.Decimal, MaxValue);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iValueList", DbType.String, ValueList);
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

        public DataTable ParameterGet(int? OperationSetupParameterID, int? OperationSetupID,  GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationSetupParameters_Get");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationSetupParameterID", DbType.Int32, OperationSetupParameterID);
                db.AddInParameter(dbCommand, "@iOperationSetupID", DbType.Int32, OperationSetupID);
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

        public GenericReturn Parameters_BulkUpsert(int? OperationSetupID, DataTable dt, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.OperationSetupParameters_BulkUpsert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOperationSetupID", DbType.Int32, OperationSetupID);
                SqlParameter p = new SqlParameter("@it_SetupParameters", dt)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);                
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

    }
}
