import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { APP_DI_CONTAINER } from "../common/app.di.container";
import { HttpClient } from '@angular/common/http';
import { AdminApiConfigService } from './admin.endpoints';
import { CommonModule } from '@angular/common';
import { AutoValidateDirective } from '../common/form.validator';
@Component({
    template: ``
    
})

export class AdminBaseComponent implements OnInit {
    _router: Router;

    public onFormSubmit: EventEmitter<any> = new EventEmitter<any>();
    public onFormSuccess: EventEmitter<any> = new EventEmitter<any>();
    public formInProgress: boolean;

    public admin_apiconfig: AdminApiConfigService;
    public http: HttpClient;
    public fb: FormBuilder;
    public router: Router;

    constructor(public router_: Router = null, public fb_: FormBuilder = null) {
        this._router = router_;
        this.fb = fb_;
        this.admin_apiconfig = APP_DI_CONTAINER.getInjector().get(AdminApiConfigService);
        
        if (this.router == null)
            this.router = APP_DI_CONTAINER.getInjector().get(Router);

        if (this.http == null)
            this.http = APP_DI_CONTAINER.getInjector().get(HttpClient);

        if (this.fb == null)
            this.fb = APP_DI_CONTAINER.getInjector().get(FormBuilder);

    }

    ngOnInit(): void { }

    validateForm(form: FormGroup): boolean {

        for (const key in form.controls) {
            if (form.controls.hasOwnProperty(key)) {
                const control: FormControl = <FormControl>form.controls[key];
                control.updateValueAndValidity();
            }
        }
        return form.valid
    }

    ngEventListioner(): void {

        if (this.onFormSubmit) {
            this.onFormSubmit.subscribe((res) => {
                this.formInProgress = true;

            })

        }

        if (this.onFormSuccess) {
            this.onFormSuccess.subscribe((res) => {
                this.formInProgress = false;
                if (res) {
                    if (res["dialog"]) {
                        res["dialog"].closeAll();
                    }

                }


            })
        }
    }
}

