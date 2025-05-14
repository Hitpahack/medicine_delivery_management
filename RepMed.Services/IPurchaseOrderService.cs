using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.POPage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using RepMed.Dtos.ShortBookPage;

namespace RepMed.Services
{
    public interface IPurchaseOrderService : IDisposable
    {
        Task<APIsResponse<CreatePODto>> CreatePO(CreatePODto reqDto);
        Task<APIsResponse<EntityShortbookDto>> AddEditItem(BaseShortbookDto reqDto, long Id);
        Task<APIsResponse<EntitySupplierDto>> AddSupplier(BaseSupplierDto reqDto);
        Task<APIsResponse<Datatable<POPagingResponse>>> GetAllPO(POPagingRequest reqDto);
        Task<APIsResponse<Datatable<ShortbookPagingResponse>>> GetShortBookItems(ShortbookPagingRequest reqDto);
        Task<APIsResponse<NextPoNumberDto>> GetNextPONumber(long pharmacyId);
        Task<APIsResponse<List<GetSupppliersDto>>> GetAllSuppliers(long pharmacyId);
        Task<APIsResponse<POPdfContentDto>> GetPOPdfDetails(long poId);
        Task<APIsResponse<List<GetPOItemsDto>>> GetPOItems(long poId);
        Task<APIsResponse<IEnumerable<EntityProductDto>>> SearchProducts(string search);
        Task<APIsResponse<byte[]>> Generate(POPdfContentDto po, List<GetPOItemsDto> items);
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
                if (supplier != null)
                    return new APIsSuccsss<EntitySupplierDto>("Supplier Added Successfully", supplier);
                else
                    return new APIsError<EntitySupplierDto>("Error inserting the supplier");

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntitySupplierDto>(ex.GetActualError()));
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
                if (await IsPharmacyExist(pharmacyId))
                {
                    return new APIsError<NextPoNumberDto>("Pharmacy not found");
                }

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
                if (result != null)
                    return await Task.FromResult(new APIsSuccsss<NextPoNumberDto>(_validateMessages.RetriveSuccess, result));
                else
                    return await Task.FromResult(new APIsSuccsss<NextPoNumberDto>(_validateMessages.InternalError));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<NextPoNumberDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<List<GetPOItemsDto>>> GetPOItems(long poId)
        {
            try
            {
                string query = $@"
                                SELECT 
                                    poi.PurchaseOrderId,
                                    poi.ProductId,
                                    p.Name AS ProductName,
                                    poi.Quantity,
                                    poi.UnitPrice,
                                    poi.TotalPrice,
                                    poi.Unit
                                FROM {DbTables.tblPurchaseOrderItems} poi
                                LEFT JOIN {DbTables.tblProduct} p ON poi.ProductId = p.Id";

                var poItems = await _idbConnection.QueryAsync<GetPOItemsDto>(query, transaction: _idbTransaction);
                return new APIsSuccsss<List<GetPOItemsDto>>(_validateMessages.RetriveSuccess, poItems);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<GetPOItemsDto>>(ex.GetActualError()));

            }
        }

        public async Task<APIsResponse<POPdfContentDto>> GetPOPdfDetails(long poId)
        {
            try
            {
                APIsResponse<POPdfContentDto> apiResponse = default;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("poId", poId, DbType.Int32);
                var result = await _idbConnection.QueryFirstOrDefaultAsync<POPdfContentDto>(
                               sql: "GET_PO_DETAILS",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                );
                #endregion
                if (result != null)
                    return await Task.FromResult(new APIsSuccsss<POPdfContentDto>(_validateMessages.RetriveSuccess, result));
                else
                    return await Task.FromResult(new APIsSuccsss<POPdfContentDto>(_validateMessages.InternalError));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<POPdfContentDto>(ex.GetActualError()));
            }
        }
        public Task<APIsResponse<byte[]>> Generate(POPdfContentDto po, List<GetPOItemsDto> items)
        {
            try
            {

                var pdfBytes = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(20);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header().Text("Purchase Order").FontSize(20).Bold().AlignCenter();

                        page.Content().Column(col =>
                        {
                            col.Spacing(10);

                            // Pharmacy and PO Details
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"Pharmacy: {po.PharmacyName}");
                                    c.Item().Text($"Address Line 1: {po.PharmacyAddress1}");
                                    c.Item().Text($"Address Line 2: {po.PharmacyAddress2}");
                                    c.Item().Text($"Mobile: {po.PharmacyMobile}");
                                    c.Item().Text($"Email: {po.PharmacyEmail}");
                                    c.Item().Text($"GST: {po.PharmacyGST}");
                                    c.Item().Text($"City: {po.PharmacyCity}, State: {po.PharmacyState}");
                                });

                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"PO Number: {po.PONumber}");
                                    c.Item().Text($"Order Date: {po.OrderDate:yyyy-MM-dd}");
                                    c.Item().Text($"Expected Date: {po.EDDate:yyyy-MM-dd}");
                                    c.Item().Text($"Status: {po.Status}");
                                    c.Item().Text($"Remarks: {po.Remarks}");
                                });
                            });

                            col.Item().LineHorizontal(1);

                            // Supplier Details
                            col.Item().Text("Supplier Information").FontSize(14).Bold();
                            col.Item().Text($"Name: {po.SupplierName}");
                            col.Item().Text($"Address: {po.SupplierAddress}");
                            col.Item().Text($"Mobile: {po.SupplierMobile}");
                            col.Item().Text($"Email: {po.SupplierEmail}");
                            col.Item().Text($"GST: {po.SupplierGST}");

                            col.Item().LineHorizontal(1);

                            // Purchase Items Table
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(); // Product Name
                                    columns.ConstantColumn(50); // Quantity
                                    columns.ConstantColumn(70); // Unit Price
                                    columns.ConstantColumn(70); // Total Price
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Product Name").Bold();
                                    header.Cell().Text("Qty").Bold();
                                    header.Cell().Text("Unit Price").Bold();
                                    header.Cell().Text("Total").Bold();
                                });

                                foreach (var item in items)
                                {
                                    table.Cell().Text(item.ProductName);
                                    table.Cell().Text(item.Quantity.ToString());
                                    table.Cell().Text($"₹{item.UnitPrice:F2}");
                                    table.Cell().Text($"₹{item.TotalPrice:F2}");
                                }
                            });

                            col.Item().AlignRight().Text($"Total: ₹{po.TotalAmount:F2}").FontSize(14).Bold();
                            col.Item().AlignRight().Text($"Tax: ₹{po.TaxAmount:F2}").FontSize(12);
                        });

                        page.Footer().AlignCenter().Text("Generated by RepMed System");
                    });
                }).GeneratePdf(); // Synchronous method

                return Task.FromResult<APIsResponse<byte[]>>(new APIsSuccsss<byte[]>("PDF generated successfully", pdfBytes));
            }
            catch (Exception ex)
            {
                return Task.FromResult<APIsResponse<byte[]>>(new APIsSuccsss<byte[]>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<CreatePODto>> CreatePO(CreatePODto reqDto)
        {
            try
            {
                APIsResponse<CreatePODto> apiResponse = default;
                #region Get ShortBook
                var sbQuery = DbTables.tblShortBook.SelectAll($@"Id IN @ShortbookIds");
                var shortBook = await _idbConnection.QueryAsync<EntityShortbookDto>(
                    sbQuery,
                    new { ShortbookIds = reqDto.ShortbookId },
                    _idbTransaction
                );
                var groupedBySupplier = shortBook.GroupBy(o=> o.SupplierId);
                #endregion
                foreach (var group in groupedBySupplier)
                {
                    #region Insert Purchase Orders
                    reqDto.CreatedAt = DateTime.Now;
                    reqDto.UpdatedAt = DateTime.Now;
                    reqDto.OrderDate = DateTime.Now;
                    reqDto.Ponumber = GetNextPONumber(reqDto.PharmacyId).Result.Data.ToString();
                    reqDto.OrderDate = DateTime.Now;
                    reqDto.Status = "Pending";
                    reqDto.SupplierId = Convert.ToUInt32(group.Key);
                    var insertPo = _idbConnection.Insert<EntityPODto>(_idbTransaction,
                                       DbTables.tblPurchaseOrders,
                                       DapperHelper.QueryAsColumnsParma<Purchaseorder, BasePODto>(),
                                       DapperHelper.QueryAsValuesParma<Purchaseorder, BasePODto>(),
                                       group);
                    #endregion
                    #region Insert PO Items
                    foreach (var item in group)
                    {
                        var poItemDetail = new BasePOItemDto
                        {
                            PurchaseOrderId = insertPo.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,  
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };

                        var insertPOItems = _idbConnection.Insert<EntityPOItemDto>(_idbTransaction,
                                       DbTables.tblPurchaseOrderItems,
                                       DapperHelper.QueryAsColumnsParma<Purchaseorderitem, BasePOItemDto>(),
                                       DapperHelper.QueryAsValuesParma<Purchaseorderitem, BasePOItemDto>(),
                                       poItemDetail);
                    }
                    #endregion
                }
                #region Delete Items from ShortBook
                var deleteQuery = $@"DELETE FROM {DbTables.tblShortBook} WHERE Id in @ShortbookIds";
                int rowsDeleted = await _idbConnection.ExecuteAsync(deleteQuery, new { ShortbookIds = reqDto.ShortbookId }, _idbTransaction);
                if(rowsDeleted >0)
                    return new APIsSuccsss<CreatePODto>(_validateMessages.AddSuccess, reqDto);
                #endregion


                    return null;
                //if (insertPo != null)
                //{
                //    List<EntityPOItemDto> list = new List<EntityPOItemDto>(); ;
                //    foreach (var item in reqDto.Items)
                //    {
                //        item.PurchaseOrderId = insertPo.Id;
                //        item.TotalPrice = item.Quantity * item.UnitPrice;
                //        item.CreatedAt = DateTime.Now;
                //        var insertItem = _idbConnection.Insert<EntityPOItemDto>(_idbTransaction,
                //                  DbTables.tblPurchaseOrderItems,
                //                  DapperHelper.QueryAsColumnsParma<Purchaseorderitem, BasePOItemDto>(),
                //                  DapperHelper.QueryAsValuesParma<Purchaseorderitem, BasePOItemDto>(),
                //                  item);
                //        list.Add(insertItem);
                //    }
                //    return new APIsSuccsss<CreatePODto>(_validateMessages.AddSuccess, reqDto);
                //}
                //else
                //{
                //    return new APIsError<CreatePODto>("Error inserting the purchase order");
                //}

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<CreatePODto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<EntityShortbookDto>> AddEditItem(BaseShortbookDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityShortbookDto> apiResponse = default;
                reqDto.AddedDate = DateTime.Now;
                if (Id == 0)
                {
                    #region Insert ShortBook Item
                    var insertShortbook = _idbConnection.Insert<EntityShortbookDto>(_idbTransaction,
                                       DbTables.tblShortBook,
                                       DapperHelper.QueryAsColumnsParma<Shortbook, BaseShortbookDto>(),
                                       DapperHelper.QueryAsValuesParma<Shortbook, BaseShortbookDto>(),
                                       reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<EntityShortbookDto>("Item added to shortbook", insertShortbook);
                }
                else
                {
                    #region Update ShortBook Item
                    EntityShortbookDto entityRoleDto = _idbConnection.Update<EntityShortbookDto>(_idbTransaction, DbTables.tblShortBook,
                    new Dictionary<string, object> {
                    { nameof(EntityShortbookDto.SupplierId), reqDto.SupplierId},
                    { nameof(EntityShortbookDto.AddedDate), reqDto.AddedDate},
                    { nameof(EntityShortbookDto.Priority), reqDto.Priority},
                    { nameof(EntityShortbookDto.Quantity), reqDto.Quantity},
                    { nameof(EntityShortbookDto.Status), reqDto.Status}
                    }, $@" {nameof(EntityShortbookDto.Id)}='{Id}' ", "RETURNING *");
                    #endregion 

                    apiResponse = new APIsSuccsss<EntityShortbookDto>("Shortbook item Updated Successfully");
                }
                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityShortbookDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<IEnumerable<EntityProductDto>>> SearchProducts(string search)
        {
            try
            {
                var query = DbTables.tblProduct.SelectAll("Name LIKE @SearchTerm ORDER BY Name ASC LIMIT 20;");
                var result = await _idbConnection.QueryAsync<EntityProductDto>(query, new { SearchTerm = $"%{search}%" },_idbTransaction);
                return new APIsSuccsss<IEnumerable<EntityProductDto>>("Item added to shortbook", result);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<IEnumerable<EntityProductDto>>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<Datatable<ShortbookPagingResponse>>> GetShortBookItems(ShortbookPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<ShortbookPagingResponse>> apiResponse = default;
                string orderBy;
                if (reqDto.Order[0].Column == 0)
                    orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|desc";
                else
                    orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All PO 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("pharmacyId", reqDto.PharmacyId, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("orderBy", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<ShortbookPagingResponse>(
                               sql: "GET_SHORTBOOK_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<ShortbookPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                if (result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<ShortbookPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<ShortbookPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<ShortbookPagingResponse>>(ex.GetActualError()));
            }
        }
    }
}
