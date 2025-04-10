import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";

@Injectable({
    providedIn: 'root'
  })
  
export class AdminUserService extends adminBaseService {
  
  
      constructor(public http: HttpClient) {
          super(http);
  
      }
  
      GetRoleInfo(userid: string) {
          return this.http.post<ApiResponse<any>>("",null,null);
        }
  }