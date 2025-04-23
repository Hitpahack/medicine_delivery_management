import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';

declare var $: any;

@Component({
    selector: 'app-User-list',
    templateUrl: 'admin.userlist.component.html',
    imports:[DatatableComponent]
})

export class UserListComponent extends AdminBaseComponent implements OnInit {
    constructor(public validator: CustomValidator) {
        super();
    }


    tableOptions = {
        tableId: 'post_pharmacylist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.user.list,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: function (d) {
                return JSON.stringify(d);
            }

        },
        searching: true,
        columns: [
            { data: 'id', title: '#', render: (data: any) => `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />` },
            { data: 'email' },
            { data: 'firstName' },
            { data: 'lastName' },
            { data: 'mobile' },
        ],
        //searchInputId: 'post-search-input',
    };



    ngOnInit(): void {


    }

}