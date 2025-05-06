import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../../../admin/services/admin.baseservice";
import { AddRoleDto } from "../../../viewmodels/role/role.dto";
import { ComponentDto } from "../../../viewmodels/role/Component.dto";


@Injectable({
    providedIn: 'root'
})

export class RoleService extends adminBaseService {
    constructor(public http: HttpClient) {
        super(http);

    }
    //#region RoleApi
    add(formData: any) {
        return this.http.post<ApiResponse<AddRoleDto>>(this.apiConfig.endpoints.pharmacyrole.add, formData, { headers: this.apiConfig.requestSettings.header })
    }
    getmodule() {
        return this.http.post<ApiResponse<ComponentDto[]>>(this.apiConfig.endpoints.pharmacyrole.getmodule, { headers: this.apiConfig.requestSettings.header })
    }
    getmodulebyroleid(id: number) {
        return this.http.post<ApiResponse<AddRoleDto[]>>(this.apiConfig.endpoints.pharmacyrole.getmodulebyid(id), { headers: this.apiConfig.requestSettings.header })
    }
    editrole(formData: any, id: number) {
        console.log("formdata", formData);
        return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.pharmacyrole.editrole(id), formData, { headers: this.apiConfig.requestSettings.header })
    }
    deleteRole(id: number) {
        return this.http.post<any>(this.apiConfig.endpoints.pharmacyrole.deleterole(id), { headers: this.apiConfig.requestSettings.header });
    }

    //#endregion
}