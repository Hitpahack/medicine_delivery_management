using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Useraddress
{
    public long Id { get; set; }

    public long PersonId { get; set; }

    public string AddressLine { get; set; }

    public long CityId { get; set; }

    public long StateId { get; set; }

    public long CountryId { get; set; }

    public string Pincode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual City City { get; set; }

    public virtual Country Country { get; set; }

    public virtual Person Person { get; set; }

    public virtual State State { get; set; }
}
