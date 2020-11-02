using System;


namespace Core.Entities
{
    public class OperationRecord : TableMaintenance
    {
        public int OperationRecordID { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public string MachineSetupName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public DateTime OperationDate { get; set; }
        public string OperationDateFormat
        {
            get { return OperationDate.ToString("yyyy-MM-dd"); }
        }
        public string JulianDay { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string StatusValue { get; set; }
        public string OperatorNumber { get; set; }
        public string AllowFilters { get; set; }
    }
}
