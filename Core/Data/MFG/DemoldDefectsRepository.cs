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
    class DemoldDefectsRepository : GenericRepository
    {
        public DataTable GetCategoriesAndData(string CatalogTag, string Category, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DemoldDefects_GetCategoriesAndData");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, CatalogTag);
                db.AddInParameter(dbCommand, "@iCategory", DbType.String, Category);
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

        public DataTable List(int? DemoldDefectID, int? ProductionLineID, int? ShiftID, string InspectorName, int? StatusID, int? VATID, DateTime? DefectDate, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefects_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDemoldDefectID", DbType.Int32, DemoldDefectID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iInspectorName", DbType.String, InspectorName);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iDefectDate", DbType.DateTime, DefectDate);
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

        public GenericReturn Insert(int? ProductionLineID, int? ShiftID, string InspectorName, string VATID, DataTable DemoldDefectDetails, DateTime? DefectDate, int ProductID, int LensGross, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefects_Insert]");
            try
            {
                // Parameters
                SqlParameter p = new SqlParameter("@t_DemoldDefectDetails", DemoldDefectDetails)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iInspectorName", DbType.String, InspectorName);
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iDefectDate", DbType.DateTime, DefectDate);
                db.AddInParameter(dbCommand, "@iProductID", DbType.Int32, ProductID);
                db.AddInParameter(dbCommand, "@iLensGross", DbType.Int32, LensGross);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oDemoldDefectID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oDemoldDefectID");
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

        public DataTable ValidateProductLens(string LenType, string Base, string Addition, string Side)
        {

            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT ID, SKUCode, LenteProducto FROM [MFG].[fn_Validate_ProductLens](@iLenType, @iBase, @iAdd, @iSide)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iLenType", DbType.String, LenType);
                db.AddInParameter(dbCommand, "@iBase", DbType.String, Base);
                db.AddInParameter(dbCommand, "@iAdd", DbType.String, Addition);
                db.AddInParameter(dbCommand, "@iSide", DbType.String, Side);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
                //ParamValue = db.ExecuteScalar(dbCommand).ToString();

            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public GenericReturn ValidateToNextFase(int? CaseControl, string LenType, string Base, string Addition, string Side, string VAT, int ProductionProcessID, string Line, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DemoldDefects_ValidateLensTypes");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCaseControl", DbType.Int32, CaseControl);
                db.AddInParameter(dbCommand, "@iLenType", DbType.String, LenType);
                db.AddInParameter(dbCommand, "@iBase", DbType.String, Base);
                db.AddInParameter(dbCommand, "@iAddition", DbType.String, Addition);
                db.AddInParameter(dbCommand, "@iSide", DbType.String, Side);
                db.AddInParameter(dbCommand, "@iVat", DbType.String, VAT);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLine", DbType.String, Line);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));

                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");

            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable FnGetDailyProductLensProduction(string VAT, string ProdIndex, string Line, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetSqlStringCommand("SELECT * FROM [MFG].[fn_DemoldDefects_GetDailyProductLensProduction](@iVat, @iProductionProcess, @iLine)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVat", DbType.String, VAT);
                db.AddInParameter(dbCommand, "@iProductionProcess", DbType.String, ProdIndex);
                db.AddInParameter(dbCommand, "@iLine", DbType.String, Line);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable GetMoldFamiliesByFacility(string VAT, int ProductionProcessID, string Line, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetSqlStringCommand("SELECT CatalogDetailID, ValueID FROM [MFG].[fn_GetMoldFamiliesByFacility](@iVat, @iProductionProcessID, @iLine, @iCultureID, @iFacilityID)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVat", DbType.String, VAT);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLine", DbType.String, Line);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

    }
}
