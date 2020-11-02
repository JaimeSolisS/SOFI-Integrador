

namespace Core.Entities
{
    public class Machine : TableMaintenance
    {
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public string MachineDescription { get; set; }
        public int ProductionLineID { get; set; }
        public string ImagePath { get; set; }
        public string RelatedLink { get; set; }
        public bool Enabled { get; set; }
        public string AllowFilters { get; set; }
        public string ProductionLineName { get; set; }
        public int MachineCategoryID { get; set; }
        public string MachineCategoryName { get; set; }
    }
}
