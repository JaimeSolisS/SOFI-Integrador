using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class MNT_EnergySensorValuesService
    {
        private static MNT_EnergySensorValuesRepository _rep;

        static MNT_EnergySensorValuesService()
        {
            _rep = new MNT_EnergySensorValuesRepository();
        }

        public static GenericReturn Insert(int? EnergySensorID, List<EnergySensorValue> AlamrConfiguration, GenericRequest request)
        {
            using (DataTable dt = AlamrConfiguration.Select(x => new { x.ValueHour, x.MaxValue }).ToList().ConvertToDataTable())
            {
                {
                    return _rep.Insert(EnergySensorID, dt, request);
                }
            }
        }

        public static List<EnergySensorValue> List(int? EnergySensorID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(EnergySensorID, request))
            {
                List<EnergySensorValue> _list = dt.ConvertToList<EnergySensorValue>();
                return _list;
            }
        }
    }
}
