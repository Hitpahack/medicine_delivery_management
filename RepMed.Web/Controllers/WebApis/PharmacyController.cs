using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Dtos.PharmacyPage;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{

    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/pharmacy")]
    public class PharmacyController : BaseAccountsController
    {
        public PharmacyController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        [Route("addeditpharmacy/{Id?}")]
        [HttpPost]
        public async Task<IActionResult> AddEdit([FromBody] PharmacyDto reqDto, long Id = 0)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPersonService personService = new PersonService(db, tran))
                    {
                        reqDto.User.Role = "pharmacy";
                        var user = await base.AddUser(reqDto.User, Id);
                        if (!user.IsSuccess)
                        {
                            tran.Rollback();
                            return BadRequest(user);
                        }
                        using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                        {
                            reqDto.Pharmacy.UserId = user.Data.Id;
                            var pharmacy = await pharmacyService.AddUpdatePharmacy(reqDto.Pharmacy, Id);
                            if (!pharmacy.IsSuccess)
                            {
                                tran.Rollback();
                                return BadRequest(pharmacy);
                            }
                            reqDto.PharmacyBankDetails.PharmacyId = pharmacy.Data.Id;
                            var pharmacybank = await pharmacyService.AddUpdatePharmacyBankDetails(reqDto.PharmacyBankDetails, Id);
                            if (!pharmacybank.IsSuccess)
                            {
                                tran.Rollback();
                                return BadRequest(pharmacybank);
                            }
                            var response = await pharmacyService.GenrateEmailToken(reqDto.Pharmacy.UserId, reqDto.Pharmacy.OfficialEmail);
                        }
                    }
                    tran.Commit();
                    return Ok();
                }

            }
        }

        [Route("getpharmacies")]
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] PharmacyPagingRequest search)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                    {
                        var pharmacy = await pharmacyService.GetAllPharmacies(search);
                        if (!pharmacy.IsSuccess)
                            return BadRequest();
                        return Ok(pharmacy.Data);
                    }
                }
            }
        }

        [Route("passwordemail")]
        // for tesing purpose
        [HttpPost]
        public async Task<IActionResult> Email(long UserId, string Email)
        {
            {
                using (var db = new MySqlConnection(_appSettings.ConnectionString))
                {
                    db.Open();
                    using (var tran = db.BeginTransaction())
                    {
                        using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                        {
                            var response = await pharmacyService.GenrateEmailToken(UserId, Email);
                            if (!response.IsSuccess)
                            {
                                tran.Rollback();
                                return BadRequest();
                            }
                            tran.Commit();
                            return Ok();
                        }
                    }
                }
            }
        }

    }
}
