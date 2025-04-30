using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage
{
    public class POPdfContentDto
    {

        // Purchase Order
        public long PurchaseOrderId { get; set; }
        public string PONumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? EDDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Remarks { get; set; }

        // Pharmacy Info
        public long PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyGST { get; set; }
        public string PharmacyAddress1 { get; set; }
        public string PharmacyAddress2 { get; set; }
        public string PharmacyMobile { get; set; }
        public string PharmacyEmail { get; set; }
        public string PharmacyCity { get; set; }
        public string PharmacyState { get; set; }

        // Supplier Info
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierMobile { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierGST { get; set; }

    }
}
