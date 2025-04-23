import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { CountData } from "src/app/viewmodels/products/productcount.dto";


@Injectable({
    providedIn: 'root'
})

export class ProductService extends adminBaseService {


    constructor(public http: HttpClient) {
        super(http);

    }

    GetRoleInfo(userid: string) {
        return this.http.post<ApiResponse<any>>("", null);
    }

    //#region PharmacyApi
    getProductList(reqData: any, id: number) {
        return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.product.get, reqData, { headers: this.apiConfig.requestSettings.header })
    }
    getcountrecord(reqData: any, id: number) {
        return this.http.post<ApiResponse<CountData>>(this.apiConfig.endpoints.product.getcount, reqData, { headers: this.apiConfig.requestSettings.header })
    }

    //#endregion
}