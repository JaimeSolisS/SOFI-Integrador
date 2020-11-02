using Core.Entities;
using System;
using System.Data;

namespace Core.Data
{
    class DemoldDefectDetailsRepository : GenericRepository
    {
        public DataTable List(int? ShiftID, int? ProductionLineID, int? VATID, string InspectorName,
            DateTime? StartDate, DateTime? EndDate, int? MoldFamily, int? BaseID, int? AdditionID, int? SideID,
            string DefectCat, int? DefectType,int? DesignID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.DemoldDefectDetails_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iInspectorName", DbType.String, InspectorName);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iMoldFamilyID", DbType.Int32, MoldFamily);
                db.AddInParameter(dbCommand, "@iBaseID", DbType.Int32, BaseID);
                db.AddInParameter(dbCommand, "@iAdditionID", DbType.Int32, AdditionID);
                db.AddInParameter(dbCommand, "@iSideID", DbType.Int32, SideID);
                db.AddInParameter(dbCommand, "@iDefectCat", DbType.String, DefectCat);
                db.AddInParameter(dbCommand, "@iDefectType", DbType.Int32, DefectType);
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

        public DataTable ListByProduct(int? ProductionLineID, int? VATID, string InspectorName, int ProductID, DateTime? DefectDate, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[ProductDemoldDefectDetails_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iInspectorName", DbType.String, InspectorName);
                db.AddInParameter(dbCommand, "@iProductID", DbType.Int32, ProductID);
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


        public GenericReturn Delete(int? DemoldDefectDetailID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefectDetails_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDemoldDefectDetailID", DbType.Int32, DemoldDefectDetailID);
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

        public GenericReturn Update(int? DemoldDefectDetailID, int? DemoldDefectID, int? ProductID, int? MoldFamilyID, int? BaseID, int? AdditionID, int? SideEyeID, int? Quantity, int? DemoldDefectTypeID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefectDetails_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDemoldDefectDetailID", DbType.Int32, DemoldDefectDetailID);
                db.AddInParameter(dbCommand, "@iDemoldDefectID", DbType.Int32, DemoldDefectID);
                db.AddInParameter(dbCommand, "@iProductID", DbType.Int32, ProductID);
                db.AddInParameter(dbCommand, "@iMoldFamilyID", DbType.Int32, MoldFamilyID);
                db.AddInParameter(dbCommand, "@iBaseID", DbType.Int32, BaseID);
                db.AddInParameter(dbCommand, "@iAdditionID", DbType.Int32, AdditionID);
                db.AddInParameter(dbCommand, "@iSideEyeID", DbType.Int32, SideEyeID);
                db.AddInParameter(dbCommand, "@iQuantity", DbType.Int32, Quantity);
                db.AddInParameter(dbCommand, "@iDemoldDefectTypeID", DbType.Int32, DemoldDefectTypeID);
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

        public DataSet DemoldDefectsExportToExcel(int? ShiftID, int? ProductionLineID, int? VATID, string InspectorName,
            DateTime? StartDate, DateTime? EndDate, string DefectCat, int? DefectType,int? DesignID, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[DemoldDefectDetails_ExcelReport]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, ProductionLineID);
                db.AddInParameter(dbCommand, "@iVATID", DbType.Int32, VATID);
                db.AddInParameter(dbCommand, "@iInspectorName", DbType.String, InspectorName);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iDefectCat", DbType.String, DefectCat);
                db.AddInParameter(dbCommand, "@iDefectType", DbType.Int32, DefectType);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
    }
}
