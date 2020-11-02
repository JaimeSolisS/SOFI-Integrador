using Core.Entities;
using Core.Entities.QA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class QA_FilmetricFilmLimitsRepository : GenericRepository
    {
        public DataTable List(int? ProductFilmLimitID, int? ProductID, int? FilmID, decimal? MaxValue, decimal? MinValue, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[QA].[ProductFilmLimits_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductFilmLimitID", DbType.Int32, ProductFilmLimitID);
                db.AddInParameter(dbCommand, "@iProductID", DbType.Int32, ProductID);
                db.AddInParameter(dbCommand, "@iFilmID", DbType.Int32, FilmID);
                db.AddInParameter(dbCommand, "@iMaxValue", DbType.Decimal, MaxValue);
                db.AddInParameter(dbCommand, "@iMinValue", DbType.Decimal, MinValue);
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
