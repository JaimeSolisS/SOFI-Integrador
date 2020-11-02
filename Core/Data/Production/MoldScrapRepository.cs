using Core.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.Data
{
    public class MoldScrapRepository : GenericRepository
    {
        #region MISC Methods
        public GenericReturn Upsert(MoldScraps _entity, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MoldScraps_Update");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iScrapDate", DbType.Int32, _entity.ScrapDate);
                db.AddInParameter(dbCommand, "@iHourValue", DbType.String, _entity.ScrapTime.Hours);
                db.AddInParameter(dbCommand, "@iQuantity", DbType.String, _entity.Quantity);
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
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn BulkUpsert(DataTable dt, MoldScraps _entity, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MoldScraps_BulkUpsert");

            try
            {
                using (dt)
                {
                    // Parameters
                    db.AddInParameter(dbCommand, "@iScrapDate", DbType.Date, _entity.ScrapDate);
                    SqlParameter p = new SqlParameter("@it_ShiftQuantities", dt)
                    {
                        SqlDbType = SqlDbType.Structured
                    };
                    dbCommand.Parameters.Add(p);
                    db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, _entity.ProductionProcessID);
                    db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, _entity.ProductionLineID);
                    db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                    db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                    db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                    db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                    db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                    db.AddInParameter(dbCommand, "@iShiftID", DbType.String, _entity.ShiftID);
                    db.AddInParameter(dbCommand, "@iDesignID", DbType.String, _entity.DesignID);
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

        public DataTable List(MoldScraps _entity, GenericRequest request)
        {
           
            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MoldScraps_ShiftList");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iScrapDate", DbType.Date, _entity.ScrapDate);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, _entity.ProductionProcessID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, _entity.ProductionLineID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, _entity.ShiftID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.String, _entity.DesignID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

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
        #endregion
    }
}
