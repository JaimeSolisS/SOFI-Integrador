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
    public class DataInterfacesRepository : GenericRepository
    {
        #region CRUD
        public DataTable List(string CultureID)
        {
            try
            {
                using (dbCommand = db.GetStoredProcCommand("DataInterfaces_List"))
                {
                    // Parameters
                    db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                    // Execute Query
                    using (DataTable dt = new DataTable())
                    {
                        dt.Load(db.ExecuteReader(dbCommand));
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable List(int? DataInterfaceLogID, int? DataInterfaceID, string FileName, int? TotalRows, string Reference, DateTime? TransactionDate, bool? Completed, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[DatainterfacesLog_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDataInterfaceLogID", DbType.Int32, DataInterfaceLogID);
                db.AddInParameter(dbCommand, "@iDataInterfaceID", DbType.Int32, DataInterfaceID);
                db.AddInParameter(dbCommand, "@iFileName", DbType.String, FileName);
                db.AddInParameter(dbCommand, "@iTotalRows", DbType.Int32, TotalRows);
                db.AddInParameter(dbCommand, "@iReference", DbType.String, Reference);
                db.AddInParameter(dbCommand, "@iTransactionDate", DbType.DateTime, TransactionDate);
                db.AddInParameter(dbCommand, "@iCompleted", DbType.Boolean, Completed);
                db.AddInParameter(dbCommand, "@iErrorCode", DbType.Int32, 0);
                db.AddInParameter(dbCommand, "@iErrorMessage", DbType.String, "");
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


        public GenericReturn SendDataToDB(int DataInterfaceID, string FileName,  string Reference, int? UserID, string CultureID, DataTable dt)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("Interface_MainProcess");

            try
            {
                using (dt)
                {
                    // Parameters
                    
                    SqlParameter p = new SqlParameter("@it_MasterTemplate", dt)
                    {
                        SqlDbType = SqlDbType.Structured
                    };
                    dbCommand.Parameters.Add(p);
                    db.AddInParameter(dbCommand, "@iDataInterfaceID", DbType.Int32, DataInterfaceID);
                    db.AddInParameter(dbCommand, "@iFileName", DbType.String, FileName);
                    db.AddInParameter(dbCommand, "@iReference", DbType.String, Reference);
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
