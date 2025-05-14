using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Role
{
    public long Id { get; set; }

    public string RoleName { get; set; }

    public long? PharmacyId { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsAdminRole { get; set; }

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();
}
