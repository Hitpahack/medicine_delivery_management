import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { PharmacyDto } from "src/app/viewmodels/pharmacy/Pharmacy.add.dto";
import { StateDto } from "src/app/viewmodels/address/state.dto";
import { CityDto } from "src/app/viewmodels/address/city.dto";
import { CountryDto } from "src/app/viewmodels/address/country.dto";

@Injectable({
    providedIn: 'root'
})

export class AdminPharmacyService extends adminBaseService {


    constructor(public http: HttpClient) {
        super(http);

    }

    GetRoleInfo(userid: string) {
        return this.http.post<ApiResponse<any>>("", null);
    }

    //#region PharmacyApi
    add(formData: any) {
        console.log("called.")
        return this.http.post<ApiResponse<any>>(this.apiConfig.endpoints.pharmacy.add,formData ,{ headers: this.apiConfig.requestSettings.header })
    }
    editpharmacy(formData: any, id: number) {
        console.log("formData", formData)
        return this.http.post<ApiResponse<any>>(this.apiConfig.endpoints.pharmacy.update(id), formData, { headers: this.apiConfig.requestSettings.header })
      }
    getPaged(reqData: any, id: number) {
        return this.http.post(this.apiConfig.endpoints.pharmacy.getPaged, reqData, { headers: this.apiConfig.requestSettings.header })
    }
    getpharmacybyId(id: number) {
        return this.http.get<PharmacyDto>(this.apiConfig.endpoints.pharmacy.get(id), { headers: this.apiConfig.requestSettings.header })
    }
    getcountry() {
        console.log('after call country');
        return this.http.get<ApiResponse<CountryDto>>(this.apiConfig.endpoints.pharmacy.getcountries, { headers: this.apiConfig.requestSettings.header })
    }
    getstatebyId(id: number) {
        return this.http.get<ApiResponse<StateDto>>(this.apiConfig.endpoints.pharmacy.getstates(id), { headers: this.apiConfig.requestSettings.header })
    }
    getcitiesbyId(id: number) {
        return this.http.get<ApiResponse<CityDto>>(this.apiConfig.endpoints.pharmacy.getcities(id), { headers: this.apiConfig.requestSettings.header })
    }

    //#endregion
}