using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Localize
{
	public class CustomerCultureProvider : RequestCultureProvider
	{
		public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
		{
			//Go away and do a bunch of work to find out what culture we should do. 
			//await Task.Yield();
			//string culture = Convert.ToString(httpContext.Request.Headers["Accept-Language"]).Split(',')[0] ?? "en";

			//if (!string.IsNullOrEmpty(culture))
			//{
			//   //httpContext.Response.Cookies.Append(
			//   //CookieRequestCultureProvider.DefaultCookieName,
			//   //CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
			//   //new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) });

			//	CultureInfo.CurrentCulture = new CultureInfo(culture);
			//	return new ProviderCultureResult(culture);

			//}
            


			//Return a provider culture result. 
			return new ProviderCultureResult("en-US");

			//In the event I can't work out what culture I should use. Return null. 
			//Code will fall to other providers in the list OR use the default. 
			//return null;
		}
	}
}
