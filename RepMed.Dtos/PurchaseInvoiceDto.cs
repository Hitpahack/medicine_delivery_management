using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BasePurchaseInvoiceDto
    {
        [Required]
        public long PharmacyId { get; set; }
        [Required]
        public long SupplierId { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        public string Ponumber { get; set; }
        public decimal? TotalDiscount { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? TaxAmount { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Remarks { get; set; }
        [Required]
        public long CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }

    }
    public class EntityPurchaseInvoiceDto : BasePurchaseInvoiceDto
    {
        public long Id { get; set; }
    }

    public class AddPurchaseInvoiceDto
    {
        public BasePurchaseInvoiceDto Invoice {  get; set; }
        public List<BasePurchaseInvoiceItemDto> Items { get; set; }
    }

    public class FetchPODto
    {
        public long ItemId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal MRP { get; set; }
        public long Quantity { get; set; }

    }

    public class FetchPoRequestDto
    {
        public string PoNumber { get; set; }
    }
}
