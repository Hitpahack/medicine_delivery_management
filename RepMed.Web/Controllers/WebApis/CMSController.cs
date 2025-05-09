using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Dtos.CMSPage;
using RepMed.Dtos.RolePage;
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
                
        [HttpPost("addstaticpage")]
        public async Task<IActionResult> CreateContactStaticPage(BaseStaticPageDto dto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.AddEditStaticPage(dto,0);
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
        [HttpPost("editstaticpage/{Id}")]
        public async Task<IActionResult> EditContactStaticPage(BaseStaticPageDto dto,long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.AddEditStaticPage(dto, Id);
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

        [Route("changestatus/{Id}")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long Id, bool status)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.ChangePageStatus(Id, status);
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

        [Route("deletestaticpage/{Id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteStaticPage(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.DeleteStaticPage(Id);
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

        [Route("getstaticpages")]
        [HttpPost]
        public async Task<IActionResult> GetAllStaticPage(CMSPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.GetAllStaticPage(reqDto);
                        if (!result.IsSuccess)
                            return BadRequest(result);
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("getstaticpage/{Id}")]
        [HttpPost]
        public async Task<IActionResult> GetStaticPage(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (ICMSServies cmsService = new CMSServies(db, tran))
                    {
                        var result = await cmsService.GetStaticPage(Id);
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
