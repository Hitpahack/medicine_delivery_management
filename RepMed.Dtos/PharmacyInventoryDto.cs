using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BasePharmacyInventoryDto
    {
        [Required]
        public long PharmacyId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public long PurchaseInvoiceId { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }

        public long CurrentStock { get; set; }

        public long MinimumStockThreshold { get; set; }

        public decimal? SellingPrice { get; set; }

        public string Description { get; set; }

        public DateTime? LastRestocked { get; set; }

        public DateTime? LastUpdated { get; set; }

    }
    public class EntityPharmacyInventoryDto : BasePharmacyInventoryDto
    {
        public long Id { get; set; }

    }
}
