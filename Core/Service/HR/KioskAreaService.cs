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
    public class KioskAreaService
    {
        private static KioskAreaRepository _rep;

        static KioskAreaService()
        {
            _rep = new KioskAreaRepository();
        }

        public static List<KioskArea> List4Parent(GenericRequest request)
        {
            using (DataTable dt = _rep.List4Parent(null, 0, request))
            {
                List<KioskArea> _list = dt.ConvertToList<KioskArea>();
                return _list;
            }
        }

        public static List<KioskArea> List(int? KioskAreaID, int? ParentKioskAreaDetailID, string Title, int? SizeID, int? Seq, bool? Enabled, bool? IsRoot, GenericRequest request)
        {
            using (DataTable dt = _rep.List(KioskAreaID, ParentKioskAreaDetailID, Title, SizeID, Seq, Enabled, IsRoot, request))
            {
                List<KioskArea> _list = dt.ConvertToList<KioskArea>();
                return _list;
            }
        }

        public static List<KioskArea> List(bool? IsRoot, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, IsRoot, request))
            {
                List<KioskArea> _list = dt.ConvertToList<KioskArea>();
                return _list;
            }
        }

        public static List<KioskArea> List(int? KioskAreaID, bool? Enabled, int? FileTypeID, bool? IsRoot, GenericRequest request, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.List(KioskAreaID, null, null, null, null, Enabled, IsRoot, request))
            {
                List<KioskArea> _list = dt.ConvertToList<KioskArea>();
                if (AddEmptyRecord)
                {
                    _list.Insert(0, new KioskArea { KioskAreaID = 0, Title = "(All)" });
                }
                return _list;
            }
        }

        public static List<KioskArea> List4Parent(int? KioskAreaID, int? ParentKioskAreaDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.List4Parent(KioskAreaID, ParentKioskAreaDetailID, request))
            {
                List<KioskArea> _list = dt.ConvertToList<KioskArea>();
                return _list;
            }
        }

        public static KioskArea Get(int? KioskAreaID, bool? IsRoot, GenericRequest request)
        {
            using (DataTable dt = _rep.List(KioskAreaID, null, null, null, null, true, IsRoot, request))
            {
                List<KioskArea> _list = dt.ConvertToList<KioskArea>();
                return _list.FirstOrDefault();
            }
        }

        public static GenericReturn QuickUpdate(int DashboardAreaID, string ColumnName, string Value, int FacilityID, int ChangedBy, string CultureID)
        {
            return _rep.QuickUpdate(DashboardAreaID, ColumnName, Value, FacilityID, ChangedBy, CultureID);
        }

        public static GenericReturn Delete(int? KioskAreaID, GenericRequest request)
        {
            return _rep.Delete(KioskAreaID, request);
        }

        public static List<GenericItem> GetGenericSettings(GenericRequest request)
        {
            using (DataTable dt = _rep.GetGeneralSettings(request))
            {
                List<GenericItem> _list = dt.ConvertToList<GenericItem>();
                return _list;
            }
        }

        public static GenericReturn Upsert(int SizeID, int Sequence, int? ParentID, List<t_GenericItem> list, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.Upsert(new KioskArea { SizeID = SizeID, Seq = Sequence, ParentID = ParentID }, dt, request);
            }
        }

        public static GenericReturn QuickTranslateUpdate(int DashboarAreaID, List<t_GenericItem> list, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.QuickTranslateUpdate(DashboarAreaID, dt, request);
            }
        }

        public static string GetKioskCarouselMediaID(int CatalogDetailID, int ParamIndex, string DefaultValue)
        {
            return _rep.GetKioskCarouselMediaID(1);
        }
    }
}
