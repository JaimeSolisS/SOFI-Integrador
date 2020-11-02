using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MiscellaneousRepository : GenericRepository
    {
        public string Param_GetValue(int CatalogDetailID, int ParamIndex, string DefaultValue)
        {

            string ParamValue = DefaultValue;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.CatalogDetail_GetParamValue(@iCatalogDetailID, @iParamIndex, @iDefaultValue)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iParamIndex", DbType.String, ParamIndex);
                db.AddInParameter(dbCommand, "@iDefaultValue", DbType.String, DefaultValue);

                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToString();

            }
            catch
            {
                ParamValue = DefaultValue;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return ParamValue;
        }

        public GenericReturn Params_SetValue(string ParamName, string ParamValue, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Params_SetValue]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEntityFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iParamName", DbType.String, ParamName);
                db.AddInParameter(dbCommand, "@iParamValue", DbType.String, ParamValue);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                //result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                //result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
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

        public bool ProductionProcess_ApplyLines(int CatalogDetailID, bool DefaultValue)
        {

            bool ParamValue = DefaultValue;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.ProductionProcess_ApplyLine(@iCatalogDetailID, @iDefaultValue)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iDefaultValue", DbType.Boolean, DefaultValue);

                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToBoolean();
            }
            catch
            {
                ParamValue = DefaultValue;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return ParamValue;
        }

        public string Param_GetValue(int FacilityID, string ParamName, string DefaultValue)
        {
            string ParamValue = DefaultValue;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.GetParamValue(@iFacilityID,@iParamName,@iDefaultValue)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iParamName", DbType.String, ParamName);
                db.AddInParameter(dbCommand, "@iDefaultValue", DbType.String, DefaultValue);

                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToString();

            }
            catch
            {
                ParamValue = DefaultValue;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return ParamValue;
        }

        public int Catalog_GetDetailID(int FacilityID, string CatalogTag, string ValueID)
        {
            int CatalogDetailID = 0;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.Catalog_GetDetailID(@iFacilityID,@iCatalogTag,@iValueID)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, CatalogTag);
                db.AddInParameter(dbCommand, "@iValueID", DbType.String, ValueID);

                // Execute Query
                CatalogDetailID = Convert.ToInt32(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return CatalogDetailID;
        }

        public int Catalog_GetID(int FacilityID, string CatalogTag, string CultureID)
        {
            int CatalogID = 0;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.Catalogs_GetCatalogID(@iCatalogTag,@iFacilityID,@iCultureID)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, CatalogTag);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                CatalogID = Convert.ToInt32(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return CatalogID;
        }


        public bool Event_ValidUser(int EventID, int UserID)
        {
            bool ParamValue;
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.ValidateUserEvent(@iEventID,@iUserID)");
            try
            {   // Parameters
                db.AddInParameter(dbCommand, "@iEventID", DbType.Int32, EventID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                // Execute Query
                ParamValue = Boolean.Parse(db.ExecuteScalar(dbCommand).ToString());
            }
            catch
            { ParamValue = false; }
            finally
            { dbCommand.Dispose(); }

            return ParamValue;
        }

        public bool Event_ValidUser_HasAccess(int EventID, int UserID)
        {
            bool ParamValue;
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.ValidateUserEvent_HasAccess(@iEventID,@iUserID)");
            try
            {   // Parameters
                db.AddInParameter(dbCommand, "@iEventID", DbType.Int32, EventID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                // Execute Query
                ParamValue = Boolean.Parse(db.ExecuteScalar(dbCommand).ToString());
            }
            catch
            { ParamValue = false; }
            finally
            { dbCommand.Dispose(); }

            return ParamValue;
        }

        public DateTime Facility_GetDate(int FacilityID)
        {
            DateTime DateValue;
            dbCommand = db.GetSqlStringCommand("SELECT [dbo].[Facilities_GetDate](@iFacilityID)");

            try
            {
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                DateValue = Convert.ToDateTime(db.ExecuteScalar(dbCommand).ToString());
            }catch
            {
                DateValue = DateTime.Now;
            }

            return DateValue;
        }

        public DataTable EntityFieldsConfiguration(int? FacilityID, int? EntityID, int? EntityIndicator, string SystemModuleTag, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("EntityFieldsConfiguration_GetSystemModuleFields");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iEntityID", DbType.Int32, EntityID);
                db.AddInParameter(dbCommand, "@iEntityIndicator", DbType.Int32, EntityIndicator);
                db.AddInParameter(dbCommand, "@iSystemModuleTag", DbType.String, SystemModuleTag);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            {
                dbCommand.Dispose();
            }
            return dt;
        }
        public DataTable eReq_GetReportData(string SQL_StoredProcedure, int? DocumentID, int? FileID, int? FacilityID, int? UserID, string CultureID, string Parameters, byte[] Logo = null)
        {
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetStoredProcCommand(SQL_StoredProcedure);

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDocumentID", DbType.Int32, DocumentID);
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddInParameter(dbCommand, "@iParameters", DbType.String, Parameters);
                db.AddInParameter(dbCommand, "@iLogo", DbType.Binary, Logo); //varbinary(max)
                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }

            }
            finally
            { dbCommand.Dispose(); }
        }

        public DataTable eReq_GetReportDataDetail(string SQL_StoredProcedure, int? DocumentID, int? FacilityID, int? UserID, string CultureID, string Parameters, byte[] Logo = null)
        {
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetStoredProcCommand(SQL_StoredProcedure);

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDocumentID", DbType.Int32, DocumentID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddInParameter(dbCommand, "@iParameters", DbType.String, Parameters);
                db.AddInParameter(dbCommand, "@iLogo", DbType.Binary, Logo); //varbinary(max)
                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }

            }
            finally
            { dbCommand.Dispose(); }
        }
    }
}
