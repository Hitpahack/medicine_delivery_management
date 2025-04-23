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
using System.Collections.Generic;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.PharmacyPage;
using RepMed.Dtos.UsersPage;

namespace RepMed.Services
{
    public interface IUserServices : IDisposable
    {
        Task<APIsResponse<EntityUsersDto>> AddEditUser(AddUsersDto reqDto, long Id);
        Task<APIsResponse<bool>> SetPassword(SetPasswordDto dto);
        Task<APIsResponse<GetUserDto>> GetUser(long Id);
        Task<APIsResponse<Datatable<UsersPagingResponse>>> GetUsers(UsersPagingRequest reqDto);
    }

    public class UserServices : BaseService, IUserServices
    {
        public UserServices(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityUsersDto>> AddEditUser(AddUsersDto reqDto, long personid)
        {
            try
            {
                APIsResponse<EntityUsersDto> apiResponse = default(APIsResponse<EntityUsersDto>);
                if (personid > 0)
                {
                    #region Check User Exist
                    string sql = $@"SELECT a.Id FROM {DbTables.tblUser} a WHERE a.{nameof(User.PersonId)} = {personid}";
                    var userData = await _idbConnection.QueryFirstOrDefaultAsync<EntityUsersDto>(sql, transaction: _idbTransaction);
                    if (userData == null)
                    {
                        apiResponse = new APIsSuccsss<EntityUsersDto>(_validateMessages.NotExist);
                    }
                    #endregion
                    #region Update User
                    var person = _idbConnection.Update<EntityUsersDto>(_idbTransaction, DbTables.tblUser,
                                               new Dictionary<string, string> {
                                                    { "FirstName", reqDto.FirstName },
                                                    { "LastName", reqDto.LastName }
                                               }, $@"PersonId='{personid}'");
                   
                    #endregion
                    apiResponse = new APIsSuccsss<EntityUsersDto>(_validateMessages.UpdateSuccess, person);
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
                    reqDto.CreatedAt = DateTime.Now;
                    reqDto.UpdatedAt = DateTime.Now;
                    EntityUsersDto response = _idbConnection.Insert<EntityUsersDto>(_idbTransaction,
                     DbTables.tblUser,
                     DapperHelper.QueryAsColumnsParma<User, AddUsersDto>(),
                     DapperHelper.QueryAsValuesParma<User, AddUsersDto>(),
                     reqDto);
                    apiResponse = new APIsSuccsss<EntityUsersDto>(_validateMessages.AddSuccess, response);

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

        public async Task<APIsResponse<GetUserDto>> GetUser(long personid)
        {
            try
            {
                var sql = $@"
                            SELECT u.Id as UserId , p.Id as PersonId, p.FirstName,p.LastName,p.Email,p.Mobile,p.Gender,p.DateOfBirth,p.Email,a.AddressLine,a.CityId,a.StateId,a.CountryId,a.Pincode
                            FROM {DbTables.tblUser} u
                            INNER JOIN {DbTables.tblPersons} p ON u.PersonId = p.Id
                            INNER JOIN {DbTables.tblUserAddress} a ON a.PersonId = p.Id
                            WHERE u.PersonId = @Id;
                        ";
                var result = await _idbConnection.QueryAsync<GetUserDto, BasicAddressDto, GetUserDto>(
                                sql,
                                (user, address) =>
                                {
                                    user.Address = address;
                                    return user;
                                },
                                new { Id = personid },
                                splitOn: "AddressLine", // this tells Dapper where to start splitting the object
                                transaction: _idbTransaction
                            );

                var userData = result.FirstOrDefault();
                if (userData == null)
                    return new APIsError<GetUserDto>(_validateMessages.NotExist);

                return new APIsSuccsss<GetUserDto>(_validateMessages.RetriveSuccess, userData);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<GetUserDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<Datatable<UsersPagingResponse>>> GetUsers(UsersPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<UsersPagingResponse>> apiResponse = default;
                string orderBy;
                if (reqDto.Order[0].Column == 0)
                    orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|desc";
                else
                    orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<UsersPagingResponse>(
                               sql: "GET_USERS_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<UsersPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                if (result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<UsersPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<UsersPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<UsersPagingResponse>>(ex.GetActualError()));
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
