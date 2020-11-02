using System;


namespace Core.Entities
{
    public class User : GenericRequest
    {
        public int ID { get; set; }
        public string UserAccountID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string eMail { get; set; }
        public bool Enabled { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeNumber { get; set; }
        public int? ShiftID { get; set; }
        public string ShiftDescription { get; set; }
        public int? BaseFacilityID { get; set; }
        public string FacilityName { get; set; }
        public string DefaultCultureID { get; set; }
        public int ChangedBy { get; set; }
        public string ChangedByName { get; set; }
        public DateTime DateLastMaint { get; set; }
        public DateTime DateAdded { get; set; }
        public string NavigationToLoginAs { get; set; }
        public string ProfileArrayNames { get; set; }
        public byte[] Signature { get; set; }
        public string ProfileArrayIds{ get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
        public string DateLastMaintFormat
        {
            get { return DateLastMaint.ToString("yyyy-MM-dd HH:mm"); }
        }

        public string DateAddedFormat
        {
            get { return DateAdded.ToString("yyyy-MM-dd HH:mm"); }
        }
        public int JobPositionID { get; set; }
        public string JobPositionName { get; set; }
    }
}
