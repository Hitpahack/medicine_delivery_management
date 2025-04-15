import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { LoginResponse } from "../../../viewmodels/accounts/base.accountsdto";
import { admin_apiconfig } from "../../admin.endpoints";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";

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

  add(formData: any, id: number) {
    return this.http.post(admin_apiconfig.endpoints.user.add(id), formData, { headers: admin_apiconfig.requestSettings.header })
  }

  getUserbyId(id: number){
    return this.http.get<AddPersonDto>(admin_apiconfig.endpoints.user.get(id), { headers: admin_apiconfig.requestSettings.header })
  }

  updateUser(formData: any, id: number){
    return this.http.put(admin_apiconfig.endpoints.user.update(id), formData, { headers: admin_apiconfig.requestSettings.header })
  }
}
