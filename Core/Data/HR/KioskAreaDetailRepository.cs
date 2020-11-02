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
    public class KioskAreaDetailRepository : GenericRepository
    {
        public DataTable List(int? KioskAreaDetailID, int? ParentKioskAreaDetailID, int? KioskAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreasDetail_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, KioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iParentKioskAreaDetailID", DbType.Int32, ParentKioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFileTypeID", DbType.Int32, FileTypeID);
                db.AddInParameter(dbCommand, "@iHostName", DbType.String, HostName);
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

        public DataTable NodeList(int? KioskAreaDetailID, int? ParentKioskAreaDetailID, int? KioskAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreasNode_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, KioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iParentKioskAreaDetailID", DbType.Int32, ParentKioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, KioskAreaID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFileTypeID", DbType.Int32, FileTypeID);
                db.AddInParameter(dbCommand, "@iHostName", DbType.String, HostName);
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

        public GenericReturn Upsert(KioskAreaDetail entity, DataTable dt, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("[HR].[KioskAreasDetail_Upsert]");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, entity.KioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iParentKioskAreaDetailID", DbType.Int32, entity.ParentKioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iKioskAreaID", DbType.Int32, entity.KioskAreaID);
                db.AddInParameter(dbCommand, "@iFileTypeID", DbType.Int32, entity.FileTypeID);
                SqlParameter p = new SqlParameter("@it_DetailName", dt)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iSizeID", DbType.Int32, entity.SizeID);
                db.AddInParameter(dbCommand, "@iIconID", DbType.Int32, entity.IconID);
                db.AddInParameter(dbCommand, "@iBackGroundImage", DbType.String, entity.BackgroundImage);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, entity.Seq);
                db.AddInParameter(dbCommand, "@iHref", DbType.String, entity.HRef);
                db.AddInParameter(dbCommand, "@iIsWindow", DbType.Boolean, entity.IsWindow);
                db.AddInParameter(dbCommand, "@iDataEffectID", DbType.Int32, entity.DataEffectID);
                db.AddInParameter(dbCommand, "@iDataEffectDuration", DbType.Int32, entity.DataEffectDuration);
                db.AddInParameter(dbCommand, "@iSourcePath", DbType.String, entity.SourcePath);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, entity.Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                db.AddInParameter(dbCommand, "@iBackgroundColor", DbType.String, entity.BackgroundColor);
                db.AddInParameter(dbCommand, "@iFontColor", DbType.String, entity.FontColor);

                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oKioskAreasDetailID", DbType.Int32, 0);


                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oKioskAreasDetailID");
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

        public GenericReturn QuickUpdate(int KioskAreaDetailID, string ColumnName, string Value, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskAreasDetail_QuickUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, KioskAreaDetailID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
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

        public GenericReturn Delete(int KioskAreaDetailID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskAreasDetail_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, KioskAreaDetailID);
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

        public DataTable GetNameInfo(int KioskAreaDetailID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("HR.KioskAreasDetail_NameInfo");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, KioskAreaDetailID);
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

        public string GetSeqNumber(int KioskAreaDetailID)
        {
            string ParamValue="";
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("select SeqNumber from [HR].[fn_GetAreaNodes_SeqNumber](@iKioskAreaDetailID)");
            try
            {   // Parameters
                db.AddInParameter(dbCommand, "@iKioskAreaDetailID", DbType.Int32, KioskAreaDetailID);
                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToString();
            }
            catch
            { ParamValue = ""; }
            finally
            { dbCommand.Dispose(); }

            return ParamValue;
        }
    }
}
