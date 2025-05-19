import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CustomValidator } from 'src/app/common/custom.validators';
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-pharmacy-po-list',
    standalone: true,
    templateUrl: './polist.component.html',
    styleUrl: './polist.component.css',
    imports: [CommonModule, RouterModule, DatatableComponent]
})
export class PoListComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router,
        public fb: FormBuilder,
        public validator: CustomValidator,
        public PurchaseOrderService: PurchaseOrderService
    ) {
        super(router, fb);
    }

    activeTab: string = 'orderwise'; // default tab
    selectedItemIds: number[] = [];

    orderwiseOptions: any;
    itemwiseOptions: any;
    distributorwiseOptions: any;

    pharmacyId: string | null = null;
    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        this.loadTableConfigs();
    }

    loadTableConfigs(): void {
        this.orderwiseOptions = {
            tableId: 'orderwiseTable',
            ajax: {
                url: this.admin_apiconfig.endpoints.purchaseorder.getItemOrderWise,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json", // Expect JSON response
                data: (d) => {
                    // Inject pharmacyId into the request payload
                    d.pharmacyId = this.pharmacyId;
                    return JSON.stringify(d);
                }
            },
            serverSide: false,
            processing: true,
            searching: true,
            columns: [
                {
                    title: '<input type="checkbox" id="select_all_main_checkbox">',
                    data: 'id',
                    render: (data) => `<input type="checkbox" class="item_checkbox" data-id="${data}">`,
                    orderable: false
                },
                { title: 'Order ID', data: 'orderId' },
                { title: 'Customer', data: 'customer' },
                { title: 'Amount', data: 'amount' }
            ],
            searchInputId: 'orderwiseSearchBox',
            delaySearchTimeOut: 1000
        };

        this.itemwiseOptions = {
            tableId: 'itemwiseTable',
            ajax: {
                url: this.admin_apiconfig.endpoints.purchaseorder.getItemWise,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json", // Expect JSON response
                data: (d) => {
                    // Inject pharmacyId into the request payload
                    d.pharmacyId = this.pharmacyId;
                    return JSON.stringify(d);
                }
            },
            serverSide: false,
            processing: true,
            searching: true,
            columns: [
                {
                    title: '<input type="checkbox" id="select_all_main_checkbox">',
                    data: 'id',
                    render: (data) => `<input type="checkbox" class="item_checkbox" data-id="${data}">`,
                    orderable: false
                },
                { title: 'Item Name', data: 'itemName' },
                { title: 'Category', data: 'category' },
                { title: 'Quantity', data: 'quantity' }
            ],
            searchInputId: 'itemwiseSearchBox',
            delaySearchTimeOut: 1000
        };

        this.distributorwiseOptions = {
            tableId: 'distributorwiseTable',
            ajax: {
                url: this.admin_apiconfig.endpoints.purchaseorder.getItemDistributorWise,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json", // Expect JSON response
                data: (d) => {
                    // Inject pharmacyId into the request payload
                    d.pharmacyId = this.pharmacyId;
                    return JSON.stringify(d);
                }
            },
            serverSide: false,
            processing: true,
            searching: true,
            columns: [
                {
                    title: '<input type="checkbox" id="select_all_main_checkbox">',
                    data: 'id',
                    render: (data) => `<input type="checkbox" class="item_checkbox" data-id="${data}">`,
                    orderable: false
                },
                { title: 'Distributor', data: 'distributorName' },
                { title: 'Contact', data: 'contact' },
                { title: 'Region', data: 'region' }
            ],
            searchInputId: 'distributorwiseSearchBox',
            delaySearchTimeOut: 1000
        };
    }

    changeTab(tab: string): void {
        this.activeTab = tab;
        this.selectedItemIds = []; // reset selection when tab changes
    }
}
