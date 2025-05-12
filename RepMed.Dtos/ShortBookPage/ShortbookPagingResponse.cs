using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.ShortBookPage
{
    public class ShortbookPagingResponse : SP_BASE_RESPONSE
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long PharmacyId { get; set; }
        public long SupplierId { get; set; }
        public long Quantity { get; set; }
        public string Priority { get; set; } // "Low" or "High"
        public string Status { get; set; }   // "Pending", "Ordered", "Delivered"
        public DateTime AddedDate { get; set; }

    }
}
