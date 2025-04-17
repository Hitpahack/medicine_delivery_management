using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Localize;
using Dapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static RepMed.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Data.Common;

namespace RepMed.Services
{
    public interface IAccountService : IDisposable
    {
        Task<APIsResponse<Login_ResDto>> LoginAsync(Login_ReqDto reqDto);
        Task<APIsResponse<EntityUsersDto>> AddRoleAsync(EntityUsersDto reqDto, params string[] roles);
        Task<APIsResponse<string>> ForgotPassword(string Email);
        Task<APIsResponse<bool>> ResetPassword(ResetPasswordDTO model);
        Task<APIsResponse<bool>> ChangePassword(ChangePasswordDto reqDto);
        Task<APIsResponse<string>> LogoutAsync(ClaimsPrincipal user);
    }

    public class AccountService : BaseService, IAccountService
    {

        public AccountService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }
        public async Task<APIsResponse<Login_ResDto>> LoginAsync(Login_ReqDto reqDto)
        {
            try
            {
                using (IPersonService personService = new PersonService(_idbConnection, _idbTransaction))
                {

                    string sql = $@"SELECT {DapperHelper.QueryAsColumnsParma<Person, BasicPersonsDto>("pe.").Replace("\"", "`")}, 
                                           {DapperHelper.QueryAsColumnsParma<User, EntityUsersPassDto>("us.").Replace("\"", "`")}
                                    FROM {DbTables.tblPersons} pe  
                                    LEFT JOIN {DbTables.tblUser} us 
                                        ON pe.`{nameof(EntityUsersDto.Id)}` = us.`{nameof(EntityUsersDto.PersonId)}`
                                    WHERE pe.`{nameof(BasicPersonsDto.Email)}` = '{reqDto.Email}';

                                    SELECT * 
                                    FROM {DbTables.tblRole} r
                                    WHERE r.`{nameof(EntityRoleDto.Id)}` IN (
                                        SELECT ur.`{nameof(EntityUserRoleDto.RoleId)}`
                                        FROM {DbTables.tblUserRoles} ur
                                    WHERE ur.`{nameof(EntityUserRoleDto.UserId)}` = (
                                        SELECT usr.`{nameof(EntityUsersDto.Id)}`
                                        FROM {DbTables.tblUser} usr
                                        WHERE usr.`{nameof(BasicPersonsDto.Email)}` = '{reqDto.Email}'
                                        LIMIT 1 ));";

                    //var mQuery = _idbConnection.QueryMultiple(sql, transaction: _idbTransaction);
                    EntityUsersPassDto response;
                        using (var mQuery = _idbConnection.QueryMultiple(sql, transaction: _idbTransaction))
                    {
                        response = mQuery.Read<EntityPersonsDto, EntityUsersPassDto, EntityUsersPassDto>((person, user) =>
                        {
                            user.Person = person;
                            return user;

                        }).SingleOrDefault();

                        if (response == null)
                            return await Task.FromResult(new APIsError<Login_ResDto>(_validateMessages.GetNotExist("User")));

                        if (!response.EmailConfirmed ?? false)
                            return await Task.FromResult(new APIsError<Login_ResDto>(_validateMessages.EmailNotConfirm));

                        if (response.IsLocked)
                            return await Task.FromResult(new APIsError<Login_ResDto>(_validateMessages.AcNotActive));

                        if (!Encryption.VerifyPasswordHash(reqDto.Password, response.PasswordHash, response.PasswordSalt))
                            return await Task.FromResult(new APIsError<Login_ResDto>(_validateMessages.InvalidPassword));

                       
                        response.Roles = mQuery.Read<EntityRoleDto>().ToList();

                        using (var service = ServiceActivator.GetScope())
                        {
                            var userObj = _mapper.Map<EntityUsersDto, EntityUsersPassDto>(response);
                            var loginObj = _mapper.Map<Login_ResDto, EntityUsersDto>(userObj);

                            IJwtManager jwtManager = service.ServiceProvider.GetService<IJwtManager>();
                            JtwTokenResponse jwtToken = jwtManager.GenerateJWT(userObj.Id, userObj.Email, userObj.Roles.Select(r => r.RoleName).ToArray());

                            loginObj.Token = new JwtTokenDto
                            {
                                Token = jwtToken.token,
                                TokenValidTill = jwtToken.validTill,

                            };

                            string tokenSql = DbTables.tblUserJWTTokenLog.SelectAll($@" `{nameof(UserTokenLogDto.UserID)}` = {response.Id}");
                            var tokenResponse = _idbConnection.QuerySingleOrDefault<EntityUserTokenLogDto>(tokenSql, transaction: _idbTransaction);

                            if (tokenResponse != null)
                            {
                                var userToken = _idbConnection.Update<UserTokenLogDto>(_idbTransaction, DbTables.tblUserJWTTokenLog,
                                                new Dictionary<string, string> {
                                                    { "Token", jwtToken.token },
                                                    { "TokenValidTill", jwtToken.validTill.ToString("yyyy-MM-dd HH:MM:ss") }
                                                }, $@"ID='{tokenResponse.Id}'");


                                if (userToken == null)
                                {
                                    return await Task.FromResult(new APIsError<Login_ResDto>(_validateMessages.InvalidToken));
                                }
                            }
                            else
                            {
                                //var tokenId = _idbConnection.Insert<EntityUserTokenLogDto>(_idbTransaction, DbTables.tblUserJWTTokenLog,
                                //     new Dictionary<string, string> {
                                //        { nameof(Userjwttokenlog.UserId), userObj.Id.ToString()},
                                //        { nameof(Userjwttokenlog.TokenValidTill), jwtToken.validTill.ToString("yyyy-MM-dd HH:mm:ss") },
                                //        { nameof(Userjwttokenlog.CreatedDate), DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") },
                                //        { nameof(Userjwttokenlog.Token), jwtToken.token }
                                //    });

                                tokenSql = $@"
                                            INSERT INTO {DbTables.tblUserJWTTokenLog} 
                                                (UserId, Token, TokenValidTill, CreatedDate) 
                                            VALUES 
                                                ({userObj.Id}, '{jwtToken.token}', '{jwtToken.validTill:yyyy-MM-dd HH:mm:ss}', '{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}');
                                            SELECT LAST_INSERT_ID();";


                                loginObj.Token.TokenId = _idbConnection.QuerySingle<int>(tokenSql, transaction: _idbTransaction);
                            }


                            return await Task.FromResult(new APIsSuccsss<Login_ResDto>(_validateMessages.Success, loginObj, jwtToken.Claims));
                        }
                    }               
                }
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new APIsError<Login_ResDto>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<EntityUsersDto>> AddRoleAsync(EntityUsersDto reqDto, params string[] roles)
        {
            try
            {

                APIsResponse<EntityUsersDto> apiResponse = default(APIsResponse<EntityUsersDto>);

                List<EntityRoleDto> addRoles = new List<EntityRoleDto>();

                #region Check UserExist
                if (roles == null || roles.Length == 0)
                {
                    apiResponse = new APIsError<EntityUsersDto>(("Please enter roles"));
                    return await Task.FromResult(apiResponse);
                }

                foreach (var role in roles)
                {
                    string sqlExist = DbTables.tblRole.SelectAll("LOWER(RoleName) = @RoleName");

                    var isExist = _idbConnection.QueryFirstOrDefault<EntityRoleDto>(
                        sqlExist,
                        new { RoleName = role.Trim().ToLower() },
                        transaction: _idbTransaction
                    );

                    if (isExist == null)
                    {
                        apiResponse = new APIsError<EntityUsersDto>(_validateMessages.GetNotExist($"{role} Role"));
                        return await Task.FromResult(apiResponse);
                    }
                    else
                    {
                        addRoles.Add(isExist);
                    }

                }
                #endregion

                var values = addRoles.Select(r => $"({reqDto.Id}, {r.Id})");
                string sql = $@"
                            INSERT INTO {DbTables.tblUserRoles} (UserId, RoleId)
                            VALUES {string.Join(",", values)};
                        ";


                var response = await _idbConnection.ExecuteAsync(sql, transaction: _idbTransaction);
                if (response > 0)
                {
                    reqDto.Roles = addRoles;
                    apiResponse = new APIsSuccsss<EntityUsersDto>(_validateMessages.Success, reqDto);
                }
                else
                {
                    apiResponse = new APIsError<EntityUsersDto>(_validateMessages.InternalError);
                }

                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityUsersDto>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<string>> ForgotPassword(string Email)
        {
            try
            {


                #region Validate User
                if (!(await IsEmailExist(Email, true)))
                {
                    return await Task.FromResult(new APIsError<string>("User doesn't exist"));
                }

                string sqlExist = DbTables.tblUser.SelectAll($@" ""{nameof(BasicPersonsDto.Email)}"" = '{Email}' ");
                EntityUsersPassDto response = _idbConnection.QueryFirstOrDefault<EntityUsersPassDto>(sqlExist, transaction: _idbTransaction);

                if (response == null)
                    return await Task.FromResult(new APIsError<string>(_validateMessages.GetNotExist("User")));

                if (!response.EmailConfirmed ?? false)
                    return await Task.FromResult(new APIsError<string>(_validateMessages.EmailNotConfirm));

                if (response.IsLocked)
                    return await Task.FromResult(new APIsError<string>(_validateMessages.AcNotActive));

                #endregion

                #region GenerateToken
                string encKey = _appSettings.AppSecreateKey;
                string token = Encryption.EncryptTripleDES(Encryption.GenerateCode(), encKey);
                #endregion

                #region Add/Edit Password Token
                AddEditResetPassToken(response.Id.ToString(), token);

                #endregion

                token = Encryption.DecryptTripleDES(token, encKey);

                var param = new Dictionary<string, string> { { "token", token }, { "email", Email } };

                var callback = QueryHelpers.AddQueryString("https://localhost:44379", param);
                return await Task.FromResult(new APIsSuccsss<string>("Success", callback));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<string>(ex.GetActualError()));
            }
        }   
        public async Task<APIsResponse<bool>> ChangePassword(ChangePasswordDto reqDto)
        {
            try
            {
                #region Get Login User details
                string query = DbTables.tblUser.SelectAll($@" `{nameof(EntityUsersDto.Id)}` = {reqDto.UserId}");

                var user = _idbConnection.QuerySingleOrDefault(query, transaction: _idbTransaction);
                if(user==null)
                    return await Task.FromResult(new APIsError<bool>("User doesn't exist"));
                #endregion

                #region Verify Current Password
                if (!Encryption.VerifyPasswordHash(reqDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                    return await Task.FromResult(new APIsError<bool>(_validateMessages.InvalidPassword));

                #endregion

                #region Update users password
                Encryption.CreatePasswordHash(reqDto.ConfirmPassword, out var passHash, out var passSalt);

                string updateUserQuery = $@" UPDATE {DbTables.tblUser} 
                                        SET PasswordHash = @PasswordHash,
                                        PasswordSalt = @PasswordSalt
                                        WHERE Id = @Id";
                await _idbConnection.ExecuteAsync(
                            updateUserQuery,
                            new
                            {
                                PasswordHash = passHash,
                                PasswordSalt = passSalt,
                                Id = reqDto.UserId
                            }, transaction: _idbTransaction);
                #endregion
                return new APIsSuccsss<bool>(_validateMessages.UpdateSuccess, true);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<bool>> ResetPassword(ResetPasswordDTO model)
        {
            try
            {


                #region Validate User
                if (!(await IsEmailExist(model.Email, true)))
                {
                    return await Task.FromResult(new APIsError<bool>("User doesn't exist"));
                }

                string sqlExist = DbTables.tblUser.SelectAll($@" ""{nameof(BasicPersonsDto.Email)}"" = '{model.Email}' ");
                EntityUsersPassDto response = _idbConnection.QueryFirstOrDefault<EntityUsersPassDto>(sqlExist, transaction: _idbTransaction);

                if (response == null)
                    return await Task.FromResult(new APIsError<bool>(_validateMessages.GetNotExist("Email")));

                if (!response.EmailConfirmed ?? false)
                    return await Task.FromResult(new APIsError<bool>(_validateMessages.EmailNotConfirm));

                if (response.IsLocked)
                    return await Task.FromResult(new APIsError<bool>(_validateMessages.AcNotActive));

                #endregion

                string encKey = _appSettings.AppSecreateKey;
                string encryptToken = Encryption.EncryptTripleDES(model.Token, encKey);

                sqlExist = DbTables.tblCodeRequest.SelectAll($@" ""{nameof(CodeRequestDtos.Userid)}"" = '{response.Id}' ");

                CodeRequestDtos token = _idbConnection.QueryFirstOrDefault<CodeRequestDtos>(sqlExist, transaction: _idbTransaction);
                if (token == null)
                    return await Task.FromResult(new APIsError<bool>(_validateMessages.GetNotExist("Token")));

                var isValidToken = token.SecurityCode == encryptToken;
                var isTokenExpired = token.ValidTo.Subtract(DateTime.UtcNow).TotalMinutes < 1;

                if (!isValidToken || isTokenExpired)
                    return await Task.FromResult(new APIsError<bool>(_validateMessages.TokenExpired));

                if (!model.ConfirmPassword.IsValidPassword(out var message))
                    return await Task.FromResult(new APIsError<bool>(message));



                //change password
                Encryption.CreatePasswordHash(model.ConfirmPassword, out var passHas, out var passSalt);
                //EntityUsersDto entityUsersDto = _idbConnection.Update<EntityUsersDto>(_idbTransaction, DbTables.tblUser,
                //    new Dictionary<string, string> {
                //    { "PasswordHash", "PasswordHash" },
                //    { "PasswordSalt", "PasswordSalt" }
                //    }, data: new Dictionary<string, object> {
                //    { "PasswordHash", passHas },
                //    { "PasswordSalt", passSalt }
                //    }, $@" ""Id""='{response.Id}' ", "RETURNING *");

                EntityUsersDto entityUsersDto = _idbConnection.Update<EntityUsersDto>(_idbTransaction, DbTables.tblUser,
                    new Dictionary<string, object> {
                    { nameof(EntityUsersPassDto.PasswordHash), passHas },
                    { nameof(EntityUsersPassDto.PasswordSalt), passSalt }
                   }, $@" ""{nameof(EntityUsersPassDto.Id)}""='{response.Id}' ", "RETURNING *");


                UserTokenLogDto userToken = _idbConnection.Update<UserTokenLogDto>(_idbTransaction, DbTables.tblCodeRequest,
                   new Dictionary<string, object> {
                    { nameof(CodeRequestDtos.ValidTo), DateTime.UtcNow.AddDays(-1) },
                    { nameof(CodeRequestDtos.IsExpired),  true }
                   }, $@" ""{nameof(CodeRequestDtos.Userid)}""='{response.Id}' ", "RETURNING *");


                //var xml = await _templatesService.GetTemplate();
                //string emailBody = xml.GetXmlNode("ResetPassword/Body");
                //string emailSubject = xml.GetXmlNode("ResetPassword/Subject");
                //var tenant_ = await _tenantService.GetTenant(user.TenantId);

                //#region Sent User Registration Mail
                //var messageToSend = @$"Hello {user.FirstName }! { Environment.NewLine }
                //                     You have succesfully changed your password,  { Environment.NewLine }
                //                    if this was you, you are all set!,  { Environment.NewLine }
                //                    if this was not you, please change your password";

                //emailBody = emailBody
                //.Replace("@@@Tenant", tenant_.TenantName)
                //.Replace("@@@UserEmail", user.FirstName)
                //.Replace("@@@_Tenantmail", tenant_.Email)
                //.Replace("@@@Message", messageToSend);

                ////send email.
                //await _emailSender.SendEmailAsync(user.Email, emailSubject, emailBody);
                //#endregion

                //resp.Message = "Password has been successfully reseted";
                //resp.isSuccess = true;

                return await Task.FromResult(new APIsSuccsss<bool>("Success", true));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }
        }

        private BaseCodeRequestDtos AddEditResetPassToken(string userId, string token)
        {

            string sql = DapperHelper.SelectAll(DbTables.tblCodeRequest, @$" ""Userid"" = '{userId}'");
            CodeRequestDtos usercode = _idbConnection.QueryFirstOrDefault<CodeRequestDtos>(sql, transaction: _idbTransaction);

            // Adding expiry for todays midnight
            var validTokenTime = DateTime.UtcNow.AddMinutes(_adminSettings.PasswordResetTokenExpiredInMiniute);

            if (usercode == null)
            {
                //usercode = _idbConnection.Insert<CodeRequestDtos>(_idbTransaction, DbTables.tblCodeRequest,
                //    //DapperHelper.QueryAsColumnsParma<Coderequest, BaseCodeRequestDtos>(),
                //    //DapperHelper.QueryAsValuesParma<Coderequest, BaseCodeRequestDtos>(),
                //    new BaseCodeRequestDtos
                //    {
                //        SecurityCode = token,
                //        ValidTo = validTokenTime,
                //        Userid = userId
                //    }, "RETURNING *");


            }
            else
            {
                usercode = _idbConnection.Update<CodeRequestDtos>(_idbTransaction, DbTables.tblCodeRequest,
                    new Dictionary<string, string> {
                        { nameof(CodeRequestDtos.SecurityCode), token },
                        { nameof(CodeRequestDtos.ValidTo), validTokenTime.ToString() }
                    }, $@" ""{nameof(CodeRequestDtos.Userid)}""='{userId}'", "RETURNING *");

            }
            return usercode;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<string>> LogoutAsync(ClaimsPrincipal user)
        {
            try
            {
                var userId = user.FindFirst("UserId")?.Value;
                var token = user.FindFirst("Token")?.Value;
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                    return new APIsError<string>("Invalid token or user");


                // Optional: Invalidate the token by deleting from DB or marking as expired
                var sql = $@"
                            UPDATE {DbTables.tblUserJWTTokenLog}
                            SET IsActive = 0, RevokedOn = '{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}'
                            WHERE UserId = @UserId AND Token = @Token AND IsActive = 1;
                        ";

                await _idbConnection.ExecuteAsync(sql, new { UserId = userId, Token = token }, transaction: _idbTransaction);

                return new APIsSuccsss<string>("Logout successful");
            }
            catch (Exception ex)
            {
                return new APIsError<string>(ex.GetActualError());
            }
        }
    }
}
