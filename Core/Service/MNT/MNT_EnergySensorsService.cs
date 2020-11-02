using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Core.Service
{
    public class MNT_EnergySensorsService
    {
        private static MNT_EnergySensorsRepository _rep;

        static MNT_EnergySensorsService()
        {
            _rep = new MNT_EnergySensorsRepository();
        }

        public static GenericReturn Insert(int? EnergySensorID, string SensorName, int? EnergySensorFamilyID, int? SensorUseID, int? UnitID, int? IndexKey, string Deviceid, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(EnergySensorID, SensorName, EnergySensorFamilyID, SensorUseID, UnitID, IndexKey, Deviceid, Enabled, request);
        }

        public static IEnumerable<string> NamesList(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, null, null, req))
            {
                List<EnergySensors> _list = dt.ConvertToList<EnergySensors>();
                var newList = _list.Select(x => x.SensorName).Distinct();
                return newList;
            }
        }

        public static List<EnergySensors> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, null, null, req))
            {
                List<EnergySensors> _list = dt.ConvertToList<EnergySensors>();
                return _list;
            }
        }


        public static List<EnergySensors> List(int? EnergySensorID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(EnergySensorID, null, null, null, req))
            {
                List<EnergySensors> _list = dt.ConvertToList<EnergySensors>();
                return _list;
            }
        }

        public static List<EnergySensors> DashboardList(int? EnergySensorFamilyID, DateTime SensorDate, int SensorHour, GenericRequest req)
        {
            using (DataTable dt = _rep.DashboardList(EnergySensorFamilyID, SensorDate, SensorHour, req))
            {
                List<EnergySensors> _list = dt.ConvertToList<EnergySensors>();
                return _list;
            }
        }

        public static List<EnergySensorFamiliesGauge> GetGaugeValues(DateTime? SensorDate, int? SensorHour, GenericRequest request)
        {
            using (DataTable dt = _rep.GetGaugeValues(SensorDate, SensorHour, request))
            {
                List<EnergySensorFamiliesGauge> _list = dt.ConvertToList<EnergySensorFamiliesGauge>();
                return _list;
            }
        }

        public static List<EnergySensors> List(string[] SensorFamiliesIDs, string[] SensorNames, string[] SensorUsesIDs, GenericRequest req)
        {
            string SensorFamilies = null;
            string SensorNamesString = null;
            string SensorUses = null;

            if (SensorFamiliesIDs != null)
                SensorFamilies = string.Join<string>(",", SensorFamiliesIDs);

            if (SensorNames != null)
                SensorNamesString = string.Join<string>(",", SensorNames);

            if (SensorUsesIDs != null)
                SensorUses = string.Join<string>(",", SensorUsesIDs);


            using (DataTable dt = _rep.List(null, SensorFamilies, SensorNamesString, SensorUses, req))
            {
                List<EnergySensors> _list = dt.ConvertToList<EnergySensors>();
                return _list;
            }
        }

        public static List<EnergySensors> List(string[] SensorFamiliesIDs, GenericRequest req)
        {
            string SensorFamilies = null;
            string SensorNamesString = null;
            string SensorUses = null;

            if (SensorFamiliesIDs != null)
                SensorFamilies = string.Join<string>(",", SensorFamiliesIDs);

            using (DataTable dt = _rep.List(null, SensorFamilies, SensorNamesString, SensorUses, req))
            {
                List<EnergySensors> _list = dt.ConvertToList<EnergySensors>();
                return _list;
            }
        }


        public static GenericReturn Delete(int? EnergySensorID, GenericRequest request)
        {
            return _rep.Delete(EnergySensorID, request);
        }

        public static GenericReturn Update(int? EnergySensorID, string SensorName, int? EnergySensorFamilyID, int? SensorUseID, int? UnitID, int? IndexKey, string Deviceid, bool? Enabled, GenericRequest request)
        {
            return _rep.Update(EnergySensorID, SensorName, EnergySensorFamilyID, SensorUseID, UnitID, IndexKey, Deviceid, Enabled, request);
        }

        public static t_YieldDefectChartData GetDataForConsumptionDayChart(int EnergySensorID, DateTime? Date, GenericRequest request)
        {
            t_YieldDefectChartData result = new t_YieldDefectChartData();
            List<t_SeriesChart> List = new List<t_SeriesChart>();

            using (DataSet ds = _rep.GetDataForConsumptionDayChart(EnergySensorID, Date, request))
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
                        data = ds.Tables[i].AsEnumerable().Select(r => r.Field<decimal?>("ydata")).ToArray()
                    });
                }

                result.SeriesList = List;
                return result;
            }
        }

        public static DataSet GetDataForConsumptionGlobalChart(int EnergySensorID, int EnergySensorFamilyID, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            return _rep.GetDataForConsumptionGlobalChart(EnergySensorID, EnergySensorFamilyID, StartDate, EndDate, request);
        }

        public static List<Catalog> GetUoMListForEnergySensors(string CatalogName, GenericRequest request)
        {
            using (DataTable dt = _rep.GetUoMListForEnergySensors(CatalogName, request))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }

        public static t_YieldDefectChartData GetHourValuesByFamiliesChartGauge(DateTime ChartDate, GenericRequest request)
        {
            t_YieldDefectChartData result = new t_YieldDefectChartData();
            List<t_SeriesChart> List = new List<t_SeriesChart>();

            using (DataSet ds = _rep.GetHourValuesByFamiliesChartGauge(ChartDate, request))
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
                        data = ds.Tables[i].AsEnumerable().Select(r => r.Field<decimal?>("ydata")).ToArray()
                    });
                }

                result.SeriesList = List;
                return result;
            }
        }

        public static DataSet SensorsAlertsExportToExcel(int EnergySensorID, DateTime StartDate, DateTime EndDate, GenericRequest request)
        {
            using (DataSet ds = _rep.SensorsAlertsExportToExcel(EnergySensorID, StartDate, EndDate, request))
            {
                return ds;
            }
        }
    }
}
