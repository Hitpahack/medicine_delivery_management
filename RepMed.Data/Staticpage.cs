using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Staticpage
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public string MetaTitle { get; set; }

    public string MetaContext { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
