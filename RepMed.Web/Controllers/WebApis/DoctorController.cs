using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos.UsersPage;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/doctor")]
    public class DoctorController : BaseAccountsController
    {
        public DoctorController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        [Route("getdoctors")]
        [HttpPost]
        public async Task<IActionResult> GetAllDoctors([FromBody] UsersPagingRequest search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IDoctorService doctorService = new DoctorService(db, tran))
                    {
                        var result = await doctorService.GetDoctors(search);
                        if (!result.IsSuccess)
                            return BadRequest();
                        return Ok(result.Data);
                    }
                }
            }
        }
    }
}
