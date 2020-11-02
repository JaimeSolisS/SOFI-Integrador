using System;
using Core.Entities;
using System.Data;

namespace Core.Data
{
    public class CatalogParameterRepository : GenericRepository
    {
        #region MISC Methods

        public GenericReturn Upsert(CatalogParameter _CatalogParameter, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.CatalogsParameters_Upsert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, _CatalogParameter.CatalogID);
                db.AddInParameter(dbCommand, "@iParamID", DbType.Int16, _CatalogParameter.ParamID);
                db.AddInParameter(dbCommand, "@iParamName", DbType.String, _CatalogParameter.ParamName);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, _CatalogParameter.Description);
                db.AddInParameter(dbCommand, "@iConfigured", DbType.Boolean, _CatalogParameter.Configured);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameter
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
            dbCommand = db.GetStoredProcCommand("CatalogsParameters_List");
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

        public GenericReturn UpsertFromdb(DataTable _CatalogParameter, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.CatalogsParameters_Upsert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@it_CatalogParameter", DbType.Object, _CatalogParameter);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameter
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
