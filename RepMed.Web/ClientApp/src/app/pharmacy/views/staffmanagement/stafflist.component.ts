import { Component, OnInit } from '@angular/core';
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

export class StaffListComponent extends AdminBaseComponent implements OnInit {
    constructor(public validator: CustomValidator, public router: Router) {
        super();
    }

    tableOptions = {
        tableId: 'post_pharmacylist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.staff.list,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: function (d) {
                return JSON.stringify(d);
            },
            datasrc:function (json) {
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
            { title: '', data: null, orderable: false, render: (data: any, type: any, row: any) => { return `<span class="edit-user" data-id="${data.personId}"><i class="bi bi-pencil-square cursor-pointer"></i></span>`; } },
        ],
        //searchInputId: 'post-search-input',
    };

    ngOnInit(): void {
        const self = this;
        $(document).on('click', '.edit-user', function () {
            const id = $(this).data('id');
            self.router.navigate(['pharmacy/staff/edit/', id]);
        });
    }
}