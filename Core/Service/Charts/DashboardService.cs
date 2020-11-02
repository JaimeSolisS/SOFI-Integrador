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
    public class DashboardService
    {
        private static DashboardRepository _rep;

        static DashboardService()
        {
            _rep = new DashboardRepository();
        }

        public static List<t_ChartData> ASNChart(DateTime? ChartDate, int ProductionProcessID,int LineID, int ShiftID, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_ASNChart(ChartDate, ProductionProcessID,LineID, ShiftID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
            
        }

        public static List<t_ChartData> ProductionChart(DateTime? ChartDate, int ProductionProcessID,int LineID,int? VAID ,int? DesignID, int? ShiftID, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_ProductionChart(ChartDate, ProductionProcessID,LineID,VAID ,DesignID.SelectedValue(), ShiftID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static t_YieldDefectChartData YieldDefectsChart(DateTime? ChartDate, int ProductionProcessID, int? LineID, int? VA, int? DesignID, int? ShiftID, int? Top, GenericRequest request)
        {
            t_YieldDefectChartData result = new t_YieldDefectChartData();
            List<t_SeriesChart> List = new List<t_SeriesChart>();
            using (DataSet ds = _rep.Dashboard_YieldDefectsChart(ChartDate, ProductionProcessID, LineID.SelectedValue(), VA.SelectedValue(), DesignID.SelectedValue(), ShiftID, Top.SelectedValue(), request.FacilityID, request.UserID, request.CultureID))
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    List.Add(new t_SeriesChart
                    {
                        label = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("label")).FirstOrDefault(),
                        yAxisID = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("yAxisID")).FirstOrDefault(),
                        type = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("type")).FirstOrDefault(),
                        borderColor = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("type")).FirstOrDefault() == "bar" ? ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("backgroundColor")).FirstOrDefault() : ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("borderColor")).FirstOrDefault(),
                        backgroundColor = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("backgroundColor")).FirstOrDefault(),
                        borderWidth = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("type")).FirstOrDefault() == "line" ? "5" : "0",
                        pointBorderColor = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("FontColor")).FirstOrDefault(),
                        data = ds.Tables[i].AsEnumerable().Select(r => r.Field<decimal?>("ydata")).ToArray()
                    });
                }

                result.SeriesList = List;
                return result;
            }
        }

        public static List<t_ChartData> TopDefectsChart(DateTime? ChartDate, int ProductionProcessID, int? LineID, int? VA, int? DesignID, List<int> DefectsIDs, int? ShiftID, GenericRequest request)
        {
            string Defects = null;
            if (DefectsIDs != null)
            {
                Defects = string.Join<int>(",", DefectsIDs);
            }
            using (DataTable dt = _rep.Dashboard_TopDefectsChart(ChartDate, ProductionProcessID, LineID.SelectedValue(), VA.SelectedValue(), DesignID.SelectedValue(), Defects, ShiftID.SelectedValue(), request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static List<t_ChartData> DefectChart(DateTime? ChartDate, int ProductionProcessID, int? LineID, int? VA, int? DesignID, int? ShiftID, string Defect, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_DefectChart(ChartDate, ProductionProcessID, LineID.SelectedValue(), VA.SelectedValue(), DesignID.SelectedValue(), ShiftID, Defect, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static List<t_ChartData> MoldScrapChart(DateTime? ChartDate, int ProductionProcessID, int? LineID,int? DesignID, int ShiftID, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_MoldScrapChart(ChartDate, ProductionProcessID, LineID.SelectedValue(), DesignID.SelectedValue(), ShiftID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static List<t_ChartData> GetShiftHours(int? ShiftID, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_GetShiftHours(ShiftID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static List<t_ChartData> GetYieldGoal(DateTime? ChartDate, int? ProductionProcessID, int? LineID, int? VA, int? DesignID, int? ShiftID, int? Top, GenericRequest request)
        {
		    
            using (DataTable dt = _rep.Dashboard_GetYieldGoal(ChartDate, ProductionProcessID, LineID.SelectedValue(), VA.SelectedValue(), DesignID.SelectedValue(), ShiftID.SelectedValue(), Top.SelectedValue(), request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static int GetCurrentShiftID( int DefaultValue, GenericRequest request)
        {
            return _rep.GetCurrentShiftID(request.FacilityID,request.UserID,request.CultureID, DefaultValue);
        }
        public static string GetYieldChartTitle(DateTime? ChartDate,int ProductionProcessID, int? LineID, int? VAID, int? DesignID, int ShiftID,string DefaultValue, GenericRequest request)
        {
            return _rep.GetYieldChartTitle(ChartDate, ProductionProcessID, LineID.SelectedValue(), VAID.SelectedValue(), DesignID.SelectedValue(), ShiftID,request.FacilityID, request.UserID, request.CultureID, DefaultValue);
        }


        public static List<t_TicketsChartData> TicketsChart(int? Year, GenericRequest request)
        {
            List<t_TicketsChartData> List = new List<t_TicketsChartData>();
            using (DataSet ds = _rep.Dashboard_TicketsChart(Year,  request.FacilityID, request.UserID, request.CultureID))
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    List.Add(new t_TicketsChartData
                    {
                        name = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("label")).FirstOrDefault(),
                        data = ds.Tables[i].AsEnumerable().Select(r => r.Field<int?>("ydata")).ToArray()
                    });
                }

                return List;
            }
        }
        public static List<t_ProjectChartData> ProjectsChart(int? Year,int? StatusID, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_ProjectsChart(Year, StatusID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ProjectChartData> _list = dt.ConvertToList<t_ProjectChartData>();
                return _list;
            }
        }
        public static List<t_ServicesChartData> ServicesChart(int? Year, GenericRequest request)
        {
            List<t_ServicesChartData> List = new List<t_ServicesChartData>();
            using (DataSet ds = _rep.Dashboard_ServicesChart(Year, request.FacilityID, request.UserID, request.CultureID))
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    List.Add(new t_ServicesChartData
                    {
                        name = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("label")).FirstOrDefault(),
                        data = ds.Tables[i].AsEnumerable().Select(r => r.Field<decimal?>("ydata")).ToArray()
                    });
                }

                return List;
            }
        }

        public static List<t_ChartData> GetYearMonths(int? Year, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_GetYearMonths(Year, request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }
        public static List<t_ChartData> TopTicketsChart(int? Year, int? Month, int? Top, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_TopTicketsChart(Year, Month.SelectedValue(), Top.SelectedValue(), request.FacilityID, request.UserID, request.CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }
    }
}
