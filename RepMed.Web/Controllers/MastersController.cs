using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RepMed.Core;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{
    [Route("api/v1/masters")]
    public class MastersController : BaseMastersController
    {
        public MastersController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        /// <summary>
        /// Get all countries 
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        [HttpGet("getcountries")]
        public async Task<IActionResult> GetCountries()
        {
            var data = await base.Countries();
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
                
        }

        /// <summary>
        /// Get all states 
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        [HttpGet("getstates/{countryid?}")]
        public async Task<IActionResult> GetStates(long? countryid =0)
        {
            var data = await base.States(countryid);
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }

        /// <summary>
        /// Get all cities 
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        [HttpGet("getcities/{stateid?}")]
        public async Task<IActionResult> GetCities( long? stateid =0)
        {
            var data = await base.Cities(stateid);
            if (data.IsSuccess)
                return Ok(data);
            return BadRequest(data);
        }
        [HttpGet("getroles")]
        public async Task<IActionResult> GetRoles()
        {
            var data = await base.Roles();
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }
        [HttpGet("getproducts")]
        public async Task<IActionResult> GetProducts()
        {
            var data = await base.Products();
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }
    }
}
