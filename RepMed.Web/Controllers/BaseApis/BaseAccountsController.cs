using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using System.Security.Claims;
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
        protected async Task<APIsResponse<string>> Logout()
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var logout = await accountService.LogoutAsync(User);
                        if (logout.IsSuccess)
                            tran.Commit();
                        else
                            tran.Rollback();

                        return logout;
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
        protected async Task<APIsResponse<bool>> ChangePassword(ChangePasswordDto reqDto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var isReset = await accountService.ChangePassword(reqDto);
                        if (isReset.IsSuccess)
                            tran.Commit();
                        else
                            tran.Rollback();

                        return isReset;
                    }
                }
            }
        }
        protected async Task<APIsResponse<bool>> SetPassword(SetPasswordDto reqdto)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IAccountService accountService = new AccountService(db, tran))
                    {
                        var result = await accountService.SetPassword(reqdto);
                        if (!result.IsSuccess)
                        {
                            tran.Rollback();
                        }
                        tran.Commit();
                        return result;
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
        protected async Task<APIsResponse<EntityUsersDto>> AddEditUser(AddPersonDto reqDto, long personid)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPersonService personService = new PersonService(db, tran))
                    {   
                        var person = await personService.AddEditPerson(reqDto,personid);
                        if(!person.IsSuccess)
                        {
                            tran.Rollback();
                            return new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = person.Message };
                        }
                        using (IUserServices userService = new UserServices(db, tran))
                        {
                            var userObj = _mapper.Map<AddUsersDto, EntityPersonsDto>(person.Data, (d) =>
                            {
                                d.PersonId = person.Data.Id;
                                d.ConfirmPassword = reqDto.ConfirmPassword;
                                d.Email = reqDto.Email;
                            });
                            var user = await userService.AddEditUser(userObj, personid);
                            if(!user.IsSuccess)
                            {
                                tran.Rollback();
                                return new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = user.Message };
                            }
                            if (personid == 0)
                            {
                                if (!string.IsNullOrEmpty(reqDto.Role))
                                {
                                    using (IAccountService accountService = new AccountService(db, tran))
                                    {
                                        var role = await accountService.AddRoleAsync(user.Data, reqDto.Role);
                                        if (!role.IsSuccess)
                                        {
                                            tran.Rollback();
                                            return new APIsResponse<EntityUsersDto> { IsSuccess = false, Message = role.Message };
                                        }
                                    }
                                }
                            }
                            tran.Commit();
                            return new APIsResponse<EntityUsersDto> { IsSuccess = true, Data= user.Data, Message = user.Message};
                        }
                    }
                    
                }
            }
        }
    }
}
