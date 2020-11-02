using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;

namespace Core.Service
{
    public class LensTypesService
    {
        private static LensTypesRepository _rep;

        static LensTypesService()
        {
            _rep = new LensTypesRepository();
        }

        public static List<LenType> List(int? LensTypeID, string LensTypeName, int? ProductionDesignID, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(LensTypeID, LensTypeName, ProductionDesignID, Enabled, request))
            {
                List<LenType> _list = dt.ConvertToList<LenType>();
                return _list;
            }
        }

        public static List<LenType> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, request))
            {
                List<LenType> _list = dt.ConvertToList<LenType>();
                return _list;
            }
        }

    }
}
