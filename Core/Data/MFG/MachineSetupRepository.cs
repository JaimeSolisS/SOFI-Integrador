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
    public class MachineSetupRepository : GenericRepository
    {
        public DataTable List(int? MachineSetupID, string MachineSetupName, bool? Enabled, int? MachineID, int? MaterialID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MachineSetups_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
                db.AddInParameter(dbCommand, "@iMachineID", DbType.Int32, MachineID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iMachineSetupName", DbType.String, MachineSetupName);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Upsert(int? MachineSetupID, string MachineSetupName, bool? Enabled, DataTable MachineMaterialSetups, DataTable MachineSetupParametrsList, DataTable TempListDeletedSections, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MachineSetup_Upsert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
                db.AddInParameter(dbCommand, "@iMachineSetupName", DbType.String, MachineSetupName);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                SqlParameter p = new SqlParameter("@iMachineMaterialSetups", MachineMaterialSetups)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);

                p = new SqlParameter("@iMachineSetupParameters", MachineSetupParametrsList)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);

                p = new SqlParameter("@iSectionsToDelete", TempListDeletedSections)
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

        public GenericReturn Delete(int? MachineSetupID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MachineSetups_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32, MachineSetupID);
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
    }
}
