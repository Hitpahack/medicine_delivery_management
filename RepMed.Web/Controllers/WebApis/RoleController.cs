using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{

    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/users")]
    public class RoleController : BaseAccountsController
    {
        public RoleController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        [Route("addrole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(BasicRoleDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();  
                using (var tran = db.BeginTransaction())
                {
                    using (IRoleService roleService = new RoleService(db, tran))
                    {
                        var result = await roleService.CreateRole(reqDto);
                        if (!result.IsSuccess)
                            return BadRequest();
                        return Ok(result.Data);
                    }
                }
            }
        }
    }
}
