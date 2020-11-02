using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MFG_CatalogsRepository : GenericRepository
    {
        public DataTable List(GenericRequest request)
        {
            dbCommand = db.GetStoredProcCommand("[MFG].[Catalogs_List]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

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

        public DataTable GetTableOfCatalog(string ReferenceID, int? CatalogID, string Param1, string Param2, string Param3, GenericRequest request)
        {
            dbCommand = db.GetStoredProcCommand("[MFG].[GetTableOfCatalog]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.String, ReferenceID);
                db.AddInParameter(dbCommand, "@iParam1", DbType.String, Param1);
                db.AddInParameter(dbCommand, "@iParam2", DbType.String, Param2);
                db.AddInParameter(dbCommand, "@iParam3", DbType.String, Param3);
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, CatalogID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

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
