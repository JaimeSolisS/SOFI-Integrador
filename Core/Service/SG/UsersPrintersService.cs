using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.SG
{
    public class UsersPrintersService
    {
        private static UsersPrintersSetupRepository _rep;
        static UsersPrintersService()
        {
            _rep = new UsersPrintersSetupRepository();
        }

        public static GenericReturn SG_GenerateLabels(int? referenceID, int? rerenceTypeID, GenericRequest request)
        {
            return _rep.SG_GenerateLabels(referenceID, rerenceTypeID, request);
        }

    }
}
