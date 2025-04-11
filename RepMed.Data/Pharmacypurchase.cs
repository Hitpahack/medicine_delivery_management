using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Pharmacypurchase
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public long ProductId { get; set; }

    public string SupplierName { get; set; }

    public string Manufacturer { get; set; }

    public long QuantityPurchased { get; set; }

    public decimal ProductPrice { get; set; }

    public bool? Gstincluded { get; set; }

    public decimal? Gstpercentage { get; set; }

    public decimal? Gstamount { get; set; }

    public decimal? TotalCost { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string InvoiceNumber { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual ICollection<Pharmacyinventory> Pharmacyinventories { get; set; } = new List<Pharmacyinventory>();

    public virtual Product Product { get; set; }
}
