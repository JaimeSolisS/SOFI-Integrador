

namespace Core.Entities
{
    public class OperationTaskDetails : TableMaintenance
    {
        public int OperationTaskDetailID { get; set; }
        public int OperationTaskID { get; set; }
        public string Comments { get; set; }
        public int Progress { get; set; }
        public string Options { get; set; }
    }
}
