using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos.CMSPage;
using RepMed.Dtos.DataTables;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepMed.Dtos.FAQ;

namespace RepMed.Services
{
    public interface IFAQServices : IDisposable
    {
        Task<APIsResponse<EntityFAQDto>> AddEditFAQ(BaseFAQDto reqDto, long Id);
        Task<APIsResponse<EntityFAQDto>> ChangeFAQStatus(long Id, bool status);
        Task<APIsResponse<bool>> DeleteFAQ(long Id);
        Task<APIsResponse<Datatable<FAQPagingResponse>>> GetAllFAQ(FAQPagingRequest reqDto);
        Task<APIsResponse<EntityFAQDto>> GetFAQ(long Id);
    }
    public class FAQService : BaseService, IFAQServices
    {
        public FAQService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {
            
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<EntityFAQDto>> AddEditFAQ(BaseFAQDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityFAQDto> apiResponse = default(APIsResponse<EntityFAQDto>);
                if (Id == 0)
                {
                    #region Add FAQ
                    reqDto.CreatedAt = DateTime.Now;
                    reqDto.UpdatedAt = DateTime.Now;
                    reqDto.IsActive = true;
                    EntityFAQDto response = _idbConnection.Insert<EntityFAQDto>(_idbTransaction,
                        DbTables.tblFAQ,
                        DapperHelper.QueryAsColumnsParma<Faq, BaseFAQDto>(),
                        DapperHelper.QueryAsValuesParma<Faq, BaseFAQDto>(),
                        reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<EntityFAQDto>("FAQ Created Successfully", response);
                }
                else
                {
                    #region Update FAQ
                    EntityFAQDto entityRoleDto = _idbConnection.Update<EntityFAQDto>(_idbTransaction, DbTables.tblFAQ,
                    new Dictionary<string, object> {
                    { nameof(EntityFAQDto.UpdatedAt), DateTime.Now },
                    { nameof(EntityFAQDto.Question), reqDto.Question},
                    { nameof(EntityFAQDto.Answer), reqDto.Answer},
                    }, $@" {nameof(EntityFAQDto.Id)}='{Id}' ", "RETURNING *");
                    #endregion 

                    apiResponse = new APIsSuccsss<EntityFAQDto>("FAQ Updated Successfully");
                }
                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityFAQDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<EntityFAQDto>> ChangeFAQStatus(long Id, bool status)
        {
            try
            {
                APIsResponse<EntityFAQDto> apiResponse = default(APIsResponse<EntityFAQDto>);
                EntityFAQDto entityRoleDto = _idbConnection.Update<EntityFAQDto>(_idbTransaction, DbTables.tblFAQ,
                   new Dictionary<string, object> {
                    { nameof(EntityFAQDto.IsActive), status},
                   }, $@" {nameof(EntityFAQDto.Id)}='{Id}' ", "RETURNING *");

                apiResponse = new APIsSuccsss<EntityFAQDto>("FAQ Status Updated Successfully", entityRoleDto);
                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityFAQDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<bool>> DeleteFAQ(long Id)
        {
            try
            {
                APIsResponse<bool> apiResponse = default(APIsResponse<bool>);
                #region Delete FAQ
                string deleteQuery = $@"DELETE FROM {DbTables.tblFAQ} WHERE Id = @FAQId;";
                int rowsAffected = await _idbConnection.ExecuteAsync(deleteQuery, new { FAQId = Id }, _idbTransaction);

                if (rowsAffected > 0)
                    apiResponse = new APIsSuccsss<bool>("FAQ deleted successfully.", true);
                else
                    apiResponse = new APIsSuccsss<bool>("FAQ not found.", true);
                #endregion

                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<Datatable<FAQPagingResponse>>> GetAllFAQ(FAQPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<FAQPagingResponse>> apiResponse = default;
                string orderBy;

                orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All FAQ
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<CMSPagingResponse>(
                               sql: "GET_STATIC_PAGES_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<CMSPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                if (result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<FAQPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<FAQPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<FAQPagingResponse>>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<EntityFAQDto>> GetFAQ(long Id)
        {
            try
            {
                var sql = DbTables.tblFAQ.SelectAll($@"{nameof(EntityFAQDto.Id)}= {Id}");
                var result = await _idbConnection.QueryFirstOrDefaultAsync<EntityFAQDto>(
                               sql,
                               transaction: _idbTransaction
                           );


                if (result == null)
                    return new APIsError<EntityFAQDto>(_validateMessages.NotExist);
                else
                    return new APIsSuccsss<EntityFAQDto>(_validateMessages.RetriveSuccess, result);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityFAQDto>(ex.GetActualError()));
            }
        }

    }
}
