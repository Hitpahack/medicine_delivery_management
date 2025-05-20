using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage.POItems
{
    public class POOWItemsPagingResponse : SP_BASE_RESPONSE
    {
        public long POId { get; set; }
        public string ItemName { get; set; }
        public long CurrentStock { get; set; }
        public string? StockAvailability { get; set; }
        public string Status { get; set; }
        public decimal MRP { get; set; }
        public decimal PTR { get; set; }
        public long RequiredQty { get; set; }
        public decimal Amount { get; set; }
    }
}
