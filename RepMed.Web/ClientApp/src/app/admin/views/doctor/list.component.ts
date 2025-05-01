import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, RouterModule } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';
import { CommonModule } from '@angular/common';
declare var $: any;

@Component({
    selector: 'app-doctor-list',
    templateUrl: './list.component.html',
    imports: [DatatableComponent, RouterModule, CommonModule]
})
export class DoctorList extends AdminBaseComponent implements OnInit {
    constructor(public validator: CustomValidator, public router: Router) {
        super();
    }

    tableOptions = {
        tableId: 'post_doctorlist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.doctor.list,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
            { title: 'Edit', data: null, orderable: false, render: (data: any, type: any, row: any) => { return `<button class="btn btn-primary edit-user" data-id="${data.personId}">Edit </button>`; } },
        ],
        //searchInputId: 'post-search-input',
    };

    ngOnInit(): void {
        const self = this;
        $(document).on('click', '.edit-user', function () {
            const id = $(this).data('id');
            self.router.navigate(['admin/user/edit/', id]);
        });
    }
}
