using Core.Entities;
using System.Collections.Generic;
using System.Data;
using Core.Entities.Utilities;
using Core.Data;

namespace Core.Service
{
    public static class CatalogParameterService
    {
        private static CatalogParameterRepository _rep;

        static CatalogParameterService()
        {
            _rep = new CatalogParameterRepository();
        }

        #region CRUD

        public static GenericReturn Upsert(CatalogParameter _CatalogParameter, GenericRequest request)
        {
            // TODO: Metodo para convertir 0 a null

            return _rep.Upsert(_CatalogParameter, request);
        }

        public static List<CatalogParameter> List(int CatalogID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(CatalogID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<CatalogParameter> _list = dt.ConvertToList<CatalogParameter>();

                return _list;
            }
        }

        public static GenericReturn UpsertFromdt(DataTable _CatalogParameter, GenericRequest request)
        {
            // TODO: Metodo para convertir 0 a null

            return _rep.UpsertFromdb(_CatalogParameter, request);
        }

        #endregion
    }
}
