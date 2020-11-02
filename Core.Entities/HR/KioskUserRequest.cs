using System;

namespace Core.Entities
{
    public class KioskUserRequest : TableMaintenance
    {
        public int RequestID { get; set; }
        public string RequestNumber { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestTypeName { get; set; }
        public string RequestedByID { get; set; }
        public string RequestedName { get; set; }
        public string UserName { get; set; }
        public string RequestedEmail { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public int StatusID { get; set; }
        public string RequestStatusName { get; set; }
        public string StatusValueID { get; set; }
        public int ResponsibleID { get; set; }
        public string ResponsibleName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftDescription { get; set; }
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public int PageCount { get; set; }
        public string AllowViewAllRequests { get; set; }
        public string AllowAssing { get; set; }
        public string AllowMarkDone { get; set; }
        public string AllowCancel { get; set; }
        public string AllowClose { get; set; }
        public string AllowReOpen { get; set; }
    }
}
