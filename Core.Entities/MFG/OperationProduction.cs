

namespace Core.Entities
{
    public class OperationProduction : TableMaintenance
    {
        public int OperationProductionID { get; set; }
        public int OperationRecordID { get; set; }
        public decimal? CycleTime { get; set; }
        public int CavitiesNumber { get; set; }
        public int ProducedQty { get; set; }
        public int InitialShotNumber { get; set; }
        public int FinalShotNumber { get; set; }
        public int TotalShotNumber { get; set; }
        public int TotalPieces { get; set; }
        public string ProductName { get; set; }
        public string MaterialName { get; set; }
        public int Shots { get; set; }
        public int RejectQty { get; set; }
    }
}
