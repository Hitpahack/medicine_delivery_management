using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Shortbook
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long PharmacyId { get; set; }

    public long? SupplierId { get; set; }

    public long Quantity { get; set; }

    public string Priority { get; set; }

    public string Status { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual Product Product { get; set; }

    public virtual Supplier Supplier { get; set; }
}
