using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Payment
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long CustomerId { get; set; }

    public string PaymentMethod { get; set; }

    public string PaymentStatus { get; set; }

    public decimal AmountPaid { get; set; }

    public string TransactionId { get; set; }

    public string FailedReason { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual User Customer { get; set; }

    public virtual Order Order { get; set; }

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
