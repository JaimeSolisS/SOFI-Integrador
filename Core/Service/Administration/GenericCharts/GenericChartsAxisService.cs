using Core.Data;
using Core.Entities;

namespace Core.Service
{
    public class GenericChartsAxisService
    {
        private static GenericChartsAxisRepository _rep;
        static GenericChartsAxisService()
        {
            _rep = new GenericChartsAxisRepository();
        }

        public static GenericReturn Update(int? GenericChartAxisID, int? GenericChartID, string AxisName, int? AxisTypeID, int? AxisChartTypeID, int? AxisDatatypeID, string AxisColor, string AxisFormat, GenericRequest request)
        {
            return _rep.Update(GenericChartAxisID, GenericChartID, AxisName, AxisTypeID, AxisChartTypeID, AxisDatatypeID, AxisColor, AxisFormat, request);
        }

        public static GenericReturn Delete(int? GenericChartAxisID, GenericRequest request)
        {
            return _rep.Delete(GenericChartAxisID, request);
        }

        public static GenericReturn Insert(int? GenericChartID, string AxisName, int? AxisTypeID, int? AxisChartTypeID, int? AxisDatatypeID, string AxisColor, string AxisFormat, GenericRequest request)
        {
            return _rep.Insert(GenericChartID, AxisName, AxisTypeID, AxisChartTypeID, AxisDatatypeID, AxisColor, AxisFormat, request);
        }

        public static GenericReturn UpdateAxisName(int? GenericChartAxisID, string AxisName, GenericRequest request)
        {
            return _rep.Update(GenericChartAxisID, null, AxisName, null, null, null, null, null, request);
        }
    }
}
