import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { SupplierDto } from "../../../viewmodels/supplier/supplier.dto";


@Injectable({
    providedIn: 'root'
})

export class SupplierService extends adminBaseService {
    constructor(public http: HttpClient) {
        super(http);

    }
    //#region Supplier Api
    addSupplier(formData: any) {
        return this.http.post<ApiResponse<SupplierDto>>(this.apiConfig.endpoints.supplier.add, formData, { headers: this.apiConfig.requestSettings.header })
    }

    getSupplierbyId(id: number) {
        return this.http.post<ApiResponse<SupplierDto>>(this.apiConfig.endpoints.supplier.get(id), { headers: this.apiConfig.requestSettings.header })
    }

    editSupplier(formData: any, id: number) {
        console.log("formdata", formData);
        return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.supplier.edit(id), formData, { headers: this.apiConfig.requestSettings.header })
    }
    deleteSupplier(id: number) {
        return this.http.post<any>(this.apiConfig.endpoints.supplier.delete(id), { headers: this.apiConfig.requestSettings.header });
    }

    //#endregion
}