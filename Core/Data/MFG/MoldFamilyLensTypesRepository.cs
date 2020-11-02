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
    public class MoldFamilyLensTypesRepository : GenericRepository
    {
        public DataTable List(string MoldFamilyIDs, string MoldLenTypesIDs, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MoldFamilies_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMoldFamilyIDs", DbType.String, MoldFamilyIDs);
                db.AddInParameter(dbCommand, "@iMoldLensIDs", DbType.String, MoldLenTypesIDs);
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

        public DataTable ListOfLens(int MoldFamilyIDs, string MoldLensTypeIDs, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MoldFamilyLens_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMoldFamilyID", DbType.Int32, MoldFamilyIDs);
                db.AddInParameter(dbCommand, "@iMoldLensTypeIDs", DbType.String, MoldLensTypeIDs);
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

        public GenericReturn Save(string MoldFamilyName, string LensTypeID, bool Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MoldFamilyLensTypes_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMoldFamilyName", DbType.String, MoldFamilyName);
                db.AddInParameter(dbCommand, "@iLensTypeIDs", DbType.String, LensTypeID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Update(int? MoldFamilyID, string MoldFamilyName, string LensTypeID, bool Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MoldFamilyLensTypes_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMoldFamilyID", DbType.Int32, MoldFamilyID);
                db.AddInParameter(dbCommand, "@iMoldFamilyName", DbType.String, MoldFamilyName);
                db.AddInParameter(dbCommand, "@iLensTypeIDs", DbType.String, LensTypeID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Delete(int? MoldFamilyLensTypeID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MoldFamilyLensTypes_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMoldFamilyLensTypeID", DbType.Int32, MoldFamilyLensTypeID);
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

        public GenericReturn SaveExcelImport(DataTable dt, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MoldFamilyLensTypes_ExcelImport]");
            try
            {
                // Parameters
                SqlParameter p = new SqlParameter("@t_MoldFamilyLensInfo", dt)
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
    }
}
