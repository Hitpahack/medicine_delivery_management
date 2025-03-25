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

namespace RepMed.Services
{
    public interface IAccountService : IDisposable
    {
        Task<APIsResponse<Login_ResDto>> LoginAsync(Login_ReqDto reqDto);
        Task<APIsResponse<EntityUsersDto>> AddRoleAsync(EntityUsersDto reqDto, params string[] roles);
        Task<APIsResponse<string>> ForgotPassword(string Email);
        Task<APIsResponse<bool>> ResetPassword(ResetPasswordDTO model);

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

                    string sql = $@"select {(DapperHelper.QueryAsColumnsParma<Person, BasicPersonsDto>("pe."))}, 
                                       {(DapperHelper.QueryAsColumnsParma<User, EntityUsersPassDto>("us."))}
                            from {DbTables.tblPersons} pe  
                            left join {DbTables.tblUser} us on pe.""{nameof(EntityUsersDto.Id)}"" = us.""{nameof(EntityUsersDto.PersonId)}""
                            WHERE pe.""{nameof(BasePerson.Email)}"" = '{reqDto.Email}'; 
                            select * from {DbTables.tblRole} where ""{nameof(EntityRoleDto.Id)}"" in (select ur.""{nameof(EntityUserRoleDto.RoleId)}"" from {DbTables.tblUserRoles} ur where ur.""{nameof(EntityUserRoleDto.UserId)}"" = (select usr.""{nameof(EntityUsersDto.Id)}"" from {DbTables.tblUser} usr where usr.""{nameof(BasePerson.Email)}"" = '{reqDto.Email}'))";
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
                            JtwTokenResponse jwtToken = jwtManager.GenerateJWT(userObj.Id, userObj.Email, userObj.Roles.Select(r => r.Name).ToArray());

                            loginObj.Token = new JwtTokenDto
                            {
                                Token = jwtToken.token,
                                TokenValidTill = jwtToken.validTill,

                            };

                            string tokenSql = DbTables.tblUserTokenLog.SelectAll($@" ""{nameof(UserTokenLogDto.UserID)}"" = '{response.Id}'");
                            var tokenResponse = _idbConnection.QuerySingleOrDefault<EntityUserTokenLogDto>(tokenSql, transaction: _idbTransaction);

                            if (tokenResponse != null)
                            {
                                var userToken = _idbConnection.Update<UserTokenLogDto>(_idbTransaction, DbTables.tblUserTokenLog,
                            new Dictionary<string, string> {
                        { "Token", jwtToken.token },
                        { "TokenValidTill", jwtToken.validTill.ToString() }
                            }, $@" ""ID""='{tokenResponse.Id}'", "RETURNING *");


                                if (userToken == null)
                                {
                                    return await Task.FromResult(new APIsError<Login_ResDto>(_validateMessages.InvalidToken));
                                }
                            }
                            else
                            {
                                var tokenId = _idbConnection.Insert<EntityUserTokenLogDto>(_idbTransaction, DbTables.tblUserTokenLog,
                                     new Dictionary<string, string> {
                                { nameof(Usertokenlog.Id), userObj.Id.ToString() },
                                { nameof(Usertokenlog.TokenValidTill), jwtToken.validTill.ToString() },
                                { nameof(Usertokenlog.CreatedDate), DateTime.UtcNow.ToString() },
                                { nameof(Usertokenlog.Token), jwtToken.token }
                                    }, @"RETURNING * ");


                                loginObj.Token.TokenId = tokenId.Id;
                                //tokenSql = $"Insert into {DbTables.tblUserTokenLog} " +
                                //    $"(UserID,Token,TokenValidTill,CreatedDate)  OUTPUT INSERTED.Id " +
                                //    $"Values ('{userObj.Id}','{jwtToken.token}','{jwtToken.validTill}','{DateTime.UtcNow}')";

                                //loginObj.Token.TokenId = _idbConnection.QuerySingle<int>(tokenSql, transaction: _idbTransaction);
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
                    string sqlExist = DbTables.tblRole.SelectAll(@$" LOWER(""{nameof(BasicRoleDto.Name)}"") = '{role.Trim().ToLower()}'");
                    EntityRoleDto isExist = _idbConnection.QueryFirstOrDefault<EntityRoleDto>(sqlExist, transaction: _idbTransaction);
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

                var values = addRoles.Select(r => string.Concat($"('{reqDto.Id}','{r.Id}')"));
                string sql = $@"insert into {DbTables.tblUserRoles} (""{nameof(EntityUserRoleDto.UserId)}"",""{nameof(EntityUserRoleDto.RoleId)}"") 
                                values {string.Join(",", values)}";

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
                usercode = _idbConnection.Insert<CodeRequestDtos>(_idbTransaction, DbTables.tblCodeRequest,
                    DapperHelper.QueryAsColumnsParma<Coderequest, BaseCodeRequestDtos>(),
                    DapperHelper.QueryAsValuesParma<Coderequest, BaseCodeRequestDtos>(),
                    new BaseCodeRequestDtos
                    {
                        SecurityCode = token,
                        ValidTo = validTokenTime,
                        Userid = userId
                    }, "RETURNING *");


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
    }
}
