import { Component, OnInit, } from '@angular/core';
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
    templateUrl: './admin.pharmacylists.component.html'
})
export class AdminPharmacyListsComponent extends AdminBaseComponent implements OnInit {

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

    ngAfterViewInit(): void {
        const self = this;
    
        // Handle Edit button click
        $('#post_pharmacylist_datatable').on('click', '.edit-btn', function () {
            const id = $(this).data('id');
            self.router.navigate(['/admin/pharmacy/edit', id]);
        });
    }

    ajaxUrl = admin_apiconfig.endpoints.pharmacy.getPaged;
    ngOnInit(): void {

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
                    data: null,
                    render: (data: any, type: any, row: any, meta: any) => {
                        return meta.row + 1;
                    },
                    orderable: false,
                    searchable: false
                },
                { data: 'storeName' },
                { data: 'ownerName' },
                { data: 'officialEmail' },
                { data: 'cityName' },
                { data: 'registeredMobile' },
                {
                    data: 'id',
                    orderable: false,
                    searchable: false,
                    render: (data: any, type: any, row: any) => {
                        return `
                        <button class="btn btn-primary btn-lg d-flex justify-content-center align-items-center edit-btn" 
                        style="width: 100px; height: 40px;" data-id="${data}"> Edit </button>
                        `;
                    }
                }
            ]
        });
    }
}
