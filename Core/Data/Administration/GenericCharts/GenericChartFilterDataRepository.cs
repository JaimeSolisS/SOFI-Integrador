using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class GenericChartFilterDataRepository : GenericRepository
    {
        public DataTable List(int? GenericChartFilterDataID, int? GenericChartHeaderDataID, int? GenericChartFilterID, int? GenericChartID, string FilterValue, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[GenericChartFilterData_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartFilterDataID", DbType.Int32, GenericChartFilterDataID);
                db.AddInParameter(dbCommand, "@iGenericChartHeaderDataID", DbType.Int32, GenericChartHeaderDataID);
                db.AddInParameter(dbCommand, "@iGenericChartFilterID", DbType.Int32, GenericChartFilterID);
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iFilterValue", DbType.String, FilterValue);
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
