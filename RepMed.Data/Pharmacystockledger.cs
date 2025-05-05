using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Pharmacystockledger
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public long ProductId { get; set; }

    public long PurchaseInvoiceId { get; set; }

    public long QuantityAdded { get; set; }

    public decimal SellingPriceAtTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual Product Product { get; set; }

    public virtual Purchaseinvoice PurchaseInvoice { get; set; }
}
