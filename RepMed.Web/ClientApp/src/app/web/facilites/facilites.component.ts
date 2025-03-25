import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { BaseComponent } from "../base.component";

@Component({
    selector: 'app-facilites-layout',
    templateUrl: './facilites.component.html',
    styles: ['']
})

export class FacilitesComponent extends BaseComponent implements OnInit{

    constructor(router:Router){
        super(router);
    }

    
}