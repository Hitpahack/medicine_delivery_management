using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Productcategory
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long CategoryId { get; set; }

    public DateTime? CreatedDate { get; set; }
}
