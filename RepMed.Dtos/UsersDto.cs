using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RepMed.Dtos
{
    public class BasicUsersDto
    {
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public long PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class EntityUsersDto : BasicUsersDto
    {
        public long Id { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public BasicPersonsDto Person { get; set; }
        public IEnumerable<EntityRoleDto> Roles { get; set; }

    }

    public class EntityUsersPassDto : BasicUsersDto
    {
        public EntityUsersPassDto()
        {
            this.Roles = new List<EntityRoleDto>();
        }
        public long Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastLoginDate { get; set; }
        [Core.IgnoreDapper]
        public BasicPersonsDto Person { get; set; }
        [Core.IgnoreDapper]
        public IList<EntityRoleDto> Roles { get; set; }
    }

    public class AddUsersDto : BasicUsersDto
    {

        [Required]
        [MaxLength(25, ErrorMessage = "Maximum 15 characters allow ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters allow ")]
        //[StandardPassword]
        [Core.IgnoreDapper]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Maximum 15 characters allow ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters allow ")]
        //[StandardPassword]
        [Compare("Password")]
        [Core.IgnoreDapper]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }


    }

    public class GetUserDto : BasicPersonsDto
    {
        public long UserId{ get; set; }
        public long PersonId{ get; set; }

    }

    public class API_ADD_USER
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Maximum 15 characters allow ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters allow ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        [Core.IgnoreDapper]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Maximum 15 characters allow ")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters allow ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        [Compare("Password")]
        [Core.IgnoreDapper]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }
    }
    public class API_EDIT_USER : BasicPersonsDto
    {
        public AddAddressDto? Address { get; set; }
    }

}
