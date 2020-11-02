using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class ShiftRepository: GenericRepository
    {
        public DataTable List(int? ShiftID, bool? Enabled, GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("ShiftsMaster_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public DataTable GetShiftsByAvailableFacilities(GenericRequest req)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("ShiftsMaster_ShiftsByAvailableFacilities");
            try
            {
                // Parameters
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

    }
}
