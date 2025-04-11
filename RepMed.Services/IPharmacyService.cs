using Dapper;
using Newtonsoft.Json.Linq;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IPharmacyService : IDisposable
    {
        Task<APIsResponse<PharmacyBankDetailsDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id);
        Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id);
        Task<APIsResponse<IEnumerable<BasePharmacyDto>>> GetAllPharmacies();
    }

    public class PharmacyService : BaseService, IPharmacyService
    {

        public PharmacyService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<PharmacyBankDetailsDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id)
        {
            try
            {
                APIsResponse<PharmacyBankDetailsDto> apiResponse = default;

                if (Id == 0)
                {
                    #region Add Pharmacy Bank Details
                    var pharmacyBank = _idbConnection.Insert<PharmacyBankDetailsDto>(_idbTransaction,
                                   DbTables.tblPharmacyBankDetails,
                                   DapperHelper.QueryAsColumnsParma<Pharmacybankdetail, PharmacyBankDetailsDto>(),
                                   DapperHelper.QueryAsValuesParma<Pharmacybankdetail, PharmacyBankDetailsDto>(),
                                   reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<PharmacyBankDetailsDto>("Pharmacy bank details added successfully", pharmacyBank);
                }
                else
                {
                    #region Update Pharmacy
                    var pharmacyBank = _idbConnection.Update<AddPharmacyDto>(
                                    _idbTransaction,
                                    DbTables.tblPharmacy,
                                    DapperHelper.QueryAsColumnsParma<Pharmacy, AddPharmacyDto>(),
                                    reqDto,
                                    reqDto.Id);
                    #endregion
                    apiResponse = new APIsSuccsss<PharmacyBankDetailsDto>("Pharmacy bank details Updated successfully", pharmacyBank);

                }
                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<PharmacyBankDetailsDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id)
        {
            try
            {
                APIsResponse<AddPharmacyDto> apiResponse = default;

                if (Id == 0)
                {
                    #region Add Pharmacy
                    var pharmacy = _idbConnection.Insert<AddPharmacyDto>(_idbTransaction,
                                   DbTables.tblPharmacy,
                                   DapperHelper.QueryAsColumnsParma<Pharmacy, AddPharmacyDto>(),
                                   DapperHelper.QueryAsValuesParma<Pharmacy, AddPharmacyDto>(),
                                   reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<AddPharmacyDto>("Pharmacy created successfully", pharmacy);
                }
                else
                {

                    #region Update Pharmacy
                    var pharmacy = _idbConnection.Update<AddPharmacyDto>(
                                    _idbTransaction,
                                    DbTables.tblPharmacy,
                                    DapperHelper.QueryAsColumnsParma<Pharmacy, AddPharmacyDto>(),
                                    reqDto,
                                    reqDto.Id);
                    #endregion
                    apiResponse = new APIsSuccsss<AddPharmacyDto>("Pharmacy Updated successfully", pharmacy);

                }
                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<AddPharmacyDto>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<IEnumerable<BasePharmacyDto>>> GetAllPharmacies()
        {
            try
            {
                APIsResponse<IEnumerable<BasePharmacyDto >> apiResponse = default;

                #region Get All Pharmacy 
                var sql = @"
                            SELECT *                             
                            FROM pharmacies p
                            LEFT JOIN pharmacybankdetails b ON p.Id = b.PharmacyId";
                var pharmacyBank = await _idbConnection.QueryAsync<AddPharmacyDto, PharmacyBankDetailsDto, BasePharmacyDto>(
                        sql,
                        (pharmacy, bankDetails) => new BasePharmacyDto
                        {
                            Pharmacy = pharmacy,
                            PharmacyBankDetails = bankDetails
                        },
                        transaction: _idbTransaction,
                        splitOn: "PharmacyId"
                );
                #endregion
                if(pharmacyBank.Any())
                    apiResponse = new APIsSuccsss<IEnumerable<BasePharmacyDto>>("Pharmacy details retrieved successfully", pharmacyBank);
                else
                    apiResponse = new APIsSuccsss<IEnumerable<BasePharmacyDto>>("No Pharmacy Found", pharmacyBank);


                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<IEnumerable<BasePharmacyDto>>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
