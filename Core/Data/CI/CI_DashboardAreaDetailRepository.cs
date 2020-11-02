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
    public class CI_DashboardAreaDetailRepository : GenericRepository
    {
        public DataTable List(int? DashboardAreaDetailID, int? ParentDashboardAreaDetailID, int? DashboardAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardAreasDetail_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardAreaDetailID", DbType.Int32, DashboardAreaDetailID);
                db.AddInParameter(dbCommand, "@iParentDashboardAreaDetailID", DbType.Int32, ParentDashboardAreaDetailID);
                db.AddInParameter(dbCommand, "@iDashboardAreaID", DbType.Int32, DashboardAreaID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFileTypeID", DbType.Int32, FileTypeID);
                db.AddInParameter(dbCommand, "@iHostName", DbType.String, HostName);
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

        public DataTable NodeList(int? DashboardAreaDetailID, int? ParentDashboardAreaDetailID, int? DashboardAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardAreasNode_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardAreaDetailID", DbType.Int32, DashboardAreaDetailID);
                db.AddInParameter(dbCommand, "@iParentDashboardAreaDetailID", DbType.Int32, ParentDashboardAreaDetailID);
                db.AddInParameter(dbCommand, "@iDashboardAreaID", DbType.Int32, DashboardAreaID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFileTypeID", DbType.Int32, FileTypeID);
                db.AddInParameter(dbCommand, "@iHostName", DbType.String, HostName);
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

        public GenericReturn Upsert(DashboardAreaDetail entity, DataTable dt, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardAreasDetail_Upsert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardAreaDetailID", DbType.Int32, entity.DashboardAreaDetailID);
                db.AddInParameter(dbCommand, "@iParentDashboardAreaDetailID", DbType.Int32, entity.ParentDashboardAreaDetailID);
                db.AddInParameter(dbCommand, "@iDashboardAreaID", DbType.Int32, entity.DashboardAreaID);
                db.AddInParameter(dbCommand, "@iFileTypeID", DbType.Int32, entity.FileTypeID);
                SqlParameter p = new SqlParameter("@it_DetailName", dt)
                {
                    SqlDbType = SqlDbType.Structured
                };
                dbCommand.Parameters.Add(p);
                db.AddInParameter(dbCommand, "@iFooter", DbType.String, entity.Footer);
                db.AddInParameter(dbCommand, "@iSizeID", DbType.Int32, entity.SizeID);
                db.AddInParameter(dbCommand, "@iBackColorID", DbType.Int32, entity.BackColorID);
                db.AddInParameter(dbCommand, "@iFontColorID", DbType.Int32, entity.FontColorID);
                db.AddInParameter(dbCommand, "@iIconID", DbType.Int32, entity.IconID);
                db.AddInParameter(dbCommand, "@iBackGroundImage", DbType.String, entity.BackgroundImage);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, entity.Seq);
                db.AddInParameter(dbCommand, "@iHref", DbType.String, entity.HRef);
                db.AddInParameter(dbCommand, "@iDataEffectID", DbType.Int32, entity.DataEffectID);
                db.AddInParameter(dbCommand, "@iDataEffectDuration", DbType.Int32, entity.DataEffectDuration);
                db.AddInParameter(dbCommand, "@iSourcePath", DbType.String, entity.SourcePath);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, entity.Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oDashboardAreasDetailID", DbType.Int32, 0);


                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oDashboardAreasDetailID");
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

        public GenericReturn QuickUpdate(int DashboardAreaDetailID, string ColumnName, string Value, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardAreasDetail_QuickUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardAreaDetailID", DbType.Int32, DashboardAreaDetailID);
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

        public GenericReturn Delete(int DashboardAreaDetailID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardAreasDetail_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardAreaDetailID", DbType.Int32, DashboardAreaDetailID);
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

        public DataTable GetNameInfo(int DashboarAreaDetailID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("CI.DashboardAreasDetail_NameInfo");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDashboardAreaDetailID", DbType.Int32, DashboarAreaDetailID);
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

        public string GetSeqNumber(int DashboarAreaDetailID)
        {
            string ParamValue="";
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("select SeqNumber from [CI].[fn_GetAreaNodes_SeqNumber](@iDashboarAreaDetailID)");
            try
            {   // Parameters
                db.AddInParameter(dbCommand, "@iDashboarAreaDetailID", DbType.Int32, DashboarAreaDetailID);
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
