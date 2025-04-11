using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Orderdetail
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long PharmacyInventoryId { get; set; }

    public long Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Order Order { get; set; }

    public virtual Pharmacyinventory PharmacyInventory { get; set; }
}
