import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CustomValidator } from 'src/app/common/custom.validators';
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { FormBuilder } from '@angular/forms';
declare var $: any;

@Component({
    selector: 'app-pharmacy-po-list',
    standalone: true,
    templateUrl: './polist.component.html',
    styleUrl: './polist.component.css',
    imports: [CommonModule, RouterModule, DatatableComponent]
})
export class PoListComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
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

    orderwiseFilter = { poNumber: '', date: '', distributor: '' };
    itemwiseFilter = { itemName: '', date: '', status: '' };
    distributorwiseFilter = { date: '', distributorName: '' };


    orderwiseFilterdata = {
        fromDate: '',
        toDate: ''
    };
    ngAfterViewInit(): void {
        $(document).on('click', '.email-icon', (e) => {
            const id = $(e.currentTarget).data('id');
            console.log('Email icon clicked for ID:', id);
            // your email logic
        });

        $(document).on('click', '.whatsapp-icon', (e) => {
            const id = $(e.currentTarget).data('id');
            console.log('WhatsApp icon clicked for ID:', id);
            // your whatsapp logic
        });

        $(document).on('click', '.delete-icon', (e) => {
            const id = $(e.currentTarget).data('id');
            console.log('Delete icon clicked for ID:', id);
            // your delete logic
        });

        $('#dateRangePicker').daterangepicker(
            {
                opens: 'right',
                autoUpdateInput: false,
                locale: {
                    cancelLabel: 'Clear',
                    format: 'YYYY-MM-DD'
                }
            },
            (start: any, end: any) => {
                this.orderwiseFilterdata.fromDate = start.format('YYYY-MM-DD');
                this.orderwiseFilterdata.toDate = end.format('YYYY-MM-DD');
                $('#dateRangePicker').val(`${this.orderwiseFilterdata.fromDate} to ${this.orderwiseFilterdata.toDate}`);
            }
        );

        $('#dateRangePicker').on('cancel.daterangepicker', function () {
            $(this).val('');
        });
    }

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
            serverSide: true,
            processing: true,
            searchable: false,
            columns: [
                { title: 'PO No.', data: 'poNumber' },
                {
                    title: 'PO Date',
                    data: 'orderDate',
                    render: function (data) {
                        if (!data) return '';
                        const date = new Date(data);

                        const pad = (n) => (n < 10 ? '0' + n : n);
                        const day = pad(date.getDate());
                        const month = pad(date.getMonth() + 1);
                        const year = date.getFullYear().toString().slice(-2);

                        let hours = date.getHours();
                        const minutes = pad(date.getMinutes());
                        const ampm = hours >= 12 ? 'PM' : 'AM';
                        hours = hours % 12;
                        hours = hours ? hours : 12; // the hour '0' should be '12'

                        return `${day}-${month}-${year} ${pad(hours)}:${minutes}${ampm}`;
                    }
                },
                { title: 'Distributor', data: 'supplierName' },
                { title: 'status', data: 'status' },
                { title: 'available', data: 'available' },
                { title: 'fulfilled', data: 'fulfilled' },
                { title: 'amount', data: 'amount' },
                {
                    title: 'Actions',
                    data: null,
                    orderable: false,
                    render: (data, type, row) => {
                        return `
                   <i class="fa fa-envelope email-icon" data-id="${row.id}" style="cursor:pointer; color:blue; margin-right:10px;" title="Email"></i>
                   <i class="fa fa-whatsapp whatsapp-icon" data-id="${row.id}" style="cursor:pointer; color:green; margin-right:10px;" title="WhatsApp"></i>
                   <i class="fa fa-trash delete-icon" data-id="${row.id}" style="cursor:pointer; color:red; cursor:pointer;" title="Delete"></i>
                    `;
                    }
                }
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
            serverSide: true,
            processing: true,
            searching: false,
            columns: [
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
