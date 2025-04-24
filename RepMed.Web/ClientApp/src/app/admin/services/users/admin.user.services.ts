import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { LoginResponse } from "../../../viewmodels/accounts/base.accountsdto";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import {Role} from "../../../viewmodels/User/role.model";

@Injectable({
  providedIn: 'root'
})

export class AdminUserService extends adminBaseService {


  constructor(public http: HttpClient) {
    super(http);

  }

  GetRoleInfo(userid: string) {
    return this.http.post<ApiResponse<any>>("", null);
  }

  add(formData: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.user.add, formData, { headers: this.apiConfig.requestSettings.header })
  }

  edituser(formData: any, id: number) {
    console.log("formdata", formData);
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.user.update(id), formData, { headers: this.apiConfig.requestSettings.header })
  }

  getUserbyId(id: number){
    return this.http.get<ApiResponse<AddPersonDto>>(this.apiConfig.endpoints.user.get(id), { headers: this.apiConfig.requestSettings.header })
  }

  setpassword(formData: any){
    return this.http.post(this.apiConfig.endpoints.user.setpassword, formData, { headers: this.apiConfig.requestSettings.header })
  }

  getRoles(){
    return this.http.get<ApiResponse<Role[]>>(this.apiConfig.endpoints.user.userroles, { headers: this.apiConfig.requestSettings.header })
  }

  changepassword(formData: any){
    return this.http.post(this.apiConfig.endpoints.user.changepassword, formData, { headers: this.apiConfig.requestSettings.header })
  }
}
