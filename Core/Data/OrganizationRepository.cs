using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class OrganizationRepository :GenericRepository
    {
        public DataTable List(bool? Enabled, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("Organizations_List");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }
        public DataTable List4Config(int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("Organizations_List4Config");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }
    }
}
