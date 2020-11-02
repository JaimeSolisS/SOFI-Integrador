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
    public class QA_FilmetricFilmLimitsService
    {
        private static QA_FilmetricFilmLimitsRepository _rep;

        static QA_FilmetricFilmLimitsService()
        {
            _rep = new QA_FilmetricFilmLimitsRepository();
        }

        public static List<FilmetricFilmLimits> FilmLimitsList(int? ProductFilmLimitID, int? ProductID, int? FilmID, decimal? MaxValue, decimal? MinValue, GenericRequest request)
        {
            using (DataTable dt = _rep.List(ProductFilmLimitID, ProductID, FilmID, MaxValue, MinValue, request))
            {
                List<FilmetricFilmLimits> _list = dt.ConvertToList<FilmetricFilmLimits>();
                return _list;
            }
        }
    }
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


        //        public static List<FilmetricInspectionDetail> DetailsList (int? FilmetricInspectionDetailID, int? FilmetricInspectionID, int? FilmID, decimal? Value, GenericRequest request){
        //		using (DataTable dt = _rep.Details_List (FilmetricInspectionDetailID, FilmetricInspectionID, FilmID, Value, request))
        //		{
        //			List<FilmetricInspectionDetail> _list = dt.ConvertToList<FilmetricInspectionDetail>();
        //			return _list;
        //		}
        //}


 



