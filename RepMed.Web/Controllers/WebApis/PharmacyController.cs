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

        [Route("addpharmacy")]
        [HttpPost]
        public async Task<IActionResult> Addpharmacy([FromBody] API_ADD_PH_DTO reqDto)
        {

            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    reqDto.User.Role = "pharmacy";
                    var userObj = _mapper.Map<AddPersonDto, API_ADD_USER>(reqDto.User);
                    using (IPersonService personService = new PersonService(db, tran))
                    {
                        var person = await personService.AddEditPerson(userObj, 0);
                        if (!person.IsSuccess)
                        {
                            tran.Rollback();
                            return BadRequest(new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = person.Message });
                        }
                        using (IUserServices userService = new UserServices(db, tran))
                        {
                            var userData = _mapper.Map<AddUsersDto, EntityPersonsDto>(person.Data, (d) =>
                            {
                                d.PersonId = person.Data.Id;
                                d.ConfirmPassword = reqDto.User.ConfirmPassword;
                                d.Email = reqDto.User.Email;
                            });
                            var user = await userService.AddEditUser(userData, 0);
                            if (!user.IsSuccess)
                            {
                                tran.Rollback();
                                return BadRequest(new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = user.Message });
                            }
                            if (!string.IsNullOrEmpty(reqDto.User.Role))
                            {
                                using (IAccountService accountService = new AccountService(db, tran))
                                {
                                    var role = await accountService.AddRoleAsync(user.Data, reqDto.User.Role);
                                    if (!role.IsSuccess)
                                    {
                                        tran.Rollback();
                                        return BadRequest(new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = role.Message });
                                    }
                                }
                            }
                            using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                            {
                                reqDto.Pharmacy.UserId = user.Data.Id;
                                var pharmacy = await pharmacyService.AddUpdatePharmacy(reqDto.Pharmacy, 0);
                                if (!pharmacy.IsSuccess)
                                {
                                    tran.Rollback();
                                    return BadRequest(new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = pharmacy.Message });
                                }
                                reqDto.PharmacyBankDetails.PharmacyId = pharmacy.Data.Id;
                                var pharmacybank = await pharmacyService.AddUpdatePharmacyBankDetails(reqDto.PharmacyBankDetails, 0);
                                if (!pharmacybank.IsSuccess)
                                {
                                    tran.Rollback();
                                    return BadRequest(new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = pharmacybank.Message });
                                }
                                var response = await pharmacyService.GenrateEmailToken(reqDto.Pharmacy.UserId, reqDto.User.Email);
                                if (!response.IsSuccess)
                                {
                                    tran.Rollback();
                                    return BadRequest(new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = response.Message });
                                }
                            }
                        }
                        tran.Commit();
                        return Ok(new APIsResponse<EntityUsersDto> { IsSuccess = true, Message = "Pharmacy Added Sucessfully" });
                    }
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
                    var userObj = _mapper.Map<AddPersonDto, API_EDIT_USER>(reqDto.User);
                    using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                    {
                        var pharmacy = await pharmacyService.AddUpdatePharmacy(reqDto.Pharmacy, Id);
                        if (!pharmacy.IsSuccess)
                        {
                            tran.Rollback();
                            return BadRequest(pharmacy);
                        }
                        var pharmacybank = await pharmacyService.AddUpdatePharmacyBankDetails(reqDto.PharmacyBankDetails, Id);
                        if (!pharmacybank.IsSuccess)
                        {
                            tran.Rollback();
                            return BadRequest(pharmacybank);
                        }
						tran.Commit();
						return Ok(new APIsResponse<EntityUsersDto> { IsSuccess = true, Message = "Pharmacy Updated Sucessfully" });
					}
                    
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

        [Route("changestatus/{Id}")]
        [HttpPost]
        public async Task<IActionResult> ChangePharmacyStatus(long Id, string status)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                    {
                        var result = await pharmacyService.ChangePharmacyStatus(Id, status);
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
