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

export class AdminCommonServices extends adminBaseService{
    constructor(public http: HttpClient) {
        super(http);

    }

    getcountry() {
        return this.http.get<ApiResponse<CountryDto[]>>(this.apiConfig.endpoints.pharmacy.getcountries, { headers: this.apiConfig.requestSettings.header })
    }
    
    getstatebyId(id: number) {
        return this.http.get<ApiResponse<StateDto[]>>(this.apiConfig.endpoints.pharmacy.getstates(id), { headers: this.apiConfig.requestSettings.header })
    }
    getcitiesbyId(id: number) {
        return this.http.get<ApiResponse<CityDto[]>>(this.apiConfig.endpoints.pharmacy.getcities(id), { headers: this.apiConfig.requestSettings.header })
    }
}