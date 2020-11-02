using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class UsersProfilesService
    {
        private static UsersProfiles_Repository _rep;

        static UsersProfilesService()
        {
            _rep = new UsersProfiles_Repository();
        }

        public static List<UsersProfile> List(int? ProfileID, int? ChangedBy, GenericRequest request)
        {
            using (DataTable dt = _rep.List(ProfileID, ChangedBy, request))
            {
                List<UsersProfile> _list = dt.ConvertToList<UsersProfile>();
                return _list;
            }
        }
        public static UsersProfile GetProfile(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, request))
            {
                List<UsersProfile> _list = dt.ConvertToList<UsersProfile>();
                return _list.FirstOrDefault();
            }
        }
    }
}
