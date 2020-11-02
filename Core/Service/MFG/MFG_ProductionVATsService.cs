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
    public class MFG_ProductionVATsService
    {
        private static MFG_ProductionVATsRepository _rep;

        static MFG_ProductionVATsService()
        {
            _rep = new MFG_ProductionVATsRepository();
        }

        public static List<ProductionVAT> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, request))
            {
                List<ProductionVAT> _list = dt.ConvertToList<ProductionVAT>();
                return _list;
            }
        }

        public static List<ProductionVAT> List(int VATID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(VATID, null, null, null, null, request))
            {
                List<ProductionVAT> _list = dt.ConvertToList<ProductionVAT>();
                return _list;
            }
        }

        public static List<ProductionVAT> List(string VatName, int[] ShiftIDs, int? ProductionProcessID, int[] ProductionLineIDs, GenericRequest request)
        {

            string ShiftID = null;
            if (ShiftIDs != null)
            {
                ShiftID = string.Join<int>(",", ShiftIDs);
            }

            string ProductionLineID = null;
            if (ProductionLineIDs != null)
            {
                ProductionLineID = string.Join<int>(",", ProductionLineIDs);
            }

            using (DataTable dt = _rep.List(null, VatName, ShiftID, ProductionProcessID, ProductionLineID, true, request))
            {
                List<ProductionVAT> _list = dt.ConvertToList<ProductionVAT>();
                return _list;
            }
        }

        public static List<ProductionVAT> List(int[] ShiftIDs, int[] ProductionLineIDs, GenericRequest request)
        {

            string ShiftID = null;
            if (ShiftIDs != null)
            {
                ShiftID = string.Join<int>(",", ShiftIDs);
            }

            string ProductionLineID = null;
            if (ProductionLineIDs != null)
            {
                ProductionLineID = string.Join<int>(",", ProductionLineIDs);
            }

            using (DataTable dt = _rep.List(null, null, ShiftID, null, ProductionLineID, true, request))
            {
                List<ProductionVAT> _list = dt.ConvertToList<ProductionVAT>();
                return _list;
            }
        }
        public static List<ProductionVAT> List(string ShiftID, string ProductionLineID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, ShiftID.SelectedValue(), null, ProductionLineID.SelectedValue(), true, request))
            {
                List<ProductionVAT> _list = dt.ConvertToList<ProductionVAT>();
                return _list;
            }
        }

        public static GenericReturn Delete(int? VATID, GenericRequest request)
        {
            return _rep.Delete(VATID, request);
        }

        public static GenericReturn Insert(string VATName, int? ShiftID, int? ProductionLineID, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(VATName, ShiftID, ProductionLineID, Enabled, request);
        }

        public static GenericReturn Update(int? VATID, string VATName, int? ShiftID, int? ProductionLineID, bool? Enabled, GenericRequest request)
        {
            return _rep.Update(VATID, VATName, ShiftID, ProductionLineID, Enabled, request);
        }
    }
}
