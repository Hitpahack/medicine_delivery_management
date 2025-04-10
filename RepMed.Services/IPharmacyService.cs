using Newtonsoft.Json.Linq;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IPharmacyService : IDisposable
    {
        Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id  );
        Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id );
    }

    public class PharmacyService : BaseService, IPharmacyService
    {

        public PharmacyService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id)
        {
            try
            {
                APIsResponse<AddPharmacyDto> apiResponse = default;
                
                if(Id==0)
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
                    var pharmacy = _idbConnection.UpdateById<AddPharmacyDto>(
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

       

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
