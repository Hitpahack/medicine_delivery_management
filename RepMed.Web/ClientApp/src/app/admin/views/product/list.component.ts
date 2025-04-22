import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from '../../admin.base.component';
import { DatatableComponent } from '../../shared/datatables/datatable.component';



@Component({
    selector: 'app-pharmacy-list',
    templateUrl: './list.component.html',
    imports: [DatatableComponent]
})
export class ProductListsComponent extends AdminBaseComponent implements OnInit {

    constructor(public validator: CustomValidator) {
        super();
    }
    
    tableOptions = {
        tableId: 'post_pharmacylist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.product.get,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Expect JSON response
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        searching: true,
        columns: [
            { data: 'id', title: '#', render: (data: any) =>`<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />`},
            { 
                data: 'imageUrl', // or keep it empty if no image URL is coming
                title: '',
                render: (data: any, type: any, row: any) => {
                  const imageUrl = data || 'assets/img/medicine/medicine.jpg'; // 👈 your dummy image path  assets/img/medicine/medicine.jpg
                  return `<img src="${imageUrl}" alt="Product" width="40" height="40" style="object-fit: cover; border-radius: 4px;" />`;
                }
            },
            { data: 'productName', title: 'name' },
            { data: 'price', title: 'price' },
            
            //{ data: '', title: 'img' },
        ],
        //searchInputId: 'post-search-input',
        // customButtons: [
        //     {
        //       text: 'Add Pharmacy',
        //       action: (dttable) => {
        //         this.router.navigate(['/admin/pharmacy/add']);
        //       },
        //       className: 'btn btn-sm btn-primary'
        //     }
        //   ]
    };

    ngOnInit(): void {
       
    }
}
