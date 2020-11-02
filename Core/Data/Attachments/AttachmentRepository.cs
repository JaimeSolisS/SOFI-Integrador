using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class AttachmentRepository : GenericRepository
    {
        #region CRUD Methods

        public DataTable PartsMasterAttachments_List(int? FileID, int? PartID, int? CompanyID, int? FileType, string FilePathName, int? UserID, string CultureID)
        {
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetStoredProcCommand("PartsMasterAttachments_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
                db.AddInParameter(dbCommand, "@iPartID", DbType.Int32, PartID);
                db.AddInParameter(dbCommand, "@iCompanyID", DbType.Int32, CompanyID);
                db.AddInParameter(dbCommand, "@iFileType", DbType.Int32, FileType);
                db.AddInParameter(dbCommand, "@iFilePathName", DbType.String, FilePathName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

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

        public GenericReturn PartsMasterAttachments_Insert(int? PartID, int? CompanyID, int? FileType, string FilePathName, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("PartsMasterAttachments_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iPartID", DbType.Int32, PartID);
                db.AddInParameter(dbCommand, "@iCompanyID", DbType.Int32, CompanyID);
                db.AddInParameter(dbCommand, "@iFileType", DbType.Int32, FileType);
                db.AddInParameter(dbCommand, "@iFilePathName", DbType.String, FilePathName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn PartsMasterAttachments_QuickUpdate(int? FileID, int? PartID, int? CompanyID, int? FileType, string FilePathName, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("PartsMasterAttachments_QuickUpdate");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
                db.AddInParameter(dbCommand, "@iPartID", DbType.Int32, PartID);
                db.AddInParameter(dbCommand, "@iCompanyID", DbType.Int32, CompanyID);
                db.AddInParameter(dbCommand, "@iFileType", DbType.Int32, FileType);
                db.AddInParameter(dbCommand, "@iFilePathName", DbType.String, FilePathName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn PartsMasterAttachments_Delete(int? FileID, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("PartsMasterAttachments_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public DataTable Attachments_List(int? FileId, int? ReferenceID, int? ReferenceType, int? FileType, string FilePathName, int? UserID, int FacilityID, string CultureID)
        {
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetStoredProcCommand("Attachments_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileId", DbType.Int32, FileId);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceType", DbType.Int32, ReferenceType);
                db.AddInParameter(dbCommand, "@iFileType", DbType.Int32, FileType);
                db.AddInParameter(dbCommand, "@iFilePathName", DbType.String, FilePathName);
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
            finally
            { dbCommand.Dispose(); }
        }

        public GenericReturn Attachments_Insert(int? ReferenceID, int? ReferenceType, int? FileType, string FilePathName, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Attachments_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceType", DbType.Int32, ReferenceType);
                db.AddInParameter(dbCommand, "@iFileType", DbType.Int32, FileType);
                db.AddInParameter(dbCommand, "@iFilePathName", DbType.String, FilePathName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
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

        public GenericReturn Attachments_QuickUpdate(int? FileId, int? ReferenceID, int? ReferenceType, int? FileType, string FilePathName, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Attachments_QuickUpdate");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileId", DbType.Int32, FileId);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iReferenceType", DbType.Int32, ReferenceType);
                db.AddInParameter(dbCommand, "@iFileType", DbType.Int32, FileType);
                db.AddInParameter(dbCommand, "@iFilePathName", DbType.String, FilePathName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn Attachments_Delete(int? FileId, int? UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Attachments_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileId", DbType.Int32, FileId);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        #region Methods

        public GenericReturn DA_InsertPOD(int OrderID, string FilePath, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("DA_Attachments_SavePOD");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOrderID", DbType.Int32, OrderID);
                db.AddInParameter(dbCommand, "@iFilePath", DbType.String, FilePath);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iDeviceUniqueID", DbType.String, request.DeviceUniqueID);
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

        public GenericReturn Properties_QuickUpdate(int? FileId, string PropertyName, string PropertyValue, int? PropertyTypeID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("AttachmentsProperties_QuickUpdate");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileId", DbType.Int32, FileId);
                db.AddInParameter(dbCommand, "@iPropertyName", DbType.String, PropertyName);
                db.AddInParameter(dbCommand, "@iPropertyValue", DbType.String, PropertyValue);
                db.AddInParameter(dbCommand, "@iPropertyTypeID", DbType.Int32, PropertyTypeID);
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
