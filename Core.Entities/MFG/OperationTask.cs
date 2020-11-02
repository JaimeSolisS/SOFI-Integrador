using System;


namespace Core.Entities
{
    public class OperationTask :TableMaintenance
    {
        public int OperationTaskID { get; set; }
        public int OperationSetupParameterID { get; set; }
        public string MachineName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftDescription { get; set; }
        public string ProblemDescription { get; set; }
        public int? ResponsibleID { get; set; }
        public string SuggestedAction { get; set; }
        public string AttendantUserName { get; set; }
        public string ResponsibleFullName { get; set; }
        public DateTime? TargetDate { get; set; }
        public string TargetDateFormat {
            get {
                if (TargetDate != null)
                {
                    return TargetDate.Value.ToString("yyyy-MM-dd HH:mm");
                }
                else {
                    return "";
                }
                
            }
        }
        public DateTime? ClosedDate { get; set; }
        public string ClosedDateFormat
        {
            get
            {
                if (ClosedDate != null)
                {
                    return ClosedDate.Value.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    return "";
                }

            }
        }
        public int StatusID { get; set; }
        public string StatusValue { get; set; }
        public string StatusName { get; set; }
        public int Options { get; set; }
        public string AllowAssing { get; set; }
        public string AllowCloseTask { get; set; }
        public string AllowCancelTask { get; set; }

        public string OpTaskDetailLastComment { get; set; }
    }
}
