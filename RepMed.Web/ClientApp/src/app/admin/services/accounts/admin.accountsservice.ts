import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { LoginResponse } from "../../../../app/viewmodels/accounts/base.accountsdto";
import { AdminApiConfigService } from "../../admin.endpoints";
import { adminBaseService } from "../admin.baseservice";
import { Observable } from "rxjs";
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class adminAccountsService extends adminBaseService {

  constructor() {
    super();
  }

  login(formData: any): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(this.apiConfig.endpoints.accounts.login, formData, { headers: this.apiConfig.requestSettings.header }
    );
  }
  // logout(): Observable<ApiResponse<any>> {
  //   return this.http.post<ApiResponse<any>>(this.apiConfig.endpoints.accounts.logout,{},{ headers: this.apiConfig.requestSettings.header });
  // }

  logout(): Observable<ApiResponse<any>> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
    });

    return this.http.post<ApiResponse<any>>(
        this.apiConfig.endpoints.accounts.logout,
        {},
        { headers }
    );
}

}