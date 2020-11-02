using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class DownTimeService
    {
        private static DownTimeRepository _rep;
        static DownTimeService()
        {
            _rep = new DownTimeRepository();
        }

        #region Methods
        public static DownTime Get(int? DownTimeID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(DownTimeID, null,null, null, null, null, null, null, null, null, req))
            {
                List<DownTime> _list = dt.ConvertToList<DownTime>();
                return _list.FirstOrDefault();
            }
        }
        public static List<DownTime> List( int? ReferenceID, int? RefenceTypeID,  GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, ReferenceID, RefenceTypeID, null, null, null, null, null, null, null, req))
            {
                List<DownTime> _list = dt.ConvertToList<DownTime>();
                return _list;
            }
        }
        public static List<DownTime> List(int? DownTimeID, int? ReferenceID, int? RefenceTypeID, DateTime? StartDate, DateTime? EndDate, int? DepartmentID, int? ReasonID, string Comments, int? DownTimeTypeID, int? StatusID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(DownTimeID, ReferenceID, RefenceTypeID, StartDate, EndDate, DepartmentID, ReasonID, Comments, DownTimeTypeID, StatusID, req))
            {
                List<DownTime> _list = dt.ConvertToList<DownTime>();
                return _list;
            }
        }
        public static GenericReturn Upsert(int? DownTimeID, int? ReferenceID, int? ReferenceTypeID, string StartTime, string EndTime, int? DepartmentID, int? ReasonID, string Comments,bool CloseTime, int? DownTimeTypeID, int? StatusID, int? ChangeInserts, GenericRequest req)
        {
            return _rep.Upsert(DownTimeID.SelectedValue(), ReferenceID, ReferenceTypeID, StartTime, EndTime.SelectedValue(), DepartmentID, ReasonID, Comments, CloseTime, DownTimeTypeID, StatusID, ChangeInserts, req);
        }
        public static GenericReturn Delete(int? DownTimeID, GenericRequest req)
        {
            return _rep.Delete(DownTimeID, req);
        }
        #endregion
    }
}
