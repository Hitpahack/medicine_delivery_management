using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Localize;
using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IUserServices : IDisposable
    {
        Task<APIsResponse<EntityUsersDto>> AddUser(AddUsersDto reqDto);
    }



    public class UserServices : BaseService, IUserServices
    {
        public UserServices(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityUsersDto>> AddUser(AddUsersDto reqDto)
        {
            try
            {
                APIsResponse<EntityUsersDto> apiResponse = default(APIsResponse<EntityUsersDto>);
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
    }
}
