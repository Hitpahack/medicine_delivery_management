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
    selector: 'app-post-list',
    templateUrl: './admin.pharmacylists.component.html',
})
export class AdminPharmacyListsComponent extends AdminBaseComponent implements OnInit  {

    constructor(
        public router: Router, public fb:FormBuilder,
        public validator: CustomValidator
        
        ) {
        super(router, fb);
    }
    ngOnInit(): void {
        const url = 'https://dummyjson.com/posts'; // Dummy API URL

        $('#post_pharmacylist_datatable').DataTable({
            processing: true,
            serverSide: true,
            searching: true,
             drawCallback: function (settings) { },
            "ajax": {
                "url": admin_apiconfig.endpoints.pharmacy.getPaged,
                "type": "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json", // Expect JSON response
                "data": function (d) {
                    d.search.value = $('#post-search-input').val();
                    return JSON.stringify(d);
                }
            },
            columns: [
                {
                    data: 'id', render: (data: any) =>
                        `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />`
                }
            ]
        });
    }
}
