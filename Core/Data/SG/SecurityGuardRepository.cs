using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class SecurityGuardRepository : GenericRepository
    {
        public DataTable List(string CheckInPersonTypes, string EmployeeNumber, string VehiclePlates, string CheckTypeID, string PersonName, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCheckInPersonTypes", DbType.String, CheckInPersonTypes);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
                db.AddInParameter(dbCommand, "@iVehiclePlates", DbType.String, VehiclePlates);
                db.AddInParameter(dbCommand, "@iCheckTypeID", DbType.String, CheckTypeID);
                db.AddInParameter(dbCommand, "@iPersonName", DbType.String, PersonName);
                db.AddInParameter(dbCommand, "@iForReport", DbType.String, false);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
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
        public DataSet ExportToExcel(string CheckInPersonTypes, string EmployeeNumber, string VehiclePlates, string CheckTypeID, string PersonName, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCheckInPersonTypes", DbType.String, CheckInPersonTypes);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
                db.AddInParameter(dbCommand, "@iVehiclePlates", DbType.String, VehiclePlates);
                db.AddInParameter(dbCommand, "@iCheckTypeID", DbType.String, CheckTypeID);
                db.AddInParameter(dbCommand, "@iPersonName", DbType.String, PersonName);
                db.AddInParameter(dbCommand, "@iForReport", DbType.String, true);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.DateTime, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.DateTime, EndDate);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public GenericReturn Insert(SecurityGuardLog SGL, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iPersonID", DbType.String, SGL.PersonID);
                db.AddInParameter(dbCommand, "@iPersonName", DbType.String, SGL.PersonName);
                db.AddInParameter(dbCommand, "@iPersonTypeID", DbType.Int32, SGL.PersonTypeID);
                db.AddInParameter(dbCommand, "@iWhoVisit", DbType.String, SGL.WhoVisit);
                db.AddInParameter(dbCommand, "@iVehiclePlates", DbType.String, SGL.VehiclePlates);
                db.AddInParameter(dbCommand, "@iVehicleMarkID", DbType.Int32, SGL.VehicleMarkID);
                //db.AddInParameter(dbCommand, "@iCheckOut", DbType.DateTime, SGL.CheckOut);
                db.AddInParameter(dbCommand, "@iSecurityOfficerID", DbType.Int32, SGL.SecurityOfficerID);
                db.AddInParameter(dbCommand, "@iBadgeID", DbType.Int32, SGL.BadgeID);
                db.AddInParameter(dbCommand, "@iIdentificationTypeID", DbType.Int32, SGL.IdentificationTypeID);
                db.AddInParameter(dbCommand, "@iIdentificationImgPath", DbType.String, SGL.IdentificationImgPath);
                db.AddInParameter(dbCommand, "@iEquipmentIDs", DbType.String, SGL.EquipmentIDs);
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
        public GenericReturn Update(int? SecurityGuardLogID, string PersonID, string PersonName, int? PersonTypeID, string WhoVisit, string VehiclePlates, int? VehicleMarkID, DateTime? CheckIn, DateTime? CheckOut, int? SecurityOfficerID, int? BadgeID, int? IdentificationTypeID, string IdentificationImgPath, string EquipmentIDs, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iSecurityGuardLogID", DbType.Int32, SecurityGuardLogID);
                db.AddInParameter(dbCommand, "@iPersonID", DbType.String, PersonID);
                db.AddInParameter(dbCommand, "@iPersonName", DbType.String, PersonName);
                db.AddInParameter(dbCommand, "@iPersonTypeID", DbType.Int32, PersonTypeID);
                db.AddInParameter(dbCommand, "@iWhoVisit", DbType.String, WhoVisit);
                db.AddInParameter(dbCommand, "@iVehiclePlates", DbType.String, VehiclePlates);
                db.AddInParameter(dbCommand, "@iVehicleMarkID", DbType.Int32, VehicleMarkID);
                db.AddInParameter(dbCommand, "@iCheckIn", DbType.DateTime, CheckIn);
                db.AddInParameter(dbCommand, "@iCheckOut", DbType.DateTime, CheckOut);
                db.AddInParameter(dbCommand, "@iSecurityOfficerID", DbType.Int32, SecurityOfficerID);
                db.AddInParameter(dbCommand, "@iBadgeID", DbType.Int32, BadgeID);
                db.AddInParameter(dbCommand, "@iIdentificationTypeID", DbType.Int32, IdentificationTypeID);
                db.AddInParameter(dbCommand, "@iIdentificationImgPath", DbType.String, IdentificationImgPath);
                db.AddInParameter(dbCommand, "@iEquipmentIDs", DbType.String, EquipmentIDs);
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
        public GenericReturn Upsert(SecurityGuardLog SGL, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_Upsert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iSecurityGuardLogID", DbType.Int32, SGL.SecurityGuardLogID);
                db.AddInParameter(dbCommand, "@iPersonID", DbType.String, SGL.PersonID);
                db.AddInParameter(dbCommand, "@iPersonName", DbType.String, SGL.PersonName);
                db.AddInParameter(dbCommand, "@iPersonTypeID", DbType.Int32, SGL.PersonTypeID);
                db.AddInParameter(dbCommand, "@iWhoVisit", DbType.String, SGL.WhoVisit);
                db.AddInParameter(dbCommand, "@iVehiclePlates", DbType.String, SGL.VehiclePlates);
                db.AddInParameter(dbCommand, "@iVehicleMarkID", DbType.Int32, SGL.VehicleMarkID);
                db.AddInParameter(dbCommand, "@iCheckIn", DbType.DateTime, SGL.CheckIn);
                db.AddInParameter(dbCommand, "@iCheckOut", DbType.DateTime, SGL.CheckOut);
                db.AddInParameter(dbCommand, "@iSecurityOfficerID", DbType.String, SGL.SecurityOfficerID);
                db.AddInParameter(dbCommand, "@iBadgeID", DbType.Int32, SGL.BadgeID);
                db.AddInParameter(dbCommand, "@iIdentificationTypeID", DbType.Int32, SGL.IdentificationTypeID);
                db.AddInParameter(dbCommand, "@iIdentificationImgPath", DbType.String, SGL.IdentificationImgPath);
                db.AddInParameter(dbCommand, "@iEquipmentIDs", DbType.String, SGL.EquipmentIDs);
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
        public DataTable GetPersonInfoByCode(string AccessCode, string CheckType, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[GetPersonInfoByCode]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
                db.AddInParameter(dbCommand, "@iCheckType", DbType.String, CheckType);
                db.AddInParameter(dbCommand, "@iUserID", DbType.String, request.UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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
        public DataTable GetSecurityGuardCheckOutInfo(string AccessCode, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[GetSecurityGuardCheckOutInfo]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
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
        public DataTable CheckSessionState(string AccessCode, string CheckType, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[CheckSessionState]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
                db.AddInParameter(dbCommand, "@iCheckType", DbType.String, CheckType);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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
        public DataTable GetPersonInfoHistory(string AccessCode, int? PersonTypeID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[GetPersonInfoHistory]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iAccessCode", DbType.String, AccessCode);
                db.AddInParameter(dbCommand, "@iPersonTypeID", DbType.Int32, PersonTypeID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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

        public DataTable GetAvailableBadges(int VendorTypeID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_GetAvailableBadges]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iVendorTypeID", DbType.String, VendorTypeID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.String, request.FacilityID);
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

    }
}
