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
using Org.BouncyCastle.Asn1.Ess;

namespace RepMed.Services
{
    public interface IUserServices : IDisposable
    {
        Task<APIsResponse<EntityUsersDto>> AddUser(AddUsersDto reqDto, long Id);
        Task<APIsResponse<bool>> SetPassword(SetPasswordDto dto);
        Task<APIsResponse<GetUserDto>> GetUser(long Id);
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

        public async Task<APIsResponse<GetUserDto>> GetUser(long id)
        {
            try
            {
                var sql = $@"
                            SELECT u.Id, p.FirstName,p.LastName,p.Email,p.Mobile,p.Gender,p.DateOfBirth
                            FROM {DbTables.tblUser} u
                            INNER JOIN {DbTables.tblPersons} p ON u.PersonId = p.Id
                            WHERE u.Id = @Id;
                        ";
                var userData = await _idbConnection.QueryFirstOrDefaultAsync<GetUserDto>(sql, new { Id = id },transaction:_idbTransaction);

                if (userData == null)
                    return new APIsError<GetUserDto>(_validateMessages.NotExist);

                return new APIsSuccsss<GetUserDto>(_validateMessages.RetriveSuccess, userData);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<GetUserDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<bool>> SetPassword(SetPasswordDto reqdto)
        {
            try
            {
                #region Get User Token
                string query = DbTables.tblUserTokens.SelectAll($@"
                            `Token` = '{reqdto.Token}'
                            AND `IsUsed` = FALSE
                            AND `ExpiresAt` > NOW()");

                var tokenData = await _idbConnection.QueryFirstOrDefaultAsync<UserTokenDto>(query,transaction:_idbTransaction);
                #endregion

                if (tokenData == null)
                    return new APIsSuccsss<bool>("No data found", false);

                Encryption.CreatePasswordHash(reqdto.ConfirmPassword, out var passHash, out var passSalt);

                #region Update users password
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
                            },transaction:_idbTransaction);
                #endregion

                #region Mark token as used (password has been set)
                string updateTokenQuery = $@" UPDATE {DbTables.tblUserTokens} SET IsUsed = TRUE WHERE Id = @Id";
                await _idbConnection.ExecuteAsync(
                    updateTokenQuery,
                    new { tokenData.Id },transaction:_idbTransaction);

                #endregion

                return new APIsSuccsss<bool>(_validateMessages.UpdateSuccess, true);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));

            }
        }
    }
}
