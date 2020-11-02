using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class MachineSetupService
    {
        private static MachineSetupRepository _rep;

        static MachineSetupService()
        {
            _rep = new MachineSetupRepository();
        }
        public static List<MachineSetup> List(int? MachineID, int? MaterialID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, null, MachineID.SelectedValue(), MaterialID.SelectedValue(), req))
            {
                List<MachineSetup> _list = dt.ConvertToList<MachineSetup>();
                return _list;
            }
        }
        public static List<MachineSetup> List(string MachineSetupName, int? MachineID, int? MaterialID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, MachineSetupName, null, MachineID.SelectedValue(), MaterialID.SelectedValue(), req))
            {
                List<MachineSetup> _list = dt.ConvertToList<MachineSetup>();
                return _list;
            }
        }

        public static GenericReturn Delete(int? MachineSetupID, GenericRequest request)
        {
            return _rep.Delete(MachineSetupID, request);
        }

        #region Methods
        public static List<MachineSetup> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, null, true, null, null, req))
            {
                List<MachineSetup> _list = dt.ConvertToList<MachineSetup>();
                return _list;
            }
        }

        public static List<MachineSetup> List(int? MachineSetupID, string MachineSetupName, bool? Enabled, GenericRequest req)
        {
            using (DataTable dt = _rep.List(MachineSetupID, MachineSetupName, Enabled, null, null, req))
            {
                List<MachineSetup> _list = dt.ConvertToList<MachineSetup>();
                return _list;
            }
        }


        public static GenericReturn Upsert(int? MachineSetupID, string MachineSetupName, bool? Enabled, List<MaterialSetup> MaterialSetupList, List<MachineSetupParameters> MachineSetupParametrsList, List<MachineSetupParameters> TempListDeletedSections, GenericRequest request)
        {
            using (DataTable dt = MaterialSetupList.Select(x => new { x.MachineID, x.MaterialID, x.CycleTime, x.ProductionProcessID }).ToList().ConvertToDataTable())
            {
                using (DataTable dt2 = MachineSetupParametrsList.Select(x => new
                {
                    x.MachineSetupID,
                    x.ParameterSectionID,
                    x.Seq,
                    x.MachineParameterID,
                    x.ParameterUoMID,
                    x.IsMandatory,
                    x.IsAlert,
                    x.MinValue,
                    x.MaxValue,
                    x.FunctionValue,
                    x.FunctionMinValue,
                    x.FunctionMaxValue
                }).ToList().ConvertToDataTable())

                using (DataTable dt3 = TempListDeletedSections.Select(x => new
                {
                    x.MachineSetupID,
                    x.ParameterSectionID
                }).ToList().ConvertToDataTable())

                {
                    return _rep.Upsert(MachineSetupID, MachineSetupName, Enabled, dt, dt2, dt3, request);
                }
            }
        }
        #endregion
    }
}