using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Rolepermission
{
    public int Id { get; set; }

    public string Permission { get; set; }

    public string Description { get; set; }

    public string Action { get; set; }

    public string Controller { get; set; }

    public string Route { get; set; }

    public string Title { get; set; }

    public bool? IsApis { get; set; }

    public string Parma { get; set; }

    public virtual ICollection<Userrolepermission> Userrolepermissions { get; set; } = new List<Userrolepermission>();
}
