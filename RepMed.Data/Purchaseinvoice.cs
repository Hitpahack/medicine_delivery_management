using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Purchaseinvoice
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public long SupplierId { get; set; }

    public string InvoiceNumber { get; set; }

    public decimal TotalAmount { get; set; }

    public string Ponumber { get; set; }

    public decimal? TotalDiscount { get; set; }

    public string PaymentMode { get; set; }

    public string PaymentStatus { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal? TaxAmount { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string Remarks { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual ICollection<Pharmacystockledger> Pharmacystockledgers { get; set; } = new List<Pharmacystockledger>();

    public virtual ICollection<Purchaseinvoiceitem> Purchaseinvoiceitems { get; set; } = new List<Purchaseinvoiceitem>();

    public virtual Supplier Supplier { get; set; }
}
