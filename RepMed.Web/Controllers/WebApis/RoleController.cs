using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{

    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/roles")]   
    public class RoleController : BaseAccountsController
    {
        public RoleController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        [Route("addrole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(CreateRoleDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IRoleService roleService = new RoleService(db, tran))
                    {
                        var result = await roleService.AddEditRole(reqDto,0);
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

        [Route("editrole/{Id}")]
        [HttpPost]
        public async Task<IActionResult> EditRole(CreateRoleDto reqDto, long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IRoleService roleService = new RoleService(db, tran))
                    {
                        var result = await roleService.AddEditRole(reqDto, Id);
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

        [Route("getrole/{Id}")]
        [HttpPost]
        public async Task<IActionResult> GetRolePermission(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IRoleService roleService = new RoleService(db, tran))
                    {
                        var result = await roleService.GetRolePermission(Id);
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

        [Route("getroles")]
        [HttpPost]
        public async Task<IActionResult> GetAllRole()
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IRoleService roleService = new RoleService(db, tran))
                    {
                        var result = await roleService.GetAllRoles();
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


        [Route("getpermissions")]
        [HttpPost]

        public async Task<IActionResult> GetAllPermissions()
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IRoleService roleService = new RoleService(db, tran))
                    {
                        var result = await roleService.GetAllPermissions();
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
