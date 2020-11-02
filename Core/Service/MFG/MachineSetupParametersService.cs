using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;

namespace Core.Service
{
    public class MachineSetupParametersService
    {
        private static MachineSetupParametersRepository _rep;

        static MachineSetupParametersService()
        {
            _rep = new MachineSetupParametersRepository();
        }

        public static List<MachineSetupParameters> List(int? MachineSetupID, int? ParameterSectionID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(MachineSetupID, ParameterSectionID, null, null, null, null, null, null, null, "", null, null, request))
            {
                List<MachineSetupParameters> _list = dt.ConvertToList<MachineSetupParameters>();
                return _list;
            }
        }

        public static GenericReturn Delete(int? MachineSetupID, int? ParameterSectionID, int? MachineSetupParameterID, GenericRequest request)
        {
            return _rep.Delete(MachineSetupID, ParameterSectionID, MachineSetupParameterID, request);
        }
    }
}
