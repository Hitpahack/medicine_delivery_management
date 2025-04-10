using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Prescription
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public long OrderId { get; set; }

    public string UploadedFile { get; set; }

    public bool? Verified { get; set; }

    public string VerifiedByDoctor { get; set; }

    public DateTime? VerifiedDate { get; set; }

    public string Note { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User Customer { get; set; }

    public virtual Order Order { get; set; }
}
