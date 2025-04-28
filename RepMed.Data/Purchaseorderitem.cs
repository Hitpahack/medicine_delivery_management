using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Purchaseorderitem
{
    public long Id { get; set; }

    public long PurchaseOrderId { get; set; }

    public long ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string Unit { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; }

    public virtual Purchaseorder PurchaseOrder { get; set; }
}
