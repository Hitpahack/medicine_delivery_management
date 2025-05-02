using RepMed.Dtos.DataTables;
using RepMed.Dtos.UsersPage;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RepMed.Core;

namespace RepMed.Services
{
    public interface IDoctorService : IDisposable
    {
        Task<APIsResponse<Datatable<UsersPagingResponse>>> GetDoctors(UsersPagingRequest reqDto);
    }
    public class DoctorService :BaseService, IDoctorService
    {
        public DoctorService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<Datatable<UsersPagingResponse>>> GetDoctors(UsersPagingRequest reqDto)
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
                               sql: "GET_DOCTOR_PAGED",
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
    }
}
