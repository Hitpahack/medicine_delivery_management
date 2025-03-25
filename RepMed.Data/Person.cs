using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Person
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Mobile { get; set; }

    public string BloodGroup { get; set; }

    public DateTime? Dob { get; set; }

    public string MotherName { get; set; }

    public string Qualifications { get; set; }

    public string Picture { get; set; }

    public string Signatures { get; set; }

    public string State { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Usercontact> Usercontacts { get; set; } = new List<Usercontact>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
