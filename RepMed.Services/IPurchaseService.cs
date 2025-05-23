using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IPurchaseService : IDisposable
    {
        Task<APIsResponse<AddPurchaseInvoiceDto>> AddPurchaseInvoice(AddPurchaseInvoiceDto reqDto);
        Task<APIsResponse<List<FetchPODto>>> FetchPO(string FetchPO, long pharmacyId);

    }
    public class PurchaseService : BaseService, IPurchaseService
    {
        public PurchaseService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }
        public async Task<APIsResponse<AddPurchaseInvoiceDto>> AddPurchaseInvoice(AddPurchaseInvoiceDto reqDto)
        {
            try
            {
                #region Check Pharmacy and Supplier Exist
                if (await IsSupplierExist(reqDto.Invoice.SupplierId))
                {
                    return new APIsError<AddPurchaseInvoiceDto>("Supplier not found");
                }
                if (await IsPharmacyExist(reqDto.Invoice.PharmacyId))
                {
                    return new APIsError<AddPurchaseInvoiceDto>("Pharmacy not found");
                }
                #endregion
                #region Check Calculation
                //var items = reqDto.Items
                // .Select(i => new
                // {
                //     i.ProductId,
                //     i.QuantityPurchased,
                //     i.ProductPrice,
                //     i.Unit,
                //     TotalPrice = i.QuantityPurchased * i.ProductPrice,
                // }).ToList();

                decimal totalDiscount = reqDto.Items.Sum(x => x.Discount ?? 0);
                decimal subTotal = reqDto.Items.Sum(x => x.TotalAmount);
                decimal totalGST = reqDto.Items.Sum(x => x.Gstamount ?? 0);

                if (reqDto.Invoice.TotalAmount != subTotal || reqDto.Invoice.TotalDiscount != totalDiscount || reqDto.Invoice.TaxAmount != totalGST)
                    return new APIsError<AddPurchaseInvoiceDto>("Invalid calucalation detected");


                #endregion
                #region Insert Invoice
                reqDto.Invoice.CreatedAt = DateTime.Now;
                var purchaseInvoice = _idbConnection.Insert<EntityPurchaseInvoiceDto>(_idbTransaction,
                                   DbTables.tblPurchaseInvoice,
                                   DapperHelper.QueryAsColumnsParma<Purchaseinvoice, BasePurchaseInvoiceDto>(),
                                   DapperHelper.QueryAsValuesParma<Purchaseinvoice, BasePurchaseInvoiceDto>(),
                                   reqDto.Invoice);
                #endregion

                #region Insert Purchase Invoice Items and Stock ledger
                foreach (var item in reqDto.Items)
                {
                    item.PurchaseInvoiceId = purchaseInvoice.Id;
                    if (item.Gstincluded == true)
                    {
                        var gst = (item.ProductPrice * item.Gstpercentage) / (100 + item.Gstpercentage);
                        if (gst != item.Gstamount)
                            return new APIsError<AddPurchaseInvoiceDto>("Invalid calucalation detected");
                        if (item.TotalAmount != (item.ProductPrice - item.Discount))
                            return new APIsError<AddPurchaseInvoiceDto>("Invalid calucalation detected");
                    }
                    else
                    {
                        var gst = (item.ProductPrice * item.Gstpercentage) / 100;
                        if (gst != item.Gstamount)
                            return new APIsError<AddPurchaseInvoiceDto>("Invalid calucalation detected");
                        if (item.TotalAmount != (item.ProductPrice + gst - item.Discount))
                            return new APIsError<AddPurchaseInvoiceDto>("Invalid calucalation detected");
                    }

                    item.CreatedAt = DateTime.Now;
                    var purchaseItems = _idbConnection.Insert<EntityPurchaseInvoiceItemDto>(_idbTransaction,
                                   DbTables.tblPurchaseInvoiceItems,
                                   DapperHelper.QueryAsColumnsParma<Purchaseinvoiceitem, BasePurchaseInvoiceItemDto>(),
                                   DapperHelper.QueryAsValuesParma<Purchaseinvoiceitem, BasePurchaseInvoiceItemDto>(),
                                   reqDto.Invoice);

                    #region Update Inventory
                    string query = DbTables.tblPharmacyInventory.SelectAll();
                    var getInventory = _idbConnection.QueryFirst<EntityPharmacyInventoryDto>(query, _idbTransaction);
                    if (getInventory != null && item.ExpiryDate == getInventory.ExpiryDate)
                    {
                        var updateInventory = _idbConnection.Update<EntityPharmacyInventoryDto>(_idbTransaction, DbTables.tblPharmacyInventory,
                            new Dictionary<string, object>
                            {
                               {nameof(BasePharmacyInventoryDto.ExpiryDate), item.ExpiryDate },
                               {nameof(BasePharmacyInventoryDto.CurrentStock), getInventory.CurrentStock + item.QuantityPurchased},
                               {nameof(BasePharmacyInventoryDto.SellingPrice), item.SellingPrice },
                               {nameof(BasePharmacyInventoryDto.LastRestocked), DateTime.Now},
                               {nameof(BasePharmacyInventoryDto.LastUpdated), DateTime.Now}
                            }, $@"Id='{getInventory.Id}'");
                    }
                    else
                    {
                        var insertInventory = _idbConnection.Insert<EntityPharmacyInventoryDto>(_idbTransaction, DbTables.tblPharmacyInventory,
                         new Dictionary<string, object>
                         {
                               {nameof(BasePharmacyInventoryDto.PharmacyId), reqDto.Invoice.PharmacyId },
                               {nameof(BasePharmacyInventoryDto.ProductId), item.ProductId},
                               {nameof(BasePharmacyInventoryDto.ExpiryDate), item.ExpiryDate},
                               {nameof(BasePharmacyInventoryDto.CurrentStock), item.QuantityPurchased},
                               {nameof(BasePharmacyInventoryDto.MinimumStockThreshold), 10},
                               {nameof(BasePharmacyInventoryDto.SellingPrice), item.SellingPrice},
                               {nameof(BasePharmacyInventoryDto.LastRestocked), DateTime.Now},
                               {nameof(BasePharmacyInventoryDto.LastUpdated), DateTime.Now}
                         });
                    }
                    #endregion
                    #region Stock Ledger(Stock inward from which invoice)
                    var insertStockLedger = _idbConnection.Insert<EntityStockLedgerDto>(_idbTransaction, DbTables.tblPharmacyStockLedger,
                         new Dictionary<string, object>
                         {
                               {nameof(EntityStockLedgerDto.PharmacyId), reqDto.Invoice.PharmacyId },
                               {nameof(EntityStockLedgerDto.ProductId), item.ProductId},
                               {nameof(EntityStockLedgerDto.PurchaseInvoiceId), purchaseInvoice.Id},
                               {nameof(EntityStockLedgerDto.QuantityAdded), item.QuantityPurchased},
                               {nameof(EntityStockLedgerDto.SellingPriceAtTime), item.SellingPrice},
                               {nameof(EntityStockLedgerDto.CreatedAt), DateTime.Now}
                         });
                    #endregion
                }
                #endregion
                return new APIsSuccsss<AddPurchaseInvoiceDto>("Supplier Added Successfully");

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<AddPurchaseInvoiceDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<List<FetchPODto>>> FetchPO(string poNumber, long pharmacyId)
        {
            try
            {

                string query = $@"
                                    SELECT 
                                        poi.Id AS ItemId,
                                        poi.ProductId,
                                        p.Name AS ProductName,
                                        poi.Quantity,
                                        p.MRP
                                    FROM {DbTables.tblPurchaseOrders} po 
                                    LEFT JOIN {DbTables.tblPurchaseOrderItems} poi ON poi.PurchaseOrderId = po.Id
                                    LEFT JOIN {DbTables.tblProduct} p ON poi.ProductId = p.Id
                                    WHERE po.PONumber = @PON";

                var result = (await _idbConnection.QueryAsync<FetchPODto>(query, new { PON = poNumber },_idbTransaction)).ToList();
                if (result.Any())
                    return new APIsSuccsss<List<FetchPODto>> ("PO fetched successfully", result);
                else
                    return new APIsSuccsss<List<FetchPODto>> ("No data found");

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<FetchPODto>>(ex.GetActualError()));
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
