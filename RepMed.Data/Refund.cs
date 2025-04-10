using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Refund
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long PaymentId { get; set; }

    public decimal RefundAmount { get; set; }

    public string RefundStatus { get; set; }

    public string Reason { get; set; }

    public long? ProcessedBy { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Order Order { get; set; }

    public virtual Payment Payment { get; set; }
}
