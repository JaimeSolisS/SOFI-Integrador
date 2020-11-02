using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Equipment : TableMaintenance
    {
        public int EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentDescription { get; set; }
        public string UPC { get; set; }
        public string IPAddress1 { get; set; }
        public string IPAddress2 { get; set; }
        public string Position { get; set; }
        public int Extension { get; set; }
        public string Number { get; set; }
        public int EquipmentTypeID { get; set; }
        public string EquipmentTypeName { get; set; }
        public string Comments { get; set; }
        public int SiteID { get; set; }
        public int CategoryID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int UseID { get; set; }
        public int SupervisorID { get; set; }
        public int ModelID { get; set; }
        public string Serial { get; set; }
        public int Year { get; set; }
        public bool Active { get; set; }
        public string BankReference { get; set; }
        public decimal? Credit { get; set; }
        public int MaintenanceTypeID { get; set; }
        public int EquipmentReferenceID { get; set; }
        public int AreaID { get; set; }
        public int SavID { get; set; }
        public string Color { get; set; }
        public int OS_ID { get; set; }
        public string ModelVisitor { get; set; }
        public string MarkVisitor { get; set; }
        public string CompanyVisitor { get; set; }
        public string UseVisitor { get; set; }
        public decimal? AvailabilityTarget { get; set; }
        public bool IsLeasing { get; set; }
        public int ReferenceTypeID { get; set; }

        public DateTime? LeasingStart { get; set; }
        public DateTime? LeasingEnd { get; set; }
        public int FacilityID { get; set; }
    }
}
