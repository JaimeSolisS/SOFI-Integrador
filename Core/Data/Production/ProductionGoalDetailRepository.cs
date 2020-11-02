﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class ProductionGoalDetailRepository : GenericRepository
    {
        #region CRUD
        public DataTable List(ProductionGoal entity, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("MFG.ProductionGoalsDetail_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iGoalID", DbType.Int32, entity.GoalID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, entity.ProductionProcessID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, entity.ProductionLineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, entity.VAID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, entity.DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, entity.ShiftID);
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

        public GenericReturn BulkUpsert(DataTable dt, ProductionGoalsDetail _entity, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("MFG.ProductionGoalsDetail_BulkUpsert");

            try
            {
                using (dt)
                {
                    // Parameters
                    db.AddInParameter(dbCommand, "@iGoalID", DbType.Int32, _entity.GoalID);
                    SqlParameter p = new SqlParameter("@it_RangeHours", dt)
                    {
                        SqlDbType = SqlDbType.Structured
                    };
                    dbCommand.Parameters.Add(p);
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
