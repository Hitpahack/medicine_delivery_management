using Dapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.PharmacyPage;
using RepMed.Dtos.POPage;
using RepMed.Dtos.ProductPage;
using RepMed.Dtos.RolePage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IPurchaseOrderService : IDisposable
    {
        Task<APIsResponse<CreatePODto>> CreatePO(CreatePODto reqDto);
        Task<APIsResponse<EntitySupplierDto>> AddSupplier(BaseSupplierDto reqDto);
        Task<APIsResponse<Datatable<POPagingResponse>>> GetAllPO(POPagingRequest reqDto);
        Task<APIsResponse<NextPoNumberDto>> GetNextPONumber(long pharmacyId);
        Task<APIsResponse<List<GetSupppliersDto>>> GetAllSuppliers(long pharmacyId);

    }
    public class PurchaseOrderService : BaseService, IPurchaseOrderService
    {
        public PurchaseOrderService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntitySupplierDto>> AddSupplier(BaseSupplierDto reqDto)
        {
            try
            {
                reqDto.Status = "Active";
                reqDto.CreatedAt = DateTime.Now;
                #region Add Supplier
                var supplier = _idbConnection.Insert<EntitySupplierDto>(_idbTransaction,
                                   DbTables.tblSuppliers,
                                   DapperHelper.QueryAsColumnsParma<Supplier, BaseSupplierDto>(),
                                   DapperHelper.QueryAsValuesParma<Supplier, BaseSupplierDto>(),
                                   reqDto);
                #endregion
                if(supplier !=null)
                    return new APIsSuccsss<EntitySupplierDto>("Supplier Added Successfully", supplier);
                else
                    return new APIsError<EntitySupplierDto>("Error inserting the supplier");

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntitySupplierDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<CreatePODto>> CreatePO(CreatePODto reqDto)
        {
            try
            {
                APIsResponse<Datatable<CreatePODto>> apiResponse = default;
                var items = reqDto.Items
                   .Select(i => new
                   {
                       i.ProductId,
                       i.Quantity,
                       i.UnitPrice,
                       i.Unit,
                       TotalPrice = i.Quantity * i.UnitPrice
                   }).ToList();
                decimal subTotal = items.Sum(x => x.TotalPrice);
                if(reqDto.PO.TotalAmount == subTotal)
                    reqDto.PO.TotalAmount = subTotal;
                else
                    return new APIsError<CreatePODto>("invalid calucalation detected");
                reqDto.PO.CreatedAt = DateTime.Now;
                reqDto.PO.OrderDate = DateTime.Now;
                reqDto.PO.Status ="Pending";
                #region Insert Purchase Orders
                var insertPo = _idbConnection.Insert<EntityPODto>(_idbTransaction,
                                   DbTables.tblPurchaseOrders,
                                   DapperHelper.QueryAsColumnsParma<Purchaseorder, BasePODto>(),
                                   DapperHelper.QueryAsValuesParma<Purchaseorder, BasePODto>(),
                                   reqDto.PO);
                #endregion

                if (insertPo != null)
                {
                    List<EntityPOItemDto> list = new List<EntityPOItemDto>(); ;
                    foreach (var item in reqDto.Items)
                    {
                        item.PurchaseOrderId = insertPo.Id;
                        item.TotalPrice = item.Quantity * item.UnitPrice;
                        item.CreatedAt = DateTime.Now;
                        var insertItem = _idbConnection.Insert<EntityPOItemDto>(_idbTransaction,
                                  DbTables.tblPurchaseOrderItems,
                                  DapperHelper.QueryAsColumnsParma<Purchaseorderitem, BasePOItemDto>(),
                                  DapperHelper.QueryAsValuesParma<Purchaseorderitem, BasePOItemDto>(),
                                  item);
                        list.Add(insertItem);
                    }
                    return new APIsSuccsss<CreatePODto>(_validateMessages.AddSuccess, reqDto);
                }
                else
                {
                    return new APIsError<CreatePODto>("Error inserting the purchase order");
                }

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<CreatePODto>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<Datatable<POPagingResponse>>> GetAllPO(POPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<POPagingResponse>> apiResponse = default;
                string orderBy;
                orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All PO 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("pharmacyId", reqDto.PharmacyId, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<POPagingResponse>(
                               sql: "GET_PO_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<POPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                if (result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<POPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<POPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<POPagingResponse>>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<List<GetSupppliersDto>>> GetAllSuppliers(long pharmacyId)
        {
            try
            {
                string query = DbTables.tblSuppliers.SelectAll($@"`{nameof(Supplier.PharmacyId)}` = {pharmacyId}");
                var suppliers = await _idbConnection.QueryAsync<GetSupppliersDto>(query, transaction: _idbTransaction);
                return new APIsSuccsss<List<GetSupppliersDto>>(_validateMessages.RetriveSuccess, suppliers);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<GetSupppliersDto>>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<NextPoNumberDto>> GetNextPONumber(long pharmacyId)
        {
            try
            {
                APIsResponse<NextPoNumberDto> apiResponse = default;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("inPharmacyId", pharmacyId, DbType.Int32);
                var result = await _idbConnection.QueryFirstOrDefaultAsync<NextPoNumberDto>(
                               sql: "GET_NEXT_PO_NUMBER",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                );
                #endregion
                if (result!=null)
                    return await Task.FromResult(new APIsSuccsss<NextPoNumberDto>(_validateMessages.RetriveSuccess,result));
                else
                    return await Task.FromResult(new APIsSuccsss<NextPoNumberDto>(_validateMessages.InternalError));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<NextPoNumberDto>(ex.GetActualError()));
            }
        }
    }
}
