using Core.Entities;
using System;
using System.Data;

namespace Core.Data
{
    public class LensTypesRepository : GenericRepository
    {
        public DataTable List(int? LensTypeID, string LensTypeName, int? ProductionDesignID, bool? Enabled, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[LensTypes_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iLensTypeID", DbType.Int32, LensTypeID);
                db.AddInParameter(dbCommand, "@iLensTypeName", DbType.String, LensTypeName);
                db.AddInParameter(dbCommand, "@iProductionDesignID", DbType.Int32, ProductionDesignID);
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
    }
}
