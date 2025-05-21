import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CustomValidator } from 'src/app/common/custom.validators';
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { Helper } from "../../../../app/common/helper.extenstions";
import { FormBuilder } from '@angular/forms';
import { debounceTime, Subject } from 'rxjs';
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

    //This code for make row clickable
    @ViewChild('dataTable') datatable!: DatatableComponent;
    @Output() rowClicked = new EventEmitter<any>();

    activeTab: string = 'orderwise'; // default tab
    selectedItemIds: number[] = [];
    pharmacyId: string | null = null;

    //this code for manage filter & orderwise section
    orderwiseOptions: any;
    itemwiseOptions: any;
    distributorwiseOptions: any;
    orderwiseFilter = { poNumber: '', date: '', SupplierName: '' };
    itemwiseFilter = { itemName: '', date: '', status: '' };
    distributorwiseFilter = { date: '', SupplierName: '' };
    private filterChangeSubject = new Subject<void>();


    POID: number | null = null;

    orderwiseFilterdata = {
        fromDate: '',
        toDate: ''
    };

    refreshTable() {
        let tableId = '';
        switch (this.activeTab) {
            case 'orderwise':
                tableId = '#orderwiseTable';
                break;
            case 'itemwise':
                tableId = '#itemwiseTable';
                break;
            case 'distributorwise':
                tableId = '#distributorwiseTable';
                break;
        }

        if (tableId) {
            setTimeout(() => {
                ($(tableId) as any).DataTable().draw();
            }, 100);
        }
    }
    ngAfterViewInit(): void {

        $(document).off('click', '.emailsend');
        $(document).off('click', '.whatsappmsgsend');
        $(document).off('click', '.deletepo');
        $(document).off('click', '.ShowAllItem');

        $(document).on('click', '.emailsend', (e) => {
            e.stopPropagation();
            const id = $(e.currentTarget).data('id');
            console.log('Email btn clicked for ID:', id);
            this.SendMailToDistributor(id);
        });

        $(document).on('click', '.whatsappmsgsend', (e) => {
            e.stopPropagation();
            const id = $(e.currentTarget).data('id');
            console.log('WhatsApp btn clicked for ID:', id);
            // your whatsapp logic
        });

        $(document).on('click', '.deletepo', (e) => {
            e.stopPropagation();
            const id = $(e.currentTarget).data('id');
            this.DeletePO(id);
        });

        $(document).on('click', '.ShowAllItem', (e) => {
            const id = $(e.currentTarget).data('id');
            this.router.navigate(['/pharmacy/itemdistrlist', id]);

        });
        this.bindOrderwiseRowClick();
        this.initializeDatePickers();
    }

    bindOrderwiseRowClick() {
        const tableId = '#' + this.orderwiseOptions.tableId + ' tbody';
        $(document).off('click', tableId + ' tr');

        $('#' + this.orderwiseOptions.tableId + ' tbody').on('click', 'tr', (event) => {
            //Check
            const target = $(event.target);
            if (target.hasClass('deletepo') || target.hasClass('emailsend') || target.hasClass('whatsappmsgsend') || target.hasClass('ShowAllItem')) {
                return;
            }

            const row = $(event.currentTarget);
            const rowData = this.datatable.dtInstance.row(row).data();
            this.POID = rowData.purchaseOrderId;
            this.router.navigate(['/pharmacy/poitemlist', this.POID]);
            const tableId = this.orderwiseOptions?.tableId;
        });
    }

    // This function use for write rowclick logic
    onRowClick(rowData: any): void {
        console.log('onRowClick method is run:', rowData);
    }

    initializeDatePickers() {
        // Orderwise
        $('#dateRangePickerOrderwise').daterangepicker({
            // options
        }, (start, end) => {
            this.orderwiseFilter.date = `${start.format('YYYY-MM-DD')} to ${end.format('YYYY-MM-DD')}`;
            this.refreshTable();
        });

        // Itemwise
        $('#dateRangePickerItemwise').daterangepicker({
            // options
        }, (start, end) => {
            this.itemwiseFilter.date = `${start.format('YYYY-MM-DD')} to ${end.format('YYYY-MM-DD')}`;
            this.refreshTable();
        });

        // Distributorwise
        $('#dateRangePickerDistributorwise').daterangepicker({
            // options
        }, (start, end) => {
            this.distributorwiseFilter.date = `${start.format('YYYY-MM-DD')} to ${end.format('YYYY-MM-DD')}`;
            this.refreshTable();
        });
    }


    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        this.loadTableConfigs();
        this.filterChangeSubject.pipe(debounceTime(300)).subscribe(() => {
            this.refreshTable();
        });
    }
    onFilterChange(value: string, filterType: string): void {
        switch (filterType) {
            case 'orderwise_poNumber':
                this.orderwiseFilter.poNumber = value;
                break;
            case 'orderwise_SupplierName':
                this.orderwiseFilter.SupplierName = value;
                break;
            case 'itemwise_itemName':
                this.itemwiseFilter.itemName = value;
                break;
            case 'itemwise_status':
                this.itemwiseFilter.status = value;
                break;
            case 'distributorwise_SupplierName':
                this.distributorwiseFilter.SupplierName = value;
                break;
            default:
                break;
        }
        this.filterChangeSubject.next();
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
                    //Search filter
                    d.poNumber = this.orderwiseFilter.poNumber;
                    d.SupplierName = this.orderwiseFilter.SupplierName;
                    if (this.orderwiseFilter.date.includes('to')) {
                        const dates = this.orderwiseFilter.date.split('to').map(x => x.trim());
                        d.fromDate = dates[0];
                        d.toDate = dates[1];
                    }
                    return JSON.stringify(d);
                }
            },
            serverSide: true,
            processing: true,
            searching: false,
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
                   <i class="fa fa-envelope emailsend" data-id="${data.purchaseOrderId}" style="cursor:pointer; color:blue; margin-right:10px;" title="Email"></i>
                   <i class="fa fa-whatsapp whatsappmsgsend" data-id="${row.id}" style="cursor:pointer; color:green; margin-right:10px;" title="WhatsApp"></i>
                   <i class="fa fa-trash deletepo" data-id="${data.purchaseOrderId}" style="cursor:pointer; color:red; cursor:pointer;" title="Delete"></i>
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

                    //search itemName filter
                    d.itemName = this.itemwiseFilter.itemName;
                    d.status = this.itemwiseFilter.status;
                    if (this.itemwiseFilter.date.includes('to')) {
                        const dates = this.itemwiseFilter.date.split('to').map(x => x.trim());
                        d.fromDate = dates[0];
                        d.toDate = dates[1];
                    }
                    return JSON.stringify(d);
                }
            },
            serverSide: true,
            processing: true,
            searching: false,
            columns: [
                {
                    title: 'Item Name',
                    data: 'itemName',
                    render: function (data: any, type: any, row: any) {
                        if (data && data.length > 40) {
                            return data.substring(0, 40) + '...';
                        }
                        return data;
                    }
                },
                { title: 'current Stock', data: 'currentStock' },
                { title: 'ordered Qty', data: 'orderedQty' },
                {
                    title: 'Ordered To',
                    data: 'orderedTo',
                    render: function (data: any, type: any, row: any) {
                        return `<span class="ShowAllItem" data-id="${row.productId}" data-name="${data}" style="color: blue; cursor: pointer;">
                        ${data}
                        </span>`;
                    }
                },
                { title: 'Total Amount', data: 'totalAmount' }
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
                    //search filter
                    d.SupplierName = this.distributorwiseFilter.SupplierName;
                    if (this.distributorwiseFilter.date.includes('to')) {
                        const dates = this.distributorwiseFilter.date.split('to').map(x => x.trim());
                        d.fromDate = dates[0];
                        d.toDate = dates[1];
                    }
                    return JSON.stringify(d);
                }
            },
            serverSide: true,
            processing: true,
            searching: false,
            columns: [
                { title: 'Distributor', data: 'supplierName' },
                { title: 'Mobile No.', data: 'mobile' },
                { title: 'Area', data: 'address' }
            ],
            searchInputId: 'distributorwiseSearchBox',
            delaySearchTimeOut: 1000
        };
    }

    onTabChange(tab: string): void {
        this.activeTab = tab;

        // Optional: refresh table for the active tab
        this.refreshTable();

        // Wait for DOM update
        setTimeout(() => {
            // destroy existing datepickers to prevent duplicate initialization
            $('.daterangepicker').remove();

            // initialize datepicker for the active tab's input
            let id = '';
            if (tab === 'orderwise') id = '#dateRangePickerOrderwise';
            else if (tab === 'itemwise') id = '#dateRangePickerItemwise';
            else if (tab === 'distributorwise') id = '#dateRangePickerDistributorwise';

            if (id) {
                $(id).daterangepicker({
                    opens: 'right',
                    autoUpdateInput: false,
                    locale: {
                        cancelLabel: 'Clear',
                        format: 'YYYY-MM-DD'
                    }
                }, (start, end) => {
                    const formatted = `${start.format('YYYY-MM-DD')} to ${end.format('YYYY-MM-DD')}`;

                    // Set selected date to respective filter object
                    if (tab === 'orderwise') {
                        this.orderwiseFilter.date = formatted;
                    } else if (tab === 'itemwise') {
                        this.itemwiseFilter.date = formatted;
                    } else if (tab === 'distributorwise') {
                        this.distributorwiseFilter.date = formatted;
                    }

                    // Set input box value so selected date is shown in UI
                    $(id).val(formatted);

                    // Refresh table after date change
                    this.refreshTable();
                });

                // Clear value on cancel
                $(id).on('cancel.daterangepicker', function () {
                    $(this).val('');
                });
            }

            // Bind row click for orderwise tab
            if (tab === 'orderwise') {
                this.bindOrderwiseRowClick();
            }
        }, 300); // Delay to ensure DOM & DataTable is ready
    }



    SendMailToDistributor(id: number): void {
        this.PurchaseOrderService.sendMailToDistributor(id).subscribe({
            next: (response) => {
                if (response?.isSuccess) {
                    Helper.ShowSuccess(response.message || 'Mail Send To Distributor successfully');
                    $('#orderwiseTable').DataTable().ajax.reload();
                } else {
                    Helper.ShowError(response.message || 'Failed to Send Mail To Distributor');
                }
            },
            error: (error) => {
                console.error('error:', error);
                const errorMessage = error?.error?.message || 'An error occurred while Send Mail To Distributor';
                Helper.ShowError(errorMessage);
            }
        });
    }

    DeletePO(id: number): void {
        if (confirm('Are you sure you want to delete this PO?')) {
            // Call the deleteRole method from the service
            this.PurchaseOrderService.deletePObypoid(id).subscribe({
                next: (response) => {
                    if (response?.isSuccess) {
                        // Show success message
                        Helper.ShowSuccess(response.message || 'PO deleted successfully');
                        // Optionally reload the DataTable
                        $('#orderwiseTable').DataTable().ajax.reload();
                    } else {
                        // Show error message if deletion failed
                        Helper.ShowError(response.message || 'Failed to delete the item');
                    }
                },
                error: (error) => {
                    console.error('Delete error:', error);
                    // Show error message if there is an API or HTTP error
                    const errorMessage = error?.error?.message || 'An error occurred while deleting the item';
                    Helper.ShowError(errorMessage);
                }
            });
        }
    }

}
