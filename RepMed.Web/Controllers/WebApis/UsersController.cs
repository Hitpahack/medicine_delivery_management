using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Dtos.UsersPage;
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
        [Route("adduser")]
        [HttpPost]
        public async Task<IActionResult> AddUser(API_ADD_USER reqDto)
        {
            var userObj = _mapper.Map<AddPersonDto, API_ADD_USER>(reqDto);
            var data = await base.AddEditUser(userObj, 0);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
        }

        [Route("edituser/{personid}")]
        [HttpPost]
        public async Task<IActionResult> EditUser(long personid, API_EDIT_USER reqDto )
        {
            var userObj = _mapper.Map<AddPersonDto, API_EDIT_USER>(reqDto);
            var data = await base.AddEditUser(userObj, personid);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
        }

        [Route("get/{Id}")]
        [HttpGet]
        public async Task<IActionResult> GET(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IUserServices userService = new UserServices(db, tran))
                    {
                        var result = await userService.GetUser(Id);
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

        [Route("getusers")]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] UsersPagingRequest search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IUserServices userService = new UserServices(db, tran))
                    {
                        var result = await userService.GetUsers(search);
                        if (!result.IsSuccess)
                            return BadRequest();
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangePasswordDto reqDto)
        {
            var data = await base.ChangePassword(reqDto);
            if (data.IsSuccess)
                return Ok(data);
            return BadRequest(data);

        }

        [Route("getpharmacystaff")]
        [HttpPost]
        public async Task<IActionResult> GetPharmacyUsers([FromBody] UsersPagingRequest search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IUserServices userService = new UserServices(db, tran))
                    {
                        var result = await userService.GetPharmacyUsers(search);
                        if (!result.IsSuccess)
                            return BadRequest();
                        return Ok(result.Data);
                    }
                }
            }
        }

    }
}
    