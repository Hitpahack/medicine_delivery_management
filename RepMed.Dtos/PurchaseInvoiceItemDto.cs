using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BasePurchaseInvoiceItemDto
    {
        [Required]
        public long PurchaseInvoiceId { get; set; }
        [Required]

        public long ProductId { get; set; }

        public string Manufacturer { get; set; }

        public string BatchNumber { get; set; }
        [Required]
        public long QuantityPurchased { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }

        public decimal? Mrp { get; set; }
        public decimal? SellingPrice { get; set; }
        [Required]
        public bool? Gstincluded { get; set; }
        public decimal? Gstpercentage { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal? Gstamount { get; set; }
        [Required]
        public decimal? Discount { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
    }
    public class EntityPurchaseInvoiceItemDto
    {
        public long Id { get; set; }
    }
}
