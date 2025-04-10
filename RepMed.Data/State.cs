using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class State
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long CountryId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Pharmacy> Pharmacies { get; set; } = new List<Pharmacy>();

    public virtual ICollection<Useraddress> Useraddresses { get; set; } = new List<Useraddress>();
}
