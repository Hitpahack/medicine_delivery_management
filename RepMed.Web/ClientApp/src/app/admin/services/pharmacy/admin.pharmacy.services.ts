import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../admin.baseservice";
import { admin_apiconfig } from "../../admin.endpoints";
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
        return this.http.post(admin_apiconfig.endpoints.pharmacy.add,formData ,{ headers: admin_apiconfig.requestSettings.header })
    }
    editpharmacy(formData: any, id: number) {
        return this.http.post(admin_apiconfig.endpoints.user.update(id), formData, { headers: admin_apiconfig.requestSettings.header })
      }
    getPaged(reqData: any, id: number) {
        return this.http.post(admin_apiconfig.endpoints.pharmacy.getPaged, reqData, { headers: admin_apiconfig.requestSettings.header })
    }
    getpharmacybyId(id: number) {
        return this.http.get<PharmacyDto>(admin_apiconfig.endpoints.pharmacy.get(id), { headers: admin_apiconfig.requestSettings.header })
    }
    getcountry() {
        return this.http.post<ApiResponse<CountryDto>>(admin_apiconfig.endpoints.pharmacy.getcountries, { headers: admin_apiconfig.requestSettings.header })
    }
    getstatebyId(id: number) {
        return this.http.get<ApiResponse<StateDto>>(admin_apiconfig.endpoints.pharmacy.getstates(id), { headers: admin_apiconfig.requestSettings.header })
    }
    getcitiesbyId(id: number) {
        return this.http.get<ApiResponse<CityDto>>(admin_apiconfig.endpoints.pharmacy.getcities(id), { headers: admin_apiconfig.requestSettings.header })
    }

    //#endregion
}