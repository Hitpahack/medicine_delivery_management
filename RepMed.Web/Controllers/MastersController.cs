using RepMed.Core;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using RepMed.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet("getstates")]
        public async Task<IActionResult> GetStates()
        {
            var data = await base.States();
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }

        /// <summary>
        /// Get all cities 
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        [HttpGet("getcities")]
        public async Task<IActionResult> GetCities()
        {
            var data = await base.Cities();
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }
    }
}
