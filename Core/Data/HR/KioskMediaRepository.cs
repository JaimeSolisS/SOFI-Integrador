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
    public class KioskMediaRepository : GenericRepository
    {
        public DataTable List(int? DashboardCarouselVideoID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskCarouselMedia_List]");
            try
            {
                // Parameters
                //db.AddInParameter(dbCommand, "@iKioskCarouselVideoID", DbType.Int32, DashboardCarouselVideoID);
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
            { dbCommand.Dispose(); }

        }
        public DataTable TempList(int KioskCarouselMediaID,int TempReferenceID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskCarouselMediaTEMP_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskCarouselMediaID", DbType.Int32, KioskCarouselMediaID);
                db.AddInParameter(dbCommand, "@iTempReferenceID", DbType.Int32, TempReferenceID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                
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

        public GenericReturn Insert(GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskCarouselMedia_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oID", DbType.Int32, 0);

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
                result.ID = 0;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }


        public GenericReturn Delete(int KioskCarouselMediaID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskCarouselMedia_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskCarouselMediaID", DbType.Int32, KioskCarouselMediaID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.String, request.FacilityID);
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

        public GenericReturn Update(string ReferenceID, DataTable FileInfo, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskCarouselMedia_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.String, ReferenceID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                SqlParameter p = new SqlParameter("@iFileInfo", FileInfo)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
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

        public int GetKioskCarouselID(GenericRequest request)
        {

            int ParamValue = 0;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT [HR].[fn_GetKioskCarouselMediaID](@iFacilityID)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);

                // Execute Query
                ParamValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));

            }
            catch
            {
                ParamValue = 0;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return ParamValue;
        }
    }
}
