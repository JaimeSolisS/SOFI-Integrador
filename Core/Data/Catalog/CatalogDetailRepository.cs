using System;
using System.Data;
using Core.Entities;

namespace Core.Data
{
    public class CatalogDetailRepository : GenericRepository
    {
        #region MISC Methods



        public GenericReturn Add(CatalogDetail _CatalogDetail, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.CatalogsDetail_Insert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, _CatalogDetail.CatalogID);
                db.AddInParameter(dbCommand, "@iValueID", DbType.String, _CatalogDetail.ValueID);
                db.AddInParameter(dbCommand, "@iParam1", DbType.String, _CatalogDetail.Param1);
                db.AddInParameter(dbCommand, "@iParam2", DbType.String, _CatalogDetail.Param2);
                db.AddInParameter(dbCommand, "@iParam3", DbType.String, _CatalogDetail.Param3);
                db.AddInParameter(dbCommand, "@iParam4", DbType.String, _CatalogDetail.Param4);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oCatalogDetailID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                _CatalogDetail.CatalogDetailID = result.ID = (int)db.GetParameterValue(dbCommand, "@oCatalogDetailID");
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn QuickUpdate(int CatalogDetailID, string ColumnName, string Value, int FacilityID, int UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.CatalogsDetail_QuickUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public DataTable List(int CatalogID, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CatalogsDetail_List");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, CatalogID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }

        public DataTable List4FormatPhase(int CatalogID, string FormatID, Guid TransactionID, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CatalogsDetail_List4FormatPhase]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, CatalogID);
                db.AddInParameter(dbCommand, "@iFormtID", DbType.String, FormatID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }
        public DataTable List4FormatPhaseEdit(int? FormatLoopRuleTempID, int CatalogID, string FormatID, Guid TransactionID, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CatalogsDetail_List4FormatPhaseEdit]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, CatalogID);
                db.AddInParameter(dbCommand, "@iFormtID", DbType.String, FormatID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }

        public GenericReturn Delete(int CatalogDetailID, int FacilityID, int UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Delete Procedure
            dbCommand = db.GetStoredProcCommand("dbo.CatalogsDetail_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public DataTable Get(int? CatalogDetailID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("CatalogsDetail_Get");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
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

        public GenericReturn Update(int? CatalogDetailID, int? CatalogID, string ValueID, string Param1, string Param2, string Param3, string Param4, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[DBO].[CatalogsDetail_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, CatalogID);
                db.AddInParameter(dbCommand, "@iValueID", DbType.String, ValueID);
                db.AddInParameter(dbCommand, "@iParam1", DbType.String, Param1);
                db.AddInParameter(dbCommand, "@iParam2", DbType.String, Param2);
                db.AddInParameter(dbCommand, "@iParam3", DbType.String, Param3);
                db.AddInParameter(dbCommand, "@iParam4", DbType.String, Param4);
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

        #endregion
    }
}
