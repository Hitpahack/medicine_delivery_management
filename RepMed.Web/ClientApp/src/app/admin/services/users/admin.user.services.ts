import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { LoginResponse } from "../../../viewmodels/accounts/base.accountsdto";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { Role } from "../../../viewmodels/User/role.model";
import { UserListDto } from "../../../viewmodels/User/User.list.dto";

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
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.user.update(id), formData, { headers: this.apiConfig.requestSettings.header })
  }

  getUserbyId(id: number) {
    return this.http.get<ApiResponse<AddPersonDto>>(this.apiConfig.endpoints.user.get(id), { headers: this.apiConfig.requestSettings.header })
  }

  setpassword(formData: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.accounts.setpassword, formData, { headers: this.apiConfig.requestSettings.header })
  }

  getRoles() {
    return this.http.get<ApiResponse<Role[]>>(this.apiConfig.endpoints.user.userroles, { headers: this.apiConfig.requestSettings.header })
  }

  changepassword(formData: any) {
    //return this.http.post(this.apiConfig.endpoints.user, formData, { headers: this.apiConfig.requestSettings.header })
  }

  forgetpassword(formdata: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.accounts.forgetpassword, formdata, { headers: this.apiConfig.requestSettings.header })
  }

  resetpassword(formdata: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.accounts.resetpassword, formdata, { headers: this.apiConfig.requestSettings.header })
  }

  getallusers() {
    return this.http.post<ApiResponse<UserListDto>>(this.apiConfig.endpoints.user.list, { headers: this.apiConfig.requestSettings.header })
  }
}
