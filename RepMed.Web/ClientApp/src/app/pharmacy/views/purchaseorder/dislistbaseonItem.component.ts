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
    POID: Number | null = null;

    ngAfterViewInit(): void {
        
    }

    tableOptions = {
        tableId: 'post_distributorList_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.purchaseorder.getItemBaseOnPONumber,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: (d) => {
                d.pharmacyId = this.pharmacyId;
                d.POId = this.POID;
                return JSON.stringify(d);
            }

        },
        searching: false,
        columns: [
            {
                title: 'Ordered To',
                data: 'itemName',
                render: function (data: any, type: any, row: any) {
                    if (data && data.length > 30) {
                        return data.substring(0, 30) + '...';
                    }
                    return data;
                }
            },
            { title: 'PO NO.', data: 'currentStock' },
            { title: 'Date.', data: 'stockAvailability' },
            { title: 'Priority', data: 'status' },
            { title: 'Qty', data: 'mrp' },
            { title: 'Total Amount', data: 'amount' }
        ],
    };

    ngOnInit(): void {
        this.POID = Number(this.route.snapshot.paramMap.get('id'));
        this.pharmacyId = sessionStorage.getItem('pharmacyId');

    }

}