using RepMed.Localize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using static RepMed.Core.Enums;

namespace RepMed.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeApiAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        public AuthorizeApiAttribute()
        {

        }
        public AuthorizeApiAttribute(params Roles[] roles)
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Checking if user is logged out or not
            string userId = context.HttpContext.User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                //Authenticate authenticate = new Authenticate();
                //var isUserLoggedOut = authenticate.AuthenticationFilterFunc(userId);
                //if (isUserLoggedOut)
                //{
                //    context.Result = new CustomUnauthorizedResult("Unauthorized");
                //    return;
                //}
            }

            bool IsAuthrized = false;
            //Validate if any permissions are passed when using attribute at controller or action level
            if (!string.IsNullOrEmpty(Roles))
            {
                foreach (var role in Roles.Split(','))
                {
                    if (IsAuthrized)
                        continue;

                    IsAuthrized = context.HttpContext.User.IsInRole(role.Trim());
                }

                if (!IsAuthrized)
                    context.Result = new CustomUnauthorizedResult("Unauthorized");

                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
                context.Result = new CustomUnauthorizedResult("Unauthorized");

            return;
        }

        

        ///// <summary>
        ///// 
        ///// </summary>
        //public class Authenticate
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="userId"></param>
        //    /// <returns></returns>
        //    public bool AuthenticationFilterFunc(string userId)
        //    {
        //        using (var serviceScope = ServiceActivator.GetScope())
        //        {
        //            bool? userData = serviceScope.ServiceProvider.GetService<IRepository<UserDetails, SpitarContext>>().Query().Filter(x => x.UserId == userId).Get().FirstOrDefault()?.IsLoggedOut;
        //            if (userData == true)
        //                return true;
        //            else
        //                return false;
        //        }
        //    }
        //}
    }

    public class RequireHeaderAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            //var headerParma = context.HttpContext.Request.GetHeaderParma();
            //var culture = headerParma.AcceptLanguage;

            //context.HttpContext.Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) }
            //);

            //CultureInfo.CurrentCulture = new CultureInfo(culture);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var headerParma = context.HttpContext.Request.GetHeaderParma();
            var culture = headerParma.AcceptLanguage;
            

            //context.HttpContext.Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) }
            //);

            //CultureInfo.CurrentCulture = new CultureInfo(culture);

            if (string.IsNullOrEmpty(culture))
            {
                context.Result = new CustomUnauthorizedResult("Required Accept-Language parma in header");
            }

            return;
            
        }

        

     
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomUnauthorizedResult : JsonResult
    {
        /// <summary>
        /// CustomUnauthorizedResult
        /// </summary>
        /// <param name="message"></param>            
        /// <returns></returns>
        public CustomUnauthorizedResult(string message): base(new CustomError(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomError
    {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// CustomError
        /// </summary>
        /// <param name="message"></param>            
        /// <returns></returns>
        public CustomError(string message)
        {
            Message = message;
            Success = false;
        }
    }


    public static class JwtExtenstion
    {
        
        public static HeaderParam GetHeaderParma(this HttpRequest request)
        {
            var headerParma = new HeaderParam();
            string culture = Convert.ToString(request.Headers["Accept-Language"]);
            if (!string.IsNullOrEmpty(culture))
                headerParma.AcceptLanguage = culture;

            return headerParma;
        }
    }

    public class HeaderParam
    {
        /// <summary>
        /// AcceptLanguage
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public string AcceptLanguage { get; set; }
        
    }

}
