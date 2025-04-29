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
    public class PurchaseOrderController : BaseAccountsController
    {
        public PurchaseOrderController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        [Route("cretepo")]
        [HttpPost]
        public async Task<IActionResult> CreatePO(CreatePODto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.CreatePO(reqDto);
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
        [Route("addsupplier")]
        [HttpPost]
        public async Task<IActionResult> AddSupplier(BaseSupplierDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.AddSupplier(reqDto);
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
