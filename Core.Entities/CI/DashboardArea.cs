namespace Core.Entities
{
    public class DashboardArea : TableMaintenance
    {
        public int? ParentID { get; set; }
        public string ParentName { get; set; }
        public int? DashboardAreaID { get; set; }
        public int CultureID { get; set; }
        public string CultureCode { get; set; }
        public string FieldValue { get; set; }
        public string Title { get; set; }
        public int? SizeID { get; set; }
        public string SizeClass { get; set; }
        public string SizeText { get; set; }
        public int Seq { get; set; }
        public bool Enabled { get; set; }
    }
}
