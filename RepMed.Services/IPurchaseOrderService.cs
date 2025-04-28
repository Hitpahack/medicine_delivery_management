using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.ProductPage;
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
        Task<APIsResponse<BaseSupplierDto>> AddSupplier(BaseSupplierDto reqDto);

    }
    public class PurchaseOrderService : BaseService, IPurchaseOrderService
    {
        public PurchaseOrderService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<BaseSupplierDto>> AddSupplier(BaseSupplierDto reqDto)
        {
            try
            {
                #region Add Supplier
                var supplier = _idbConnection.Insert<BaseSupplierDto>(_idbTransaction,
                                   DbTables.tblPurchaseOrders,
                                   DapperHelper.QueryAsColumnsParma<Supplier, BaseSupplierDto>(),
                                   DapperHelper.QueryAsValuesParma<Supplier, BaseSupplierDto>(),
                                   reqDto);
                #endregion
                if(supplier !=null)
                    return new APIsSuccsss<BaseSupplierDto>(_validateMessages.AddSuccess, supplier);
                else
                    return new APIsError<BaseSupplierDto>("Error inserting the supplier");

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<BaseSupplierDto>(ex.GetActualError()));
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
                reqDto.PO.TotalAmount = subTotal;
                reqDto.PO.CreatedAt = DateTime.Now;
                reqDto.PO.OrderDate = DateTime.Now;
                reqDto.PO.Status ="Pending";
                #region Insert Purchase Orders
                var insertPo = _idbConnection.Insert<BasePODto>(_idbTransaction,
                                   DbTables.tblPurchaseOrders,
                                   DapperHelper.QueryAsColumnsParma<Purchaseorder, BasePODto>(),
                                   DapperHelper.QueryAsValuesParma<Purchaseorder, BasePODto>(),
                                   reqDto);
                #endregion

                if (insertPo != null)
                {
                    List<EntityPOItemDto> list = new List<EntityPOItemDto>(); ;
                    foreach (var item in reqDto.Items)
                    {
                        item.TotalPrice = item.Quantity * item.UnitPrice;
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

    }
}
