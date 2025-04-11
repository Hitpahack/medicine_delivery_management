using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Deliveryperson
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long AssignedPharmacyId { get; set; }

    public string GovernmentIdType { get; set; }

    public string GovernmentIdNumber { get; set; }

    public string VehicleDetails { get; set; }

    public bool? IsActive { get; set; }

    public string BankAccountNumber { get; set; }

    public string AccountHolderName { get; set; }

    public string BankName { get; set; }

    public string BranchName { get; set; }

    public string Ifsccode { get; set; }

    public string UpiId { get; set; }

    public virtual Pharmacy AssignedPharmacy { get; set; }

    public virtual ICollection<Deliverydetail> Deliverydetails { get; set; } = new List<Deliverydetail>();

    public virtual User User { get; set; }
}
