

namespace Core.Entities
{
    public class MaterialSetup : TableMaintenance
    {
        public int? MachineSetupID { get; set; }
        public string MachineSetupName { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
        public double CycleTime { get; set; }
        public int ProductionProcessID { get; set; }
        public string ProductionProcess { get; set; }
    }
}
