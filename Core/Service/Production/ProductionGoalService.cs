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
    public class ProductionGoalService
    {
        private static ProductionGoalRepository _rep;

        static ProductionGoalService()
        {
            _rep = new ProductionGoalRepository();
        }

        #region CRUD

        public static List<ProductionGoal> List(ProductionGoal entity, GenericRequest request)
        {
            using (DataTable dt = _rep.List(entity, request))
            {
                List<ProductionGoal> _list = dt.ConvertToList<ProductionGoal>();
                return _list;
            }
        }

        public static ProductionGoal Find(ProductionGoal entity, GenericRequest request)
        {
            using (DataTable dt = _rep.List(entity, request))
            {
                List<ProductionGoal> _list = dt.ConvertToList<ProductionGoal>();
                if (_list != null && _list.Count > 0)
                {
                    return _list.FirstOrDefault();
                }

                return null;
            }
        }

        public static GenericReturn Add(ProductionGoal entity, GenericRequest request)
        {
                return _rep.Add(entity, request);
        }

        public static GenericReturn Upsert(ProductionGoal entity, GenericRequest request)
        {
            return _rep.Upsert(entity, request);
        }

        public static GenericReturn Delete(int GoalID, GenericRequest request)
        {
            return _rep.Delete(GoalID, request);
        }

        #endregion
    }
}
