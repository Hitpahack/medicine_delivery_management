using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Permission
{
    public long Id { get; set; }

    public string Module { get; set; }

    public string Route { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();
}
