import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";

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
        return this.http.post(this.apiConfig.endpoints.product.get, reqData, { headers: this.apiConfig.requestSettings.header })
    }

    //#endregion
}