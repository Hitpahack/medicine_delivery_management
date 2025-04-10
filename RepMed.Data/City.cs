using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class City
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long StateId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Pharmacy> Pharmacies { get; set; } = new List<Pharmacy>();

    public virtual State State { get; set; }

    public virtual ICollection<Useraddress> Useraddresses { get; set; } = new List<Useraddress>();
}
