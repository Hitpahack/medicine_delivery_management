using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage
{
    public class POIWPagingResponse:SP_BASE_RESPONSE
    {
        
        public string ItemName { get; set; }
        public long ProductId { get; set; }
        public long CurrentStock { get; set; }
        public long OrderedQty { get; set; }
        public string OrderedTo { get; set; }
        public string TotalAmount { get; set; }
    }
}
