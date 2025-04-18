import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { admin_apiconfig } from "../../admin.endpoints";
import 'datatables.net';
import 'datatables.net-bs4';
import { AdminBaseComponent } from '../../admin.base.component';

declare var $: any;

@Component({
    selector: 'app-User-list',
    templateUrl: 'admin.userlist.component.html'
})

export class UserListComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router, public fb: FormBuilder,
        public validator: CustomValidator

    ) {
        super(router, fb);
    }

    dtColumns = [
        { title: 'ID', data: 'id' }
    ];

    dtOptions = {
        processing: true,
        serverSide: true,
        searching: true
    };

    //ajaxUrl = admin_apiconfig.endpoints.user.list

    ngOnInit(): void {

        const table = $('#UserList_datatable').DataTable({
            processing: true,
            serverSide: true,
            searching: true,
            
            drawCallback: function (settings) { },
            "ajax": {
                "url": admin_apiconfig.endpoints.user.list,
                "type": "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                "data": function (d) {
                    d.search.value = $('#post-search-input').val();
                    return JSON.stringify(d);
                },
                dataSrc: function (json) {
                    return json.data;
                }
            },
            columns: [
                { data: 'email' },
                { data: 'firstName' },
                { data: 'lastName' },
                { data: 'mobile' },
            ]
        });
    }

}