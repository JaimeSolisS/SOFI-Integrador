using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class vw_CatalogRepository: GenericRepository
    {
        public DataTable List(string CatalogTag, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("vw_Catalogs_List");
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
        public DataTable List4Formats(int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[vw_Catalogs_List4Formats]");
            try
            {
                // Parameters    
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

        public DataTable GetInfo(int CatalogID, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("Catalogs_Get");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, CatalogID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
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

        public DataTable Get(int CatalogDetailID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("vw_Catalogs_Get");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

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

        public DataTable ListByParams(Catalog _Catalog, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("vw_Catalogs_ListByParams");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, _Catalog.CatalogTag);
                db.AddInParameter(dbCommand, "@iParam1", DbType.String, _Catalog.Param1);
                db.AddInParameter(dbCommand, "@iParam2", DbType.String, _Catalog.Param2);
                db.AddInParameter(dbCommand, "@iParam3", DbType.String, _Catalog.Param3);
                db.AddInParameter(dbCommand, "@iParam4", DbType.String, _Catalog.Param4);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

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


        #region Methods
        public DataTable List4Config(GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("Catalogs_List4Config");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

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

        public GenericReturn Add(Catalog _Catalog, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.Catalogs_Insert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, _Catalog.CatalogTag);
                db.AddInParameter(dbCommand, "@iCatalogName", DbType.String, _Catalog.CatalogName);
                db.AddInParameter(dbCommand, "@iCatalogDescription", DbType.String, _Catalog.CatalogDescription);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, _Catalog.OrganizationID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, _Catalog.FacilityID);
                db.AddInParameter(dbCommand, "@iIsSystemValue", DbType.Boolean, _Catalog.IsSystemValue);
                db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oCatalogID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                _Catalog.CatalogID = result.ID = (int)db.GetParameterValue(dbCommand, "@oCatalogID");
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

        public GenericReturn Update(Catalog _Catalog, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Catalogs_Update");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, _Catalog.CatalogID);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, _Catalog.CatalogTag);
                db.AddInParameter(dbCommand, "@iCatalogName", DbType.String, _Catalog.CatalogName);
                db.AddInParameter(dbCommand, "@iCatalogDescription", DbType.String, _Catalog.CatalogDescription);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, _Catalog.OrganizationID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, _Catalog.FacilityID);
                db.AddInParameter(dbCommand, "@iIsSystemValue", DbType.Boolean, _Catalog.IsSystemValue);
                db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
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
