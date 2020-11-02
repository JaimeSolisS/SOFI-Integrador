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
    public class KioskAreaRepository : GenericRepository
    {
        public DataTable List4Parent(int? KioskAreaID, int? ParentKioskAreaDetailID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskAreas_List4Parent");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
                db.AddInParameter(dbCommand, "@iParentKioskAreaDetailID", DbType.Int32, ParentKioskAreaDetailID);
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

        public DataTable List(int? KioskAreaID, int? ParentKioskAreaDetailID, string Title, int? SizeID, int? Seq, bool? Enabled, bool? IsRoot, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreas_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
                db.AddInParameter(dbCommand, "@iParentKioskAreaDetailID", DbType.Int32, ParentKioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iTitle", DbType.String, Title);
                db.AddInParameter(dbCommand, "@iSizeID", DbType.Int32, SizeID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, Seq);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iIsRoot", DbType.Boolean, IsRoot);
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

        public DataTable GetGeneralSettings(GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreas_GeneralSettings]");
            try
            {
                // Parameters
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

        public GenericReturn Upsert(KioskArea entity, DataTable dt, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreas_Upsert]");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.String, entity.KioskAreaID);
                SqlParameter p = new SqlParameter("@it_GenericItem", dt)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iParentKioskAreaDetailID", DbType.Int32, entity.ParentID);
                db.AddInParameter(dbCommand, "@iSizeID", DbType.Int32, entity.SizeID);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, entity.Seq);
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

        public GenericReturn QuickUpdate(int KioskAreaID, string ColumnName, string Value, int FacilityID, int ChangedBy, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskAreas_QuickUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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

        public GenericReturn Delete(int? KioskAreaID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreas_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
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

        public GenericReturn QuickTranslateUpdate(int KioskAreaID, DataTable dt, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskAreas_QuickTranslateUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
                SqlParameter p = new SqlParameter("@it_Translates", dt)
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

        public string GetKioskCarouselMediaID(int FacilityID)
        {

            string ParamValue = "";

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT HR.fn_GetKioskCarouselMediaID(@iFacilityID)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);

                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToString();

            }
            catch
            {
                ParamValue = "";
            }
            finally
            {
                dbCommand.Dispose();
            }

            return ParamValue;
        }
    }
}
