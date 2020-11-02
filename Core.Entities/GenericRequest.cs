namespace Core.Entities
{
    public class GenericRequest
    {
        public int FacilityID { get; set; }
        public int UserID { get; set; }
        public string CultureID { get; set; }
        public int CompanyID { get; set; }
        public string DeviceUniqueID { get; set; }
        public string OperatorNumber { get; set; }
    }
}
