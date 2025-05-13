import { AfterViewInit, Component, OnInit } from '@angular/core';
import { RouterModule, Router } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../common/custom.validators";
import { CommonModule } from '@angular/common';
import { AdminBaseComponent } from '../../../admin/admin.base.component';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';
declare var $: any;

@Component({
    selector: 'app-User-list',
    templateUrl: 'stafflist.component.html',
    imports: [DatatableComponent, RouterModule, CommonModule]
})

export class StaffListComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(public validator: CustomValidator, public router: Router) {
        super();
    }
    pharmacyId: string | null = null;

    ngAfterViewInit(): void {
        $(document).off('click', '.toggle-status-btn');

        // Active/Inactive toggle button click handler
        $(document).on('click', '.toggle-status-btn', (event) => {
            const button = $(event.currentTarget);
            const id = $(event.currentTarget).data('id');
            console.log('this is active id', id);
            const currentStatus = button.data('status');
            const newStatus = !currentStatus;

            // Call your API to update the status
            this.toggleStatus(id, newStatus, button, currentStatus);
        });

    }

    // Method to update the status (Active/Inactive)
    toggleStatus(id: number, newStatus: boolean, button: any, currentStatus: boolean): void {
        const apiUrl = `${this.admin_apiconfig.endpoints.user.updateStatus}/${id}?status=${newStatus}`;

        $.ajax({
            url: apiUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (response) => {
                if (response.isSuccess) {
                    // Update button UI
                    button.removeClass(currentStatus ? 'btn-success' : 'btn-danger')
                        .addClass(newStatus ? 'btn-success' : 'btn-danger')
                        .attr('title', newStatus ? 'Deactivate' : 'Activate')
                        .html(`
                              ${newStatus ? '<i class="bi bi-check-circle" style="font-size: 16px;"></i>' : '<i class="bi bi-x-circle" style="font-size: 16px;"></i>'}
                              ${newStatus ? 'Active' : 'Inactive'}
                          `);
                    button.data('status', newStatus);

                    $('#post_pharmacystafflist_datatable').DataTable().ajax.reload();

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
        tableId: 'post_pharmacystafflist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.staff.list,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: (d: any) => {
                d.pharmacyId = this.pharmacyId; // ✅ now works correctly
                return JSON.stringify(d);
            },
            datasrc: function (json) {
                debugger;
                console.log('User list API response:', json);
                return json.data || json;
            }
        },
        searching: true,
        columns: [
            { data: 'id', title: '#', render: (data: any) => `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />` },
            { data: 'email' },
            { data: 'roleName' },
            { data: 'firstName' },
            { data: 'lastName' },
            { data: 'mobile' },
            {
                title: '',
                data: null,
                orderable: false,
                render: (data: any, type: any, row: any) => {
                    return `
                    <button class="btn btn-sm btn-primary edit-pharmacystaff" data-id="${data.personId}" title="Edit" style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                    <i class="bi bi-pencil-square" style="font-size: 16px;"></i>
                    </button>

                    <button class="btn btn-sm ${!row.isLocked ? 'btn-success' : 'btn-danger'} toggle-status-btn" 
                    data-id="${row.userId}" data-status="${row.isLocked}" 
                    title="${row.isLocked ? 'Deactivate' : 'Activate'}" 
                    style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; 
                    padding: 5px 10px; border-radius: 12px; min-width: 120px;">
                    ${!row.isLocked ? '<i class="bi bi-check-circle" style="font-size: 16px;"></i>' : '<i class="bi bi-x-circle" style="font-size: 16px;"></i>'}
                    ${!row.isLocked ? 'Active' : 'Inactive'}
                    </button>
                    
                    `;
                }
            },
        ],
        //searchInputId: 'post-search-input',
    };

    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const self = this;
        $(document).on('click', '.edit-pharmacystaff', function () {
            const id = $(this).data('id');
            self.router.navigate(['pharmacy/staff/edit/', id]);
        });
    }
}