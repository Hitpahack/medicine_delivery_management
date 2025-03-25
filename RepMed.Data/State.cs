using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class State
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? CountryId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; }
}
