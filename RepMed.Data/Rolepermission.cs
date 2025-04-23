using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Rolepermission
{
    public long Id { get; set; }

    public long? RoleId { get; set; }

    public long? PermissionId { get; set; }

    public virtual Permission Permission { get; set; }

    public virtual Role Role { get; set; }
}
