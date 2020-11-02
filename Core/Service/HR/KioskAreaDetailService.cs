using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public static class KioskAreaDetailService
    {
        private static KioskAreaDetailRepository _rep;

        static KioskAreaDetailService()
        {
            _rep = new KioskAreaDetailRepository();
        }

        #region Methods
        public static KioskAreaDetail Get(int? KioskAreaDetailID,  GenericRequest request)
        {            
            using (DataTable dt = _rep.List(KioskAreaDetailID, null, null, true, null, null, request))
            {
                List<KioskAreaDetail> _list = dt.ConvertToList<KioskAreaDetail>();
                return _list.FirstOrDefault();
            }
        }

        public static List<KioskAreaDetail> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, true, null, null, request))
            {
                List<KioskAreaDetail> _list = dt.ConvertToList<KioskAreaDetail>();
                return _list;
            }
        }

        public static List<KioskAreaDetail> List(int? KioskAreaDetailID,int? ParentKioskAreaDetailID, int? KioskAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            if(ParentKioskAreaDetailID == 0) { ParentKioskAreaDetailID = null; }
            using (DataTable dt = _rep.List(KioskAreaDetailID, ParentKioskAreaDetailID, KioskAreaID, Enabled, FileTypeID, HostName, request))
            {
                List<KioskAreaDetail> _list = dt.ConvertToList<KioskAreaDetail>();
                return _list;
            }
        }
        public static List<KioskAreaNode> NodeList(int? KioskAreaDetailID, int? ParentKioskAreaDetailID, int? KioskAreaID, bool? Enabled, int? FileTypeID, string HostName, GenericRequest request)
        {
            if (ParentKioskAreaDetailID == 0) { ParentKioskAreaDetailID = null; }
            using (DataTable dt = _rep.NodeList(KioskAreaDetailID.SelectedValue(), ParentKioskAreaDetailID.SelectedValue(), KioskAreaID.SelectedValue(), Enabled, FileTypeID.SelectedValue(), HostName, request))
            {
                List<KioskAreaNode> _list = dt.ConvertToList<KioskAreaNode>();
                return _list;
            }
        }
        public static GenericReturn QuickUpdate(int KioskAreaDetailID, string ColumnName, string Value, GenericRequest request)
        {
            return _rep.QuickUpdate(KioskAreaDetailID, ColumnName, Value, request);
        }

        public static GenericReturn Delete(int KioskAreaDetailID, GenericRequest request)
        {
            return _rep.Delete(KioskAreaDetailID, request);
        }

        public static KioskAreaDetail Find(int KioskAreaDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(KioskAreaDetailID,null, null, true, null, null, request))
            {
                List<KioskAreaDetail> _list = dt.ConvertToList<KioskAreaDetail>();
                if(_list != null)
                {
                    return _list.Find(p => p.KioskAreaDetailID == KioskAreaDetailID);
                }
                return new KioskAreaDetail();
            }
        }

        public static List<Catalog> GetNameInfo(int KioskAreaDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetNameInfo(KioskAreaDetailID, request))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }

        public static string GetSeqNumber(int KioskAreaDetailID)
        {
            return _rep.GetSeqNumber(KioskAreaDetailID);
        }

        public static GenericReturn Upsert(KioskAreaDetail entity, List<t_GenericItem> list, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.Upsert(entity, dt, request);
            }
        }
        #endregion
    }
}
