import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "../../../../app/common/api.response";
import { adminBaseService } from "../../../../app/admin/services/admin.baseservice";

@Injectable({
    providedIn: 'root'
})

export class InvoiceService extends adminBaseService {
    constructor(public http: HttpClient) {
        super(http);

    }

    getProductListByPONumber(poNumber: string) {
        return this.http.get<any>(this.apiConfig.endpoints.Invoice.getPOItemList(poNumber), { headers: this.apiConfig.requestSettings.header }
        );
    }

    updateProductDetails(productData: any) {
        return this.http.post<any>(
            this.apiConfig.endpoints.Invoice.updateItemDetails(), productData, { headers: this.apiConfig.requestSettings.header }
        );
    }

}
