using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class GenericChartDataService
    {
        private static GenericChartDataRepository _rep;

        static GenericChartDataService()
        {
            _rep = new GenericChartDataRepository();
        }

        public static List<string> Get(int? GenericChartHeaderDataID, GenericRequest request)
        {
            List<string> data = new List<string>();
            using (DataTable dt = _rep.Get(GenericChartHeaderDataID, request))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data.Add(dt.Rows[i].Field<string>("Value"));
                }
            };
            return data;
        }

        public static GenericReturn Insert(string FitlerInfo, int GenericChartID, List<GenericChartData> GenericChartData, GenericRequest request)
        {
            using (DataTable dt = GenericChartData.Select(x => new
            {
                x.GenericChartHeaderDataID,
                x.Field1,
                x.Field2,
                x.Field3,
                x.Field4,
                x.Field5,
                x.Field6,
                x.Field7,
                x.Field8,
                x.Field9,
                x.Field10,
                x.Field11,
                x.Field12,
                x.Field13,
                x.Field14,
                x.Field15,
                x.Field16,
                x.Field17,
                x.Field18,
                x.Field19,
                x.Field20,
            }).ToList().ConvertToDataTable())
            {
                return _rep.Insert(FitlerInfo, GenericChartID, dt, request);
            }
        }

        public static List<GenericChartData> List(int? GenericChartID, string ValuesFromFilters, GenericRequest request)
        {
            using (DataTable dt = _rep.List(GenericChartID, null, null, ValuesFromFilters, request))
            {
                List<GenericChartData> _list = dt.ConvertToList<GenericChartData>();
                return _list;
            }
        }

        public static List<GenericChartData> ListFromDynamicFilters(string FilterData, GenericRequest request)
        {
            using (DataTable dt = _rep.ListFromDynamicFilters(FilterData, request))
            {
                List<GenericChartData> _list = dt.ConvertToList<GenericChartData>();
                return _list;
            }
        }

        public static int GET_GenericChartHeaderDataID(int? GenericChartID, int? GenericChartFilterID, GenericRequest request)
        {
            using (DataTable dt = _rep.GenericChartHeaderData_List(GenericChartID, GenericChartFilterID, request))
            {
                List<GenericChartHeaderData> _list = dt.ConvertToList<GenericChartHeaderData>();
                return _list.FirstOrDefault().GenericChartHeaderDataID;
            }
        }

        public static List<GenericChartHeaderData> GET_GenericChartHeaderDataIDForChart(int GenericChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.GenericChartHeaderData_List(null, GenericChartID, request))
            {
                List<GenericChartHeaderData> _list = dt.ConvertToList<GenericChartHeaderData>();
                return _list;
            }
        }

    }
}
