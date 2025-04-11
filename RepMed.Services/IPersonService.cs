using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IPersonService : IDisposable
    {
        /// <summary>
        /// Add person 
        /// </summary>
        /// <param name="reqDto">AddPersonDto</param>
        /// <returns>[PersonsContactDto] Person info with contact</returns>
        Task<APIsResponse<EntityPersonsDto>> AddPerson(AddPersonDto reqDto, long Id);
        Task<APIsResponse<bool>> IsPersonExist(string email);

    }

    public class PersonService : BaseService, IPersonService
    {
        public PersonService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {
        }

        public async Task<APIsResponse<EntityPersonsDto>> AddPerson(AddPersonDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityPersonsDto> apiResponse = default(APIsResponse<EntityPersonsDto>);
                if (Id>0)
                {
                    #region Update Person
                    var person = _idbConnection.Update<EntityPersonsDto>(
                                    _idbTransaction,
                                    DbTables.tblPersons,
                                    DapperHelper.QueryAsColumnsParma<Person, AddPersonDto>(),
                                    reqDto,
                                    Id);
                    #endregion
                    apiResponse = new APIsSuccsss<EntityPersonsDto>(_validateMessages.Success, person);
                }
                else
                {
                    #region Check Email/Phone exist
                    if ((await IsEmailExist(reqDto.Email, true)))
                    {
                        return await Task.FromResult(new APIsError<EntityPersonsDto>(
                            _validateMessages.GetAlreadyExist(reqDto.Email, "Please choose another one")) as APIsResponse<EntityPersonsDto>);
                    }

                    if ((await IsPhoneExist(reqDto.Mobile)))
                    {
                        return await Task.FromResult(new APIsError<EntityPersonsDto>(_validateMessages.GetAlreadyExist(reqDto.Mobile, "Please choose another one")) as APIsResponse<EntityPersonsDto>);
                    }
                    #endregion
                    string sql;
                    #region Add Person
                    EntityPersonsDto person = _idbConnection.Insert<EntityPersonsDto>(_idbTransaction,
                        DbTables.tblPersons,
                        DapperHelper.QueryAsColumnsParma<Person, AddPersonDto>(),
                        DapperHelper.QueryAsValuesParma<Person, AddPersonDto>(),
                        reqDto);

                    #endregion

                    #region Add Person Address
                    if (reqDto.Address != null)
                    {
                        reqDto.Address.PersonId = person.Id;
                        EntityAddressDto address = _idbConnection.Insert<EntityAddressDto>(_idbTransaction,
                        DbTables.tblUserAddress,
                        DapperHelper.QueryAsColumnsParma<Useraddress, AddAddressDto>(),
                        DapperHelper.QueryAsValuesParma<Useraddress, AddAddressDto>(),
                        reqDto.Address);
                        apiResponse = new APIsSuccsss<EntityPersonsDto>(_validateMessages.Success, person);
                    }
                    #endregion
                }

                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityPersonsDto>(ex.GetActualError()));
            }
        }

         public async Task<APIsResponse<bool>> IsPersonExist(string email)
        {
            #region Check Email/Phone exist
            if ((await IsEmailExist(email)))
                return await Task.FromResult(new APIsSuccsss<bool>(_validateMessages.GetAlreadyExist(email, "Please choose another one")) as APIsResponse<bool>);

            return await Task.FromResult(new APIsSuccsss<bool>(_validateMessages.GetNotExist("user")) as APIsResponse<bool>);
            #endregion
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
