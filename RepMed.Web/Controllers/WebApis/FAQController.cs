using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos.CMSPage;
using RepMed.Dtos;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;
using RepMed.Dtos.FAQ;

namespace RepMed.Web.Controllers.WebApis
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/faq")]
    public class FAQController : BaseAccountsController
    {
        public FAQController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        [HttpPost("addfaq")]
        public async Task<IActionResult> AddFAQ(BaseFAQDto dto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IFAQServices faqService = new FAQService(db, tran))
                    {
                        var result = await faqService.AddEditFAQ(dto, 0);
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
        [HttpPost("editfaq/{Id}")]
        public async Task<IActionResult> EditFAQ(BaseFAQDto dto, long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IFAQServices faqService = new FAQService(db, tran))
                    {
                        var result = await faqService.AddEditFAQ(dto, Id);
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
                    using (IFAQServices faqService = new FAQService(db, tran))
                    {
                        var result = await faqService.ChangeFAQStatus(Id, status);
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

        [Route("deletefaq/{Id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteFAQ(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IFAQServices faqService = new FAQService(db, tran))
                    {
                        var result = await faqService.DeleteFAQ(Id);
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

        [Route("getfaqs")]
        [HttpPost]
        public async Task<IActionResult> GetAllFAQ(FAQPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IFAQServices faqService = new FAQService(db, tran))
                    {
                        var result = await faqService.GetAllFAQ(reqDto);
                        if (!result.IsSuccess)
                            return BadRequest(result);
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("getfaq/{Id}")]
        [HttpPost]
        public async Task<IActionResult> GetFAQ(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IFAQServices faqService = new FAQService(db, tran))
                    {
                        var result = await faqService.GetFAQ(Id);
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
