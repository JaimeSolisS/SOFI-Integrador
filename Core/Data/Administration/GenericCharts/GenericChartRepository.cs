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
    public class GenericChartRepository : GenericRepository
    {
        public DataTable List(int? GenericChartID, string ChartName, string ChartTitle, int? ChartAreaID, int? ChartTypeID, bool? Enabled, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[GenericCharts_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iChartName", DbType.String, ChartName);
                db.AddInParameter(dbCommand, "@iChartTitle", DbType.String, ChartTitle);
                db.AddInParameter(dbCommand, "@iChartAreaID", DbType.Int32, ChartAreaID);
                db.AddInParameter(dbCommand, "@iChartTypeID", DbType.Int32, ChartTypeID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));

                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }

        }

        public GenericReturn Delete(int GenericChartID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[GenericCharts_Delete]");
            try
            {

                // Parameters
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

        public DataTable GetAxes_List(int? GenericChartAxisID, int? GenericChartID, string AxisName, int? AxisTypeID, int? AxisChartTypeID, int? AxisDatatypeID, string AxisColor, string AxisFormat, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[GenericChartsAxes_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartAxisID", DbType.Int32, GenericChartAxisID);
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iAxisName", DbType.String, AxisName);
                db.AddInParameter(dbCommand, "@iAxisTypeID", DbType.Int32, AxisTypeID);
                db.AddInParameter(dbCommand, "@iAxisChartTypeID", DbType.Int32, AxisChartTypeID);
                db.AddInParameter(dbCommand, "@iAxisDatatypeID", DbType.Int32, AxisDatatypeID);
                db.AddInParameter(dbCommand, "@iAxisColor", DbType.String, AxisColor);
                db.AddInParameter(dbCommand, "@iAxisFormat", DbType.String, AxisFormat);
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

        public GenericReturn Insert(int? GenericChartHeaderDataID, int? GenericChartID, DataTable GenericCharts, DataTable GenericChartsFilters, DataTable GenericChartsAxes, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[DBO].[GenericCharts_Insert]");
            try
            {
                // Parameters
                SqlParameter p = new SqlParameter("@iGenericCharts", GenericCharts)
                {
                    SqlDbType = SqlDbType.Structured
                };
                SqlParameter p2 = new SqlParameter("@iGenericChartsFilters", GenericChartsFilters)
                {
                    SqlDbType = SqlDbType.Structured
                };
                SqlParameter p3 = new SqlParameter("@iGenericChartsAxes", GenericChartsAxes)
                {
                    SqlDbType = SqlDbType.Structured
                };

                dbCommand.Parameters.Add(p);
                dbCommand.Parameters.Add(p2);
                dbCommand.Parameters.Add(p3);
                db.AddInParameter(dbCommand, "@iGenericChartHeaderDataID", DbType.Int32, GenericChartHeaderDataID);
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
    }
}
