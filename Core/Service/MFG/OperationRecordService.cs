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
    public class OperationRecordService
    {
        private static OperationRecordRepository _rep;
        static OperationRecordService()
        {
            _rep = new OperationRecordRepository();
        }

        #region Methods
        public static OperationRecord Get(int OperationRecordID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(OperationRecordID, null, null, null, null, null, req))
            {
                List<OperationRecord> _list = dt.ConvertToList<OperationRecord>();
                return _list.FirstOrDefault();
            }
        }
        public static List<OperationRecord> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, req))
            {
                List<OperationRecord> _list = dt.ConvertToList<OperationRecord>();
                return _list;
            }
        }
        public static List<OperationRecord> List(int? OperationRecordID, int? MachineID, int? ShiftID, DateTime? StartDate, DateTime? EndDate, int? StatusID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(OperationRecordID, MachineID.SelectedValue(), ShiftID.SelectedValue(), StartDate, EndDate, StatusID.SelectedValue(), req))
            {
                List<OperationRecord> _list = dt.ConvertToList<OperationRecord>();
                return _list;
            }
        }
        public static GenericReturn Insert(int? MachineID, int? ShiftID, DateTime? OperationDate, int? MachineSetupID, int? MaterialID, string OperatorNumber, GenericRequest req)
        {
            return _rep.Insert(MachineID, ShiftID, OperationDate, MachineSetupID, MaterialID, OperatorNumber, req);
        }
        public static GenericReturn Close(int? OperationRecordID, int OperationProductionID, int? CurrentShotNumber, GenericRequest request)
        {
            return _rep.Close(OperationRecordID, OperationProductionID, CurrentShotNumber, request);
        }
        public static GenericReturn CloseShift(int? OperationRecordID, int OperationProductionID, int? CurrentShotNumber, GenericRequest request)
        {
            return _rep.CloseShift(OperationRecordID, OperationProductionID, CurrentShotNumber, request);
        }
        public static string GetJulianDay(DateTime Date, string DefaultValue, GenericRequest req)
        {
            return _rep.GetJulianDay(Date, DefaultValue, req);
        }

        #endregion

        #region Reports       
        public static DataSet EER_Yield_Report(int? Shift, int? MachineID, DateTime? Startdate, DateTime? Enddate, DateTime? StartDateExcel, DateTime? EndDateExcel, GenericRequest request)
        {
            using (DataSet ds = _rep.Gaskets_EER_Yield_Report(Shift.SelectedValue(), MachineID.SelectedValue(), Startdate, Enddate, StartDateExcel, EndDateExcel, request))
            {
                return ds;
            }
        }
        public static DataSet EER_Yield_Details_Report(DateTime? StartDateExcel, DateTime? EndDateExcel, int[] ShiftIDs, int[] MachinesIDs, GenericRequest request)
        {
            string Shifts = null;
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            if (ShiftIDs != null)
            {
                Shifts = string.Join<int>(",", ShiftIDs);
            }
            using (DataSet ds = _rep.Gaskets_EER_Yield_Details_Report(StartDateExcel, EndDateExcel, Shifts, Machines, request))
            {
                return ds;
            }
        }
        public static DataSet Parameters_Report(int[] MachineParameterID, int[] MachinesIDs, int[] ProductionProcessID, int[] ShiftID,
        int[] MaterialID, bool? IsGoodAnswer, DateTime? StartDatetime, DateTime? EndDatetime, GenericRequest request)
        {
            string MachineParameterIDs = null;
            string Machines = null;
            string ProductionProcessIDs = null;
            string ShiftIDs = null;
            string MaterialIDs = null;

            if (MachineParameterID != null)
            {
                MachineParameterIDs = string.Join<int>(",", MachineParameterID);
            }
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            if (ProductionProcessID != null)
            {
                ProductionProcessIDs = string.Join<int>(",", ProductionProcessID);
            }
            if (ShiftID != null)
            {
                ShiftIDs = string.Join<int>(",", ShiftID);
            }
            if (MaterialID != null)
            {
                MaterialIDs = string.Join<int>(",", MaterialID);
            }
            using (DataSet ds = _rep.OperationRecords_Parameters_Report(MachineParameterIDs, Machines,
                ProductionProcessIDs, ShiftIDs, MaterialIDs, IsGoodAnswer,
                StartDatetime, EndDatetime, request))
            {
                return ds;
            }
        }
        public static DataSet GasketDiameters_Report(int[] GasketIDs, int[] MachinesIDs, int[] CavitiesIDs, int[] ProductionProcessID, int[] ShiftID,
        int[] MaterialID, DateTime? StartDatetime, DateTime? EndDatetime, GenericRequest request)
        {
            string Gaskets = null;
            string Cavities = null;
            string Machines = null;
            string ProductionProcessIDs = null;
            string ShiftIDs = null;
            string MaterialIDs = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            if (GasketIDs != null)
            {
                Gaskets = string.Join<int>(",", GasketIDs);
            }
            if (CavitiesIDs != null)
            {
                Cavities = string.Join<int>(",", CavitiesIDs);
            }
            if (ProductionProcessID != null)
            {
                ProductionProcessIDs = string.Join<int>(",", ProductionProcessID);
            }
            if (ShiftID != null)
            {
                ShiftIDs = string.Join<int>(",", ShiftID);
            }
            if (MaterialID != null)
            {
                MaterialIDs = string.Join<int>(",", MaterialID);
            }

            using (DataSet ds = _rep.OperationRecords_GasketDiameters_Report(Gaskets, Machines, Cavities,
                ProductionProcessIDs, ShiftIDs, MaterialIDs, StartDatetime, EndDatetime, request))
            {
                return ds;
            }
        }


        public static DataSet DashboardGasket_YieldDefectsReport(DateTime? ChartDate, int[] MachinesIDs, int? ShiftID, int? ProductionProcessID, int? MaterialID, GenericRequest request)
        {
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            return _rep.DashboardGasket_YieldDefectsReport(ChartDate, Machines, ShiftID, ProductionProcessID, MaterialID, request);
        }

        public static DataSet DashboardGasket_DowntimesReport(DateTime? ChartDate, int[] MachinesIDs, int? ShiftID, int? ProductionProcessID, int? MaterialID, GenericRequest request)
        {
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            return _rep.DashboardGasket_DowntimesReport(ChartDate, Machines, ShiftID, ProductionProcessID, MaterialID, request);
        }

        public static DataSet DashboardGasket_ProductionReport(DateTime? ChartDate, int[] MachinesIDs, int? ShiftID, GenericRequest request)
        {
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            return _rep.DashboardGasket_ProductionReport(ChartDate, Machines, ShiftID, request);
        }

        public static DataSet DashboardChartsAllReports(
            DateTime ChartDate
            , int? ShiftID
            //Datos para Yield
            , int[] yieldchart_SelectedMachines
            , int? yieldchart_process
            , int? yieldchart_materials
            //Datos para Downtimes
            , int[] dtchart_SelectedMachines
            , int? dtchart_process
            , int? dtchart_materials
            //Datos de Production
            , int[] prodchart_SelectedMachines
            , GenericRequest request)
        {
            string YieldMachines = null;
            string DowntimesMachines = null;
            string ProductionMachines = null;

            if (yieldchart_SelectedMachines != null)
            {
                YieldMachines = string.Join<int>(",", yieldchart_SelectedMachines);
            }

            if (dtchart_SelectedMachines != null)
            {
                DowntimesMachines = string.Join<int>(",", dtchart_SelectedMachines);
            }

            if (prodchart_SelectedMachines != null)
            {
                ProductionMachines = string.Join<int>(",", prodchart_SelectedMachines);
            }

            return _rep.DashboardChartsAllReports(ChartDate
                , ShiftID
                , YieldMachines
                , yieldchart_process
                , yieldchart_materials
                , DowntimesMachines
                , dtchart_process
                , dtchart_materials
                , ProductionMachines
                , request);
        }


        #endregion

        #region Charts      
        public static t_YieldDefectChartData YieldDefectsChart(DateTime? ChartDate, int[] MachinesIDs, int? ProductionProcessID, int? MaterialID, int? ShiftID, int? Top, GenericRequest request)
        {
            t_YieldDefectChartData result = new t_YieldDefectChartData();
            List<t_SeriesChart> List = new List<t_SeriesChart>();
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            using (DataSet ds = _rep.YieldDefectsChart(ChartDate, Machines, ProductionProcessID.SelectedValue(), MaterialID.SelectedValue(), ShiftID, Top, request))
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
        public static y_YieldDowntimesChartData EERDowntimesChart(DateTime? ChartDate, int[] MachinesIDs, int? ProductionProcessID, int? MaterialID, int? ShiftID, int? Top, GenericRequest request)
        {
            y_YieldDowntimesChartData result = new y_YieldDowntimesChartData();
            List<t_SeriesChart> List = new List<t_SeriesChart>();
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            using (DataSet ds = _rep.EERDowntimesChart(ChartDate, Machines.SelectedValue(), ProductionProcessID.SelectedValue(), MaterialID.SelectedValue(), ShiftID, Top, request))
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
        public static List<t_ChartData> ProductionChart(DateTime? ChartDate, int[] MachinesIDs, int? ShiftID, GenericRequest request)
        {
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            using (DataTable dt = _rep.Dashboard_ProductionChart(ChartDate, Machines, ShiftID, request))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }
        public static y_YieldDowntimesChartData DowntimesChart(DateTime? ChartDate, int[] MachinesIDs, int? ProductionProcessID, int? MaterialID, int? ShiftID, int? Top, GenericRequest request)
        {
            y_YieldDowntimesChartData result = new y_YieldDowntimesChartData();
            List<t_SeriesChart> List = new List<t_SeriesChart>();
            string Machines = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            using (DataSet ds = _rep.DowntimesChart(ChartDate, Machines.SelectedValue(), ProductionProcessID.SelectedValue(), MaterialID.SelectedValue(), ShiftID, Top, request))
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
        #endregion
    }
}


