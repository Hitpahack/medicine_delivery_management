import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../app/common/api.response";
import { adminBaseService } from "../../../app/admin/services/admin.baseservice";
import { GetSupppliersDto } from "../../viewmodels/purchaseorder/supplier.dto"
import { SupplierDto } from "../../viewmodels/supplier/supplier.dto";
import { Product } from "../../viewmodels/purchaseorder/product.dto";

@Injectable({
  providedIn: 'root'
})

export class PurchaseOrderService extends adminBaseService {


  constructor(public http: HttpClient) {
    super(http);

  }

  add(formData: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.user.add, formData, { headers: this.apiConfig.requestSettings.header })
  }

  edituser(formData: any, id: number) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.user.update(id), formData, { headers: this.apiConfig.requestSettings.header })
  }

  getsupplier(id: number) {
    return this.http.post<ApiResponse<GetSupppliersDto[]>>(this.apiConfig.endpoints.purchaseorder.getsuppliers(id), { headers: this.apiConfig.requestSettings.header })
  }
  getPoNumber(pharmacyId: number) {
    return this.http.post<ApiResponse<{nextPONumber:string}>>(this.apiConfig.endpoints.purchaseorder.getPoNumber(pharmacyId),{ headers: this.apiConfig.requestSettings.header });
  }
  addSupplier(formData: any) {
    return this.http.post<ApiResponse<SupplierDto>>(this.apiConfig.endpoints.purchaseorder.addSupplier, formData, { headers: this.apiConfig.requestSettings.header })
  }
  getproductlist() {
    return this.http.post<ApiResponse<Product[]>>(this.apiConfig.endpoints.purchaseorder.getproduct,{ headers: this.apiConfig.requestSettings.header })
  }

//   getallusers() {
//     return this.http.post<ApiResponse<UserListDto>>(this.apiConfig.endpoints.user.list, { headers: this.apiConfig.requestSettings.header })
//   }
}
