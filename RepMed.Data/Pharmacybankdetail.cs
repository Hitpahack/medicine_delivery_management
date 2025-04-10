using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Pharmacybankdetail
{
    public long Id { get; set; }

    public long PharmacyId { get; set; }

    public string BankName { get; set; }

    public string AccountHolderName { get; set; }

    public string AccountNumber { get; set; }

    public string Ifsccode { get; set; }

    public string BranchName { get; set; }

    public string UpiId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }
}
