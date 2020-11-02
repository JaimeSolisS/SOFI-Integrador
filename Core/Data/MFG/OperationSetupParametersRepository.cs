using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class OperationSetupParametersRepository : GenericRepository
    {
        public DataTable List(int? OperationSetupParameterID, int? OperationSetupID, int? ParameterSectionID, int? Seq, int? OperationParameterID, int? ParameterUoMID, bool? IsMandatory, bool? UseReference, string Reference, decimal? MinValue, decimal? MaxValue, string Value, string ValueList, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[OperationSetupParameters_List]");
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
    }
}
