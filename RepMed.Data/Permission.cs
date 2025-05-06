using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Permission
{
    public long Id { get; set; }

    public string Label { get; set; }

    public string Module { get; set; }

    public string Route { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual ICollection<Childpermission> Childpermissions { get; set; } = new List<Childpermission>();

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();
}
