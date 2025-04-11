using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Pharmacyinventory
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public long ProductId { get; set; }

    public long? PharmacyPurchaseId { get; set; }

    public DateTime ExpiryDate { get; set; }

    public long CurrentStock { get; set; }

    public long MinimumStockThreshold { get; set; }

    public decimal? SellingPrice { get; set; }

    public string Description { get; set; }

    public DateTime? LastRestocked { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual Pharmacypurchase PharmacyPurchase { get; set; }

    public virtual Product Product { get; set; }
}
