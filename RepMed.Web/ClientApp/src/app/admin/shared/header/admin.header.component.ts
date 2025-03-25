import { Component, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";
import { AdminBaseComponent } from "../../admin.base.component";


@Component({
    selector: 'admin-header',
    templateUrl: './admin.header.component.html',
    styles: ['']
})

export class AdminHeaderComponent extends AdminBaseComponent implements OnInit  {
    
    constructor(public router: Router,fb:FormBuilder) {
        super(router,fb);
    }

    ngOnInit(): void {
        
    }
    
}