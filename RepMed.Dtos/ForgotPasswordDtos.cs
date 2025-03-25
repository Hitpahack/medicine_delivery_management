using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepMed.Dtos
{
    public class ForgotPasswordDtos
    {
        [Required]
        public string Email { get; set; }
    }
    public class ResponseForgotPass
    {

    }

    public class ResetPasswordDTO
    {
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
    public class UpdatePasswordDTO : ResetPasswordDTO
    {
        public string CurrentPassword { get; set; }
    }
}
