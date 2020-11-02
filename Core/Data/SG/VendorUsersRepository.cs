using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class VendorUsersRepository : GenericRepository
    {
        public DataTable List(int? VendorUserID, int? VendorID, string FullName, string AccessCode, string InsuranceNumber, DateTime? ExpirationDate, bool? Enabled, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("VendorUsers_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorUserID", DbType.Int32, VendorUserID);
                db.AddInParameter(dbCommand, "@iVendorID", DbType.Int32, VendorID);
                db.AddInParameter(dbCommand, "@iFullName", DbType.String, FullName);
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
                db.AddInParameter(dbCommand, "@iInsuranceNumber", DbType.String, InsuranceNumber);
                db.AddInParameter(dbCommand, "@iExpirationDate", DbType.DateTime, ExpirationDate);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Insert(VendorUser vendorUser, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[VendorUsers_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorID", DbType.Int32, vendorUser.VendorID);
                db.AddInParameter(dbCommand, "@iFullName", DbType.String, vendorUser.FullName);
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, vendorUser.AccessCode);
                db.AddInParameter(dbCommand, "@iInsuranceNumber", DbType.Int64, vendorUser.InsuranceNumber);
                db.AddInParameter(dbCommand, "@iExpirationDate", DbType.DateTime, vendorUser.ExpirationDate);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, vendorUser.Enabled);
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

        public GenericReturn Update(VendorUser vendorUser, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[VendorUsers_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorUserID", DbType.Int32, vendorUser.VendorUserID);
                db.AddInParameter(dbCommand, "@iVendorID", DbType.Int32, vendorUser.VendorID);
                db.AddInParameter(dbCommand, "@iFullName", DbType.String, vendorUser.FullName);
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, vendorUser.AccessCode);
                db.AddInParameter(dbCommand, "@iInsuranceNumber", DbType.String, vendorUser.InsuranceNumber);
                db.AddInParameter(dbCommand, "@iExpirationDate", DbType.DateTime, vendorUser.ExpirationDate);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, vendorUser.Enabled);
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

        public GenericReturn Delete(int? VendorUserID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[VendorUsers_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorUserID", DbType.Int32, VendorUserID);
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
