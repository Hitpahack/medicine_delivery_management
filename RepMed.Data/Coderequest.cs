using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Coderequest
{
    public int Id { get; set; }

    public string SecurityCode { get; set; }

    public string Userid { get; set; }

    public DateTime? ValidTo { get; set; }
}
