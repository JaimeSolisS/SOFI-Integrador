using Core.Data;
using Core.Entities;
using Core.Entities.Production;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class DefectProcessService
    {
        private static DefectProcessRepository _rep;

        static DefectProcessService()
        {
            _rep = new DefectProcessRepository();
        }

        #region CRUD

        public static GenericReturn Upsert(DefectProcess defect, GenericRequest request)
        {
            return _rep.Upsert(defect, request);
        }

        public static List<DefectProcess> List(DefectProcess defect, GenericRequest request)
        {
            using (DataTable dt = _rep.List(defect, request))
            {
                List<DefectProcess> _list = dt.ConvertToList<DefectProcess>();
                return _list;
            }
        }

        public static DefectProcess Find(DefectProcess defect, GenericRequest request)
        {
            using (DataTable dt = _rep.List(defect, request))
            {
                List<DefectProcess> _list = dt.ConvertToList<DefectProcess>();
                if (_list != null)
                    return _list.FirstOrDefault();
            }

            return null;
        }


        public static GenericReturn Delete(int DefectProcessID, GenericRequest request)
        {
            return _rep.Delete(DefectProcessID, request);
        }

        public static GenericReturn DeleteAllDetail(int DefectID, GenericRequest request)
        {
            return _rep.DeleteAllDetail(DefectID, request);
        }
        

        #endregion
    }
}
