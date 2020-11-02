using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class ProductionRejectRepository : GenericRepository
    {
        public DataTable List(int? ProductionRejectID, int? ReferenceID, int? ReferenceTypeID, int? RejectTypeID, decimal? Quantity,int? Hour, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("mfg.ProductionRejects_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionRejectID", DbType.Int32, ProductionRejectID);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceTypeID", DbType.Int32, ReferenceTypeID);
                db.AddInParameter(dbCommand, "@iRejectTypeID", DbType.Int32, RejectTypeID);
                db.AddInParameter(dbCommand, "@iQuantity", DbType.Decimal, Quantity);
                //@iHour
                db.AddInParameter(dbCommand, "@iHour", DbType.Int32, Hour);
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
        public GenericReturn Upsert(int? ProductionRejectID, int? ReferenceID, int? ReferenceTypeID, int? RejectTypeID, decimal? Quantity,int? Hour, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.ProductionRejects_Upsert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionRejectID", DbType.Int32, ProductionRejectID);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceTypeID", DbType.Int32, ReferenceTypeID);
                db.AddInParameter(dbCommand, "@iRejectTypeID", DbType.Int32, RejectTypeID);
                db.AddInParameter(dbCommand, "@iQuantity", DbType.Decimal, Quantity);
                db.AddInParameter(dbCommand, "@iHour", DbType.Int32, Hour);
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
        public GenericReturn Delete(int? ProductionRejectID, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("mfg.ProductionRejects_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionRejectID", DbType.Int32, ProductionRejectID);
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
