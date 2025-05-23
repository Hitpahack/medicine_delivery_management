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


    getProductListByPONumber(id: number, poNumber: string) {
        return this.http.post<any>(this.apiConfig.endpoints.Invoice.getPOItemList(id),{ poNumber },{ headers: this.apiConfig.requestSettings.header }
        );
    }

    // getProductListByPONumber(id: number, poNumber: string) {
    //     const url = this.apiConfig.endpoints.Invoice.getPOItemList(id);  // e.g. /api/invoice/getPOItemList/123
    //     const body = { poNumber: poNumber };  // Ensure value is a string

    //     const headers = {
    //         'Content-Type': 'application/json'
    //     };

    //     return this.http.post<any>(url, body, { headers });
    // }

    updateProductDetails(productData: any) {
        return this.http.post<any>(
            this.apiConfig.endpoints.Invoice.updateItemDetails(), productData, { headers: this.apiConfig.requestSettings.header }
        );
    }

}
