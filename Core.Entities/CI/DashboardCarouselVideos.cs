using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DashboardCarouselVideos : TableMaintenance
    {
        public int DashboardCarouselVideoID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int Seq { get; set; }

        public string VideoID
        {
            get
            {
                return "slideVideo" + Seq.ToString();
            }
        }
    }
}
