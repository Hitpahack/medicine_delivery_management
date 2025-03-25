using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Country
{
    public int Id { get; set; }

    public string CountryCode { get; set; }

    public string CountryCodeTwo { get; set; }

    public string Currency { get; set; }

    public string CurrencySymbol { get; set; }

    public string Name { get; set; }

    public string TimeZone { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
