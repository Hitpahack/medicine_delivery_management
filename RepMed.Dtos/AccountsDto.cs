using RepMed.Localize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepMed.Dtos
{
    public class BaseAccountsDto
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public BasicPersonsDto Person { get; set; }
    }

    public class Login_ReqDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }

    public class Login_ResDto : BaseAccountsDto
    {
        public JwtTokenDto Token { get; set; }
        public long RoleId { get; set; }
        public List<string> Permissions { get; set; }

    }
    public class JwtTokenDto
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidTill { get; set; }
    }
    public class SetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}

