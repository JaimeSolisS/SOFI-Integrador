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
    public class VendorUsersService
    {
        private static VendorUsersRepository _rep;

        static VendorUsersService()
        {
            _rep = new VendorUsersRepository();
        }

        public static List<VendorUser> ListByVendor(int? VendorID, string FullName, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, VendorID, FullName, null, null, null, null, request))
            {
                List<VendorUser> _list = dt.ConvertToList<VendorUser>();
                return _list;
            }
        }

        public static List<VendorUser> ListByVendor(int? VendorID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, VendorID, null, null, null, null, null, request))
            {
                List<VendorUser> _list = dt.ConvertToList<VendorUser>();
                return _list;
            }
        }

        public static List<VendorUser> List(int? VendorUserID, int? VendorID, string FullName, string AccessCode, string InsuranceNumber, DateTime? ExpirationDate, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(VendorUserID, VendorID, FullName, AccessCode, InsuranceNumber, ExpirationDate, Enabled, request))
            {
                List<VendorUser> _list = dt.ConvertToList<VendorUser>();
                return _list;
            }
        }

        public static VendorUser Get(int? VendorUserID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(VendorUserID, null, null, null, null, null, null, request))
            {
                VendorUser _entity = dt.ConvertToList<VendorUser>().FirstOrDefault();
                return _entity;
            }
        }

        public static GenericReturn Insert(VendorUser vendorUser, GenericRequest request)
        {
            return _rep.Insert(vendorUser, request);
        }

        public static GenericReturn Update(VendorUser vendorUser, GenericRequest request)
        {
            return _rep.Update(vendorUser, request);
        }

        public static GenericReturn Delete(int? VendorUserID, GenericRequest request)
        {
            return _rep.Delete(VendorUserID, request);
        }

    }
}
