using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/cms")]
    public class CMSController : BaseAccountsController
    {
        public CMSController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
                
        [HttpPost("static/contact")]
        public async Task<IActionResult> CreateContactStaticPage(BaseStaticPageDto dto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.CreateStaticPage(dto);
                        if (!result.IsSuccess)
                        {
                            tran.Rollback();
                            return BadRequest(result);
                        }
                        tran.Commit();
                        return Ok(result);
                    }
                }
            }
        }

    }
}
