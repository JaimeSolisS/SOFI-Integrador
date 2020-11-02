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
    public class MoldFamilyLensTypesService
    {
        private static MoldFamilyLensTypesRepository _rep;

        static MoldFamilyLensTypesService()
        {
            _rep = new MoldFamilyLensTypesRepository();
        }

        public static List<MoldFamily> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, request))
            {
                List<MoldFamily> _list = dt.ConvertToList<MoldFamily>();
                return _list;
            }
        }

        public static List<MoldFamily> List(int[] MoldFamilyIDs, int[] MoldLenTypesIDs, GenericRequest request)
        {
            string MoldFamilies = null;
            if (MoldFamilyIDs != null)
            {
                MoldFamilies = string.Join<int>(",", MoldFamilyIDs);
            }

            string MoldLenTypes = null;
            if (MoldLenTypesIDs != null)
            {
                MoldLenTypes = string.Join<int>(",", MoldLenTypesIDs);
            }

            using (DataTable dt = _rep.List(MoldFamilies, MoldLenTypes, request))
            {
                List<MoldFamily> _list = dt.ConvertToList<MoldFamily>();
                return _list;
            }
        }

        public static List<LenType> ListOfLens(int MoldFamilyIDs, GenericRequest request)
        {
            using (DataTable dt = _rep.ListOfLens(MoldFamilyIDs, null, request))
            {
                List<LenType> _list = dt.ConvertToList<LenType>();
                return _list;
            }
        }

        public static List<LenType> ListOfLens(int MoldFamilyIDs, int?[] MoldLensTypeIDs, GenericRequest request)
        {
            string MoldLensIDs = null;
            if (MoldLensTypeIDs != null)
            {
                MoldLensIDs = string.Join(",", MoldLensTypeIDs);
            }

            using (DataTable dt = _rep.ListOfLens(MoldFamilyIDs, MoldLensIDs, request))
            {
                List<LenType> _list = dt.ConvertToList<LenType>();
                return _list;
            }
        }

        public static GenericReturn Save(string MoldFamilyName, string LensTypeIDs, bool Enabled, GenericRequest request)
        {
            return _rep.Save(MoldFamilyName, LensTypeIDs, Enabled, request);
        }

        public static GenericReturn Update(int? MoldFamilyID, string MoldFamilyName, string LensTypeIDs, bool Enabled, GenericRequest request)
        {
            return _rep.Update(MoldFamilyID, MoldFamilyName, LensTypeIDs, Enabled, request);
        }

        public static GenericReturn Delete(int? MoldFamilyLensTypeID, GenericRequest request)
        {
            return _rep.Delete(MoldFamilyLensTypeID, request);
        }

        public static GenericReturn SaveExcelImport(DataTable dt, GenericRequest request)
        {
            return _rep.SaveExcelImport(dt, request);
        }

    }
}
