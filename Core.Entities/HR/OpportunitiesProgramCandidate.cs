using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class OpportunitiesProgramCandidate : TableMaintenance
    {
        public int OPCandidateID { get; set; }
        public int OpportunityProgramID { get; set; }
        public string CandidateID { get; set; }
        public string CandidateName { get; set; }
        public string ClaveDepto { get; set; }
        public string DepartmentName { get; set; }
        public string ShortMessage { get; set; }
        public bool IsDiscarted { get; set; }

    }
}
