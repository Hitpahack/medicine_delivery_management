using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Usercontact
{
    public Guid Id { get; set; }

    public Guid? PersonId { get; set; }

    public string Address { get; set; }

    public string Address1 { get; set; }

    public string LandMark { get; set; }

    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public string MobileNo { get; set; }

    public string MobileNo2 { get; set; }

    public string Fax { get; set; }

    public virtual Person Person { get; set; }
}
