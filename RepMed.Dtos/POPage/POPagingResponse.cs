using System;

namespace RepMed.Dtos.POPage
{
    public class POPagingResponse : SP_BASE_RESPONSE
    {
        public long PurchaseOrderId { get; set; }
        public long PharmacyId { get; set; }
        public long SupplierId { get; set; }
        public string PONumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }        
        public string SupplierName { get; set; }
        public long Available{ get; set; }
        public long Fulfilled{ get; set; }
        public long Amount{ get; set; }
        
    }
}


