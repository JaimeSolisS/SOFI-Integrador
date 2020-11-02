using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class DemoldDefectChartsService
    {
        private static DemoldDefectChartsRepository _rep;

        static DemoldDefectChartsService()
        {
            _rep = new DemoldDefectChartsRepository();
        }

        public static t_DemoldDefectsChartData GetDataCharts(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs, DateTime? StartDate, DateTime? EndDate, string DefectType, GenericRequest request)
        {
            t_DemoldDefectsChartData result = new t_DemoldDefectsChartData();
            List<t_DemoldDefectsCharts> List = new List<t_DemoldDefectsCharts>();

            using (DataSet ds = _rep.List(ProductionLineIDs, MoldFamilyIDs, ShiftIDs, StartDate, EndDate, DefectType, request))
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    List.Add(new t_DemoldDefectsCharts
                    {
                        label = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("label")).ToArray(),
                        yAxisID = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("yAxisID")).FirstOrDefault(),
                        type = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("type")).FirstOrDefault(),
                        borderColor = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("type")).FirstOrDefault() == "bar" ? ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("backgroundColor")).FirstOrDefault() : ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("borderColor")).FirstOrDefault(),
                        backgroundColor = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("backgroundColor")).ToArray(),
                        borderWidth = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("type")).FirstOrDefault() == "line" ? "5" : "0",
                        data = ds.Tables[i].AsEnumerable().Select(r => r.Field<decimal?>("ydata")).ToArray(),
                        Category = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("Category")).FirstOrDefault(),
                        IndexValue = ds.Tables[i].AsEnumerable().Select(r => r.Field<int>("IndexValue")).FirstOrDefault(),
                        FontColor = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("FontColor")).FirstOrDefault()

                    });
                }

                result.SeriesList = List;
                return result;
            }
        }

        public static t_DemoldDefectsCharts GetBarChartData(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs, DateTime? StartDate, DateTime? EndDate, string DefectType, int? DesignID, GenericRequest request)
        {
            using (DataTable dt = _rep.BarChartData(ProductionLineIDs.SelectedValue(), MoldFamilyIDs.SelectedValue(), ShiftIDs.SelectedValue(), StartDate, EndDate, DefectType, DesignID.SelectedValue(), request))
            {
                List<t_DemoldDefectsCharts> _list = dt.ConvertToList<t_DemoldDefectsCharts>();
                t_DemoldDefectsCharts result = new t_DemoldDefectsCharts(); 

                if (_list != null && _list.Any())
                {
                    result = _list.FirstOrDefault();
                    result.data = _list.Select(s => s.ydata).ToArray();
                    result.label = _list.Select(s => s.labelData).ToArray();
                    result.backgroundColor = _list.Select(s => s.backgroundColorData).ToArray();
                    result.Gross = _list.Select(s => s.GrossData).ToArray();
                }
                return result;
            }
        }
        public static t_DemoldDefectsCharts GetPieChartData(string ProductionLineIDs, string MoldFamilyIDs, string ShiftIDs, DateTime? StartDate, DateTime? EndDate, string DefectType, int? DesignID, GenericRequest request)
        {
            using (DataTable dt = _rep.PieChartData(ProductionLineIDs, MoldFamilyIDs, ShiftIDs, StartDate, EndDate, DefectType, DesignID.SelectedValue(), request))
            {
                List<t_DemoldDefectsCharts> _list = dt.ConvertToList<t_DemoldDefectsCharts>();
                t_DemoldDefectsCharts result = new t_DemoldDefectsCharts();
                if (result != null && _list.Any())
                {
                    result = _list.FirstOrDefault();
                    result.data = _list.Select(s => s.ydata).ToArray();
                    result.label = _list.Select(s => s.labelData).ToArray();
                    result.backgroundColor = _list.Select(s => s.backgroundColorData).ToArray();
                }
                return result;
            }
        }
    }
}
