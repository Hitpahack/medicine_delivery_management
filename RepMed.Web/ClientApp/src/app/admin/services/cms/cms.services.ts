import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { CMSDto } from "../../../viewmodels/cms/cms.dto";


@Injectable({
    providedIn: 'root'
})

export class CMSService extends adminBaseService {
    constructor(public http: HttpClient) {
        super(http);

    }
    //#region RoleApi
    add(formData: any) {
        return this.http.post<ApiResponse<CMSDto>>(this.apiConfig.endpoints.CMS.add, formData, { headers: this.apiConfig.requestSettings.header })
    }
    getbyid(id: number) {
        return this.http.post<ApiResponse<CMSDto>>(this.apiConfig.endpoints.CMS.getbyid(id), { headers: this.apiConfig.requestSettings.header })
    }
    edit(formData: any, id: number) {
        console.log("formdata", formData);
        return this.http.post<ApiResponse<CMSDto>>(this.apiConfig.endpoints.CMS.edit(id), formData, { headers: this.apiConfig.requestSettings.header })
    }
    delete(id: number) {
        return this.http.post<any>(this.apiConfig.endpoints.CMS.delete(id), { headers: this.apiConfig.requestSettings.header });
    }

    //#endregion
}