using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class GenericChartService
    {
        private static GenericChartRepository _rep;
        static GenericChartService()
        {
            _rep = new GenericChartRepository();
        }

        public static List<GenericChart> List(int? GenericChartID, string ChartName, string ChartTitle, int? ChartAreaID, int? ChartTypeID, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GenericChartID, ChartName, ChartTitle, ChartAreaID, ChartTypeID, Enabled, request))
            {
                List<GenericChart> _list = dt.ConvertToList<GenericChart>();
                return _list;
            }
        }

        public static List<GenericChart> List(int ChartAreaID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, ChartAreaID, null, null, request))
            {
                List<GenericChart> _list = dt.ConvertToList<GenericChart>();
                return _list;
            }
        }

        public static List<GenericChart> List(int GenericChartID, bool? Enable, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GenericChartID, null, null, null, null, null, request))
            {
                List<GenericChart> _list = dt.ConvertToList<GenericChart>();
                return _list;
            }
        }

        public static List<GenericChart> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, request))
            {
                List<GenericChart> _list = dt.ConvertToList<GenericChart>();
                return _list;
            }
        }

        public static List<GenericChart> List(int? ChartAreaID, int? GenericChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GenericChartID, null, null, ChartAreaID, null, null, request))
            {
                List<GenericChart> _list = dt.ConvertToList<GenericChart>();
                return _list;
            }
        }


        public static GenericReturn Delete(int GenericChartID, GenericRequest request)
        {
            return _rep.Delete(GenericChartID, request);
        }

        public static GenericReturn Insert(int? GenericChartHeaderDataID, int? GenericChartID, List<GenericChart> GenericCharts,
            List<GenericChartsFilters> GenericChartsFilters, List<GenericChartsAxis> GenericChartsAxes, GenericRequest request)
        {
            using (DataTable dt = GenericCharts.Select(x => new
            {
                x.GenericChartID,
                x.ChartName,
                x.ChartTitle,
                x.ChartAreaID,
                x.ChartTypeID,
                x.Enabled,
            }).ToList().ConvertToDataTable())
            {
                using (DataTable dt2 = GenericChartsFilters.Select(x => new
                {
                    x.GenericChartFilterID,
                    x.GenericChartID,
                    x.FilterName,
                    x.FilterTypeID,
                    x.FilterListID,
                    x.DefaultValue,
                    x.DefaultValueFormula,
                    x.Enabled
                }).ToList().ConvertToDataTable())
                {
                    using (DataTable dt3 = GenericChartsAxes.Select(x => new
                    {
                        x.GenericChartAxisID,
                        x.GenericChartID,
                        x.AxisName,
                        x.AxisTypeID,
                        x.AxisChartTypeID,
                        x.AxisDatatypeID,
                        x.AxisColor,
                        x.AxisFormat,
                        x.ShowLine,
                        x.DataLabelRotation,
                        x.DataLabelShow,
                        x.DataLabelFontSize,
                        x.DataLabelFontColor,
                        x.DataLabelFontBGColor
                    }).ToList().ConvertToDataTable())
                    {
                        return _rep.Insert(GenericChartHeaderDataID, GenericChartID, dt, dt2, dt3, request);
                    }
                }
            }
        }

        public static List<GenericChartsAxis> GetAxes_List(int? GenericChartAxisID, int? GenericChartID, string AxisName, int? AxisTypeID, int? AxisChartTypeID, int? AxisDatatypeID, string AxisColor, string AxisFormat, GenericRequest request)
        {
            using (DataTable dt = _rep.GetAxes_List(GenericChartAxisID, GenericChartID, AxisName, AxisTypeID, AxisChartTypeID, AxisDatatypeID, AxisColor, AxisFormat, request))
            {
                List<GenericChartsAxis> _list = dt.ConvertToList<GenericChartsAxis>();
                return _list;
            }
        }

        public static List<GenericChartsAxis> GetAxes_List(int? GenericChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetAxes_List(null, GenericChartID, null, null, null, null, null, null, request))
            {
                List<GenericChartsAxis> _list = dt.ConvertToList<GenericChartsAxis>();
                return _list;
            }
        }

    }
}
