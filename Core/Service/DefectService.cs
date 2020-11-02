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
    public class DefectService
    {
        private static DefectRepository _rep;

        static DefectService()
        {
            _rep = new DefectRepository();
        }

        #region Methods

        public static List<Defect> SelectList(int? DefectID, string DefectName, int? ProductionProcessID, bool Enabled, GenericRequest request)
        {
            using (DataTable dt = _rep.Defects_SelectList(DefectID, DefectName, ProductionProcessID, request.FacilityID, request.UserID, request.CultureID, Enabled))
            {
                List<Defect> _list = dt.ConvertToList<Defect>();
                return _list;
            }
        }

        public static List<Defect> List(Defect defect, DefectProcess defectProcess, GenericRequest request, bool EmptyFirst)
        {
            using (DataTable dt = _rep.Defects_List(defect, defectProcess, request))
            {
                List<Defect> _list = dt.ConvertToList<Defect>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Defect() { DefectID = 0, DefectName = "" });
                }
                return _list;
            }
        }
        #endregion
    }
}
