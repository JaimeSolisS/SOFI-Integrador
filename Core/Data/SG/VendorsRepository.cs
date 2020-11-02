using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class VendorsRepository : GenericRepository
    {
        public DataTable List(int? VendorID, int? OrganizationID, string VendorName, string StatusIDs, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Vendors_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorID", DbType.Int32, VendorID);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, OrganizationID);
                db.AddInParameter(dbCommand, "@iVendorName", DbType.String, VendorName);
                db.AddInParameter(dbCommand, "@iStatusIDs", DbType.String, StatusIDs);
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
        public GenericReturn Insert(Vendor vendor, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Vendors_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, vendor.OrganizationID);
                db.AddInParameter(dbCommand, "@iVendorName", DbType.String, vendor.VendorName);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, vendor.Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
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
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }
        public GenericReturn Delete(int? VendorID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Vendors_Delete");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorID", DbType.Int32, VendorID);
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
