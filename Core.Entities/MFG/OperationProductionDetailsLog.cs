

namespace Core.Entities
{
    public class OperationProductionDetailsLog : TableMaintenance
    {
        public int OperationProductionDetailLogID { get; set; }
        public int OperationProductionID { get; set; }
        public int CurrentShotNumber { get; set; }
    }
}
