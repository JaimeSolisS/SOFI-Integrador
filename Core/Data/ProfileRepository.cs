using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class ProfileRepository : GenericRepository
    {
        public DataTable GetProfilesEvents(int ProfileID, int MenuID, int UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("GetProfilesEvents");
            try
            {
                // Parameters                
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iMenuID", DbType.Int32, MenuID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            {
                dbCommand.Dispose();
            }

            return dt;
        }

        public DataTable ProfilesList(int? ProfileID, string ProfileName, int? OrganizationID, int UserID, int FacilityID, string CultureID, int? DefaultMenuID, bool? ProductionProcessRequired)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Profiles_list");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iProfileName", DbType.String, ProfileName);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, OrganizationID);
                db.AddInParameter(dbCommand, "@iDefaultMenuID", DbType.Int32, DefaultMenuID);
                db.AddInParameter(dbCommand, "@iProductionProcessRequired", DbType.Binary, ProductionProcessRequired);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            {
                dbCommand.Dispose();
            }

            return dt;
        }

        public DataTable ProfilesListToExcel(int? ProfileID, string ProfileName, int? OrganizationID, int UserID, int FacilityID, string CultureID, int? DefaultMenuID, bool? ProductionProcessRequired)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Profiles_List_ExportToExcel");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iProfileName", DbType.String, ProfileName);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, OrganizationID);
                db.AddInParameter(dbCommand, "@iDefaultMenuID", DbType.Int32, DefaultMenuID);
                db.AddInParameter(dbCommand, "@iProductionProcessRequired", DbType.Binary, ProductionProcessRequired);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            {
                dbCommand.Dispose();
            }

            return dt;
        }

        public GenericReturn ProfilesUpsert(int? ProfileID, string ProfileName, int? OrganizationID, string MenusIDs, int UserID,int FacilityID,  bool? ProductionProcessRequired, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Profiles_Upsert");
            try
            {
                // Parameters                
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iProfileName", DbType.String, ProfileName);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, OrganizationID);
                db.AddInParameter(dbCommand, "@iMenusIDs", DbType.String, MenusIDs);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iProductionProcessRequired", DbType.Boolean, ProductionProcessRequired);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oProfileID", DbType.Int32, 0);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oProfileID");
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
        public GenericReturn ProfilesDelete(int ProfileID, int UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Profiles_Delete");
            try
            {
                // Parameters                
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
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
        public DataTable GetProfilesMenus(string ProfileArrayID, int UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("GetProfilesMenus");
            try
            {
                // Parameters                
                db.AddInParameter(dbCommand, "@iProfileArrayID", DbType.String, ProfileArrayID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            {
                dbCommand.Dispose();
            }

            return dt;
        }
        public GenericReturn ProfilesEventsUpdate(int ProfileID, int MenuID, string EventsIDs, int UserID, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("ProfilesEventsUpdate");
            try
            {
                // Parameters                
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iMenuID", DbType.Int32, MenuID);
                db.AddInParameter(dbCommand, "@iEventsIDs", DbType.String, EventsIDs);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
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
    }
}
