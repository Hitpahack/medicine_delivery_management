using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Userrolepermission
{
    public Guid Id { get; set; }

    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public virtual Rolepermission Permission { get; set; }

    public virtual Role Role { get; set; }
}
