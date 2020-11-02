using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class GenericChartsFiltersRepository : GenericRepository
    {
        public DataTable List(int? GenericChartFilterID, int? GenericChartID, string FilterName, int? FilterTypeID, int? FilterListID, string DefaultValue, int? DefaultValueFormula, bool? Enabled, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("GenericChartsFilters_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartFilterID", DbType.Int32, GenericChartFilterID);
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iFilterName", DbType.String, FilterName);
                db.AddInParameter(dbCommand, "@iFilterTypeID", DbType.Int32, FilterTypeID);
                db.AddInParameter(dbCommand, "@iFilterListID", DbType.Int32, FilterListID);
                db.AddInParameter(dbCommand, "@iDefaultValue", DbType.String, DefaultValue);
                db.AddInParameter(dbCommand, "@iDefaultValueFormula", DbType.Int32, DefaultValueFormula);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Insert(int? GenericChartID, string FilterName, int? FilterTypeID, int? FilterListID, string DefaultValue, int? DefaultValueFormula, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[GenericChartsFilters_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iFilterName", DbType.String, FilterName);
                db.AddInParameter(dbCommand, "@iFilterTypeID", DbType.Int32, FilterTypeID);
                db.AddInParameter(dbCommand, "@iFilterListID", DbType.Int32, FilterListID);
                db.AddInParameter(dbCommand, "@iDefaultValue", DbType.String, DefaultValue);
                db.AddInParameter(dbCommand, "@iDefaultValueFormula", DbType.Int32, DefaultValueFormula);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Delete(int? GenericChartFilterID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[GenericChartsFilters_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartFilterID", DbType.Int32, GenericChartFilterID);
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

        public GenericReturn Update(int? GenericChartFilterID, int? GenericChartID, string FilterName, int? FilterTypeID, int? FilterListID, string DefaultValue, int? DefaultValueFormula, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("GenericChartsFilters_Update");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGenericChartFilterID", DbType.Int32, GenericChartFilterID);
                db.AddInParameter(dbCommand, "@iGenericChartID", DbType.Int32, GenericChartID);
                db.AddInParameter(dbCommand, "@iFilterName", DbType.String, FilterName);
                db.AddInParameter(dbCommand, "@iFilterTypeID", DbType.Int32, FilterTypeID);
                db.AddInParameter(dbCommand, "@iFilterListID", DbType.Int32, FilterListID);
                db.AddInParameter(dbCommand, "@iDefaultValue", DbType.String, DefaultValue);
                db.AddInParameter(dbCommand, "@iDefaultValueFormula", DbType.Int32, DefaultValueFormula);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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
