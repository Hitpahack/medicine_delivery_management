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
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }

    public class GetUserDto : BasePersonDto
    {
        public long Id{ get; set; }

    }
}
