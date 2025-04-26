import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from "@angular/router";
import { RouterModule } from '@angular/router';
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';

declare var $: any;

@Component({
    selector: 'app-Role-list',
    templateUrl: 'rolelist.component.html',
    imports: [DatatableComponent, RouterModule]
})

export class RoleListComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(public validator: CustomValidator) {
        super();
    }

    ngAfterViewInit(): void {
        $(document).on('click', '.edit-btn', (event) => {
            const id = $(event.currentTarget).data('id');
            console.log('Edit clicked for ID:', id);
            this.router.navigate(['/admin/role/edit', id]);
            // Here you can navigate or open a popup
        });
    }

    tableOptions = {
        tableId: 'post_pharmacylist_datatable',
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
                        <button class="btn btn-sm btn-primary edit-btn" data-id="${row.id}" title="Edit" style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                            <i class="bi bi-pencil-square" style="font-size: 16px;"></i>
                        </button>
                    `;
                }
            }
        ],
        //searchInputId: 'post-search-input',
    };



    ngOnInit(): void {


    }

}