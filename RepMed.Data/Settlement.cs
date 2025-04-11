using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Settlement
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public decimal TotalAmountDue { get; set; }

    public decimal? CommissionAmount { get; set; }

    public decimal AmountPaid { get; set; }

    public string TransactionId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string PaymentStatus { get; set; }

    public string SettlementPeriod { get; set; }

    public string Notes { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }
}
