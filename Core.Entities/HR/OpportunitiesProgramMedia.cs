using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class OpportunitiesProgramMedia : TableMaintenance
    {
        public int OpportunityProgramMediaID { get; set; }
        public string Path { get; set; }
        public string FileName { get { return System.IO.Path.GetFileName(Path); } set { } }
        public int? Seq { get; set; }
        public string MediaID { get; set; }
        public int Fileid { get; set; }
    }
}