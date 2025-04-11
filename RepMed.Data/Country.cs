using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Country
{
    public long Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Pharmacy> Pharmacies { get; set; } = new List<Pharmacy>();

    public virtual ICollection<State> States { get; set; } = new List<State>();

    public virtual ICollection<Useraddress> Useraddresses { get; set; } = new List<Useraddress>();
}
