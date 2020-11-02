using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class GenericChartsAxisRepository : GenericRepository
    {

        public GenericReturn Insert(int? GenericChartID, string AxisName, int? AxisTypeID, int? AxisChartTypeID, int? AxisDatatypeID, string AxisColor, string AxisFormat, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[GenericChartsAxes_Insert]");
            try
            {
                // Parameters
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

        public GenericReturn Delete(int? GenericChartAxisID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[GenericChartsAxes_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartAxisID", DbType.Int32, GenericChartAxisID);
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

        public GenericReturn Update(int? GenericChartAxisID, int? GenericChartID, string AxisName, int? AxisTypeID, int? AxisChartTypeID, int? AxisDatatypeID, string AxisColor, string AxisFormat, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[GenericChartsAxes_Update]");
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
