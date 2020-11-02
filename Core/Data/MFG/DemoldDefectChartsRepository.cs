using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class DemoldDefectChartsRepository : GenericRepository
    {
        public DataSet List(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs, DateTime? StartDate, DateTime? EndDate, string DefectType, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DemoldDefect_ChartsData");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionLineIDs", DbType.String, ProductionLineIDs);
                db.AddInParameter(dbCommand, "@iMoldFamilyIDs", DbType.String, MoldFamilyIDs);
                db.AddInParameter(dbCommand, "@iShifts", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, "DemoldDefectsTypes");
                db.AddInParameter(dbCommand, "@iDefectType", DbType.String, DefectType);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataTable BarChartData(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs,
            DateTime? StartDate, DateTime? EndDate, string DefectType,int? DesignID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].DemoldDefect_BarChartsData");
            dbCommand.CommandTimeout = 60;
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionLineIDs", DbType.String, ProductionLineIDs);
                db.AddInParameter(dbCommand, "@iMoldFamilyIDs", DbType.String, MoldFamilyIDs);
                db.AddInParameter(dbCommand, "@iShifts", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, "DemoldDefectsTypes");
                db.AddInParameter(dbCommand, "@iDefectType", DbType.String, DefectType);
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
        public DataTable PieChartData(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs,
            DateTime? StartDate, DateTime? EndDate, string DefectType, int? DesignID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].DemoldDefect_PieChartsData");
            dbCommand.CommandTimeout = 60;
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionLineIDs", DbType.String, ProductionLineIDs);
                db.AddInParameter(dbCommand, "@iMoldFamilyIDs", DbType.String, MoldFamilyIDs);
                db.AddInParameter(dbCommand, "@iShifts", DbType.String, ShiftIDs);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, "DemoldDefectsTypes");
                db.AddInParameter(dbCommand, "@iDefectType", DbType.String, DefectType);
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
