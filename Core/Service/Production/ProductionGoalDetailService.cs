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
    public class ProductionGoalDetailService
    {
        private static ProductionGoalDetailRepository _rep;

        static ProductionGoalDetailService()
        {
            _rep = new ProductionGoalDetailRepository();
        }

        #region CRUD

        public static List<ProductionGoalsDetail> List(ProductionGoal entity, GenericRequest request)
        {
            using (DataTable dt = _rep.List(entity, request))
            {
                List<ProductionGoalsDetail> _list = dt.ConvertToList<ProductionGoalsDetail>();
                return _list;
            }
        }

        public static GenericReturn BulkUpsert(List<Entities.SQL_DataType.t_ProductionGoalsDetail> list, ProductionGoalsDetail entity, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.BulkUpsert(dt, entity, request);
            }
        }
        #endregion
    }
}
