import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from "@angular/router";
import { RouterModule } from '@angular/router';
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { SupplierService } from "../../../admin/services/supplier/supplier.services";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from '../../../admin/admin.base.component';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';

declare var $: any;

@Component({
    selector: 'app-supplier-list',
    templateUrl: 'supplierlist.component.html',
    imports: [DatatableComponent, RouterModule]
})

export class SupplierListComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(
        public validator: CustomValidator, 
        public SupplierService: SupplierService,

    ) {
        super();
    }
    pharmacyId: string | null = null;
    ngAfterViewInit(): void {
        $(document).off('click', '.edit-supplier');
        $(document).off('click', '.toggle-status-btn');
        $(document).off('click', '.delete-supplier');


        $(document).on('click', '.edit-supplier', (event) => {
            const id = $(event.currentTarget).data('id');
            this.router.navigate(['/pharmacy/supplier/edit', id]);
        });

        // Active/Inactive toggle button click handler
        $(document).on('click', '.toggle-status-btn', (event) => {
            const button = $(event.currentTarget);
            const id = $(event.currentTarget).data('id');
            const currentStatus = button.data('status');
            const newStatus = !currentStatus;

            // Call your API to update the status
            this.toggleStatus(id, newStatus, button, currentStatus);
        });

        $(document).on('click', '.delete-supplier', (event) => {
            const id = $(event.currentTarget).data('id');
            this.onDelete(id);
        });
    }

    // Method to update the status (Active/Inactive)
    toggleStatus(id: number, newStatus: boolean, button: any, currentStatus: boolean): void {
        const apiUrl = `${this.admin_apiconfig.endpoints.pharmacyrole.updateStatus}/${id}?status=${newStatus}`;

        $.ajax({
            url: apiUrl,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (response) => {
                if (response.isSuccess) {
                    button.removeClass(currentStatus ? 'btn-success' : 'btn-danger')
                        .addClass(newStatus ? 'btn-success' : 'btn-danger')
                        .attr('title', newStatus ? 'Deactivate' : 'Activate')
                        .html(`
                              ${newStatus ? '<i class="bi bi-check-circle" style="font-size: 16px;"></i>' : '<i class="bi bi-x-circle" style="font-size: 16px;"></i>'}
                              ${newStatus ? 'Active' : 'Inactive'}
                          `);
                    button.data('status', newStatus);

                    $('#post_supplierlist_datatable').DataTable().ajax.reload();

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
        tableId: 'post_supplierlist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.pharmacyrole.list,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: (d) => {
                d.pharmacyId = this.pharmacyId;
                return JSON.stringify(d);
            }

        },
        searching: true,
        columns: [
            { title: 'Distributor', data: 'roleName' },
            { title: 'Email', data: 'roleName' },
            { title: 'Mobile', data: 'roleName' },
            {
                data: null,
                orderable: false,
                searchable: false,
                render: (data, type, row) => {
                    return `
                        <button class="btn btn-sm btn-primary edit-supplier" data-id="${row.id}" title="Edit" style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                            <i class="bi bi-pencil-square" style="font-size: 16px;"></i>
                        </button>

                        <button class="btn btn-sm btn-danger delete-supplier" data-id="${row.id}" title="Delete" 
                            style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                            <i class="bi bi-trash" style="font-size: 16px;"></i>
                        </button>

                        <button class="btn btn-sm ${row.isActive ? 'btn-success' : 'btn-danger'} toggle-status-btn" 
                         data-id="${row.id}" data-status="${row.isActive}" 
                         title="${row.isActive ? 'Deactivate' : 'Activate'}" 
                         style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; 
                         padding: 5px 10px; border-radius: 12px; min-width: 120px;">
                         ${row.isActive ? '<i class="bi bi-check-circle" style="font-size: 16px;"></i>' : '<i class="bi bi-x-circle" style="font-size: 16px;"></i>'}
                         ${row.isActive ? 'Active' : 'Inactive'}
                        </button>
                    `;
                }
            }
        ],
    };

    onDelete(id: number): void {
        if (confirm('Are you sure you want to delete this Supplier?')) {
            this.SupplierService.deleteSupplier(id).subscribe({
                next: (response) => {
                    if (response?.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Supplier deleted successfully');
                        $('#post_supplierlist_datatable').DataTable().ajax.reload();
                    } else {
                        Helper.ShowError(response.message || 'Failed to delete Supplier');
                    }
                },
                error: (error) => {
                    const errorMessage = error?.error?.message || 'An error occurred while deleting the item';
                    Helper.ShowError(errorMessage);
                }
            });
        }
    }

    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');

    }

}