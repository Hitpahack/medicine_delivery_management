using Dapper;
using Newtonsoft.Json.Linq;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
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
        Task<APIsResponse<EntityPersonsDto>> AddEditPerson(AddPersonDto reqDto, long personid);
        Task<APIsResponse<bool>> IsPersonExist(string email);

    }

    public class PersonService : BaseService, IPersonService
    {
        public PersonService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {
        }

        public async Task<APIsResponse<EntityPersonsDto>> AddEditPerson(AddPersonDto reqDto, long personid)
        {
            try
            {
                APIsResponse<EntityPersonsDto> apiResponse = default(APIsResponse<EntityPersonsDto>);
                if (personid > 0)
                {
                    #region Check Person Exist
                    string sql = $@"SELECT a.Id FROM {DbTables.tblPersons} a WHERE a.{nameof(Person.Id)} = {personid}";
                    var personData = await _idbConnection.QueryFirstOrDefaultAsync<EntityPersonsDto>(sql, transaction: _idbTransaction);
                    if(personData ==null)
                    {
                        return new APIsSuccsss<EntityPersonsDto>(_validateMessages.NotExist);
                    }
                    #endregion
                    #region Update Person
                    var person = _idbConnection.Update<EntityPersonsDto>(_idbTransaction, DbTables.tblPersons,
                                                new Dictionary<string, string> {
                                                    { "Gender", MapGender(reqDto.Gender) },
                                                    { "DateOfBirth", reqDto.DateOfBirth.HasValue ? reqDto.DateOfBirth.Value.ToString("yyyy-MM-dd") : null },
                                                    { "FirstName", reqDto.FirstName },
                                                    { "LastName", reqDto.LastName },
                                                    { "Picture", reqDto.Picture },                                                    
                                                    { "Mobile", reqDto.Mobile },                                                    
                                                    { "UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}                                               
                                                }, $@"ID='{personid}'");

                   
                    #endregion
                    #region Update Person Address
                    if (reqDto.Address != null)
                    {
                        if (personid > 0)
                        {
                            string sqlquery = $@"SELECT a.Id FROM {DbTables.tblUserAddress} a WHERE a.{nameof(Useraddress.PersonId)} = {personid}";
                            var addres = await _idbConnection.QueryFirstOrDefaultAsync<EntityAddressDto>(sqlquery, transaction: _idbTransaction);

                            if (addres != null)
                            {
                                
                                #region Update Address
                                var address = _idbConnection.Update<AddAddressDto>(_idbTransaction,DbTables.tblUserAddress,
                                new Dictionary<string, Object> {
                                                    { "AddressLine", reqDto.Address.AddressLine },
                                                    { "CityId", reqDto.Address.CityId },
                                                    { "StateId", reqDto.Address.StateId },
                                                    { "CountryId", reqDto.Address.CountryId },
                                                    { "Pincode", reqDto.Address.Pincode },
                                                    { "Latitude", reqDto.Address.Latitude.ToString()},
                                                    { "Longitude", reqDto.Address.Longitude.ToString()},
                                }, $@"PersonId='{person.Id}'" );
                                #endregion
                            }
                            else
                            {
                                #region Add Person Address
                                if (reqDto.Address != null)
                                {
                                    reqDto.Address.PersonId = person.Id;
                                    EntityAddressDto address = _idbConnection.Insert<EntityAddressDto>(_idbTransaction,
                                    DbTables.tblUserAddress,
                                    DapperHelper.QueryAsColumnsParma<Useraddress, AddAddressDto>(),
                                    DapperHelper.QueryAsValuesParma<Useraddress, AddAddressDto>(),
                                    reqDto.Address);
                                }
                                #endregion
                            }
                        }


                    }
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
                    reqDto.Gender=MapGender(reqDto.Gender);
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
                    }
                    #endregion
                     apiResponse = new APIsSuccsss<EntityPersonsDto>(_validateMessages.Success, person);
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
        private static string? MapGender(string? gender)
        {
            return gender?.ToLower() switch
            {
                "male" => "1",
                "female" => "2",
                "others" => "3",
                "" => null,
                null => null,
                _ => null // optional: log or throw if invalid
            };
        }

    }
}
