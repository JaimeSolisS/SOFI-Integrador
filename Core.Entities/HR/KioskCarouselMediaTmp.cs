using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskCarouselMediaTmp : TableMaintenance
    {
        public int KioskCarouselMediaID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int? Seq { get; set; }
        public string MediaID { get; set; }
    }
}
