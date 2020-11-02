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
    public class DemoldDefectsService
    {
        private static DemoldDefectsRepository _rep;

        static DemoldDefectsService()
        {
            _rep = new DemoldDefectsRepository();
        }

        public static List<DemoldDefects> List(int? DemoldDefectID, int? ProductionLineID, int? ShiftID, string InspectorName, int? StatusID, int? VATID, DateTime? DefectDate, GenericRequest request)
        {
            using (DataTable dt = _rep.List(DemoldDefectID, ProductionLineID, ShiftID, InspectorName, StatusID, VATID, DefectDate, request))
            {
                List<DemoldDefects> _list = dt.ConvertToList<DemoldDefects>();
                return _list;
            }
        }

        public static List<DemoldDefects> List(int DemoldDefectID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(DemoldDefectID, null, null, null, null, null, null, request))
            {
                List<DemoldDefects> _list = dt.ConvertToList<DemoldDefects>();
                return _list;
            }
        }

        public static List<Catalog> GetCategoriesAndData(string CatalogTag, string Category, GenericRequest request)
        {
            using (DataTable dt = _rep.GetCategoriesAndData(CatalogTag, Category, request))
            {
                List<Catalog> _list = dt.ConvertToList<Catalog>();
                return _list;
            }
        }

        public static GenericReturn ValidateToNextFase(int? CaseControl, string LenType, string Base, string Addition, string Side, string Vat, int ProductionProcessID, string Line, GenericRequest request)
        {
            return _rep.ValidateToNextFase(CaseControl, LenType, Base, Addition.SelectedValue(), Side.SelectedValue(), Vat, ProductionProcessID, Line, request);
        }

        public static GenericReturn Insert(DemoldDefects DemoldDefectEntity, List<DemoldDefectDetail> DefectMoldDetailsEntities, DateTime? DefectDate, int ProductID, int LensGross, GenericRequest request)
        {
            using (DataTable dt = DefectMoldDetailsEntities.Select(x => new { x.ProductID, x.MoldFamilyID, x.LensTypeID, x.BaseID, x.AdditionID, x.SideEyeID, x.Quantity, x.DemoldDefectTypeID, x.InspectorNameDetail }).ToList().ConvertToDataTable())
            {
                return _rep.Insert(DemoldDefectEntity.ProductionLineID, DemoldDefectEntity.ShiftID, DemoldDefectEntity.InspectorName, DemoldDefectEntity.VATID, dt, DefectDate, ProductID, LensGross, request);
            }
        }

        public static List<ValidateProductLen> ValidateProductLens(string LenType, string Base, string Addition, string Side)
        {
            using (DataTable dt = _rep.ValidateProductLens(LenType, Base, Addition, Side))
            {
                List<ValidateProductLen> _list = dt.ConvertToList<ValidateProductLen>();
                return _list;
            }
        }

        public static List<Catalog> GetMoldFamiliesByFacility(string VAT, int ProductionProcessID, string Line, GenericRequest request)
        {
            List<Catalog> _list = new List<Catalog>();
            try
            {
                using (DataTable dt = _rep.GetMoldFamiliesByFacility(VAT, ProductionProcessID, Line, request))
                {
                    _list = dt.ConvertToList<Catalog>();
                }
            }
            catch (Exception)
            {

            }
            return _list;
        }

        public static List<string> FnGetDailyProductLensProduction(string VAT, string ProductionProcess, string Line, GenericRequest request)
        {
            DataTable dt = _rep.FnGetDailyProductLensProduction(VAT, ProductionProcess, Line, request);
            List<string> _list = new List<string>();

            _list = dt.AsEnumerable().Select(r => r.Field<string>("LenType")).Distinct().ToList();
            return _list;
        }

    }
}
