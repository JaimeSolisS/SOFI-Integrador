using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Core.Service
{
    public class MachineService
    {
        private static MachineRepository _rep;
        static MachineService()
        {
            _rep = new MachineRepository();
        }

        #region Methods
        public static List<Machine> List4Select(GenericRequest req, string ResourcesAll, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.List(null, null, null, null, true, req))
            {
                List<Machine> _list = dt.ConvertToList<Machine>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Machine() { MachineID = 0, MachineName = ResourcesAll });
                }
                return _list;
            }
        }
        public static List<Machine> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, null, null, true, req))
            {
                List<Machine> _list = dt.ConvertToList<Machine>();
                return _list;
            }
        }
        public static List<Machine> GetSpecificMachine(int MachineID, GenericRequest req)
        {
            {
                using (DataTable dt = _rep.List4Select(MachineID, null, null, null, true, req))
                {
                    List<Machine> _list = dt.ConvertToList<Machine>();
                    return _list;
                }
            }
        }
        public static Machine Get(int? MachineID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(MachineID, null, null, null, null, req))
            {
                List<Machine> _list = dt.ConvertToList<Machine>();
                return _list.FirstOrDefault();
            }
        }
        public static List<Machine> List(int? MachineID, string MachineName, string MachineDescription, int? ProductionLineID, bool? Enabled, GenericRequest req)
        {
            using (DataTable dt = _rep.List(MachineID, MachineName, MachineDescription, ProductionLineID, Enabled, req))
            {
                List<Machine> _list = dt.ConvertToList<Machine>();
                return _list;
            }
        }
        public static List<DashboardMachine> Dashboard_List(DateTime? OperationDate, int? ShiftID, GenericRequest req)
        {
            using (DataTable dt = _rep.Dashboard_List(OperationDate, ShiftID, req))
            {
                List<DashboardMachine> _list = dt.ConvertToList<DashboardMachine>();
                return _list;
            }
        }
        public static List<DashboardMachine> Dashboard_List(GenericRequest req)
        {
            using (DataTable dt = _rep.Dashboard_List(null, null, req))
            {
                List<DashboardMachine> _list = dt.ConvertToList<DashboardMachine>();
                return _list;
            }
        }
        public static List<DashboardOperationParamater> Dashboard_OperationParameters_List(int? OperationRecordID, GenericRequest request)
        {
            using (DataTable dt = _rep.Dashboard_Parameters_List(OperationRecordID, request))
            {
                List<DashboardOperationParamater> _list = dt.ConvertToList<DashboardOperationParamater>();
                return _list;
            }
        }
        public static GenericReturn Insert(string MachineName, string MachineDescription, int? ProductionLineID, bool? Enabled, int? MachineCategoryID, GenericRequest request)
        {
            return _rep.Insert(MachineName, MachineDescription, ProductionLineID, Enabled, MachineCategoryID, request);
        }
        public static GenericReturn Delete(int? MachineID, GenericRequest request)
        {
            return _rep.Delete(MachineID, request);
        }
        public static GenericReturn Update(int? MachineID, string MachineName, string MachineDescription, int? ProductionLineID, string ImagePath, bool? Enabled, int? MachineCategoryID, GenericRequest request)
        {
            return _rep.Update(MachineID, MachineName, MachineDescription, ProductionLineID, ImagePath, Enabled, MachineCategoryID, request);
        }

        #endregion
    }
}
