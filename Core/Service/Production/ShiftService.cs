using Core.Data;
using Core.Entities;
using Core.Entities.Production;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class ShiftService
    {
        private static ShiftRepository _rep;

        static ShiftService()
        {
            _rep = new ShiftRepository();
        }
        #region Methods
        public static List<ShiftsMaster> List4Select(GenericRequest req, string ResourcesAll, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List(null, true, req))
            {
                List<ShiftsMaster> _list = dt.ConvertToList<ShiftsMaster>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = ResourcesAll });
                }
                return _list;
            }
        }
        public static List<ShiftsMaster> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, true, req))
            {
                List<ShiftsMaster> _list = dt.ConvertToList<ShiftsMaster>();
                return _list;
            }
        }
        public static List<ShiftsMaster> List(int? ShiftID, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(ShiftID, Enabled, request))
            {
                List<ShiftsMaster> _list = dt.ConvertToList<ShiftsMaster>();
                return _list;
            }
        }
        public static ShiftsMaster Get(int? ShiftID, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(ShiftID, Enabled, request))
            {
                List<ShiftsMaster> _list = dt.ConvertToList<ShiftsMaster>();
                return _list.FirstOrDefault();
            }
        }

        public static List<ShiftsMaster> GetShiftsByAvailableFacilities(GenericRequest request)
        {
            using (DataTable dt = _rep.GetShiftsByAvailableFacilities(request))
            {
                List<ShiftsMaster> _list = dt.ConvertToList<ShiftsMaster>();
                return _list;
            }
        }

        public static List<ShiftsMaster> GetShiftsByAvailableFacilities4Select(GenericRequest request, string ResourcesAll, bool EmptyFirst = true)
        {

            using (DataTable dt = _rep.GetShiftsByAvailableFacilities(request))
            {
                List<ShiftsMaster> _list = dt.ConvertToList<ShiftsMaster>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new ShiftsMaster() { ShiftID = 0, ShiftDescription = ResourcesAll });
                }
                return _list;
            }

        }

        #endregion
    }
}
