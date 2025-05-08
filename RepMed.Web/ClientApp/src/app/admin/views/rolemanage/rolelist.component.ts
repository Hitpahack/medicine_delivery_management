import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from "@angular/router";
import { RouterModule } from '@angular/router';
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';
import { RoleService } from "../../services/role/role.services";
import { Helper } from "../../../../app/common/helper.extenstions";

declare var $: any;

@Component({
    selector: 'app-Role-list',
    templateUrl: 'rolelist.component.html',
    imports: [DatatableComponent, RouterModule]
})

export class RoleListComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(public validator: CustomValidator, public RoleService: RoleService) {
        super();
    }

    ngAfterViewInit(): void {
        $(document).off('click', '.edit-role');
        $(document).off('click', '.toggle-status-btn');
        $(document).off('click', '.delete-role');
    

        $(document).on('click', '.edit-role', (event) => {
            const id = $(event.currentTarget).data('id');
            this.router.navigate(['/admin/role/edit', id]);
            // Here you can navigate or open a popup
        });

        // Active/Inactive toggle button click handler
        $(document).on('click', '.toggle-status-btn', (event) => {
            const button = $(event.currentTarget);
            const id = $(event.currentTarget).data('id');
            //const id = button.data('id');
            console.log('this is active id', id);
            const currentStatus = button.data('status'); // Current active/inactive status
            const newStatus = !currentStatus; // Toggle status

            // Call your API to update the status
            this.toggleStatus(id, newStatus, button, currentStatus);
        });

        $(document).on('click', '.delete-role', (event) => {
            const id = $(event.currentTarget).data('id');
            this.onDelete(id);
        });
    }

    // Method to update the status (Active/Inactive)
    toggleStatus(id: number, newStatus: boolean, button: any, currentStatus: boolean): void {
        const apiUrl = `${this.admin_apiconfig.endpoints.role.updateStatus}/${id}?status=${newStatus}`;

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

                    $('#post_rolelist_datatable').DataTable().ajax.reload();

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
        tableId: 'post_rolelist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.role.list,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: function (d) {
                return JSON.stringify(d);
            }

        },
        searching: true,
        columns: [
            { data: 'roleName' },
            { data: 'description' },
            {
                data: null,
                orderable: false,
                searchable: false,
                render: (data, type, row) => {
                    return `
                        <button class="btn btn-sm btn-primary edit-role" data-id="${row.id}" title="Edit" style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                            <i class="bi bi-pencil-square" style="font-size: 16px;"></i>
                        </button>

                        <button class="btn btn-sm btn-danger delete-role" data-id="${row.id}" title="Delete" 
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
        //searchInputId: 'post-search-input',
    };

    onDelete(id: number): void {
        if (confirm('Are you sure you want to delete this item?')) {
            // Call the deleteRole method from the service
            this.RoleService.deleteRole(id).subscribe({
                next: (response) => {
                    if (response?.isSuccess) {
                        // Show success message
                        Helper.ShowSuccess(response.message || 'Item deleted successfully');
                        // Optionally reload the DataTable
                        $('#post_rolelist_datatable').DataTable().ajax.reload();
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
    
    ngOnInit(): void {


    }

}