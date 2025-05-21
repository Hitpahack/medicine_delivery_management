import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { RouterModule } from '@angular/router';
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { PurchaseOrderService } from "../../services/purchaseorder/purchaseorder.services";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from '../../../admin/admin.base.component';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';
import { Title } from '@angular/platform-browser';
declare var $: any;

@Component({
    selector: 'app-distributor-list',
    templateUrl: 'dislistbaseonItem.component.html',
    imports: [DatatableComponent, RouterModule]
})

export class DistributorListBaseOnItemComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(
        public validator: CustomValidator,
        public PurchaseOrderService: PurchaseOrderService,
        public route: ActivatedRoute
    ) {
        super();
    }
    pharmacyId: string | null = null;
    ProductId: Number | null = null;

    ngAfterViewInit(): void {

    }

    tableOptions = {
        tableId: 'post_distributorList_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.purchaseorder.getPOListBaseOnItemID,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: (d) => {
                d.pharmacyId = this.pharmacyId;
                d.ProductId = this.ProductId;
                return JSON.stringify(d);
            }

        },
        searching: false,
        columns: [
            {
                title: 'Ordered To',
                data: 'supplierName',
                render: function (data: any, type: any, row: any) {
                    if (data && data.length > 40) {
                        return data.substring(0, 40) + '...';
                    }
                    return data;
                }
            },
            { title: 'PO NO.', data: 'poNumber' },
            {
                title: 'Date',
                data: 'orderDate',
                render: function (data: any) {
                    if (!data) return '';
                    const date = new Date(data);
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const day = String(date.getDate()).padStart(2, '0');
                    let hours = date.getHours();
                    const minutes = String(date.getMinutes()).padStart(2, '0');
                    const ampm = hours >= 12 ? 'PM' : 'AM';
                    hours = hours % 12;
                    hours = hours ? hours : 12;
                    const strHours = String(hours).padStart(2, '0');
                    return `${month}-${day}, ${strHours}:${minutes} ${ampm}`;
                }
            },
            { title: 'Qty', data: 'quantity' },
            { title: 'Total Amount', data: 'amount' }
        ],
    };

    ngOnInit(): void {
        this.ProductId = Number(this.route.snapshot.paramMap.get('id'));
        this.pharmacyId = sessionStorage.getItem('pharmacyId');

    }

}