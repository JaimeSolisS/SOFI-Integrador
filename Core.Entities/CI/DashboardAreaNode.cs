using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DashboardAreaNode : DashboardAreaDetail
    {
        public string SeqNumber { get; set; }
        public string SectionName { get; set; }
        public string BackColorHex { get; set; }
        public string FontColorHex { get; set; }
        public bool HasChildren { get; set; }
        public int SectionSeq { get; set; }
    }
}
