using Core.Data;
using Core.Entities;
using Core.Entities.QA;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
  public class QA_FilmetricInspectionService
    {
        private static QA_FilmetricInspectionRepository _rep;

        static QA_FilmetricInspectionService()
        {
            _rep = new QA_FilmetricInspectionRepository();
        }


        //public static FilmetricInspection Get(int? FilmetricInspectionID, GenericRequest req)
        //{
        //    using (DataTable dt = _rep.List(FilmetricInspectionID, null, null, null, null, null, null, null, req.FacilityID, req.UserID, req.CultureID))
        //    {
        //        FilmetricInspection _entity = dt.ConvertToList<FilmetricInspection>().FirstOrDefault();
        //        if (_entity != null)
        //        {
        //            _entity.Details = DetailsList(_entity.FilmetricInspectionID, req);
        //        }

        //        return _entity;
        //    }
        //}

        public static List<FilmetricInspectionDetail> DetailsList(int? FilmetricInspectionID,int? ProductID, GenericRequest request)
        {
            using (DataTable dt = _rep.Details_List(null, FilmetricInspectionID, null , ProductID,null, request))
            {
                List<FilmetricInspectionDetail> _list = dt.ConvertToList<FilmetricInspectionDetail>();
                return _list;
            }
        }

        //        public static List<FilmetricInspectionDetail> DetailsList (int? FilmetricInspectionDetailID, int? FilmetricInspectionID, int? FilmID, decimal? Value, GenericRequest request){
        //		using (DataTable dt = _rep.Details_List (FilmetricInspectionDetailID, FilmetricInspectionID, FilmID, Value, request))
        //		{
        //			List<FilmetricInspectionDetail> _list = dt.ConvertToList<FilmetricInspectionDetail>();
        //			return _list;
        //		}
        //}


        public static List<FilmetricInspection> List (int? FilmetricInspectionID, int? ProductID, int? SubstractID, int? BaseID, decimal? AdditionID, int? LineID, DateTime? StartDate, DateTime? EndDate, GenericRequest req){
		using (DataTable dt = _rep.List(FilmetricInspectionID, ProductID.SelectedValue(), SubstractID.SelectedValue(), BaseID.SelectedValue(), AdditionID, LineID,StartDate,EndDate, req.FacilityID, req.UserID, req.CultureID))
		{
			List<FilmetricInspection> _list = dt.ConvertToList<FilmetricInspection>();
			return _list;
		}
}
        public static List<FilmetricInspection> List( GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null,null,null, req.FacilityID, req.UserID, req.CultureID))
            {
                List<FilmetricInspection> _list = dt.ConvertToList<FilmetricInspection>();
                return _list;
            }
        }

        public static GenericReturn Insert(int? ProductID, int? MaterialID, int? SubstractID, int? BaseID, int? AdditionID, int? LineID, int? UserID, decimal? HcValue, decimal? BcValue, decimal? PuValue, decimal? EuValue, GenericRequest req)
        {
            return _rep.Insert(ProductID, MaterialID, SubstractID, BaseID, AdditionID, LineID, UserID, HcValue, BcValue, PuValue, EuValue, req);
        }
    }
}
