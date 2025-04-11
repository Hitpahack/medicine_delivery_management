using RepMed.Localize;
using System;
using System.ComponentModel.DataAnnotations;

namespace RepMed.Dtos
{
    public class BaseAccountsDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastLoginDate { get; set; }

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

    }
    public class JwtTokenDto
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidTill { get; set; }
    }
    public class SetPasswordDto
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

