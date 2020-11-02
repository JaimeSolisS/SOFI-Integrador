using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class UsersProfiles_Repository : GenericRepository
    {
        public DataTable List(int? ProfileID, int? ChangedBy, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("UsersProfiles_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, ChangedBy);
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
