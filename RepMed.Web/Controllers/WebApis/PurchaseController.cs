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
    [Route("api/v1/admin/po")]
    public class PurchaseController : BaseAccountsController
    {
        public PurchaseController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }   
        [Route("purchaseinvoice")]
        [HttpPost]
        public async Task<IActionResult> AddPurchaseInvoice(AddPurchaseInvoiceDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseService purchaseService = new PurchaseService(db, tran))
                    {
                        var result = await purchaseService.AddPurchaseInvoice(reqDto);
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

        [Route("fetch_po/{pharmacyId}")]
        [HttpPost]
        public async Task<IActionResult> FetchPO(FetchPoRequestDto reqDto, long pharmacyId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseService purchaseService = new PurchaseService(db, tran))
                    {
                        var result = await purchaseService.FetchPO(reqDto.PoNumber, pharmacyId);
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
