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
    public static class vw_CatalogService
    {
        private static vw_CatalogRepository _rep;

        static vw_CatalogService()
        {
            _rep = new vw_CatalogRepository();
        }

        public static List<Catalog> List(string CatalogTag, GenericRequest req)
        {
            using (DataTable dt = _rep.List(CatalogTag, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }
        public static List<Catalog> List4Formats( GenericRequest req, bool EmptyFirst = false)
        {
            using (DataTable dt = _rep.List4Formats( req.FacilityID, req.UserID, req.CultureID))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = "" });
                }
                return _list;
            }
        }

        public static Catalog Get(int CatalogDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.Get(CatalogDetailID, request))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list.FirstOrDefault();
            }
        }

        public static Catalog GetInfo(int CatalogID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetInfo(CatalogID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                if (_list == null)
                    return new Catalog();
                else
                    return _list[0];
            }
        }

        public static List<Catalog> List4Select(string CatalogTag, GenericRequest req, bool EmptyFirst = true, string EmptyText = "")
        {
            using (DataTable dt = _rep.List(CatalogTag, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = EmptyText });
                }
                return _list;
            }
        }

        public static List<Catalog> WhereList4Select(string CatalogTag, Func<Catalog, bool> whereClause, GenericRequest req, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List(CatalogTag, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                var list = _list.Where(whereClause).ToList(); //para no usar el where seria w =>true
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    list.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = "" });
                }
                return list;
            }
        }

        #region CRUD
        public static GenericReturn Add(Catalog _Catalog, GenericRequest request)
        {
            _Catalog.OrganizationID = _Catalog.OrganizationID.SelectedValue();
            _Catalog.FacilityID = _Catalog.FacilityID.SelectedValue();

            return _rep.Add(_Catalog, request);
        }

        public static GenericReturn Update(Catalog _Catalog, GenericRequest request)
        {
            return _rep.Update(_Catalog, request);
        }

        #endregion

        #region Methods

        public static List<Catalog> List4Config(GenericRequest request, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List4Config(request))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                if (EmptyFirst) {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Catalog() { CatalogID = 0, CatalogName = "" });
                }

                return _list;
            }
        }

        //public static List<CatalogDetail> ListByParams(string CatalogTag, string Param1, string Param2, string Param3, string Param4, GenericRequest request)
        //{
        //    using (DataTable dt = _rep.ListByParams(CatalogTag, Param1, Param2, Param3, Param4, request))
        //    {
        //        List<CatalogDetail> _list = dt.ConvertToList<CatalogDetail>();
        //        return _list;
        //    }
        //}
        #endregion
    }
}
