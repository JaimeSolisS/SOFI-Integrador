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
    public class OpportunitiesProgramResponsibleService
    {
        private static OpportunitiesProgramResponsibleRepository _rep;
        static OpportunitiesProgramResponsibleService()
        {
            _rep = new OpportunitiesProgramResponsibleRepository();
        }

        public static List<OpportunitiesProgram_Responsable> List(int? OpportunityProgramID, int? OPResponsableID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OpportunityProgramID, OPResponsableID, request))
            {
                List<OpportunitiesProgram_Responsable> _list = dt.ConvertToList<OpportunitiesProgram_Responsable>();
                return _list;
            }
        }

        public static List<OpportunitiesProgram_Responsable> List(int? OpportunityProgramID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OpportunityProgramID, null, request))
            {
                List<OpportunitiesProgram_Responsable> _list = dt.ConvertToList<OpportunitiesProgram_Responsable>();
                return _list;
            }
        }

        public static GenericReturn Delete(int? OPResponsableID, GenericRequest request)
        {
            return _rep.Delete(OPResponsableID, request);
        }
    }
}
