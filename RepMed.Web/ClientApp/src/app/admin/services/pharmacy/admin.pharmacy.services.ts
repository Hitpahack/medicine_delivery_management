import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { admin_apiconfig } from "../../admin.endpoints";
import { PharmacyDto } from "src/app/viewmodels/pharmacy/Pharmacy.add.dto";

@Injectable({
    providedIn: 'root'
})

export class AdminPharmacyService extends adminBaseService {


    constructor(public http: HttpClient) {
        super(http);

    }

    GetRoleInfo(userid: string) {
        return this.http.post<ApiResponse<any>>("", null);
    }

    //#region PharmacyApi
    add(reqData: any, id: number) {
        console.log("called.")
        return this.http.post(admin_apiconfig.endpoints.pharmacy.add(id), reqData, { headers: admin_apiconfig.requestSettings.header })
    }
    getPaged(reqData: any, id: number) {
        return this.http.post(admin_apiconfig.endpoints.pharmacy.getPaged, reqData, { headers: admin_apiconfig.requestSettings.header })
    }
    getpharmacybyId(id: number) {
        return this.http.get<ApiResponse<PharmacyDto>>(admin_apiconfig.endpoints.pharmacy.get(id), { headers: admin_apiconfig.requestSettings.header })
    }

    //#endregion
}