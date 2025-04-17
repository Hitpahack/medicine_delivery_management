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
                        var user = await base.AddEditUser(reqDto.User, Id);
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

        [Route("addpharmacy")]
        [HttpPost]
        public async Task<IActionResult> Addpharmacy([FromBody] API_ADD_PH_DTO reqDto)
        {
            
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPersonService personService = new PersonService(db, tran))
                    {
                        reqDto.User.Role = "pharmacy";
                        var userObj = _mapper.Map<AddPersonDto, API_ADD_USER>(reqDto.User);
                        var user = await base.AddEditUser(userObj, 0);
                        if (!user.IsSuccess)
                        {
                            tran.Rollback();
                            return BadRequest(user);
                        }
                        using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                        {
                            reqDto.Pharmacy.UserId = user.Data.Id;
                            var pharmacy = await pharmacyService.AddUpdatePharmacy(reqDto.Pharmacy, 0);
                            if (!pharmacy.IsSuccess)
                            {
                                tran.Rollback();
                                return BadRequest(pharmacy);
                            }
                            reqDto.PharmacyBankDetails.PharmacyId = pharmacy.Data.Id;
                            var pharmacybank = await pharmacyService.AddUpdatePharmacyBankDetails(reqDto.PharmacyBankDetails, 0);
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

        [Route("editpharmacy/{Id}")]
        [HttpPost]
        public async Task<IActionResult> Editpharmacy([FromBody] API_EDIT_PH_DTO reqDto, long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPersonService personService = new PersonService(db, tran))
                    {
                        var userObj = _mapper.Map<AddPersonDto, API_EDIT_USER>(reqDto.User);
                        var user = await base.AddEditUser(userObj, Id);
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
                        }
                    }
                    tran.Commit();
                    return Ok();
                }

            }
        }

        [Route("editprofile/{Id}")]
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromBody] API_EDIT_PH_DTO reqDto, long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPersonService personService = new PersonService(db, tran))
                    {
                        var userObj = _mapper.Map<AddPersonDto, API_EDIT_USER>(reqDto.User);
                        var user = await base.AddEditUser(userObj, Id);
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
                        }
                    }
                    tran.Commit();
                    return Ok();
                }

            }
        }


        [Route("getpharmacies")]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PharmacyPagingRequest search)
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

        [Route("getpharmacy/{Id?}")]
        [HttpGet]
        public async Task<IActionResult> Get(long Id)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                    {
                        var pharmacy = await pharmacyService.GetPharmacy(Id);
                        if (!pharmacy.IsSuccess)
                            return BadRequest();
                        return Ok(pharmacy.Data);
                    }
                }
            }
        }

    }
}
