using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Person
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Mobile { get; set; }

    public bool? MobileVerified { get; set; }

    public bool? EmailVerified { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; }

    public string BloodGroup { get; set; }

    public string Picture { get; set; }

    public string Signature { get; set; }

    public string MotherName { get; set; }

    public string Qualification { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Useraddress> Useraddresses { get; set; } = new List<Useraddress>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
