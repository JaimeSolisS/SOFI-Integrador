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
    public class VendorsService
    {
        private static VendorsRepository _rep;

        static VendorsService()
        {
            _rep = new VendorsRepository();
        }

        public static Vendor Get(int VendorID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(VendorID, null, null, "0,1", request))
            {
                List<Vendor> _list = dt.ConvertToList<Vendor>();
                return _list.FirstOrDefault();
            }
        }
        public static List<Vendor> List(string StatusIDs, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, StatusIDs, request))
            {
                List<Vendor> _list = dt.ConvertToList<Vendor>();
                return _list;
            }
        }

        public static List<Vendor> List(int? VendorID, string VendorName, string StatusIDs, GenericRequest request)
        {
            using (DataTable dt = _rep.List(VendorID, null, VendorName, StatusIDs, request))
            {
                List<Vendor> _list = dt.ConvertToList<Vendor>();
                return _list;
            }
        }

        public static List<Vendor> List4Select(string StatusIDs, GenericRequest request, bool EmptyFirst = true, string EmptyText = "")
        {
            using (DataTable dt = _rep.List(null, null, null, StatusIDs, request))
            {
                List<Vendor> _list = dt.ConvertToList<Vendor>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    _list.Insert(0, new Vendor() { VendorID = 0, VendorName = EmptyText });
                }
                return _list;
            }
        }

        public static GenericReturn Insert(Vendor vendor, GenericRequest request)
        {
            return _rep.Insert(vendor, request);
        }

        public static GenericReturn Delete(int? VendorID, GenericRequest request)
        {
            return _rep.Delete(VendorID, request);
        }
    }
}
