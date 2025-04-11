using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Userrole
{
    public long UserId { get; set; }

    public long RoleId { get; set; }

    public virtual Role Role { get; set; }

    public virtual User User { get; set; }
}
