using Core.Entities;
using Core.Entities.SQL_DataType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DataInterfacesLogRepository : GenericRepository
    {
        #region CRUD
        public DataTable FilesToProcess(int DataInterfaceID, DataTable dtIn)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            try
            {
                using (dbCommand = db.GetStoredProcCommand("DataInterfacesLog_FilesToProcess"))
                {
                    dbCommand.CommandTimeout = 3600;
                    using (dtIn)
                    {
                        // Parameters
                        db.AddInParameter(dbCommand, "@iDataInterfaceID", DbType.Int32, DataInterfaceID);
                        SqlParameter p = new SqlParameter("@it_AllValidFiles", dtIn)
                        {
                            SqlDbType = SqlDbType.Structured
                        };
                        dbCommand.Parameters.Add(p);
                        // Execute Query
                        using (DataTable dtOut = new DataTable())
                        {
                            dtOut.Load(db.ExecuteReader(dbCommand));
                            return dtOut;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GenericReturn Upsert(DataInterfacelog entity)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("DataInterfacesLog_Upsert");

            try
            {
                db.AddInParameter(dbCommand, "@iDataInterfaceLogID", DbType.Int32, entity.DataInterfaceLogID);
                db.AddInParameter(dbCommand, "@iDataInterfaceID", DbType.Int32, entity.DataInterfaceID);
                db.AddInParameter(dbCommand, "@iFileName", DbType.String, entity.FileName);
                db.AddInParameter(dbCommand, "@iTotalRows", DbType.Int32, entity.TotalRows);
                db.AddInParameter(dbCommand, "@iTransactionDate", DbType.DateTime, entity.TransactionDate);
                db.AddInParameter(dbCommand, "@iCompleted", DbType.Boolean, entity.Completed);
                db.AddInParameter(dbCommand, "@iReference", DbType.String, entity.Reference);
                db.AddInParameter(dbCommand, "@iErrorCode", DbType.Int32, entity.ErrorCode);
                db.AddInParameter(dbCommand, "@iErrorMessage", DbType.String, entity.ErrorMessage);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oDataInterfaceLogID", DbType.Int32, 255);
             
                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oDataInterfaceLogID");
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
