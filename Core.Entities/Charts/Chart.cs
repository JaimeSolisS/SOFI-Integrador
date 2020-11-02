
using System.Collections.Generic;


namespace Core.Entities
{
    public class Chart : TableMaintenance
    {
        public int ChartID { get; set; }
        public string ChartName { get; set; }
        public int ChartType { get; set; }
        public int Sequence { get; set; }
        public IEnumerable<ChartOption> Options { get; set; }
    }
}
