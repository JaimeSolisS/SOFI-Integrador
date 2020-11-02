using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public static class CI_DashboardAreaDetailService
    {
        private static CI_DashboardAreaDetailRepository _rep;

        static CI_DashboardAreaDetailService()
        {
            _rep = new CI_DashboardAreaDetailRepository();
        }

        #region Methods
        public static DashboardAreaDetail Get(int? DashboardAreaDetailID,  GenericRequest request)
        {            
            using (DataTable dt = _rep.List(DashboardAreaDetailID, null, null, true, null, null, request))
            {
                List<DashboardAreaDetail> _list = dt.ConvertToList<DashboardAreaDetail>();
                return _list.FirstOrDefault();
            }
        }
        public static List<DashboardAreaDetail> List(int? DashboardAreaDetailID,int? ParentDashboardAreaDetailID, int? DashboardAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            if(ParentDashboardAreaDetailID == 0) { ParentDashboardAreaDetailID = null; }
            using (DataTable dt = _rep.List(DashboardAreaDetailID, ParentDashboardAreaDetailID, DashboardAreaID, Enabled, FileTypeID, HostName, request))
            {
                List<DashboardAreaDetail> _list = dt.ConvertToList<DashboardAreaDetail>();
                return _list;
            }
        }
        public static List<DashboardAreaNode> NodeList(int? DashboardAreaDetailID, int? ParentDashboardAreaDetailID, int? DashboardAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            if (ParentDashboardAreaDetailID == 0) { ParentDashboardAreaDetailID = null; }
            using (DataTable dt = _rep.NodeList(DashboardAreaDetailID.SelectedValue(), ParentDashboardAreaDetailID.SelectedValue(), DashboardAreaID.SelectedValue(), Enabled, FileTypeID.SelectedValue(), HostName, request))
            {
                List<DashboardAreaNode> _list = dt.ConvertToList<DashboardAreaNode>();
                return _list;
            }
        }

        public static GenericReturn Upsert(DashboardAreaDetail entity, List<t_GenericItem> list, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.Upsert(entity, dt, request);
            }
        }

        public static GenericReturn QuickUpdate(int DashboardAreaDetailID, string ColumnName, string Value, GenericRequest request)
        {
            return _rep.QuickUpdate(DashboardAreaDetailID, ColumnName, Value, request);
        }

        public static GenericReturn Delete(int DashboardAreaDetailID, GenericRequest request)
        {
            return _rep.Delete(DashboardAreaDetailID, request);
        }

        public static DashboardAreaDetail Find(int DashboardAreaDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(DashboardAreaDetailID,null, null, true, null, null, request))
            {
                List<DashboardAreaDetail> _list = dt.ConvertToList<DashboardAreaDetail>();
                if(_list != null)
                {
                    return _list.Find(p => p.DashboardAreaDetailID == DashboardAreaDetailID);
                }
                return new DashboardAreaDetail();
            }
        }

        public static List<Catalog> GetNameInfo(int DashboardAreaDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetNameInfo(DashboardAreaDetailID, request))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }

        public static string GetSeqNumber(int DashboardAreaDetailID)
        {
            return _rep.GetSeqNumber(DashboardAreaDetailID);
        }

        #endregion
    }
}
