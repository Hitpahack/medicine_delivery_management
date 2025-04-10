using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Product
{
    public long Id { get; set; }

    public long CategoryId { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public decimal? Price { get; set; }

    public decimal? Mrp { get; set; }

    public decimal? Discount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual ICollection<Pharmacyinventory> Pharmacyinventories { get; set; } = new List<Pharmacyinventory>();

    public virtual ICollection<Pharmacypurchase> Pharmacypurchases { get; set; } = new List<Pharmacypurchase>();

    public virtual User UpdatedByNavigation { get; set; }
}
