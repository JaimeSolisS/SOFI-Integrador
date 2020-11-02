using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;

namespace Core.Service
{
    public class MachineParametersService
    {
        private static MachineParametersRepository _rep;
        static MachineParametersService()
        {
            _rep = new MachineParametersRepository();
        }

        public static List<MachineParameters> List(int? MachineParameterID, string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, int? ParameterListID, bool? UseReference, string ReferenceName, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(MachineParameterID, ParameterName, ParameterTypeID, ParameterLength, ParameterPrecision, ParameterListID, UseReference, ReferenceName, IsCavity, Enabled, request))
            {
                List<MachineParameters> _list = dt.ConvertToList<MachineParameters>();
                return _list;
            }
        }

        public static List<MachineParameters> List(int? MachineParameterID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(MachineParameterID, null, null, null, null, null, null, null, null, null, request))
            {
                List<MachineParameters> _list = dt.ConvertToList<MachineParameters>();
                return _list;
            }
        }
        public static List<MachineParameters> CavitiesList(int? MachineParameterID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(MachineParameterID, null, null, null, null, null, null, null, true, null, request))
            {
                List<MachineParameters> _list = dt.ConvertToList<MachineParameters>();
                return _list;
            }
        }

        public static GenericReturn Insert(string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, string ParameterTag, bool? UseReference, string ReferenceName, int? ReferenceTypeID, int? ReferenceListID, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(ParameterName, ParameterTypeID, ParameterLength, ParameterPrecision, ParameterTag, UseReference, ReferenceName.SelectedValue(), ReferenceTypeID.SelectedValue(), ReferenceListID.SelectedValue(), IsCavity, Enabled, request);
        }

        public static GenericReturn Delete(int? MachineParameterID, GenericRequest request)
        {
            return _rep.Delete(MachineParameterID, request);
        }

        public static GenericReturn Update(int? MachineParameterID, string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, int? ParameterListID, bool? UseReference, string ReferenceName, int? ReferenceTypeID, int? ReferenceListID, string FunctionValue, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            return _rep.Update(MachineParameterID, ParameterName, ParameterTypeID, ParameterLength, ParameterPrecision, ParameterListID, UseReference, ReferenceName.SelectedValue(), ReferenceTypeID.SelectedValue(), ReferenceListID.SelectedValue(), FunctionValue, IsCavity, Enabled, request);
        }
    }
}
