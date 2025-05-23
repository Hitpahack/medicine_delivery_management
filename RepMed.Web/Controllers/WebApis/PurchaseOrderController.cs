using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.POPage;
using RepMed.Dtos.POPage.POItems;
using RepMed.Dtos.ShortBookPage;
using RepMed.Dtos.SupplierPage;
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

        [Route("edit_item/{Id}")]
        [HttpPost]
        public async Task<IActionResult> EditShortBookItem(BaseShortbookDto reqDto,long Id)
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
        public async Task<IActionResult> AddShortBookItem(BaseShortbookDto reqDto)
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
        public async Task<IActionResult> DeleteShortBookItem(long itemId)
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
        public async Task<IActionResult> GeneratePO(CreatePODto reqDto)
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

        [Route("delete_po/{poId}")]
        [HttpPost]
        public async Task<IActionResult> DeletePO(long poId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.DeletePO(poId);
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
        public async Task<IActionResult> GetAllSuppliers(long pharmacyId, string search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetAllSuppliers(pharmacyId,search);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result);
                    }
                }
            }
        }

        [Route("get_po_orderwise")]
        [HttpPost]
        public async Task<IActionResult> GetPOOrdeWise(POPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPOOrdeWise(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("get_po_itemwise")]
        [HttpPost]
        public async Task<IActionResult> GetPOItemWise(POIWPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPOItemWise(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("get_po_distwise")]
        [HttpPost]
        public async Task<IActionResult> GetPODistWise(PODWPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPODistWise(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("get_po_owitems")]
        [HttpPost]
        public async Task<IActionResult> GetPOOWItems(POOWItemsPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPOOWItems(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result.Data);
                    }
                }
            }
        }

        [Route("get_po_iwitems")]
        [HttpPost]
        public async Task<IActionResult> GetPOIWItems(POIWItemsPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPOIWItems(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result.Data);
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
                        return Ok(result.Data);
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

        [Route("get_po_item/{poItemId}")]
        [HttpPost]
        public async Task<IActionResult> GetPOItem(long poItemId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetPOItem(poItemId);
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

        [Route("edit_po_item/{poItemId}")]
        [HttpPost]
        public async Task<IActionResult> UpdatePOItem(long poItemId, long qty)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.UpdatePOItem(poItemId,qty);
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

        [Route("delete_po_item/{poItemId}")]
        [HttpPost]
        public async Task<IActionResult> DeletePOItem(long poItemId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.DeletePOItem(poItemId);
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

        [Route("place_po_order/{poId}")]
        [HttpPost]
        public async Task<IActionResult> PlacePOOrder(long poId)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.SendPOEmail(poId);
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
                        var result = await purchaseOrderService.AddEditSupplier(reqDto,0);
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

        [Route("editsupplier/{Id}")]
        [HttpPost]
        public async Task<IActionResult> EditSupplier(BaseSupplierDto reqDto, long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.AddEditSupplier(reqDto,Id);
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

        [Route("get_suppliers")]
        [HttpPost]
        public async Task<IActionResult> GetSuppliers(SupplierPagingRequest reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPurchaseOrderService purchaseOrderService = new PurchaseOrderService(db, tran))
                    {
                        var result = await purchaseOrderService.GetSuppliers(reqDto);
                        if (!result.IsSuccess)
                        {
                            return BadRequest(result);
                        }
                        return Ok(result.Data);
                    }
                }
            }
        }
    }
}
