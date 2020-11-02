using Core.Data;
using Core.Entities;
using Core.Entities.Interface;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class DataInterfacesConnectionParametersService
    {
        private static DataInterfacesConnectionParametersRepository _rep;

        static DataInterfacesConnectionParametersService()
        {
            _rep = new DataInterfacesConnectionParametersRepository();
        }
        public static List<DataInterfaceConnectionParameters> List( int? DataInterfaceID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, DataInterfaceID, null, null, request))
            {
                List<DataInterfaceConnectionParameters> _list = dt.ConvertToList<DataInterfaceConnectionParameters>();
                return _list;
            }
        }
        public static List<DataInterfaceConnectionParameters> List (int? DataInterfaceConectionParameterID, int? DataInterfaceID, string ParameterName, string ParameterValue, GenericRequest request)
        {
		    using (DataTable dt = _rep.List (DataInterfaceConectionParameterID, DataInterfaceID, ParameterName, ParameterValue, request))
		    {
			    List<DataInterfaceConnectionParameters> _list = dt.ConvertToList<DataInterfaceConnectionParameters>();
			    return _list;
		    }
        }
    }
}
