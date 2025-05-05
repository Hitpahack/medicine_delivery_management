using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Purchaseinvoiceitem
{
    public long Id { get; set; }

    public long PurchaseInvoiceId { get; set; }

    public long ProductId { get; set; }

    public string Manufacturer { get; set; }

    public string BatchNumber { get; set; }

    public long QuantityPurchased { get; set; }

    public string Unit { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal? Mrp { get; set; }

    public decimal? SellingPrice { get; set; }

    public bool? Gstincluded { get; set; }

    public decimal? Gstpercentage { get; set; }

    public decimal? Gstamount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? Discount { get; set; }

    public DateTime ExpiryDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; }

    public virtual Purchaseinvoice PurchaseInvoice { get; set; }
}
