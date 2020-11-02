#region Namespaces

using Core.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;


#endregion
namespace Core.Data
{
    public class TranslationDetailRepository : GenericRepository
    {
        #region MISC Methods

        public GenericReturn Add(TranslationDetail _TranslationDetail, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.TranslationsDetail_Insert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, _TranslationDetail.CatalogDetailID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, _TranslationDetail.CultureID);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, _TranslationDetail.Description);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCurrentCultureID", DbType.String, request.CultureID);
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

        public DataTable List(int CatalogDetailID, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("TranslationsDetail_List");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
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
        public DataTable GetTranslation4Tag(string CatalogTag, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[TranslationsDetail_Get4Tag]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, CatalogTag);
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

        public GenericReturn QuickUpdate(int CatalogDetailID, string CultureID, string ColumnName, string Value, int FacilityID, int UserID, string CurrentCultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.TranslationsDetail_QuickUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCurrentCultureID", DbType.String, CurrentCultureID);
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

        #endregion
    }
}
