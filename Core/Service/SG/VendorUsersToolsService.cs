using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class VendorUsersToolsService
    {
        private static VendorUsersToolsRepository _rep;

        static VendorUsersToolsService()
        {
            _rep = new VendorUsersToolsRepository();
        }

        public static GenericReturn Insert(VendorUserTool VendorUserTool, GenericRequest request)
        {
            return _rep.Insert(VendorUserTool, request);
        }

    }
}
