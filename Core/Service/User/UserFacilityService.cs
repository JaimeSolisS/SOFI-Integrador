
namespace Core.Service
{
    #region namespaces

    using Core.Entities.Utilities;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Data;

    #endregion

    public class UserFacilityService
    {
        private static UserFacilityRepository _rep;

        static UserFacilityService()
        {
            _rep = new UserFacilityRepository();
        }

        #region CRUD Methods

        public static GenericReturn Add(UserFacility _Entity, GenericRequest request)
        {
            return _rep.Add(_Entity, request);
        }

        public static GenericReturn Delete(int UserID, int CompanyID, int FacilityID, Guid? TransactionID, GenericRequest request)
        {
            return _rep.Delete(UserID, CompanyID, FacilityID, TransactionID, request);
        }

        public static List<UserFacility> List(int? UserID, int? FacilityID, int CurrentFacilityID, int CurrentUserID, string CultureID)
        {
            using (DataTable dt = _rep.List(UserID, FacilityID, CurrentFacilityID, CurrentUserID, CultureID))
            {
                List<UserFacility> _list = dt.ConvertToList<UserFacility>();
                return _list;
            }
        }

        #endregion

        #region Methods

        public static List<UserFacility> Access(int CatalogID, int? FacilityID, int UserID, string CultureID)
        {
            using (DataTable dt = _rep.Access(CatalogID, FacilityID, UserID, CultureID))
            {
                List<UserFacility> _list = dt.ConvertToList<UserFacility>();
                return _list;
            }
        }

        public static List<UserFacility> List4Select(int CompanyID, GenericRequest req)
        {
            using (DataTable dt = _rep.List4Select(CompanyID, req))
            {
                List<UserFacility> _list = dt.ConvertToList<UserFacility>();
                return _list;
            }
        }

        #endregion
    }
}
