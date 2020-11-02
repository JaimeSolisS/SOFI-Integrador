using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class GenericChartsFiltersService
    {
        private static GenericChartsFiltersRepository _rep;
        static GenericChartsFiltersService()
        {
            _rep = new GenericChartsFiltersRepository();
        }

        public static List<GenericChart> List(int? GenericChartFilterID, int? GenericChartID, string FilterName, int? FilterTypeID, int? FilterListID, string DefaultValue, int? DefaultValueFormula, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, GenericChartID, FilterName, FilterTypeID, FilterListID, DefaultValue, DefaultValueFormula, Enabled, request))
            {
                List<GenericChart> _list = dt.ConvertToList<GenericChart>();
                return _list;
            }
        }

        public static List<GenericChartsFilters> List(int GenericChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, GenericChartID, null, null, null, null, null, null, request))
            {
                List<GenericChartsFilters> _list = dt.ConvertToList<GenericChartsFilters>();
                return _list;
            }
        }

        public static List<GenericChartsFilters> List(int? GenericChartFilterID, int? GenericChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GenericChartFilterID, null, null, null, null, null, null, null, request))
            {
                List<GenericChartsFilters> _list = dt.ConvertToList<GenericChartsFilters>();
                return _list;
            }
        }

        public static GenericChartsFilters GetDefault(int GenericChartID, int FilterListID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, GenericChartID, null, null, FilterListID, null, null, null, request))
            {
                return dt.ConvertToList<GenericChartsFilters>().FirstOrDefault();
            }
        }

        public static GenericReturn Insert(int? GenericChartID, string FilterName, int? FilterTypeID, int? FilterListID, string DefaultValue, int? DefaultValueFormula, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(GenericChartID, FilterName, FilterTypeID, FilterListID, DefaultValue, DefaultValueFormula, Enabled, request);
        }

        public static GenericReturn Update(int? GenericChartFilterID, int? GenericChartID, string FilterName, int? FilterTypeID, int? FilterListID, string DefaultValue, int? DefaultValueFormula, bool? Enabled, GenericRequest request)
        {
            return _rep.Update(GenericChartFilterID, GenericChartID, FilterName, FilterTypeID, FilterListID, DefaultValue, DefaultValueFormula, Enabled, request);
        }

        public static GenericReturn Delete(int? GenericChartFilterID, GenericRequest request)
        {
            return _rep.Delete(GenericChartFilterID, request);
        }
    }
}
