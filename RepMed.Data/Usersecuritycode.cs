using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Usersecuritycode
{
    public long Id { get; set; }

    public string SecurityCode { get; set; }

    public long UserId { get; set; }

    public DateTime ValidTo { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public sbyte? IsExpired { get; set; }

    public virtual User User { get; set; }
}
