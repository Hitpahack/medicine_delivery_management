using RepMed.Dtos.DataTables;
using RepMed.Dtos.UsersPage;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepMed.Dtos.ProductPage;
using Dapper;
using RepMed.Core;

namespace RepMed.Services
{
    public interface IProductService : IDisposable
    {
        Task<APIsResponse<Datatable<ProductsPagingResponse>>> GetProducts(ProductsPagingRequest reqDto);
        Task<APIsResponse<DashboardDto>> GetCount();
    }

    public class ProductService : BaseService, IProductService
    {
        public ProductService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<DashboardDto>> GetCount()
        {
            try
            {
                APIsResponse<Datatable<DashboardDto>> apiResponse = default;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                var result = (await _idbConnection.QueryAsync<DashboardDto>(
                               sql: "GET_COUNT",
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).FirstOrDefault();
                #endregion
                
                if (result!=null)
                    return await Task.FromResult(new APIsSuccsss<DashboardDto>(_validateMessages.RetriveSuccess, result));
                else
                    return await Task.FromResult(new APIsSuccsss<DashboardDto>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<DashboardDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<Datatable<ProductsPagingResponse>>> GetProducts(ProductsPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<ProductsPagingResponse>> apiResponse = default;
                string orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<ProductsPagingResponse>(
                               sql: "GET_PRODUCTS_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<ProductsPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                if (result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<ProductsPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<ProductsPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<ProductsPagingResponse>>(ex.GetActualError()));
            }
        }
    }
}
