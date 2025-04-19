import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';

declare var $: any;

@Component({
    selector: 'app-pharmacy-list',
    templateUrl: './list.component.html',
    imports: [DatatableComponent]
})
export class AdminPharmacyListsComponent extends AdminBaseComponent implements OnInit {

    constructor(public validator: CustomValidator) {
        super();
    }
    
    tableOptions = {
        tableId: 'post_pharmacylist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.pharmacy.getPaged,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: function (d) {
                d.search.value = $('#post-search-input').val();
                return JSON.stringify(d);
            }
            
        },
        searching: true,
        columns: [
            { data: 'id', title: '#', render: (data: any) =>`<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />`},
            { data: 'storeName', title: 'Store' },
            { data: 'ownerName', title: 'Owner' },
            { data: 'officialEmail', title: 'Email' },
            { data: 'cityName', title: 'City' },
            { data: 'registeredMobile', title: 'Mobile' }
        ],
        //searchInputId: 'post-search-input',
        customButtons: [
            {
              text: 'Add Pharmacy',
              action: (dttable) => {
                this.router.navigate(['/admin/pharmacy/add']);
              },
              className: 'btn btn-sm btn-primary'
            }
          ]
    };

    ajaxUrl = this.admin_apiconfig.endpoints.pharmacy.getPaged;
    ngOnInit(): void {

        // $('#post_pharmacylist_datatable').DataTable({
        //     processing: true,
        //     serverSide: true,
        //     searching: true,
        //     drawCallback: function (settings) { },
        //     "ajax": {
        //         "url": this.admin_apiconfig.endpoints.pharmacy.getPaged,
        //         "type": "POST",
        //         contentType: "application/json; charset=utf-8",
        //         dataType: "json", // Expect JSON response
        //         "data": function (d) {
        //             d.search.value = $('#post-search-input').val();
        //             return JSON.stringify(d);
        //         }
        //     },
        //     columns: [
        //         {
        //             data: 'id', render: (data: any) =>
        //                 `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />`
        //         },
        //         { data: 'storeName' },
        //         { data: 'ownerName' },
        //         { data: 'officialEmail' },
        //         { data: 'cityName' },
        //         { data: 'registeredMobile' }
        //     ]
        // });
    }
}
