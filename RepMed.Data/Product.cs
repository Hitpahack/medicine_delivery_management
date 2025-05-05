using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public decimal? Price { get; set; }

    public decimal? Mrp { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<Pharmacyinventory> Pharmacyinventories { get; set; } = new List<Pharmacyinventory>();

    public virtual ICollection<Pharmacystockledger> Pharmacystockledgers { get; set; } = new List<Pharmacystockledger>();

    public virtual ICollection<Purchaseinvoiceitem> Purchaseinvoiceitems { get; set; } = new List<Purchaseinvoiceitem>();

    public virtual ICollection<Purchaseorderitem> Purchaseorderitems { get; set; } = new List<Purchaseorderitem>();
}
