using Core.Data;
using Core.Entities;
using Core.Entities.MFG;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class DemoldDefectAlertsService
    {
        private static DemoldDefectAlertsRepository _rep;

        static DemoldDefectAlertsService()
        {
            _rep = new DemoldDefectAlertsRepository();
        }

        public static List<DemoldDefectAlert> List(int? DemoldDefectAlertID, int? ShiftID, int? ProductionLineID, int? MoldFamilyID, int? Gross, int? DefectCategory, int? HourInterval, bool? Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.List(DemoldDefectAlertID, ShiftID, ProductionLineID, MoldFamilyID, Gross, DefectCategory, HourInterval, Enabled, request))
            {
                List<DemoldDefectAlert> _list = dt.ConvertToList<DemoldDefectAlert>();
                return _list;
            }
        }

        public static List<DemoldDefectAlert> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, null, null, request))
            {
                List<DemoldDefectAlert> _list = dt.ConvertToList<DemoldDefectAlert>();
                return _list;
            }
        }

        public static GenericReturn Upsert(List<DemoldDefectAlert> DemoldDefectAlerts, GenericRequest request)
        {
            using (DataTable dt = DemoldDefectAlerts.Select(x => new {
                x.DemoldDefectAlertID,
                x.ShiftID,
                x.ProductionLineID,
                x.MoldFamilyID,
                x.LensTypeID,
                x.DefectCategory,
                x.Gross,
                x.HourInterval,
                x.Enabled }).ToList().ConvertToDataTable())
            {
                return _rep.Upsert(dt, request);
            }
        }

    }
}
