
using AutoMapper.Configuration.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RepMed.Dtos
{
   public class BasePerson
    {
        public ulong? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
   
    public class BasePersonDto : BasePerson
    {
        [RegularExpression("^(Male|Female|Others)$", ErrorMessage = "Gender must be 'Male', 'Female', or 'Others'.")]
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; } 
        [RegularExpression(@"^(\d{10})|(\d{4}[- ])(\d{3}[- ])(\d{3})|(\d{1,3}[- ])(\d{4}[- ]\d{3}[- ]\d{3})|(\+\d{1,3}[- ])(\d{4}[- ]\d{3}[- ]\d{3})|(\d{1,3}[- ])(\d{7,10})|(\d{1,3}[- ])(\d{3}[- ])(\d{4})|(\+\d{1,3}[- ]?)(\d{7,12})|(\+\d{1,3}[- ]?)(\d{3}[- ])(\d{4})$/", ErrorMessage = "Invalid phone no")]
        public string? Mobile { get; set; }
        
    }
    public class BasicPersonsDto : BasePersonDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? Picture { get; set; }       
    }

    public class EntityPersonsDto : BasicPersonsDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool? MobileVerified { get; set; }

        public bool? EmailVerified { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string BloodGroup { get; set; }

        public string Picture { get; set; }

        public string Signature { get; set; }

        public string MotherName { get; set; }

        public string Qualification { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        
    }

    public class AddPersonDto : BasicPersonsDto
    {
     
        [Core.IgnoreDapper]
        public string ConfirmPassword { get; set; }
        [Core.IgnoreDapper]
        public AddAddressDto? Address { get; set; }
        [Core.IgnoreDapper]
        public string Role { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }

    }


}
