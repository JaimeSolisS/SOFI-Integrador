using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;


namespace Core.Service
{
    public class OperationTaskDetailsService
    {
        private static OperationTaskDetailsRepository _rep;
        static OperationTaskDetailsService()
        {
            _rep = new OperationTaskDetailsRepository();
        }
        public static List<OperationTaskDetails> List(int? OperationTaskDetailID, int? OperationTaskID, string Comments, int? Progress, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OperationTaskDetailID, OperationTaskID, Comments, Progress, request))
            {
                List<OperationTaskDetails> _list = dt.ConvertToList<OperationTaskDetails>();
                return _list;
            }
        }

        public static GenericReturn Insert(int? OperationTaskID, string Comments, GenericRequest request)
        {
            return _rep.Insert(OperationTaskID, Comments, request);
        }
    }
}
