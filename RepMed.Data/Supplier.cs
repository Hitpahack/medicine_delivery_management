using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Supplier
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }

    public string Gstnumber { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; } = new List<Purchaseinvoice>();

    public virtual ICollection<Purchaseorder> Purchaseorders { get; set; } = new List<Purchaseorder>();

    public virtual ICollection<Shortbook> Shortbooks { get; set; } = new List<Shortbook>();
}
