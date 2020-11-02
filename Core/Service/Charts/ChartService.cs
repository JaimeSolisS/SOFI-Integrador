using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Core.Service
{
    public class ChartService
    {
        private static ChartRepository _rep;

        static ChartService()
        {
            _rep = new ChartRepository();
        }

        public static Chart Get(int? ChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.Charts_List(ChartID, null, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Chart> _list = dt.ConvertToList<Chart>();
                foreach (var item in _list)
                {
                    item.Options = ListOptions(item.ChartID, request);
                }
                return _list.FirstOrDefault();
            }
        }
        public static List<Chart> List(int? ChartID, int? ChartType, GenericRequest request)
        {
            using (DataTable dt = _rep.Charts_List(ChartID, ChartType, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Chart> _list = dt.ConvertToList<Chart>();
                //foreach (var item in _list)
                //{
                //    item.Options = ListOptions(item.ChartID, request);
                //}
                return _list;
            }
        }
        public static List<ChartOption> ListOptions(int? ChartOptionID, int? ChartID, int? OptionID, string OptionValue, GenericRequest request)
        {
            using (DataTable dt = _rep.ChartOptions_List(ChartOptionID, ChartID, OptionID, OptionValue, request.FacilityID, request.UserID, request.CultureID))
            {
                List<ChartOption> _list = dt.ConvertToList<ChartOption>();
                return _list;
            }
        }
        public static List<ChartOption> ListOptions(int? ChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.ChartOptions_List(null, ChartID, null, null, request.FacilityID, request.UserID, request.CultureID))
            {
                List<ChartOption> _list = dt.ConvertToList<ChartOption>();
                return _list;
            }
        }
        public static List<Chart_ComputerSetting> ComputerSettings_List(int? Chart_SettingID, string ComputerName, int? ChartID, GenericRequest request)
        {
            using (DataTable dt = _rep.Charts_ComputerSettings_List(Chart_SettingID, ComputerName, ChartID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Chart_ComputerSetting> _list = dt.ConvertToList<Chart_ComputerSetting>();
                foreach (var item in _list)
                {
                    item.Options = ComputerSettingsDetail_List(item.Chart_SettingID, request);
                }
                return _list;
            }
        }
        public static Chart_ComputerSetting ComputerSettings_Get(int? Chart_SettingID, GenericRequest request)
        {
            using (DataTable dt = _rep.Charts_ComputerSettings_List(Chart_SettingID, null, null, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Chart_ComputerSetting> _list = dt.ConvertToList<Chart_ComputerSetting>();
                foreach (var item in _list)
                {
                    item.Options = ComputerSettingsDetail_List(item.Chart_SettingID, request);
                }
                return _list.FirstOrDefault();
            }
        }
        public static List<Charts_ComputerSettingsDetail> ComputerSettingsDetail_List(int? Chart_SetttingDetailID, int? Chart_SettingID, int? OptionID, string OptionValue, GenericRequest request)
        {
            using (DataTable dt = _rep.Charts_ComputerSettingsDetail_List(Chart_SetttingDetailID, Chart_SettingID, OptionID, OptionValue, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Charts_ComputerSettingsDetail> _list = dt.ConvertToList<Charts_ComputerSettingsDetail>();
                return _list;
            }
        }

        public static List<Charts_ComputerSettingsDetail> ComputerSettingsDetail_List(int? Chart_SettingID, GenericRequest request)
        {
            using (DataTable dt = _rep.Charts_ComputerSettingsDetail_List(null, Chart_SettingID, null, null, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Charts_ComputerSettingsDetail> _list = dt.ConvertToList<Charts_ComputerSettingsDetail>();
                return _list;
            }
        }

        public static List<t_ChartData> GetChartSliderData(int? Chart_SettingID, int? FacilityID, int? UserID, string CultureID)
        {
            using (DataTable dt = _rep.GetChartSliderData(Chart_SettingID, FacilityID, UserID, CultureID))
            {
                List<t_ChartData> _list = dt.ConvertToList<t_ChartData>();
                return _list;
            }
        }

        public static GenericReturn ComputerSettings_Insert(string ComputerName, int? ChartID, int ShiftID, GenericRequest request)
        {
            return _rep.Charts_ComputerSettings_Insert(ComputerName, ChartID, ShiftID, request.FacilityID, request.UserID, request.CultureID);
        }
        public static GenericReturn ComputerSettings_Delete(int? Chart_SettingID, GenericRequest request)
        {
            return _rep.Charts_ComputerSettings_Delete(Chart_SettingID, request.FacilityID, request.UserID, request.CultureID);
        }
        public static GenericReturn ComputerSettings_QuickUpdate(int? Chart_SettingID, string ColumnName, string Value, GenericRequest request)
        {
            return _rep.Charts_ComputerSettings_QuickUpdate(Chart_SettingID, ColumnName, Value, request.FacilityID, request.UserID, request.CultureID);
        }
        public static GenericReturn Charts_ComputerSettingsDetail_QuickUpdate(int? Chart_SetttingDetailID, string ColumnName, string Value, GenericRequest request)
        {
            return _rep.Charts_ComputerSettingsDetail_QuickUpdate(Chart_SetttingDetailID, ColumnName, Value, request.FacilityID, request.UserID, request.CultureID);
        }

        public static GenericReturn Charts_ComputerSettings_UpdateConfiguration(List<Chart_ComputerSetting> Charts_ComputerSettings, List<Charts_ComputerSettingsDetail> Charts_ComputerSettingsDetail, GenericRequest request)
        {
            using (DataTable dt = Charts_ComputerSettings.Select(x => new { 
                x.Chart_SettingID
                ,x.ComputerName
                ,x.ChartID
                ,x.ChartName
                ,x.OptionsTitle
                ,x.ChartType
                ,x.Seq
                ,x.ShiftID
                ,x.IntervalRefreshTime
                
            }).ToList().ConvertToDataTable())
            {
                using (DataTable dt2 = Charts_ComputerSettingsDetail.Select(x => new
                {
                    x.Chart_SetttingDetailID
                    ,x.Chart_SettingID
                    ,x.OptionID
                    ,x.OptionName
                    ,x.OptionType
                    ,x.OptionTypeCatalog
                    ,x.OptionValue
                }).ToList().ConvertToDataTable())
                {
                    return _rep.Charts_ComputerSettings_UpdateConfiguration(dt, dt2, request);
                }
            }
        }
    }
}
