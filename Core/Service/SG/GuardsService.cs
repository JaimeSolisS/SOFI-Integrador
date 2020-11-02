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
    public class GuardsService
    {
        private static GuardsRepository _rep;
        static GuardsService()
        {
            _rep = new GuardsRepository();
        }

        public static List<Guard> List(string StatusIDs, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, StatusIDs, request))
            {
                List<Guard> _list = dt.ConvertToList<Guard>();
                return _list;
            }
        }
        public static List<Guard> List(int? GuardID, string GuardName, string UniqueNumber, string StatusIDs, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GuardID, GuardName, UniqueNumber, StatusIDs, request))
            {
                List<Guard> _list = dt.ConvertToList<Guard>();
                return _list;
            }
        }

        public static Guard GetByCode(string UniqueNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, UniqueNumber, "1", request))
            {
                List<Guard> _list = dt.ConvertToList<Guard>();
                if (_list.Count > 0)
                {
                    return _list.FirstOrDefault();
                }
                else
                {
                    return new Guard();
                }
            }
        }

        public static GenericReturn Insert(string GuardName, GenericRequest request)
        {
            return _rep.Insert(GuardName, request);
        }

        public static GenericReturn Update(int? GuardID, string GuardName, string UniqueNumber, bool? Enabled, GenericRequest request)
        {
            return _rep.Update(GuardID, GuardName, UniqueNumber, Enabled, request);
        }

        public static GenericReturn EnableDisable(int GuardID, bool Enabled, GenericRequest request)
        {
            return _rep.Update(GuardID, null, null, Enabled, request);
        }

        public static bool ValidateGuardCode(string UniqueNumber)
        {
            return _rep.ValidateGuardCode(UniqueNumber);
        }
    }
}
