
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

    public long? TotalPages { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual User UpdatedByNavigation { get; set; }
}
