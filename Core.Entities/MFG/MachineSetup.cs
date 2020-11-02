

namespace Core.Entities
{
    public class MachineSetup : TableMaintenance
    {
        public int MachineSetupID { get; set; }
        public string MachineSetupName { get; set; }
        public bool? Enabled { get; set; }
        public string EnableText { get; set; }
    }
}
