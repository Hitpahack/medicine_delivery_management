import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../../../../app/admin/services/admin.baseservice";
import { GetSupppliersDto } from "../../../viewmodels/purchaseorder/supplier.dto"
import { SupplierDto } from "../../../viewmodels/supplier/supplier.dto";
import { Product } from "../../../viewmodels/purchaseorder/product.dto";
import { ProductDto } from "../../../viewmodels/purchaseorder/productdetails.dto";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class PurchaseOrderService extends adminBaseService {


  constructor(public http: HttpClient) {
    super(http);

  }

  addpurchaseorder(formData: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.purchaseorder.add, formData, { headers: this.apiConfig.requestSettings.header })
  }

  getsupplier(id: number) {
    return this.http.post<ApiResponse<GetSupppliersDto[]>>(this.apiConfig.endpoints.purchaseorder.getsuppliers(id), { headers: this.apiConfig.requestSettings.header })
  }
  getPoNumber(pharmacyId: number) {
    return this.http.post<ApiResponse<{ nextPONumber: string }>>(this.apiConfig.endpoints.purchaseorder.getPoNumber(pharmacyId), { headers: this.apiConfig.requestSettings.header });
  }
  addSupplier(formData: any) {
    return this.http.post<ApiResponse<SupplierDto>>(this.apiConfig.endpoints.purchaseorder.addSupplier, formData, { headers: this.apiConfig.requestSettings.header })
  }
  getproductlist() {
    return this.http.get<ApiResponse<Product[]>>(this.apiConfig.endpoints.purchaseorder.getproduct, { headers: this.apiConfig.requestSettings.header })
  }

  getFilteredProducts(searchText: string): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.apiConfig.endpoints.purchaseorder.searchProducts}`, {
      params: { search: searchText }
    });
  }

  getFilteredSupplier(pharmacyId: number, searchText: string): Observable<ProductDto[]> {
    return this.http.post<ProductDto[]>(
      `${this.apiConfig.endpoints.purchaseorder.searchsupplier}/${pharmacyId}`,
      { search: searchText }
    );
  }

  addProductToOrder(payload: any) {
    return this.http.post<any>(`${this.apiConfig.endpoints.purchaseorder.addProduct}`, payload);
  }

  pogenerate(payload: any) {
    return this.http.post<any>(`${this.apiConfig.endpoints.purchaseorder.generatepo}`, payload);
  }

  updateProductInOrder(payload: any, id: number) {
    return this.http.post<any>(
      `${this.apiConfig.endpoints.purchaseorder.updateItem}/${id}`,
      payload,
      { headers: this.apiConfig.requestSettings.header }
    );
  }

  deleteitembyid(id: number) {
    return this.http.post<any>(this.apiConfig.endpoints.purchaseorder.deleteitem(id), { headers: this.apiConfig.requestSettings.header });
  }

  deletePOItembyid(id: number) {
    return this.http.post<any>(this.apiConfig.endpoints.purchaseorder.deletePOItem(id), { headers: this.apiConfig.requestSettings.header });
  }

  deletePObypoid(id: number) {
    return this.http.post<any>(this.apiConfig.endpoints.purchaseorder.deletePO(id), { headers: this.apiConfig.requestSettings.header });
  }

  sendMailToDistributor(id: number) {
    return this.http.post<any>(this.apiConfig.endpoints.purchaseorder.SendMailToDistributor(id), { headers: this.apiConfig.requestSettings.header });
  }

  getOrderProducts(pharmacyId: number): Observable<any> {
    const url = this.apiConfig.endpoints.purchaseorder.shortbookitemlist;
    const reqBody = {
      pharmacyId: pharmacyId,
      start: 1,
      length: 1000,
      status: '',
      statusFilter: '',
      date: ''
    };

    return this.http.post<any>(url, reqBody); // send POST with body
  }


  updateItemQty(poItemId: number, qty: number) {
    return this.http.post<any>(
      `${this.apiConfig.endpoints.purchaseorder.updateitem(poItemId)}?qty=${qty}`,null,{ headers: this.apiConfig.requestSettings.header }
    );
  }

}
