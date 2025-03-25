using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }

    public string Ein { get; set; }

    public string Ssn { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public Guid? PersonId { get; set; }

    public virtual Person Person { get; set; }

    public virtual ICollection<Usertokenlog> Usertokenlogs { get; set; } = new List<Usertokenlog>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
