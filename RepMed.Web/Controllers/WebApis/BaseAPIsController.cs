using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using RepMed.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    [ApiController]
    [RequireHeader]
    public class BaseAPIsController : ControllerBase
    {
        public static HeaderParam _headerParam;
        public static AppSettings _appSettings;
        public static IGenericMapper _mapper;

        public BaseAPIsController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _mapper = ServiceActivator.GetScope().ServiceProvider.GetService<IGenericMapper>();
            _headerParam = ServiceActivator.GetScope().ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Request.GetHeaderParma();
        }
    }
}
