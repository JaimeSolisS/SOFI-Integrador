using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

using System.Data;
using Core.Entities.Utilities;

namespace Core.Service
{
    public static class ProfileService
    {
        private static ProfileRepository _rep;

        static ProfileService()
        {
            _rep = new ProfileRepository();
        }

        public static GenericReturn Upsert(Profile entity, string MenusIDs, GenericRequest request)
        {
            return _rep.ProfilesUpsert(entity.ProfileID, entity.ProfileName, entity.OrganizationID, MenusIDs, request.UserID,request.FacilityID, entity.ProductionProcessRequired, request.CultureID);
        }

        public static GenericReturn Delete(int ProfileID, GenericRequest request)
        {
            return _rep.ProfilesDelete(ProfileID, request.UserID, request.CultureID);
        }

        public static List<Profile> List(Profile entity, GenericRequest request, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.ProfilesList(entity.ProfileID, entity.ProfileName, entity.OrganizationID, request.UserID, request.FacilityID, request.CultureID, entity.DefaultMenuID,  entity.ProductionProcessRequired))
            {
                List<Profile> EntitiesList = dt.ConvertToList<Profile>();
                if(AddEmptyRecord)
                    EntitiesList.Insert(0, new Profile() { ProfileID = 0, ProfileName = "" });

                return EntitiesList;
            }            
        }
        public static Profile Get(int ProfileID,GenericRequest request)
        {
            using (DataTable dt = _rep.ProfilesList(ProfileID, null, null, request.UserID, request.FacilityID, request.CultureID, null, null))
            {
                List<Profile> EntitiesList = dt.ConvertToList<Profile>();
                return EntitiesList.FirstOrDefault();
            }
        }

        public static List<AppMenu> GetProfilesMenus(string ProfileArrayID, int UserID, string CultureID)
        {
            List<AppMenu> EntitiesList = new List<AppMenu>();
            foreach (DataRow r in _rep.GetProfilesMenus(ProfileArrayID, UserID, CultureID).Rows)
            {
                EntitiesList.Add(new AppMenu
                {
                    MenuID = Int32.Parse(r["MenuID"].ToString()),
                    ParentMenuID = Int32.Parse(r["ParentMenuID"].ToString() == "" ? "0" : r["ParentMenuID"].ToString()),
                    Description = r["Description"].ToString(),
                    Enabled = (r["Enabled"].ToString() == "1" ? true : false),
                    NavigateTo = r["NavigateTo"].ToString(),
                    Sequence = Int32.Parse(r["Sequence"].ToString())
                });
            }
            return EntitiesList;
        }

        public static List<AppEvents> GetProfilesEvents(int ProfileID, int MenuID, int UserID, string CultureID)
        {
            List<AppEvents> EntitiesList = new List<AppEvents>();
            using (DataTable dt = _rep.GetProfilesEvents(ProfileID, MenuID, UserID, CultureID))
            {
                EntitiesList = dt.ConvertToList<AppEvents>();
            }
            return EntitiesList;
        }
        public static GenericReturn ProfilesEventsUpdate(int ProfileID, int MenuID, string EventsIDs, int UserID, string CultureID)
        {
            return _rep.ProfilesEventsUpdate(ProfileID, MenuID, EventsIDs, UserID, CultureID);
        }
    }
}
