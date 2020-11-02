using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class EquipmentRepository : GenericRepository
    {
        public DataTable List(int? EquipmentID, string Serial, string EquipmentName, string EquipmentTypeIDs, string UPC, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Equipment_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEquipmentID", DbType.Int32, EquipmentID);
                db.AddInParameter(dbCommand, "@iSerial", DbType.String, Serial);
                db.AddInParameter(dbCommand, "@iEquipmentName", DbType.String, EquipmentName);
                db.AddInParameter(dbCommand, "@iEquipmentTypeIDs", DbType.String, EquipmentTypeIDs);
                db.AddInParameter(dbCommand, "@iUPC", DbType.String, UPC);
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

        public DataTable GetHistoryEquipment(string AccessCode, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[SecurityGuardLog_GetHistoryEquipment]");
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

        public GenericReturn Insert(Equipment equipment, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Equipment_Insert]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEquipmentName", DbType.String, equipment.EquipmentName);
                db.AddInParameter(dbCommand, "@iEquipmentDescription", DbType.String, equipment.EquipmentDescription);
                db.AddInParameter(dbCommand, "@iUPC", DbType.String, equipment.UPC);
                db.AddInParameter(dbCommand, "@iIPAddress1", DbType.String, equipment.IPAddress1);
                db.AddInParameter(dbCommand, "@iIPAddress2", DbType.String, equipment.IPAddress2);
                db.AddInParameter(dbCommand, "@iPosition", DbType.String, equipment.Position);
                db.AddInParameter(dbCommand, "@iExtension", DbType.Int32, equipment.Extension);
                db.AddInParameter(dbCommand, "@iNumber", DbType.String, equipment.Number);
                db.AddInParameter(dbCommand, "@iEquipmentTypeID", DbType.Int32, equipment.EquipmentTypeID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, equipment.Comments);
                db.AddInParameter(dbCommand, "@iSiteID", DbType.Int32, equipment.SiteID);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, equipment.CategoryID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, equipment.DepartmentID);
                db.AddInParameter(dbCommand, "@iUseID", DbType.Int32, equipment.UseID);
                db.AddInParameter(dbCommand, "@iSupervisorID", DbType.Int32, equipment.SupervisorID);
                db.AddInParameter(dbCommand, "@iModelID", DbType.Int32, equipment.ModelID);
                db.AddInParameter(dbCommand, "@iSerial", DbType.String, equipment.Serial);
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, equipment.Year);
                db.AddInParameter(dbCommand, "@iActive", DbType.Boolean, equipment.Active);
                db.AddInParameter(dbCommand, "@iBankReference", DbType.String, equipment.BankReference);
                db.AddInParameter(dbCommand, "@iCredit", DbType.Decimal, equipment.Credit);
                db.AddInParameter(dbCommand, "@iMaintenanceTypeID", DbType.Int32, equipment.MaintenanceTypeID);
                db.AddInParameter(dbCommand, "@iEquipmentReferenceID", DbType.Int32, equipment.EquipmentReferenceID);
                db.AddInParameter(dbCommand, "@iAreaID", DbType.Int32, equipment.AreaID);
                //db.AddInParameter(dbCommand, "@iTypeID", DbType.Int32, equipment.TypeID);
                db.AddInParameter(dbCommand, "@iSavID", DbType.Int32, equipment.SavID);
                db.AddInParameter(dbCommand, "@iColor", DbType.String, equipment.Color);
                db.AddInParameter(dbCommand, "@iOS_ID", DbType.Int32, equipment.OS_ID);
                db.AddInParameter(dbCommand, "@iModelVisitor", DbType.String, equipment.ModelVisitor);
                db.AddInParameter(dbCommand, "@iMarkVisitor", DbType.String, equipment.MarkVisitor);
                db.AddInParameter(dbCommand, "@iUseVisitor", DbType.String, equipment.UseVisitor);
                db.AddInParameter(dbCommand, "@iCompanyVisitor", DbType.String, equipment.CompanyVisitor);
                db.AddInParameter(dbCommand, "@iAvailabilityTarget", DbType.Decimal, equipment.AvailabilityTarget);
                db.AddInParameter(dbCommand, "@iIsLeasing", DbType.Boolean, equipment.IsLeasing);
                db.AddInParameter(dbCommand, "@iLeasingStart", DbType.DateTime, equipment.LeasingStart);
                db.AddInParameter(dbCommand, "@iLeasingEnd", DbType.DateTime, equipment.LeasingEnd);
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

        public GenericReturn Update(Equipment equipment, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Equipment_Update]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEquipmentID", DbType.Int32, equipment.EquipmentID);
                db.AddInParameter(dbCommand, "@iEquipmentName", DbType.String, equipment.EquipmentName);
                db.AddInParameter(dbCommand, "@iEquipmentDescription", DbType.String, equipment.EquipmentDescription);
                db.AddInParameter(dbCommand, "@iUPC", DbType.String, equipment.UPC);
                db.AddInParameter(dbCommand, "@iIPAddress1", DbType.String, equipment.IPAddress1);
                db.AddInParameter(dbCommand, "@iIPAddress2", DbType.String, equipment.IPAddress2);
                db.AddInParameter(dbCommand, "@iPosition", DbType.String, equipment.Position);
                db.AddInParameter(dbCommand, "@iExtension", DbType.Int32, equipment.Extension);
                db.AddInParameter(dbCommand, "@iNumber", DbType.String, equipment.Number);
                db.AddInParameter(dbCommand, "@iEquipmentTypeID", DbType.Int32, equipment.EquipmentTypeID);
                db.AddInParameter(dbCommand, "@iComments", DbType.String, equipment.Comments);
                db.AddInParameter(dbCommand, "@iSiteID", DbType.Int32, equipment.SiteID);
                db.AddInParameter(dbCommand, "@iCategoryID", DbType.Int32, equipment.CategoryID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, equipment.DepartmentID);
                db.AddInParameter(dbCommand, "@iUseID", DbType.Int32, equipment.UseID);
                db.AddInParameter(dbCommand, "@iSupervisorID", DbType.Int32, equipment.SupervisorID);
                db.AddInParameter(dbCommand, "@iModelID", DbType.Int32, equipment.ModelID);
                db.AddInParameter(dbCommand, "@iSerial", DbType.String, equipment.Serial);
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, equipment.Year);
                db.AddInParameter(dbCommand, "@iActive", DbType.Boolean, equipment.Active);
                db.AddInParameter(dbCommand, "@iBankReference", DbType.String, equipment.BankReference);
                db.AddInParameter(dbCommand, "@iCredit", DbType.Decimal, equipment.Credit);
                db.AddInParameter(dbCommand, "@iMaintenanceTypeID", DbType.Int32, equipment.MaintenanceTypeID);
                db.AddInParameter(dbCommand, "@iEquipmentReferenceID", DbType.Int32, equipment.EquipmentReferenceID);
                db.AddInParameter(dbCommand, "@iAreaID", DbType.Int32, equipment.AreaID);
                db.AddInParameter(dbCommand, "@iSavID", DbType.Int32, equipment.SavID);
                db.AddInParameter(dbCommand, "@iColor", DbType.String, equipment.Color);
                db.AddInParameter(dbCommand, "@iOS_ID", DbType.Int32, equipment.OS_ID);
                db.AddInParameter(dbCommand, "@iModelVisitor", DbType.String, equipment.ModelVisitor);
                db.AddInParameter(dbCommand, "@iMarkVisitor", DbType.String, equipment.MarkVisitor);
                db.AddInParameter(dbCommand, "@iUseVisitor", DbType.String, equipment.UseVisitor);
                db.AddInParameter(dbCommand, "@iCompanyVisitor", DbType.String, equipment.CompanyVisitor);
                db.AddInParameter(dbCommand, "@iAvailabilityTarget", DbType.Decimal, equipment.AvailabilityTarget);
                db.AddInParameter(dbCommand, "@iIsLeasing", DbType.Boolean, equipment.IsLeasing);
                db.AddInParameter(dbCommand, "@iLeasingStart", DbType.DateTime, equipment.LeasingStart);
                db.AddInParameter(dbCommand, "@iLeasingEnd", DbType.DateTime, equipment.LeasingEnd);
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

        public GenericReturn Delete(int? EquipmentID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[SG].[Equipment_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iEquipmentID", DbType.Int32, EquipmentID);
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
