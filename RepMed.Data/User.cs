using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class User
{
    public long Id { get; set; }

    public long PersonId { get; set; }

    public byte[] PasswordHash { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public byte[] PasswordSalt { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public bool? IsLocked { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Deliveryperson> Deliverypeople { get; set; } = new List<Deliveryperson>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Person Person { get; set; }

    public virtual ICollection<Pharmacy> Pharmacies { get; set; } = new List<Pharmacy>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUpdatedByNavigations { get; set; } = new List<Product>();
}
