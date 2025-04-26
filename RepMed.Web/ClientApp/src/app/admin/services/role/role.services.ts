import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { AddRoleDto } from "src/app/viewmodels/role/role.dto";
import { ComponentDto } from "src/app/viewmodels/role/Component.dto";


@Injectable({
    providedIn: 'root'
})

export class RoleService extends adminBaseService {
    constructor(public http: HttpClient) {
        super(http);

    }
    //#region RoleApi
    add(formData: any) {
        return this.http.post<ApiResponse<AddRoleDto>>(this.apiConfig.endpoints.role.add, formData, { headers: this.apiConfig.requestSettings.header })
    }
    getmodule() {
        return this.http.post<ApiResponse<ComponentDto[]>>(this.apiConfig.endpoints.role.getmodule, { headers: this.apiConfig.requestSettings.header })
    }
    getmodulebyroleid(id: number) {
        return this.http.post<ApiResponse<AddRoleDto[]>>(this.apiConfig.endpoints.role.getmodulebyid(id), { headers: this.apiConfig.requestSettings.header })
    }
    editrole(formData: any, id: number) {
        console.log("formdata", formData);
        return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.role.editrole(id), formData, { headers: this.apiConfig.requestSettings.header })
      }

    //#endregion
}