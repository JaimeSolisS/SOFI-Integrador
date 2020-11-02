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
    public class CI_DashboardCarouselVideosTmpRepository : GenericRepository
    {
        public DataTable List(string TransactionID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideosTmp_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, Guid.Parse(TransactionID));
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
            { dbCommand.Dispose(); }

        }

        public GenericReturn Delete(int FileID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideosTmp_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFileID", DbType.Int32, FileID);
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
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn CreateSettingsOnTmp(string TransactionID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideosTmp_CreateSettings");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, Guid.Parse(TransactionID));
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
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn Insert(string TransactionID, DataTable dtAttachments, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideosTmp_Insert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, Guid.Parse(TransactionID));
                SqlParameter p = new SqlParameter("@it_AttachmentsFiles", dtAttachments)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn SetSort(string TransactionID, DataTable dt, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardCarouselVideosTmp_SetSort");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iTransactionID", DbType.Guid, Guid.Parse(TransactionID));
                SqlParameter p = new SqlParameter("@it_Files", dt)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
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
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }
    }
}

