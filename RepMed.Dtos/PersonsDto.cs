
using System;
using System.ComponentModel.DataAnnotations;

namespace RepMed.Dtos
{
   public class BasePerson
    {
        [MinLength(3)]
        public string? FirstName { get; set; }
        [MinLength(3)]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

    }
    public class BasePersonDto : BasePerson
    {
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})|(\d{4}[- ])(\d{3}[- ])(\d{3})|(\d{1,3}[- ])(\d{4}[- ]\d{3}[- ]\d{3})|(\+\d{1,3}[- ])(\d{4}[- ]\d{3}[- ]\d{3})|(\d{1,3}[- ])(\d{7,10})|(\d{1,3}[- ])(\d{3}[- ])(\d{4})|(\+\d{1,3}[- ]?)(\d{7,12})|(\+\d{1,3}[- ]?)(\d{3}[- ])(\d{4})$/", ErrorMessage = "Invalid phone no")]
        public string Mobile { get; set; }
        
    }
    public class BasicPersonsDto : BasePersonDto
    {
        public string Picture { get; set; }       
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
     
        [Required]
        [MaxLength(25, ErrorMessage = "Maximum 15 characters allow ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters allow ")]
        //[StandardPassword]
        [DataType(DataType.Password)]
        [Core.IgnoreDapper]
        public string Password { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Maximum 15 characters allow ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters allow ")]
        //[StandardPassword]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Core.IgnoreDapper]
        public string ConfirmPassword { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        [Core.IgnoreDapper]
        public AddAddressDto? Address { get; set; }
        [Core.IgnoreDapper]
        [Required]
        public string Role { get; set; }

		
	}
    
}
