using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;

namespace Core.Service
{
    public class GenericChartFilterDataService
    {
        private static GenericChartFilterDataRepository _rep;
        static GenericChartFilterDataService()
        {
            _rep = new GenericChartFilterDataRepository();
        }

        public static List<GenericChartFilterData> List(int? GenericChartFilterDataID, int? GenericChartHeaderDataID, int? GenericChartFilterID, int? GenericChartID, string FilterValue, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GenericChartFilterDataID, GenericChartHeaderDataID, GenericChartFilterID, GenericChartID, FilterValue, request))
            {
                List<GenericChartFilterData> _list = dt.ConvertToList<GenericChartFilterData>();
                return _list;
            }
        }

        public static List<GenericChartFilterData> List(int? GenericChartHeaderDataID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, GenericChartHeaderDataID, null, null, null, request))
            {
                List<GenericChartFilterData> _list = dt.ConvertToList<GenericChartFilterData>();
                return _list;
            }
        }

        public static List<GenericChartFilterData> List(int? GenericChartFilterID, string Aux, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, GenericChartFilterID, null, null, request))
            {
                List<GenericChartFilterData> _list = dt.ConvertToList<GenericChartFilterData>();
                return _list;
            }
        }

    }
}
