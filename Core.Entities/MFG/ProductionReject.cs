

namespace Core.Entities
{
    public class ProductionReject : TableMaintenance
    {
        public int ProductionRejectID { get; set; }
        public int ReferenceID { get; set; }
        public int ReferenceTypeID { get; set; }
        public int RejectTypeID { get; set; }
        public string RejectTypeName { get; set; }
        public decimal? Quantity { get; set; }
        public int Hour { get; set; }
    }
}
