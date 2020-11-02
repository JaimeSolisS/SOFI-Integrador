using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class OpportunitiesProgramResponsibleRepository : GenericRepository
    {
        public DataTable List(int? OpportunityProgramID, int? OPResponsableID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("HR.OpportunitiesProgram_Responsable_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOPResponsableID", DbType.Int32, OPResponsableID);
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
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

        public GenericReturn Delete(int? OPResponsableID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Responsable_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOPResponsableID", DbType.Int32, OPResponsableID);
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
