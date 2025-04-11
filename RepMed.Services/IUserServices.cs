using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Localize;
using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Generators;
using Newtonsoft.Json.Linq;

namespace RepMed.Services
{
    public interface IUserServices : IDisposable
    {
        Task<APIsResponse<EntityUsersDto>> AddUser(AddUsersDto reqDto, long Id);
        Task<APIsResponse<bool>> SetPasswordAsync(SetPasswordDto dto);
    }



    public class UserServices : BaseService, IUserServices
    {
        public UserServices(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityUsersDto>> AddUser(AddUsersDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityUsersDto> apiResponse = default(APIsResponse<EntityUsersDto>);
                if (Id > 0)
                {
                    #region Update User
                    var response = _idbConnection.Update<EntityUsersDto>(
                                    _idbTransaction,
                                    DbTables.tblUser,
                                    DapperHelper.QueryAsColumnsParma<User, AddUsersDto>(),
                                    reqDto,
                                    Id);
                    #endregion
                    apiResponse = new APIsSuccsss<EntityUsersDto>(_validateMessages.Success, response);
                }
                else
                {
                    #region Check UserExist
                    var isUserEmailExist = await IsEmailExist(reqDto.Email, true);
                    if (isUserEmailExist)
                        return await Task.FromResult(new APIsError<EntityUsersDto>(_validateMessages.GetAlreadyExist(reqDto.Email)) as APIsResponse<EntityUsersDto>);
                    #endregion
                    Encryption.CreatePasswordHash(reqDto.ConfirmPassword, out var passHas, out var passSalt);
                    reqDto.PasswordHash = passHas;
                    reqDto.PasswordSalt = passSalt;
                    reqDto.Status = "Active";
                    EntityUsersDto response = _idbConnection.Insert<EntityUsersDto>(_idbTransaction,
                     DbTables.tblUser,
                     DapperHelper.QueryAsColumnsParma<User, AddUsersDto>(),
                     DapperHelper.QueryAsValuesParma<User, AddUsersDto>(),
                     reqDto);
                    apiResponse = new APIsSuccsss<EntityUsersDto>(_validateMessages.RetriveSuccess, response);

                }
                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityUsersDto>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<bool>> SetPasswordAsync(SetPasswordDto reqdto)
        {
            if (reqdto.Password != reqdto.ConfirmPassword)
                return new APIsSuccsss<bool>(_validateMessages.InvalidPassword, false);

            string query = DbTables.tblUserTokens.SelectAll($@"
                            ""Token"" = '{reqdto.Token}'
                            AND ""IsUsed"" = FALSE
                            AND ""ExpiresAt"" > NOW()");

            var tokenData = await _idbConnection.QueryFirstOrDefaultAsync<UserTokenDto>(query);

            if (tokenData == null)
                return new APIsSuccsss<bool>("No data found", false);


            // Hash the password using BCrypt
            Encryption.CreatePasswordHash(reqdto.ConfirmPassword, out var passHash, out var passSalt);

            // Update user's password
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
                            Id = tokenData.UserId
                        });

            // Mark the token as used
            string updateTokenQuery = $@" UPDATE {DbTables.tblUser} SET IsUsed = TRUE WHERE Id = @Id";
            await _idbConnection.ExecuteAsync(
                updateTokenQuery,
                new { tokenData.Id });

            return new APIsSuccsss<bool>(_validateMessages.UpdateSuccess, true);
        }
    }
}
