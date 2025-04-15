import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { admin_apiconfig } from "../../admin.endpoints";

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

    add(formData:any, id: number){
        console.log("called.")
        return this.http.post(admin_apiconfig.endpoints.addpharmacy.add(id), formData, { headers: admin_apiconfig.requestSettings.header })
      }
}