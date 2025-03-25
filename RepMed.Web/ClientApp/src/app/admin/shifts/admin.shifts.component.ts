import { Component, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";
import { AdminBaseComponent } from "../admin.base.component";


@Component({
    selector: 'admin-shifts',
    templateUrl: './admin.shifts.component.html',
    styles: ['']
})

export class AdminShiftsComponent extends AdminBaseComponent implements OnInit{

    constructor(router:Router,fb:FormBuilder){
        super(router,fb);
    }

    ngOnInit(): void {
        
    }
}