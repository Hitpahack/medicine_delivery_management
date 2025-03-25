
using System;
using System.ComponentModel.DataAnnotations;

namespace RepMed.Dtos
{
   public class BasePerson
    {

        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

    }
    public class BasePersonDto : BasePerson
    {
        [Required]
        public int? Gender { get; set; }
        [Required]
        public DateTime? DOB { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})|(\d{4}[- ])(\d{3}[- ])(\d{3})|(\d{1,3}[- ])(\d{4}[- ]\d{3}[- ]\d{3})|(\+\d{1,3}[- ])(\d{4}[- ]\d{3}[- ]\d{3})|(\d{1,3}[- ])(\d{7,10})|(\d{1,3}[- ])(\d{3}[- ])(\d{4})|(\+\d{1,3}[- ]?)(\d{7,12})|(\+\d{1,3}[- ]?)(\d{3}[- ])(\d{4})$/", ErrorMessage = "Invalid phone no")]
        public string Mobile { get; set; }
        public string MotherName { get; set; }
        
    }
    public class BasicPersonsDto : BasePersonDto
    {
        public string Picture { get; set; }
        
       
    }

    public class EntityPersonsDto : BasicPersonsDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public string State { get; set; }
        public string Signatures { get; set; }
        public string Qualifications { get; set; }
        public int? Age { get; set; }
        public Guid Userid { get; set; }
        public EntityContactsDto UserContacts { get; set; }
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

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        [Core.IgnoreDapper]
        public AddContactsDto Contact { get; set; }



    }
    
}
