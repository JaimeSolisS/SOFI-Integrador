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
    public class FormatsRepository : GenericRepository
    {
        public GenericReturn EntityFieldsConfiguration_Insert (int EntityID, int EntityIndicator, int SystemModule, int IsVisible, int IsMandatory, string ValueID,
            string Param1, string Param2, string Param3, string CatalogTagID, int DataTypeID, int FieldLength, int FieldPrecission, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[TableAdditionalFields_Insert4Request]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEntityID", DbType.Int32, EntityID);
                db.AddInParameter(dbCommand, "@iEntityIndicator", DbType.Int32, EntityIndicator);
                db.AddInParameter(dbCommand, "@iSystemModule", DbType.Int32, SystemModule);
                db.AddInParameter(dbCommand, "@iIsVisible", DbType.Int32, IsVisible);
                db.AddInParameter(dbCommand, "@iIsMandatory", DbType.Int32, IsMandatory);
                db.AddInParameter(dbCommand, "@iValueID", DbType.String, ValueID);
                db.AddInParameter(dbCommand, "@iParam1", DbType.String, Param1);
                db.AddInParameter(dbCommand, "@iParam2", DbType.String, Param2);
                db.AddInParameter(dbCommand, "@iParam3", DbType.String, Param3);
                db.AddInParameter(dbCommand, "@iCatalogTagID", DbType.String, CatalogTagID);
                db.AddInParameter(dbCommand, "@iDataTypeID", DbType.Int32, DataTypeID);
                db.AddInParameter(dbCommand, "@iFieldLength", DbType.Int32, FieldLength);
                db.AddInParameter(dbCommand, "@iFieldPrecission", DbType.Int32, FieldPrecission);
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

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }        
        
        public GenericReturn Upsert (int? FormatID, string FormatName, int Use2FA, int DirectApproval, int HasDetail, int HasAttachment, Guid TransactionID , string FormatPath, int PositionYInitial, int MaxDetail, int Interline, DataTable dtFormatFields, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFormatName", DbType.String, FormatName);
                db.AddInParameter(dbCommand, "@iUse2FA", DbType.Int32, Use2FA);
                db.AddInParameter(dbCommand, "@iDirectApproval", DbType.Int32, DirectApproval);
                db.AddInParameter(dbCommand, "@iHasDetail", DbType.Int32, HasDetail);
                db.AddInParameter(dbCommand, "@iHasAttachment", DbType.Int32, HasAttachment);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);   
                db.AddInParameter(dbCommand, "@iFormatPath", DbType.String, FormatPath);   
                db.AddInParameter(dbCommand, "@iPositionYInitial", DbType.Int32, PositionYInitial);   
                db.AddInParameter(dbCommand, "@iMaxDetail", DbType.Int32, MaxDetail);   
                db.AddInParameter(dbCommand, "@iInterline", DbType.Int32, Interline);                   
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                //Parametro tipo tabla
                SqlParameter p = new SqlParameter("@it_FormatFields", dtFormatFields)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public GenericReturn EntityFieldUpsert(EntityField _EntityField, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.EntityFieldsConfiguration_Upsert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEntityId", DbType.Int32, _EntityField.EntityID);
                db.AddInParameter(dbCommand, "@iEntityIndicator", DbType.Int32, _EntityField.EntityIndicator);
                db.AddInParameter(dbCommand, "@iSystemModule", DbType.Int32, _EntityField.SystemModule);
                db.AddInParameter(dbCommand, "@iFieldId", DbType.Int32, _EntityField.FieldID);
                db.AddInParameter(dbCommand, "@iTranslation", DbType.String, _EntityField.CustomTranslation);
                db.AddInParameter(dbCommand, "@iIsVisible", DbType.Boolean, _EntityField.IsVisible);
                db.AddInParameter(dbCommand, "@iIsMandatory", DbType.Boolean, _EntityField.IsMandatory);
                db.AddInParameter(dbCommand, "@iFacilityId", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oFieldConfigurationID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                _EntityField.ID = result.ID = (int)db.GetParameterValue(dbCommand, "@oFieldConfigurationID");
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
        public GenericReturn FormatGenericDetailUpsert (int? FormatGenericDetailTempID, int FormatID, string NameES, string NameEN, string Description, int DataTypeID, int FieldLength, int FieldPrecision,
            string CatalogTag, int IsMandatory, int Enabled, Guid TransactionID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatGenericDetailTemp_Upsert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatGenericDetailTempID", DbType.Int32, FormatGenericDetailTempID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iNameTag", DbType.String, NameEN.Replace(" ",""));
                db.AddInParameter(dbCommand, "@iNameES", DbType.String, NameES);
                db.AddInParameter(dbCommand, "@iNameEN", DbType.String, NameEN);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, Description);
                db.AddInParameter(dbCommand, "@iDataTypeID", DbType.Int32, DataTypeID);
                db.AddInParameter(dbCommand, "@iFieldLength", DbType.Int32, FieldLength);
                db.AddInParameter(dbCommand, "@iFieldPrecision", DbType.Int32, FieldPrecision);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, CatalogTag);
                db.AddInParameter(dbCommand, "@iIsMandatory", DbType.Int32, IsMandatory);                
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Int32, Enabled);                
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

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public DataTable FormatGenericDetailList(int FormatID, Guid TransactionID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatGenericDetail_List4Format]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);     
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
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
        public DataTable FormatsList(string FormatName, string Approver, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatName", DbType.String, FormatName);     
                db.AddInParameter(dbCommand, "@iApprover", DbType.String, Approver);
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

        public DataTable GetTranslationList(int CatalogDetailTempID, int CatalogDetailID, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[TranslationsDetailTemp_List]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iCatalogDetailTempID", DbType.Int32, CatalogDetailTempID);
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

        public GenericReturn FormatGenericDetailTempInsert(int FormatID, Guid TransactionID,GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_InitTEMP]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
         public GenericReturn FormatAccessTempInsert(int FormatID, Guid TransactionID, string FacilityID, string UserListID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatAccessTemp_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.String, FacilityID);
                db.AddInParameter(dbCommand, "@iUserListID", DbType.String, UserListID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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

        public DataTable FormatAccessTemp_List4Transaction(int FormatID, Guid TransactionID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("eReq.FormatAccessTemp_List4Transaction");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCurrentFacilityID", DbType.Int32, request.FacilityID);
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
        public DataTable GetFormatAccessTemp_List(Guid TransactionID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatAccessTemp_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
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
        
        public DataTable FormatsLoopsRulesTemp_List (Guid TransactionID, int FacilitySelectID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesTemp_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFacilitySelectID", DbType.Int32, FacilitySelectID);
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
        public DataTable PDFFilesDetail_TEMP_List4Detail(Guid TransactionID, int FileID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[PDFFilesDetail_TEMP_List4Detail]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
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
        public DataTable PDFFilesDetail_TEMP_List4Signatures(Guid TransactionID, int FileID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[PDFFilesDetail_TEMP_List4Signatures]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
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
         public DataTable PDFFilesDetail_TEMP_List4Header(Guid TransactionID, int FileID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[PDFFilesDetail_TEMP_List4Header]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
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
        
        public DataTable UsersFacilitiesPermission(string FacilitiesList, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("dbo.UsersFacilitiesPermission");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilitiesList", DbType.String, FacilitiesList);
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

        public DataTable FormatsLoopsRules_TEMP_List(int FormatLoopRuleTempID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRules_TEMP_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
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

        public GenericReturn AddNewFlowPhase(CatalogDetail _CatalogDetail, Guid Transaction, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CatalogsDetailTemp_Insert]");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogID", DbType.Int32, _CatalogDetail.CatalogID);
                db.AddInParameter(dbCommand, "@iTransaction", DbType.Guid, Transaction);
                db.AddInParameter(dbCommand, "@iValueID", DbType.String, _CatalogDetail.ValueID);
                db.AddInParameter(dbCommand, "@iParam1", DbType.String, _CatalogDetail.Param1);
                db.AddInParameter(dbCommand, "@iParam2", DbType.String, _CatalogDetail.Param2);
                db.AddInParameter(dbCommand, "@iParam3", DbType.String, _CatalogDetail.Param3);
                db.AddInParameter(dbCommand, "@iParam4", DbType.String, _CatalogDetail.Param4);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oCatalogDetailTempID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                _CatalogDetail.CatalogDetailID = result.ID = (int)db.GetParameterValue(dbCommand, "@oCatalogDetailTempID");
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
        public GenericReturn TranslationDetaulUpdate(string Tag, string DescriptionEN, string DescriptionES, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("TranslationsDetail_Update");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTag", DbType.String, Tag);
                db.AddInParameter(dbCommand, "@iDescriptionEN", DbType.String, DescriptionEN);
                db.AddInParameter(dbCommand, "@iDescriptionES", DbType.String, DescriptionES);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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

        public GenericReturn FormatGenericDetailTempDelete(int FormatID, string ColumnName, Guid TransactionID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[FormatGenericDetailTemp_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        public GenericReturn FormatAccess_TEMP_Delete(int FormatAccessTempID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatAccess_TEMP_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatAccessTempID", DbType.Int32, FormatAccessTempID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        public GenericReturn FormatsLoopsRulesDetail_TEMP_Delete4Edit(int FormatLoopRuleTempID, int Seq, Guid TransactionID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("eReq.FormatsLoopsRulesDetail_TEMP_Delete4Edit");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, Seq);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        public GenericReturn FormatPhase_CatalogDetail_TEMP_Delete(int CatalogDetailTempID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CatalogsDetailTemp_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailTempID", DbType.Int32, CatalogDetailTempID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        public GenericReturn FormatsLoopsApprovers_TEMP_Delete(int FormatLoopApproverTempID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsApprovers_TEMP_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopApproverTempID", DbType.Int32, FormatLoopApproverTempID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        public GenericReturn FormatsLoopsRules_TEMP_Delete(int FormatLoopRuleTempID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRules_TEMP_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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

        public DataTable FormatGenericDetailList4FormatGenericDetail(int FormatGenericDetailTempID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatGenericDetail_List4FormatGenericDetail]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatGenericDetailTempID", DbType.Int32, FormatGenericDetailTempID);
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
        public DataTable FormatGenericDetailTemp_List4PDFConfiguration(Guid TransactionID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[FormatGenericDetailTemp_List4PDFConfiguration]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
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
        public DataTable CatalogsDetailTemp_List4ConfigurationPDF(Guid TransactionID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CatalogsDetailTemp_List4ConfigurationPDF]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
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
        public DataTable FormatsLoopsFlow_TEMP_List(int FormatLoopRuleTempID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsFlow_TEMP_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
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
        public DataTable FormatsLoopsRulesDetail_TEMP_List4Edit(int FormatLoopRuleTempID, Guid TransactionID, int FormatID, GenericRequest request)
        { 
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesDetail_TEMP_List4Edit]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
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
        public DataTable FormatsLoopsRulesDetail_TEMP_List4EditExceptions(int FormatLoopRuleTempID, Guid TransactionID, int FormatID, Decimal Seq, GenericRequest request)
        { 
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesDetail_TEMP_List4EditExceptions]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Decimal, Seq);
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
        public DataTable FormatsLoopsAprovers_TEMP_List(int FormatLoopFlowTempID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsApprovers_TEMP_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopFlowTempID", DbType.Int32, FormatLoopFlowTempID);
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

        public DataTable FormatsLoopsAprovers_TEMP_List4ID(int FormatLoopApproverTempID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsApprovers_TEMP_List4ID]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopApproverTempID", DbType.Int32, FormatLoopApproverTempID);
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
        public GenericReturn CreateRule(DataTable dtFormatsLoopsFlow, int FacilitySelectedID, Guid TransactionID, int FormatID, string Description, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsCreateNewRule_TEMP_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilitySelectedID", DbType.Int32, FacilitySelectedID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, Description);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                //Parametro tipo tabla
                SqlParameter p = new SqlParameter("@iT_FormatsLoopsFlow", dtFormatsLoopsFlow)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        } 
        public GenericReturn EditRules(int FormatLoopRuleTempID, DataTable dtFormatsLoopsFlow, int FacilitySelectedID, Guid TransactionID, int FormatID, string Description, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Create Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsCreateNewRule_TEMP_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iFacilitySelectedID", DbType.Int32, FacilitySelectedID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iDescription", DbType.String, Description);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                //Parametro tipo tabla
                SqlParameter p = new SqlParameter("@iT_FormatsLoopsFlow", dtFormatsLoopsFlow)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        } 
        
        public GenericReturn AddFormatsLoopsApprovers(int FormatLoopFlowTempID, Guid TransactionID, int DepartmentOriginID, int JobPositionID, int DepartmentID, string ApproverIDs, int AddTolerance, int Tolerance, int ToleranceUoM, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsApprovers_TEMP_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopFlowTempID", DbType.Int32, FormatLoopFlowTempID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iDepartmentOriginID", DbType.Int32, DepartmentOriginID);
                db.AddInParameter(dbCommand, "@iJobPositionID", DbType.Int32, JobPositionID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iApproverID", DbType.String, ApproverIDs);
                db.AddInParameter(dbCommand, "@iAddTolerance", DbType.Int32, AddTolerance);
                db.AddInParameter(dbCommand, "@iTolerance", DbType.Int32, Tolerance);
                db.AddInParameter(dbCommand, "@iToleranceUoM", DbType.Int32, ToleranceUoM);
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
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }  
        public GenericReturn PDFFilesDetail_TEMP_Add(Guid TransactionID, int FileID, string FieldNames, string FieldType, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[PDFFilesDetail_TEMP_Add]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
                db.AddInParameter(dbCommand, "@iFieldNames", DbType.String, FieldNames);
                db.AddInParameter(dbCommand, "@iFieldType", DbType.String, FieldType);
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
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        } 
        public GenericReturn CreateNewFormat(string FormatName, int Use2FA, int DirectApproval, int HasDetail, int HasAttachment, int? FileID, int Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatName", DbType.String, FormatName);
                db.AddInParameter(dbCommand, "@iUse2FA", DbType.Int32, Use2FA);
                db.AddInParameter(dbCommand, "@iDirectApproval", DbType.Int32, DirectApproval);
                db.AddInParameter(dbCommand, "@iHasDetail", DbType.Int32, HasDetail);
                db.AddInParameter(dbCommand, "@iHasAttachment", DbType.Int32, HasAttachment);
                db.AddInParameter(dbCommand, "@iFileID", DbType.String, FileID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Int32, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oFormatID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oFormatID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                var a = ex.ToString();
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public GenericReturn FormatsLoopsFlow_TEMP_Delete(int FormatLoopFlowTempID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsFlow_TEMP_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatLoopFlowTempID", DbType.Int32, FormatLoopFlowTempID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        
        public void Formats_GenericFieldData(int CatalogDetailID, int FacilityID, out string DataTypeValue, out int IsAdditionalField, out string CatalogTag, out int TableAdditionalFieldID)
        {
            DataTypeValue = "";
            IsAdditionalField = 0;
            CatalogTag = "";
            TableAdditionalFieldID = 0;
            // Get DbCommand to Execute Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_GetFieldData]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCatalogDetailID", DbType.Int32, CatalogDetailID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddOutParameter(dbCommand, "@oIsAdditionalField", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oDataTypeValue", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oCatalogTag", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oTableAdditionalFieldID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                IsAdditionalField = (int)db.GetParameterValue(dbCommand, "@oIsAdditionalField");
                TableAdditionalFieldID = (int)db.GetParameterValue(dbCommand, "@oTableAdditionalFieldID");
                DataTypeValue = (string)db.GetParameterValue(dbCommand, "@oDataTypeValue");
                CatalogTag = (string)db.GetParameterValue(dbCommand, "@oCatalogTag");
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                dbCommand.Dispose();
            }
        }  
        public void Formats_GetFieldData4ValueID(string ValueID, int FacilityID, string CultureID, out string DataTypeValue, out int IsAdditionalField, out string CatalogTag, out int TableAdditionalFieldID)
        {
            DataTypeValue = "";
            IsAdditionalField = 0;
            CatalogTag = "";
            TableAdditionalFieldID = 0;
            // Get DbCommand to Execute Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_GetFieldData4ValueID]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iValueID", DbType.String, ValueID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oIsAdditionalField", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oDataTypeValue", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oCatalogTag", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oTableAdditionalFieldID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                IsAdditionalField = (int)db.GetParameterValue(dbCommand, "@oIsAdditionalField");
                TableAdditionalFieldID = (int)db.GetParameterValue(dbCommand, "@oTableAdditionalFieldID");
                DataTypeValue = (string)db.GetParameterValue(dbCommand, "@oDataTypeValue");
                CatalogTag = (string)db.GetParameterValue(dbCommand, "@oCatalogTag");
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                dbCommand.Dispose();
            }
        }

        public GenericReturn FormatsLoopsRulesDetail_TEMP_Create(Guid TransactionID, int FormatLoopRuleTempID, int FieldID, int IsAdditionalField, string DatePartArgument, int ComparisonOperator, string RuleDetailType, decimal? Seq, string ValuesArray, int FacilityID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesDetail_TEMP_Create]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iFieldID", DbType.Int32, FieldID);
                db.AddInParameter(dbCommand, "@iIsAdditionalField", DbType.Int32, IsAdditionalField);
                db.AddInParameter(dbCommand, "@iDatePartArgument", DbType.String, DatePartArgument);
                db.AddInParameter(dbCommand, "@iComparisonOperator", DbType.Int32, ComparisonOperator);
                db.AddInParameter(dbCommand, "@iRuleDetailType", DbType.String, RuleDetailType);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Decimal, Seq);
                db.AddInParameter(dbCommand, "@iValuesArray", DbType.String, ValuesArray);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oSeq", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oSeq");
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
        public GenericReturn FormatsLoopsRulesDetail_TEMP_Delete(int FormatLoopRuleDetailTempID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesDetail_TEMP_Delete]");
            try
            {
                // Parameters
                
                db.AddInParameter(dbCommand, "@iFormatLoopRuleDetailTempID", DbType.Int32, FormatLoopRuleDetailTempID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
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
        public DataTable FormatsLoopsRulesDetail_TEMP_List4EditRules(int FormatID, int FormatLoopRuleTempID, Guid TransactionID, decimal Seq, int FacilityID, GenericRequest request)
        {
            // Get DbCommand to Execute Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesDetail_TEMP_List4EditRules]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Decimal, Seq);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
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
         public DataTable FormatsLoopsRulesDetail_TEMP_List4EditRulesExceptions(int FormatID, int FormatLoopRuleTempID, Guid TransactionID, decimal Seq, int FacilityID, GenericRequest request)
        {
            // Get DbCommand to Execute Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatsLoopsRulesDetail_TEMP_List4EditExceptions]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFormatLoopRuleTempID", DbType.Int32, FormatLoopRuleTempID);
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Decimal, Seq);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
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

        public DataTable FormatsListGeneralFields(int FormatID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_ListGeneralFields]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
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
        public DataTable FormatGenericDetailField_List(int FormatID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatGenericDetail_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
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
        public DataTable Formats_ListPhases(int FormatID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[Formats_ListPhases]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
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
        public DataTable FormatAccess_List(int FormatID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[FormatAccess_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
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
        public DataTable FormatsPreviewFileData(int FormatID,int FileID, Guid TransactionID, int PositionYInitial, int MaxDetail, int Interline, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Formats_PreviewFileData]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
                db.AddInParameter(dbCommand, "@iTransationID", DbType.Guid, TransactionID);
                db.AddInParameter(dbCommand, "@iPositionYInitial", DbType.Int32, PositionYInitial);
                db.AddInParameter(dbCommand, "@iMaxDetail", DbType.Int32, MaxDetail);
                db.AddInParameter(dbCommand, "@iInterline", DbType.Int32, Interline);
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

    }
}
