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
    public class BadgesService
    {
        private static BadgesRepository _rep;
        static BadgesService()
        {
            _rep = new BadgesRepository();
        }

        public static List<Badge> List(GenericRequest request)
        {
            var SecurityBadgesTypesList = vw_CatalogService.List("SecurityBadgesTypes", request);
            var SecurityBadgesTypes = "";
            if (SecurityBadgesTypesList.Count() > 0)
            {
                SecurityBadgesTypes = string.Join(",", SecurityBadgesTypesList.Select(c => c.CatalogDetailID));
            }
            using (DataTable dt = _rep.List(null, null, null, SecurityBadgesTypes, request))
            {
                List<Badge> _list = dt.ConvertToList<Badge>();
                return _list;
            }
        }

        public static List<Badge> List(int? BadgeID, string BadgeNumber, string UniqueNumber, string badgeTypeIDs, GenericRequest request)
        {
            using (DataTable dt = _rep.List(BadgeID, BadgeNumber, UniqueNumber, badgeTypeIDs, request))
            {
                List<Badge> _list = dt.ConvertToList<Badge>();
                return _list;
            }
        }

        public static GenericReturn Insert(string BadgeNumber, int? BadgeTypeID, GenericRequest request)
        {
            return _rep.Insert(BadgeNumber, BadgeTypeID, request);
        }

        public static GenericReturn Update(int? BadgeID, string BadgeNumber, string UniqueNumber, int? BadgeTypeID, GenericRequest request)
        {
            return _rep.Update(BadgeID, BadgeNumber, UniqueNumber, BadgeTypeID, request);
        }

        public static GenericReturn UpdateBadgeNumber(int? BadgeID, string BadgeNumber, string UniqueNumber, GenericRequest request)
        {
            return _rep.Update(BadgeID, BadgeNumber, UniqueNumber, null, request);
        }

        public static GenericReturn Delete(int? BadgeID, GenericRequest request)
        {
            return _rep.Delete(BadgeID, request);
        }

    }
}
