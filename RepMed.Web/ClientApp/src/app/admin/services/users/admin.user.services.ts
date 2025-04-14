import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { LoginResponse } from "../../../viewmodels/accounts/base.accountsdto";
import { admin_apiconfig } from "../../admin.endpoints";

@Injectable({
    providedIn: 'root'
  })
  
export class AdminUserService extends adminBaseService {
  
  
      constructor(public http: HttpClient) {
          super(http);
  
      }
  
      GetRoleInfo(userid: string) {
          return this.http.post<ApiResponse<any>>("",null);
        }

        add(formData:any){
          console.log("called.ssss")
          return this.http.post(admin_apiconfig.endpoints.user.add, formData, { headers: admin_apiconfig.requestSettings.header })
        }
  }