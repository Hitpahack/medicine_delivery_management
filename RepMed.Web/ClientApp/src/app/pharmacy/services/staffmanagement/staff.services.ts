import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../../../admin/services/admin.baseservice";
import { LoginResponse } from "../../../viewmodels/accounts/base.accountsdto";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { Role } from "../../../viewmodels/User/role.model";
import { UserListDto } from "../../../viewmodels/User/User.list.dto";

@Injectable({
  providedIn: 'root'
})

export class StaffService extends adminBaseService {


  constructor(public http: HttpClient) {
    super(http);

  }
  add(formData: any) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.staff.add, formData, { headers: this.apiConfig.requestSettings.header })
  }

  edituser(formData: any, id: number) {
    return this.http.post<ApiResponse<Task>>(this.apiConfig.endpoints.staff.update(id), formData, { headers: this.apiConfig.requestSettings.header })
  }

  getUserbyId(id: number) {
    return this.http.get<ApiResponse<AddPersonDto>>(this.apiConfig.endpoints.staff.get(id), { headers: this.apiConfig.requestSettings.header })
  }

  getRoles() {
    return this.http.get<ApiResponse<Role[]>>(this.apiConfig.endpoints.staff.userroles, { headers: this.apiConfig.requestSettings.header })
  }

  getallusers() {
    return this.http.post<ApiResponse<UserListDto>>(this.apiConfig.endpoints.staff.list, { headers: this.apiConfig.requestSettings.header })
  }
}
