using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage.POItems
{
    public class POIWItemsPagingResponse :SP_BASE_RESPONSE
    {
        public long POId { get; set; }
        public string PONumber { get; set; }
        public string Status { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public long PharmacyId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
