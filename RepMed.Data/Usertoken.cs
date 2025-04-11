using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Usertoken
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Token { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime? CreatedAt { get; set; }
}
