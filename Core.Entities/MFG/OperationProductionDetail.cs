

namespace Core.Entities
{
    public class OperationProductionDetail : TableMaintenance
    {
        public int OperationProductionDetailID { get; set; }
        public int OperationProductionID { get; set; }
        public int Hour { get; set; }
        public string HourFormat { get; set; }
        public decimal ShotNumber { get; set; }
        public decimal ShotCounter { get; set; }
        public string ShotCounterFormat { get; set; }
        public decimal ProducedQty { get; set; }
        public int RejectsQty { get; set; }

        public decimal Accepteds { get; set; }
    }
}
