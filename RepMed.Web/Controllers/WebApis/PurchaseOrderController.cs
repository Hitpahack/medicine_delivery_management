using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.POPage;
using RepMed.Dtos.ShortBookPage;
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

        [Route("edit_item")]
        [HttpPost]
        public async Task<IActionResult> EditItem(BaseShortbookDto reqDto,long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.AddEditItem(reqDto,Id);
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

        [Route("add_item")]
        [HttpPost]
        public async Task<IActionResult> AddItem(BaseShortbookDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.AddEditItem(reqDto, 0);
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

        [Route("delete_item/{itemId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(long itemId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.DeleteItem(itemId);
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
        [Route("createpo")]
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

        [Route("getsuppliers/{pharmacyId}")]
        [HttpPost]
        public async Task<IActionResult> GetAllSuppliers(long pharmacyId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetAllSuppliers(pharmacyId);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }

        [Route("getpo")]
        [HttpPost]
        public async Task<IActionResult> GetAllPO(POPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetAllPO(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }

        [Route("getponumber/{pharmacyId}")]
        [HttpPost]
        public async Task<IActionResult> GetNextPONumber(long pharmacyId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetNextPONumber(pharmacyId);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }

        [HttpGet("generate-po-pdf/{poId}")]
        public async Task<IActionResult> GeneratePoPdf(int poId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPOPdfDetails(poId);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        var items = await purchaseOrderService.GetPOItems(poId);
                        if(!result.IsSuccess)
                        {
                            return BadRequest(items);
                        }
                        var pdfBytes = await purchaseOrderService.Generate(result.Data,items.Data);

                        return File(pdfBytes.Data, "application/pdf", $"PurchaseOrder_{result.Data.PONumber}.pdf");
                    }   
                }
            }
            
        }

        [HttpGet("search_products")]
        public async Task<IActionResult> SearchProducts(string search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.SearchProducts(search);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }

        [Route("get_items")]
        [HttpPost]
        public async Task<IActionResult> GetShortBookItems(ShortbookPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetShortBookItems(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }

        [Route("get_item/{itemId}")]
        [HttpPost]
        public async Task<IActionResult> GetItem(long itemId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetItem(itemId);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }
    }
}
