using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Usertokenlog
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public string Token { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? TokenValidTill { get; set; }

    public virtual User User { get; set; }
}
