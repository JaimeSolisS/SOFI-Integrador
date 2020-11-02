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
    public class OrganizationService
    {
        private static OrganizationRepository _rep;

        static OrganizationService()
        {
            _rep = new OrganizationRepository();
        }

        public static List<Organization> List(bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(Enabled, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Organization> _list = dt.ConvertToList<Organization>();
                return _list;
            }
        }
        public static List<Organization> List4Select(GenericRequest request)
        {
            using (DataTable dt = _rep.List(true, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Organization> _list = dt.ConvertToList<Organization>();

                // Anexar fila vacia al principio
                //_list.Insert(0, new Organization() { OrganizationID = 0, OrganizationName = "" });

                return _list;
            }
        }

        public static List<Organization> List4Config(GenericRequest request)
        {
            using (DataTable dt = _rep.List4Config(request.FacilityID, request.UserID, request.CultureID))
            {
                List<Organization> _list = dt.ConvertToList<Organization>();

                // Anexar fila vacia al principio
                //_list.Insert(0, new Organization() { OrganizationID = 0, OrganizationName = "" });

                return _list;
            }
        }
    }
}
