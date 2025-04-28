using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Purchaseorder
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public long SupplierId { get; set; }

    public string Ponumber { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? Eddate { get; set; }

    public string Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public string Remarks { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual ICollection<Purchaseorderitem> Purchaseorderitems { get; set; } = new List<Purchaseorderitem>();

    public virtual Supplier Supplier { get; set; }
}
