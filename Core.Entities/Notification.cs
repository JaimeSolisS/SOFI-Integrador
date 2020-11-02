using System;

namespace Core.Entities
{
    public class Notification : TableMaintenance
    {
        public int ID { get; set; }
        public int FacilityID { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public int DistributionListID { get; set; }
        public string DistributionListName { get; set; }
        public string eMailAddressFrom { get; set; }
        public string Subject { get; set; }
        public string HTMLBody { get; set; }
        public string eMailAddressesTo { get; set; }
        public string eMailAddressesCC { get; set; }
        public string eMailAddressesBC { get; set; }
        public string eMailAttachments { get; set; }
        public string eMailImages { get; set; }
        public string eMailPathReports { get; set; }
        public bool Sent { get; set; }
        public DateTime? SendingDate { get; set; }
        public int Tries { get; set; }
        public string LastErrorMessage { get; set; }
        public string UserName { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string Smtp_EnableSsl { get; set; }
    }
}
