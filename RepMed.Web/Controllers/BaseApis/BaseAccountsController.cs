using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using Microsoft.Extensions.Options;
using Npgsql;
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

            using (var db = new NpgsqlConnection(_appSettings.ConnectionString))
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
            using (var db = new NpgsqlConnection(_appSettings.ConnectionString))
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
            using (var db = new NpgsqlConnection(_appSettings.ConnectionString))
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
    }
}
