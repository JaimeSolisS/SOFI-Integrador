#region namespaces
using Core.Data;
using Core.Entities;
using System.Collections.Generic;
using System.Data;
using Core.Entities.Utilities;

#endregion

namespace Core.Service
{
    public class FacilityService
    {
        private static FacilityRepository _rep;

        static FacilityService()
        {
            _rep = new FacilityRepository();
        }

        #region CRUD Methods

        public static List<Facility> List(int? CompanyID, int? FacilityID, bool? Enabled, int CurrentFacilityID, int UserID, string CultureID)
        {
            using (DataTable dt = _rep.List(CompanyID, FacilityID, Enabled, CurrentFacilityID, UserID, CultureID))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                return _list;
            }
        }

        public static Facility Get(int CompanyID, GenericRequest request)
        {
            using (DataTable dt = _rep.Get(CompanyID, request))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (_list == null)
                    return new Facility();
                else
                    return _list[0];
            }
        }

        public static GenericReturn QuickUpdate(int? FacilityID, string ColumnName, string Value, GenericRequest request)
        {
            return _rep.QuickUpdate(FacilityID, ColumnName, Value, request);
        }

        #endregion

        #region Methods

        public static List<Facility> List4Select(GenericRequest req, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List(null, null, true, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                }
                return _list;
            }
        }

        public static List<Facility> List4Select(int CompanyID, GenericRequest req, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List(CompanyID, null, true, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                }
                return _list;
            }
        }


        public static List<Facility> List4Config(int CompanyID, GenericRequest req, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List4Config(CompanyID, EmptyFirst, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                }
                return _list;
            }
        }

        public static List<Facility> List4Config(int OrganizationID, int? CompanyID, GenericRequest req, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List4Config(OrganizationID, CompanyID, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                }
                return _list;
            }
        }

        public static List<Facility> ListUserAccess(int? CompanyID, GenericRequest request, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.ListUserAccess(CompanyID, request))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                }
                return _list;
            }
        }

        public static List<Facility> ListUserAccessByFacility(GenericRequest request, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.ListUserAccessByFacility(request))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    if (_list.Count > 1)
                    {
                        // Anexar fila vacia al principio
                        _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                    }
                }
                return _list;
            }
        }
        #endregion
    }
}
