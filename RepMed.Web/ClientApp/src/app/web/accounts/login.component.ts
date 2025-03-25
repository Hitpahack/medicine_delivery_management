import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { BaseComponent } from "../base.component";

@Component({
    selector: 'app-login-layout',
    templateUrl: './login.component.html',
    styles: ['']
})

export class LoginComponent extends BaseComponent implements OnInit{

    constructor(router:Router){
        super(router);
    }
    
}