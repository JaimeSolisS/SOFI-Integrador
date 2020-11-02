using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class CI_DashboardCarouselVideosRepository : GenericRepository
    {
        public DataTable List(int? DashboardCarouselVideoID,  GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideos_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardCarouselVideoID", DbType.Int32, DashboardCarouselVideoID);
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
            { dbCommand.Dispose(); }

        }

        public DataTable GetBySeq(int? CurrentSeq, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideos_GetBySeq");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCurrentSeq", DbType.Int32, CurrentSeq);
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
            { dbCommand.Dispose(); }

        }
    }
}
