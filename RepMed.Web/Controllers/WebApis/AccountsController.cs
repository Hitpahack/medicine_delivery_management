using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/accounts")]
    public class AccountsController : BaseAccountsController
    {
        public AccountsController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> AppLogin([FromBody] Login_ReqDto reqDto)
        {
            var data = await base.Login(reqDto);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
        }

        [Route("forgotpassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPass(ForgotPasswordDtos reqDto)
        {
            var data = await base.ForgotPassword(reqDto);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }

        [Route("resetpassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPass(ResetPasswordDTO reqDto)
        {
            var data = await base.ResetPassword(reqDto);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }

        private async Task SignInAsync(IEnumerable<Claim> Claims)
        {
            #region HttpContext SignIn
            /// Enable cookies auth for web.
            var identity_ = new ClaimsPrincipal(new ClaimsIdentity(Claims));
            var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            #endregion
        }
    }
}
