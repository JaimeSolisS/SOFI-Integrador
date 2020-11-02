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
    public class OpportunitiesProgramCandidatesService
    {
        private static OpportunitiesProgramCandidatesRepository _rep;
        static OpportunitiesProgramCandidatesService()
        {
            _rep = new OpportunitiesProgramCandidatesRepository();
        }

        public static List<OpportunitiesProgramCandidate> List(int? OpportunityProgramID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OpportunityProgramID, request))
            {
                List<OpportunitiesProgramCandidate> _list = dt.ConvertToList<OpportunitiesProgramCandidate>();
                return _list;
            }
        }

        public static GenericReturn Update(int? OPCandidateID, int? OpportunityProgramID, string CandidateID, string ShortMessage, bool? IsDiscarted, GenericRequest request)
        {
            return _rep.Update(OPCandidateID, OpportunityProgramID, CandidateID, ShortMessage, IsDiscarted, request);
        }

        public static GenericReturn Insert(int? OpportunityProgramID, string CandidateID, string ShortMessage, bool? IsDiscarted, GenericRequest request)
        {
            return _rep.Insert(OpportunityProgramID, CandidateID, ShortMessage, IsDiscarted, request);
        }
    }
}
