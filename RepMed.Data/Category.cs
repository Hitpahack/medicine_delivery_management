using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Category
{
    public long Id { get; set; }

    public string CategoryName { get; set; }

    public long? ParentCategoryId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public ulong? IsActive { get; set; }

    public string Url { get; set; }

    public int? TotalPages { get; set; }
}
