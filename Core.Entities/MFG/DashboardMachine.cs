using System;


namespace Core.Entities
{
    public class DashboardMachine : TableMaintenance
    {
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public bool Enabled { get; set; }
        public int OperationRecordID { get; set; }
        public int ShiftID { get; set; }
        public DateTime OperationDate { get; set; }
        public string ImagePath { get; set; }
        public string RelatedLink { get; set; }
        public bool isAlert { get; set; }
        public string AlertCssClass { get; set; }
        public int MachineCategoryID { get; set; }
        public string MachineCategoryName { get; set; }

    }
}
