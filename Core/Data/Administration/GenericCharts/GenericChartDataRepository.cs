using Core.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.Data
{
    public class GenericChartDataRepository : GenericRepository
    {
        public DataTable Get(int? GenericChartHeaderDataID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("GenericChartDataFields_Get");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartHeaderDataID", DbType.Int32, GenericChartHeaderDataID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public GenericReturn Insert(string FitlerInfo, int GenericChartID, DataTable GenericChartData, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[DBO].[GenericChartData_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFilterInfo", DbType.String, FitlerInfo);
                SqlParameter p = new SqlParameter("@iGenericChartData", GenericChartData)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable List(int? GenericChartID, int? GenericChartDataID, int? GenericChartHeaderDataID, string ValuesFromFilters, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[GenericChartData_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iGenericChartDataID", DbType.Int32, GenericChartDataID);
                db.AddInParameter(dbCommand, "@iGenericChartHeaderDataID", DbType.Int32, GenericChartHeaderDataID);
                db.AddInParameter(dbCommand, "@iDataFilterString", DbType.String, ValuesFromFilters);
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

        public DataTable ListFromDynamicFilters(string FilterData, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[GenericDynamicChartData_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFilterData", DbType.String, FilterData);
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

        public DataTable GenericChartHeaderData_List(int? GenericChartID, int? GenericChartFilterID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[GenericChartHeaderData_Get]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iGenericChartFilterID", DbType.Int32, GenericChartFilterID);
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
