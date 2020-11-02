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
    public class ProductionLineService
    {
        private static ProductionLineRepository _rep;

        static ProductionLineService()
        {
            _rep = new ProductionLineRepository();
        }

        #region CRUD

        public static List<ProductionLine> List(ProductionLine entity, GenericRequest request)
        {
            using (DataTable dt = _rep.List(entity, request))
            {
                List<ProductionLine> _list = dt.ConvertToList<ProductionLine>();
                return _list;
            }
        }
        public static List<ProductionLine> ListbyProdProcess(int? ProductionProcessID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(request.FacilityID, request))
            {
                List<ProductionLine> _list = dt.ConvertToList<ProductionLine>();
                return _list.Where(x => x.ProductionProcessID == ProductionProcessID).ToList();
            }
        }
        public static List<ProductionLine> List(int FacilityID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(FacilityID, request))
            {
                List<ProductionLine> _list = dt.ConvertToList<ProductionLine>();
                return _list;
            }
        }

        public static GenericReturn InsertByUser(int? UserID, string LineNumber, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(null, UserID, LineNumber, Enabled, request);
        }

        public static GenericReturn Insert(int? ProductionProcessID, string LineNumber, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(ProductionProcessID, null, LineNumber, Enabled, request);
        }
        #endregion
    }
}
