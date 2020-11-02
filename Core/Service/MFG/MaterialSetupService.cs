using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;

namespace Core.Service
{
    public class MaterialSetupService
    {
        private static MaterialSetupRepository _rep;

        static MaterialSetupService()
        {
            _rep = new MaterialSetupRepository();
        }


        #region Methods
        public static List<MaterialSetup> List (int? MaterialSetupID, int? MachineID, int? MaterialID, int? MachineSetupID, GenericRequest request){
		        using (DataTable dt = _rep.List (MaterialSetupID, MachineID, MaterialID, MachineSetupID, request))
		        {
			        List<MaterialSetup> _list = dt.ConvertToList<MaterialSetup>();
			        return _list;
		        }
        }

        public static List<MaterialSetup> List(int? MachineSetupID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, MachineSetupID, request))
            {
                List<MaterialSetup> _list = dt.ConvertToList<MaterialSetup>();
                return _list;
            }
        }
        #endregion
    }
}