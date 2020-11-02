
using System.Collections.Generic;


namespace Core.Entities
{
    public class OperationSetup : TableMaintenance
    {
        public int OperationSetupID { get; set; }
        public int OperationRecordID { get; set; }
        public int OperationRecordSeq { get; set; }
        public int MaterialID { get; set; }
        public int MachineSetupID { get; set; }
        public int MachineSetupCavities { get; set; }
        public int StatusID { get; set; }
        public string ProductionProcessName { get; set; }
        public List<OperationSetupParameter> Parameters { get; set; }
    }
}
