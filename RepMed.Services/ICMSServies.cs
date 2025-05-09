using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.CMSPage;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.RolePage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface ICMSServies : IDisposable
    {
        Task<APIsResponse<EntityStaticPageDto>> AddEditStaticPage(BaseStaticPageDto reqDto, long Id);
        Task<APIsResponse<EntityStaticPageDto>> ChangePageStatus(long Id, bool status);
        Task<APIsResponse<bool>> DeleteStaticPage(long Id);
        Task<APIsResponse<Datatable<CMSPagingResponse>>> GetAllStaticPage(CMSPagingRequest reqDto);
        Task<APIsResponse<EntityStaticPageDto>> GetStaticPage(long Id);
    }
    public class CMSServies:BaseService, ICMSServies
    {
        public CMSServies(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public async Task<APIsResponse<EntityStaticPageDto>> AddEditStaticPage(BaseStaticPageDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityStaticPageDto> apiResponse = default(APIsResponse<EntityStaticPageDto>);
                if (Id == 0)
                {
                    #region Add Static Page
                    reqDto.CreatedDate = DateTime.Now;
                    reqDto.UpdatedDate = DateTime.Now;
                    reqDto.IsActive = true;
                    EntityStaticPageDto response = _idbConnection.Insert<EntityStaticPageDto>(_idbTransaction,
                        DbTables.tblStaticPages,
                        DapperHelper.QueryAsColumnsParma<Staticpage, BaseStaticPageDto>(),
                        DapperHelper.QueryAsValuesParma<Staticpage, BaseStaticPageDto>(),
                        reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<EntityStaticPageDto>("Page Created Successfully", response);
                }
                else
                {
                    #region Update Static Page
                    EntityStaticPageDto entityRoleDto = _idbConnection.Update<EntityStaticPageDto>(_idbTransaction, DbTables.tblStaticPages,
                    new Dictionary<string, object> {
                    { nameof(EntityStaticPageDto.UpdatedDate), DateTime.Now },
                    { nameof(EntityStaticPageDto.Title), reqDto.Title},
                    { nameof(EntityStaticPageDto.Slug), reqDto.Slug},
                    { nameof(EntityStaticPageDto.Content), reqDto.Content},
                    }, $@" {nameof(EntityStaticPageDto.Id)}='{Id}' ", "RETURNING *");
                    #endregion 

                    apiResponse = new APIsSuccsss<EntityStaticPageDto>("Page Updated Successfully");
                }
                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityStaticPageDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<EntityStaticPageDto>> ChangePageStatus(long Id, bool status)
        {
            try
            {
                APIsResponse<EntityStaticPageDto> apiResponse = default(APIsResponse<EntityStaticPageDto>);
                EntityStaticPageDto entityRoleDto = _idbConnection.Update<EntityStaticPageDto>(_idbTransaction, DbTables.tblStaticPages,
                   new Dictionary<string, object> {
                    { nameof(EntityStaticPageDto.IsActive), status},
                   }, $@" {nameof(EntityStaticPageDto.Id)}='{Id}' ", "RETURNING *");

                apiResponse = new APIsSuccsss<EntityStaticPageDto>("Page Status Updated Successfully", entityRoleDto);
                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityStaticPageDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<bool>> DeleteStaticPage(long Id)
        {
            try
            {
                APIsResponse<bool> apiResponse = default(APIsResponse<bool>);
                #region Delete the role
                string deleteQuery = $@"DELETE FROM {DbTables.tblStaticPages} WHERE Id = @PageId;";
                int rowsAffected = await _idbConnection.ExecuteAsync(deleteQuery, new { PageId = Id }, _idbTransaction);

                if (rowsAffected > 0)
                    apiResponse = new APIsSuccsss<bool>("Page deleted successfully.", true);
                else
                    apiResponse = new APIsSuccsss<bool>("Page not found.", true);
                #endregion

                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<Datatable<CMSPagingResponse>>> GetAllStaticPage(CMSPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<CMSPagingResponse>> apiResponse = default;
                string orderBy;

                orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All Pharmacy 
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
                    return await Task.FromResult(new APIsSuccsss<Datatable<CMSPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<CMSPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<CMSPagingResponse>>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<EntityStaticPageDto>> GetStaticPage(long Id)
        {
            try
            {
                var sql = DbTables.tblStaticPages.SelectAll($@"{nameof(EntityStaticPageDto.Id)}= {Id}");
                var result = await _idbConnection.QueryFirstOrDefaultAsync<EntityStaticPageDto>(
                               sql,
                               transaction: _idbTransaction
                           );

               
                if (result == null)
                    return new APIsError<EntityStaticPageDto>(_validateMessages.NotExist);
                else
                    return new APIsSuccsss<EntityStaticPageDto>(_validateMessages.RetriveSuccess, result);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityStaticPageDto>(ex.GetActualError()));
            }
        }
    }
}
