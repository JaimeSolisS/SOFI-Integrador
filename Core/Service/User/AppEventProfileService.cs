using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public static class AppEventProfileService
    {
        private static AppEventProfileRepository _rep;

        static AppEventProfileService()
        {
            _rep = new AppEventProfileRepository();
        }

        #region Methods

        public static bool IsValid(int EventID, GenericRequest request)
        {
            return _rep.IsValid(EventID, request);
        }

        public static GenericReturn UpdateAccess(int EventID, int ProfileID, bool Access, int? AccessTypeID, GenericRequest request)
        {
            return _rep.UpdateAccess(EventID, ProfileID, Access, AccessTypeID, request);
        }


        #endregion
    }
}
