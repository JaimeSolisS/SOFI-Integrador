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
    public class OperationSetupParametersService
    {
        private static OperationSetupParametersRepository _rep;
        static OperationSetupParametersService()
        {
            _rep = new OperationSetupParametersRepository();
        }

        public static List<OperationSetupParameter> List(int? OperationSetupParameterID, int? OperationSetupID, int? ParameterSectionID, int? Seq, int? OperationParameterID, int? ParameterUoMID, bool? IsMandatory, bool? UseReference, string Reference, decimal? MinValue, decimal? MaxValue, string Value, string ValueList, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OperationSetupParameterID, OperationSetupID, ParameterSectionID, Seq, OperationParameterID, ParameterUoMID, IsMandatory, UseReference, Reference, MinValue, MaxValue, Value, ValueList, request))
            {
                List<OperationSetupParameter> _list = dt.ConvertToList<OperationSetupParameter>();
                return _list;
            }
        }

        public static List<OperationSetupParameter> List(int? OperationSetupParameterID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OperationSetupParameterID, null, null, null, null, null, null, null, null, null, null, null, null,  request))
            {
                List<OperationSetupParameter> _list = dt.ConvertToList<OperationSetupParameter>();
                return _list;
            }
        }

        public static OperationSetupParameter Get(int? OperationSetupParameterID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OperationSetupParameterID, null, null, null, null, null, null, null, null, null, null, null, null, request))
            {
                List<OperationSetupParameter> _list = dt.ConvertToList<OperationSetupParameter>();
                return _list.FirstOrDefault();
            }
        }
    }
}
