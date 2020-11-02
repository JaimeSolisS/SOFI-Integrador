using Core.Entities.Utilities;

namespace Core.Service
{
    #region namespaces

    using Data;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion

    public static class DepartmentService
    {
        private static DepartmentRepository _rep;

        static DepartmentService()
        {
            _rep = new DepartmentRepository();
        }

        #region CRUD Methods

        public static List<Department> List(int? OrganizationID, int? DepartmentID, bool? Enabled, int FacilityID, int UserID, string CultureID, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.List(OrganizationID, DepartmentID, Enabled, FacilityID, UserID, CultureID))
            {
                List<Department> _list = dt.ConvertToList<Department>();
                if (AddEmptyRecord && _list.Count > 1)
                {
                    _list.Insert(0, new Department { DepartmentID = 0, DepartmentName = "" });
                }
                return _list;
            }
        }
        public static List<Department> List4Facility(int FacilityID, GenericRequest req, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.List4Facility(FacilityID, req))
            {
                List<Department> _list = dt.ConvertToList<Department>();
                if (AddEmptyRecord && _list.Count > 1)
                {
                    _list.Insert(0, new Department { DepartmentID = 0, DepartmentName = "" });
                }
                return _list;
            }
        }

        #endregion

        #region Methods

        public static List<Department> List4Select(GenericRequest req, bool EmptyFirst = true, string EmptyText = "")
        {
            using (DataTable dt = _rep.List(null, null, true, req.FacilityID, req.UserID, req.CultureID))
            {
                List<Department> _list = dt.ConvertToList<Department>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst) {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Department() { DepartmentID = 0, DepartmentName = EmptyText });
                }

                return _list;
            }
        }

        #endregion
    }
}
