import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, RouterModule } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';
import { CommonModule } from '@angular/common';
declare var $: any;

@Component({
    selector: 'app-pharmacy-list',
    templateUrl: './list.component.html',
    imports: [DatatableComponent, RouterModule, CommonModule]
})
export class AdminPharmacyListsComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(public validator: CustomValidator, public router: Router) {
        super();
    }

    ngAfterViewInit(): void {
        $(document).off('click', '.toggle-status-btn');

        $(document).on('click', '.toggle-status-btn', (event) => {
            const button = $(event.currentTarget);
            const id = $(event.currentTarget).data('id');
           // const currentStatus = button.data('status');
            const currentStatus = button.attr('data-status') === 'true';
            const newStatus = !currentStatus;

            // Call your API to update the status
            this.toggleStatus(id, newStatus, button, currentStatus);
        });
    }

    // Method to update the status (Active/Inactive)
    toggleStatus(id: number, newStatus: boolean, button: any, currentStatus: boolean): void {
        button.attr('data-status', newStatus);
        const statusText = newStatus ? 'Active' : 'Inactive'; // 🔥 string based status
        const apiUrl = `${this.admin_apiconfig.endpoints.pharmacy.updateStatus}/${id}?status=${statusText}`;
    
        $.ajax({
            url: apiUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (response) => {
                if (response.isSuccess) {
                    button.removeClass(currentStatus ? 'btn-success' : 'btn-danger')
                        .addClass(newStatus ? 'btn-success' : 'btn-danger')
                        .attr('title', newStatus === true ? 'Deactivate' : 'Activate')
                        .html(`
                            ${newStatus ? '<i class="bi bi-check-circle" style="font-size: 16px;"></i>' : '<i class="bi bi-x-circle" style="font-size: 16px;"></i>'}
                            ${newStatus ? 'Active' : 'Inactive'}
                        `);
                    button.data('status', newStatus);
    
                    setTimeout(() => {
                        $('#post_pharmacylist_datatable').DataTable().ajax.reload();
                    }, 500); // 0.5 second delay
                } else {
                    alert('Failed to update the status. Please try again.');
                }
            },
            error: () => {
                alert('Error while updating the status. Please try again.');
            }
        });
    }
    

    tableOptions = {
        tableId: 'post_pharmacylist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.pharmacy.getPaged,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: function (d) {
                return JSON.stringify(d);
            },
            datasrc: function (json) {
                return json.data || json;
            }
        },
        order: [[0, 'desc']],
        searching: true,
        columns: [
            { data: 'id', title: '#', render: (data: any) => `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />` },
            { data: 'storeName', title: 'Store' },
            { data: 'ownerName', title: 'Owner' },
            { data: 'officialEmail', title: 'Email' },
            { data: 'cityName', title: 'City' },
            { data: 'registeredMobile', title: 'Mobile' },
            { data: 'address1', title: 'Address' },
            { data: 'countryName', title: 'Country' },
            {
                data: null,
                orderable: false,
                render: (data: any, type: any, row: any) => {
                    const isActive = row.status === 'Active';
                    return `
                    <div class="d-flex gap-2 align-items-center">
                    <button class="btn btn-sm btn-primary edit-pharmacy" data-id="${row.id}" title="Edit" style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                            <i class="bi bi-pencil-square" style="font-size: 16px;"></i>
                        </button>
                    <button class="btn btn-sm ${isActive ? 'btn-success' : 'btn-danger'} toggle-status-btn" 
                    data-id="${row.id}" data-status="${isActive}" 
                    title="${isActive ? 'Deactivate' : 'Activate'}" 
                    style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; 
                    padding: 5px 10px; border-radius: 12px; min-width: 120px;">
                    ${isActive ? '<i class="bi bi-check-circle" style="font-size: 16px;"></i>' : '<i class="bi bi-x-circle" style="font-size: 16px;"></i>'}
                    ${isActive ? 'Active' : 'Inactive'}
                    </button>
                    `;
                }
            },
        ],
    };

    ngOnInit(): void {
        const self = this;
        $(document).on('click', '.edit-pharmacy', function () {
            const id = $(this).data('id');
            self.router.navigate(['admin/pharmacy/edit/', id]);
        });
    }
}
