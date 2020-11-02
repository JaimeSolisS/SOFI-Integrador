using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class RequestsGenericDetailService
    {
        private static RequestsGenericDetailRepository _rep;

        static RequestsGenericDetailService()
        {
            _rep = new RequestsGenericDetailRepository();
        }

        public static GenericReturn Update(RequestsGenericDetail RequestsGenericDetail, string Comments, GenericRequest request)
        {
            return _rep.Update(RequestsGenericDetail, Comments, request);
        }
    }
}
