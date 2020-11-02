using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class KioskSuggestionsAdministratorRepository : GenericRepository
    {
        public DataTable List(int? KioskEmployeeSuggestionID, string EmployeeID, int? CategoryID, string FacilityIDs, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskEmployeeSuggestions_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskEmployeeSuggestionID", DbType.Int32, KioskEmployeeSuggestionID);
                db.AddInParameter(dbCommand, "@iEmployeeID", DbType.String, EmployeeID);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, CategoryID);
                db.AddInParameter(dbCommand, "@iFacilityIDs", DbType.String, FacilityIDs);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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

        public DataSet ListDataSet(int? CategoryID, string FacilityIDs, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            DataSet ds = new DataSet();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskEmployeeSuggestions_ExcelReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, CategoryID);
                db.AddInParameter(dbCommand, "@iFacilityIDs", DbType.String, FacilityIDs);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                ds = db.ExecuteDataSet(dbCommand);

            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public GenericReturn Delete(int? KioskEmployeeSuggestionID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskEmployeeSuggestions_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskEmployeeSuggestionID", DbType.Int32, KioskEmployeeSuggestionID);
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
