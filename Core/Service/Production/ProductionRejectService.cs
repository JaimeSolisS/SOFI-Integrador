using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class ProductionRejectService
    {
        private static ProductionRejectRepository _rep;
        static ProductionRejectService()
        {
            _rep = new ProductionRejectRepository();
        }
        public static ProductionReject Get(int? ProductionRejectID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(ProductionRejectID, null, null, null, null,null, req))
            {
                List<ProductionReject> _list = dt.ConvertToList<ProductionReject>();
                return _list.FirstOrDefault();
            }
        }
        public static List<ProductionReject> List( int? ReferenceID, int? ReferenceTypeID,int? Hour, GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, ReferenceID, ReferenceTypeID, null, null, Hour.SelectedValue(), req))
            {
                List<ProductionReject> _list = dt.ConvertToList<ProductionReject>();
                return _list;
            }
        }
        public static List<ProductionReject> List(int? ProductionRejectID, int? ReferenceID, int? ReferenceTypeID, int? RejectTypeID, decimal? Quantity,int? Hour, GenericRequest req)
        {
            using (DataTable dt = _rep.List(ProductionRejectID, ReferenceID, ReferenceTypeID, RejectTypeID, Quantity,Hour, req))
            {
                List<ProductionReject> _list = dt.ConvertToList<ProductionReject>();
                return _list;
            }
        }
        public static GenericReturn Upsert(int? ProductionRejectID, int? ReferenceID, int? ReferenceTypeID, int? RejectTypeID, decimal? Quantity,int? Hour, GenericRequest req)
        {
            return _rep.Upsert(ProductionRejectID, ReferenceID, ReferenceTypeID, RejectTypeID, Quantity,Hour, req);
        }
        public static GenericReturn Delete(int? ProductionRejectID, GenericRequest req)
        {
            return _rep.Delete(ProductionRejectID, req);
        }
    }
}
