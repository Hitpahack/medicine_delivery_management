using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Childpermission
{
    public long Id { get; set; }

    public long PermissionId { get; set; }

    public string Label { get; set; }

    public string Route { get; set; }

    public virtual Permission Permission { get; set; }
}
