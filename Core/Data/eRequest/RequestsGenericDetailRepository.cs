using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class RequestsGenericDetailRepository : GenericRepository
    {
        public GenericReturn Update(RequestsGenericDetail RequestsGenericDetail, string Comments, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[RequestsGenericDetail_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestGenericDetailID", DbType.Int32, RequestsGenericDetail.RequestGenericDetailID);
                db.AddInParameter(dbCommand, "@iConcept", DbType.String, RequestsGenericDetail.Concept);
                db.AddInParameter(dbCommand, "@iReference1", DbType.String, RequestsGenericDetail.Reference1);
                db.AddInParameter(dbCommand, "@iReference2", DbType.String, RequestsGenericDetail.Reference2);
                db.AddInParameter(dbCommand, "@iReference3", DbType.String, RequestsGenericDetail.Reference3);
                db.AddInParameter(dbCommand, "@iReference4", DbType.String, RequestsGenericDetail.Reference4);
                db.AddInParameter(dbCommand, "@iReference5", DbType.String, RequestsGenericDetail.Reference5);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, Comments);
                db.AddInParameter(dbCommand, "@iDateReturn", DbType.DateTime, RequestsGenericDetail.DateReturn);
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
