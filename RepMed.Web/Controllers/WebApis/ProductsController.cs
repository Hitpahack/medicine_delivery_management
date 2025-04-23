using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.ProductPage;
using RepMed.Dtos.UsersPage;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/users")]
    public class ProductsController : BaseAccountsController
    {
        public ProductsController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }
        [Route("getproducts")]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] ProductsPagingRequest search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IProductService productService = new ProductService(db, tran))
                    {
                        var result = await productService.GetProducts(search);
                        if (!result.IsSuccess)
                            return BadRequest(new APIsResponse<string> { IsSuccess = false, Message = result.Message });
                        return Ok(result.Data);
                    }
                }
            }
        }
        [Route("getcount")]
        [HttpPost]
        public async Task<IActionResult> GetCount()
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IProductService productService = new ProductService(db, tran))
                    {
                        var result = await productService.GetCount();
                        if (!result.IsSuccess)
                            return BadRequest(new APIsResponse<string> { IsSuccess = false, Message = result.Message });
                        return Ok(new APIsResponse<DashboardDto> { IsSuccess = false, Message = result.Message, Data= result.Data });
                    }
                }
            }
        }
    }
}   
