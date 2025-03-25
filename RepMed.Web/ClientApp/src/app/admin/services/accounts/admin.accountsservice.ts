import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "src/app/common/api.response";
import { LoginResponse } from "src/app/viewmodels/accounts/base.accountsdto";
import { admin_apiconfig } from "../../admin.endpoints";
import { adminBaseService } from "../admin.baseservice";

@Injectable({
  providedIn: 'root'
})

export class adminAccountsService extends adminBaseService {


    constructor(public http: HttpClient) {
        super(http);

    }

    login(formData: any) {
        return this.http.post<ApiResponse<LoginResponse>>(
          admin_apiconfig.endpoints.accounts.login,
          formData,
          { headers: admin_apiconfig.requestSettings.header }
        );
      }
}