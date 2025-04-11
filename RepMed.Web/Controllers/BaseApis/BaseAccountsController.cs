using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.BaseApis
{
    public abstract class BaseAccountsController : BaseAPIsController
    {
        public BaseAccountsController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        protected async Task<APIsResponse<Login_ResDto>> Login(Login_ReqDto reqDto)
        {

            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var person_ = await accountService.LoginAsync(reqDto);
                        if (person_.IsSuccess)
                            tran.Commit();
                        else
                            tran.Rollback();

                        return person_;
                    }
                }
            }
        }


        protected async Task<APIsResponse<string>> ForgotPassword(ForgotPasswordDtos reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var callbackUrl = await accountService.ForgotPassword(reqDto.Email);
                        if (callbackUrl.IsSuccess)
                            tran.Commit();
                        else
                            tran.Rollback();

                        return callbackUrl;
                    }
                }

            }

        }

        protected async Task<APIsResponse<bool>> ResetPassword(ResetPasswordDTO reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var isReset = await accountService.ResetPassword(reqDto);
                        if (isReset.IsSuccess)
                            tran.Commit();
                        else
                            tran.Rollback();

                        return isReset;
                    }
                }
            }
        }

        protected async Task<APIsResponse<bool>> AddRole(EntityUsersDto reqDto, string role)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var isReset = await accountService.AddRoleAsync(reqDto,role);
                        if (isReset.IsSuccess)
                            tran.Commit();
                        else
                            tran.Rollback();

                        return new APIsResponse<bool> { IsSuccess = false, Message= " role added" };
                    }
                }
            }
        }
        protected async Task<APIsResponse<EntityUsersDto>> AddUser(AddPersonDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPersonService personService = new PersonService(db, tran))
                    {   
                        var person = await personService.AddPerson(reqDto);
                        if(!person.IsSuccess)
                        {
                            tran.Rollback();
                            return new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = "Failed to add person." };
                        }
                        using (IUserServices userService = new UserServices(db, tran))
                        {
                            var user = await userService.AddUser(new AddUsersDto()
                            {
                                PersonId = person.Data.Id,
                                ConfirmPassword = reqDto.ConfirmPassword
                            });
                            if(!user.IsSuccess)
                            {
                                tran.Rollback();
                                return new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = "Failed to add user." };
                            }
                            using (IAccountService accountService = new AccountService(db, tran))
                            {
                                var role = await accountService.AddRoleAsync(user.Data, reqDto.Role);
                                if (!role.IsSuccess)
                                {
                                    tran.Rollback();
                                    return new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = "Failed to add role." };
                                }
                            }
                            tran.Commit();
                            return new APIsResponse<EntityUsersDto> { IsSuccess = true, Data= user.Data, Message = "Successfuly Added User Details" };
                        }
                    }
                    
                }
            }
        }
    }
}
