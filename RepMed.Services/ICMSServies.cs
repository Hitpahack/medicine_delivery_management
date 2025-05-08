using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
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

    }
}
