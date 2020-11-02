using System;
using System.Data;

namespace Core.Data
{
    public class CI_DashboardVisitRepository : GenericRepository
    {
        public int Add(int UserID)
        {
            int TotalVisits = 0;

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardVisitsCountAdd");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddOutParameter(dbCommand, "@oTotalVisits", DbType.Int32, 0);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                TotalVisits = (int)db.GetParameterValue(dbCommand, "@oTotalVisits");
            }
            catch
            {
                TotalVisits = 0;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return TotalVisits;
        }

        public int Counter(DateTime? Date)
        {
            int TotalVisits = 0;

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardVisitsCounter");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDate", DbType.Date, Date);
                db.AddOutParameter(dbCommand, "@oTotalVisits", DbType.Int32, 0);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                TotalVisits = (int)db.GetParameterValue(dbCommand, "@oTotalVisits");
            }
            catch
            {
                TotalVisits = 0;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return TotalVisits;
        }
    }
}
