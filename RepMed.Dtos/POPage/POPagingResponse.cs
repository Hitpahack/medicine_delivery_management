using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage
{
    public class POPagingResponse : SP_BASE_RESPONSE
    {
        public long PurchaseOrderId { get; set; }
        public long PharmacyId { get; set; }
        public long SupplierId { get; set; }
        public string PONumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? EDDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Remarks { get; set; }
        public string SupplierName { get; set; }
    }
}


