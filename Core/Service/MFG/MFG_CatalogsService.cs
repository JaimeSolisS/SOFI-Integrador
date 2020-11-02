using Core.Data;
using System.Collections.Generic;
using System.Data;
using Core.Entities;
using Core.Entities.Utilities;

namespace Core.Service
{
    public class MFG_CatalogsService
    {
        private static MFG_CatalogsRepository _rep;
        private static CatalogDetailRepository _repCat;

        static MFG_CatalogsService()
        {
            _rep = new MFG_CatalogsRepository();
            _repCat = new CatalogDetailRepository();
        }

        public static List<Catalogs> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(req))
            {
                List<Catalogs> _list = dt.ConvertToList<Catalogs>();
                return _list;
            }
        }

        public static List<Machine> TableForMachines (string ReferenceID, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(ReferenceID, null, null, null, null, req))
            {
                List<Machine> _list = dt.ConvertToList<Machine>();
                return _list;
            }
        }

        public static List<Machine> TableForMachinesFiltered(string ReferenceID, string Param1, string Param2, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(ReferenceID, null, Param1, Param2, null, req))
            {
                List<Machine> _list = dt.ConvertToList<Machine>();
                return _list;
            }
        }

        public static List<MachineParameters> TableForMachineParametersFiltered(string ReferenceID, string Param1, string Param2, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(ReferenceID, null, Param1, Param2, null, req))
            {
                List<MachineParameters> _list = dt.ConvertToList<MachineParameters>();
                return _list;
            }
        }

        public static List<Catalog> TableForCatalogsFiltered(int? CatalogID, string ReferenceID, string Param1, string Param2, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(ReferenceID, CatalogID, Param1, Param2, null, req))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }


        public static List<MachineParameters> TableForMachineParameters(string ReferenceID, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(ReferenceID, null, null, null, null, req))
            {
                List<MachineParameters> _list = dt.ConvertToList<MachineParameters>();
                return _list;
            }
        }

        public static List<Catalog> TableForCatalog(string ReferenceID, int? CatalogID, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(ReferenceID, CatalogID, null, null, null, req))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }

        public static List<Catalog> TableForCatalog(int? CatalogID, GenericRequest req)
        {
            using (DataTable dt = _rep.GetTableOfCatalog(null, CatalogID, null, null, null, req))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }

        public static GenericReturn Delete(int EntityID, GenericRequest request)
        {
            return _repCat.Delete(EntityID, request.FacilityID, request.UserID, request.CultureID);
        }

    }
}
