using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public static class CI_DashboardAreaService
    {
        private static CI_DashboardAreaRepository _rep;

        static CI_DashboardAreaService()
        {
            _rep = new CI_DashboardAreaRepository();
        }

        #region Methods
        public static DashboardArea Get(int? DashboardAreaID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(DashboardAreaID, true, null, request))
            {
                List<DashboardArea> _list = dt.ConvertToList<DashboardArea>();
                return _list.FirstOrDefault();
            }
        }
        public static List<DashboardArea> List(int? DashboardAreaID, bool? Enabled, int? FileTypeID, GenericRequest request, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.List(DashboardAreaID, Enabled, FileTypeID, request))
            {
                List<DashboardArea> _list = dt.ConvertToList<DashboardArea>();
                if (AddEmptyRecord)
                {
                    _list.Insert(0, new DashboardArea { DashboardAreaID = 0, Title = "(All)" });
                }
                return _list;
            }
        }

        public static List<DashboardArea> List4Parent(int? DashboardAreaID, int? ParentDashboardAreaDetailID, bool? Enabled, GenericRequest request, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.List4Parent(DashboardAreaID, ParentDashboardAreaDetailID, Enabled, request))
            {
                List<DashboardArea> _list = dt.ConvertToList<DashboardArea>();
                if (AddEmptyRecord)
                {
                    _list.Insert(0, new DashboardArea { DashboardAreaID = 0, Title = "(All)" });
                }
                return _list;
            }
        }

        public static GenericReturn QuickUpdate(int DashboardAreaID, string ColumnName, string Value, int FacilityID, int ChangedBy, string CultureID)
        {
            return _rep.QuickUpdate(DashboardAreaID, ColumnName, Value, FacilityID, ChangedBy, CultureID);
        }

        public static GenericReturn Delete(int DashboardAreaID, GenericRequest request)
        {
            return _rep.Delete(DashboardAreaID, request);
        }

        public static List<GenericItem> GetGenericSettings(GenericRequest request)
        {
            using (DataTable dt = _rep.GetGeneralSettings(request))
            {
                List<GenericItem> _list = dt.ConvertToList<GenericItem>();
                return _list;
            }
        }

        public static GenericReturn GeneralSettingsUpdate(string ImageURL, string VideoURL, int? ScreenSaverInterval, int? CloseWindowAfter, string TransactionID, string HostName, GenericRequest request)
        {
            return _rep.GeneralSettingsUpdate(ImageURL, VideoURL, ScreenSaverInterval, CloseWindowAfter, TransactionID, HostName, request);
        }

        public static GenericReturn Upsert(int SizeID, int Sequence,int? ParentID, List<t_GenericItem> list, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.Upsert(new DashboardArea { SizeID = SizeID, Seq = Sequence,ParentID = ParentID }, dt, request);
            }
        }

        public static GenericReturn QuickTranslateUpdate(int DashboarAreaID, List<t_GenericItem> list, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.QuickTranslateUpdate(DashboarAreaID, dt, request);
            }
        }

        public static List<DashboardArea> GetTitleInfo(int DashboardAreaID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetTitleInfo(DashboardAreaID, request))
            {
                List<DashboardArea> _list = dt.ConvertToList<DashboardArea>();
                return _list;
            }
        }

        #endregion
    }
}
