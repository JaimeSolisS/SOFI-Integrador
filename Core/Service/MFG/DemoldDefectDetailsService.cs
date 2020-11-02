using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class DemoldDefectDetailsService
    {
        private static DemoldDefectDetailsRepository _rep;

        static DemoldDefectDetailsService()
        {
            _rep = new DemoldDefectDetailsRepository();
        }

        public static List<DemoldDefectDetail> List(int? ShiftID, int? ProductionLineID, int? VATID, string InspectorName,
            DateTime? StartDate, DateTime? EndDate, string DefectCat, int? DefectType, int? DesignID,
            GenericRequest request)
        {
            using (DataTable dt = _rep.List(ShiftID, ProductionLineID, VATID, InspectorName, StartDate, EndDate, null, 
                null, null, null, DefectCat, DefectType.SelectedValue(),DesignID.SelectedValue(), request))
            {
                List<DemoldDefectDetail> _list = dt.ConvertToList<DemoldDefectDetail>();
                return _list;
            }
        }

        public static List<DemoldDefectDetail> ListByProduct(int? ProductionLineID, int? VATID, string InspectorName, int ProductID, DateTime? DefectDate, GenericRequest request)
        {
            using (DataTable dt = _rep.ListByProduct(ProductionLineID, VATID, InspectorName, ProductID, DefectDate, request))
            {
                List<DemoldDefectDetail> _list = dt.ConvertToList<DemoldDefectDetail>();
                return _list;
            }
        }

        public static List<DemoldDefectDetail> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, null, null, null, null,null,null,null, request))
            {
                List<DemoldDefectDetail> _list = dt.ConvertToList<DemoldDefectDetail>();
                return _list;
            }
        }

        public static GenericReturn Delete(int? DemoldDefectDetailID, GenericRequest request)
        {
            return _rep.Delete(DemoldDefectDetailID, request);
        }

        public static GenericReturn Update(int? DemoldDefectDetailID, int? DemoldDefectID, int? ProductID, int? MoldFamilyID, int? BaseID, int? AdditionID, int? SideEyeID, int? Quantity, int? DemoldDefectTypeID, GenericRequest request)
        {
            return _rep.Update(DemoldDefectDetailID, DemoldDefectID, ProductID, MoldFamilyID, BaseID, AdditionID, SideEyeID, Quantity, DemoldDefectTypeID, request);
        }

        public static DataSet DemoldDefectsExportToExcel(int? ShiftID, int? ProductionLineID, int? VATID, string InspectorName,
            DateTime? StartDate, DateTime? EndDate, string DefectCat, int? DefectType,int? DesignID, GenericRequest request)
        {
            using (DataSet ds = _rep.DemoldDefectsExportToExcel(ShiftID, ProductionLineID, VATID, InspectorName, StartDate, EndDate,DefectCat,DefectType.SelectedValue(), DesignID.SelectedValue(), request))
            {
                return ds;
            }
        }
    }
}
