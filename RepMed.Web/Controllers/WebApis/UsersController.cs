using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Org.BouncyCastle.Crypto.Generators;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/users")]
    public class UsersController : BaseAccountsController
    {
        public UsersController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        [Route("addedituser/{Id?}")]
        [HttpPost]
        public async Task<IActionResult> AddEditUser(AddPersonDto reqDto, long Id = 0)
        {
            reqDto.Role = "Customer";
            var data = await base.AddUser(reqDto, Id);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
        }
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto reqdto)
        {
            if (reqdto.Password != reqdto.ConfirmPassword)
                return BadRequest("Passwords do not match");

            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IUserServices userService = new UserServices(db, tran))
                    {
                        var result = await userService.SetPasswordAsync(reqdto);
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
