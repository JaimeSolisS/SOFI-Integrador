using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class OpportunitiesProgramCandidatesRepository : GenericRepository
    {
        public DataTable List(int? OpportunityProgramID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Candidates_List]");
            try
            {
                // Parameters
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

        public GenericReturn Update(int? OPCandidateID, int? OpportunityProgramID, string CandidateID, string ShortMessage, bool? IsDiscarted, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Candidates_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOPCandidateID", DbType.Int32, OPCandidateID);
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iCandidateID", DbType.String, CandidateID);
                db.AddInParameter(dbCommand, "@iShortMessage", DbType.String, ShortMessage);
                db.AddInParameter(dbCommand, "@iIsDiscarted", DbType.Boolean, IsDiscarted);
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

        public GenericReturn Insert(int? OpportunityProgramID, string CandidateID, string ShortMessage, bool? IsDiscarted, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[OpportunitiesProgram_Candidates_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOpportunityProgramID", DbType.Int32, OpportunityProgramID);
                db.AddInParameter(dbCommand, "@iCandidateID", DbType.String, CandidateID);
                db.AddInParameter(dbCommand, "@iShortMessage", DbType.String, ShortMessage);
                db.AddInParameter(dbCommand, "@iIsDiscarted", DbType.Boolean, IsDiscarted);
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
