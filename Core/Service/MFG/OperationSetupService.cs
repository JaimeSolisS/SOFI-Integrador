using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Core.Service
{
    public class OperationSetupService
    {
        private static OperationSetupRepository _rep;

        static OperationSetupService()
        {
            _rep = new OperationSetupRepository();
        }
        public static OperationSetup Get(int? OperationRecordID,  GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, OperationRecordID, null, null, null, null, req))
            {
                OperationSetup _entity = dt.ConvertToList<OperationSetup>().OrderByDescending(o => o.OperationSetupID).FirstOrDefault();
                if (_entity != null)
                {
                    _entity.Parameters = ParameterList(_entity.OperationSetupID, req);
                }
                
                return _entity;
            }
        }
        public static List<OperationSetup> List(int? OperationSetupID, int? OperationRecordID, int? OperationRecordSeq, int? MaterialID, int? MachineSetupID, int? StatusID,GenericRequest req)
        {
            using (DataTable dt = _rep.List(OperationSetupID, OperationRecordID, OperationRecordSeq, MaterialID, MachineSetupID, StatusID, req))
            {
                List<OperationSetup> _list = dt.ConvertToList<OperationSetup>();

                return _list;
            }
        }

        public static OperationSetupParameter GetParameter(int? OperationSetupParameterID, GenericRequest req)
        {
            using (DataTable dt = _rep.ParameterGet(OperationSetupParameterID, null, req))
            {
                List<OperationSetupParameter> _list = dt.ConvertToList<OperationSetupParameter>();
                return _list.FirstOrDefault();
            }
        }
        public static List<OperationSetupParameter> ParameterList(int? OperationSetupID,  GenericRequest req)
        {
            using (DataTable dt = _rep.ParameterList(null, OperationSetupID, null, null, null, null, null, null, null, null, null, null, null, req))
            {
                List<OperationSetupParameter> _list = dt.ConvertToList<OperationSetupParameter>();
                return _list;
            }
        }
        public static List<OperationSetupParameter> ParameterList(int? OperationSetupParameterID, int? OperationSetupID, int? ParameterSectionID, int? Seq, int? OperationParameterID, int? ParameterUoMID, bool? IsMandatory, bool? UseReference, string Reference, decimal? MinValue, decimal? MaxValue, string Value, string ValueList, GenericRequest req)
        {
            using (DataTable dt = _rep.ParameterList(OperationSetupParameterID, OperationSetupID, ParameterSectionID, Seq, OperationParameterID, ParameterUoMID, IsMandatory, UseReference, Reference, MinValue, MaxValue, Value, ValueList, req))
            {
                List<OperationSetupParameter> _list = dt.ConvertToList<OperationSetupParameter>();
                return _list;
            }
        }
        public static GenericReturn ParametersBulkUpsert(int OperationSetupID, List<OperationSetupParameter> list, GenericRequest request)
        {
            using (DataTable dt = list.Select(s=> new {
                s.OperationSetupParameterID,
                s.OperationSetupID,
                s.ParameterSectionID,
                s.Seq,
                s.MachineParameterID,
                s.ParameterUoMID,
                s.IsMandatory,
                s.UseReference,
                s.Reference,
                s.MinValue,
                s.MaxValue,
                s.Value,
                s.ValueList
            }).ToList().ConvertToDataTable())
            {
                return _rep.Parameters_BulkUpsert(OperationSetupID, dt, request);
            }
        }

    }
}
