import { Component, OnInit, NgZone } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from "../../admin.base.component";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";
import { AutoValidateDirective } from "../../../../app/common/form.validator";
import { AfterViewInit } from '@angular/core';
import { RouterModule } from '@angular/router';
declare const window: any;



@Component({
    selector: 'admin-login',
    templateUrl: './admin.login.component.html',
    styles: [''],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective, RouterModule],
})

export class AdminLoginComponent extends AdminBaseComponent implements OnInit, AfterViewInit {

    constructor(
        public validator: CustomValidator,
        public accountservice: adminAccountsService,
        public router: Router,
        private zone: NgZone

    ) {
        super(router);
    }
    uniqueId = '';
    ngAfterViewInit(): void {
        if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
            window.Helpers.initPasswordToggle();
        }
    }
    loginForm: FormGroup;
    ngOnInit(): void {
        this.uniqueId = Math.random().toString(36).substring(2);
        this.loginForm = this.initForm();
    }
    rolename: string;

    initForm(): FormGroup {
        return this.fb.group({
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
            password: new FormControl(null, [Validators.required]),
            remamber: new FormControl(false)

        });
    }
    loginError: string = '';
    onSubmit() {
        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();
            return;
        }
        this.loginError = '';

        this.accountservice.login(this.loginForm.value).subscribe(
            (response) => {
                if (response.isSuccess) {
                    // This is userId..
                    sessionStorage.setItem('userId', response.data.id.toString());

                    // This is personid..
                    sessionStorage.setItem('personid', response.data.person.id.toString());

                    // This is pharmacyId..
                    sessionStorage.setItem('pharmacyId', response.data.pharmacyId.toString());

                    // This is permissions..
                    sessionStorage.setItem('accessData', JSON.stringify(response.data.permissions));

                    // This is roleName..
                    sessionStorage.setItem('rolename', response.data.roleName);
                    this.rolename = (sessionStorage.getItem('rolename') || '').toLowerCase().trim();

                    // This is token..
                    localStorage.setItem('token', response.data.token.token);
                    
                    if (this.rolename === 'admin') {
                        this.router.navigate(['/admin/admindashboard']);
                    }
                    else if (this.rolename === 'pharmacy') {
                        this.router.navigate(['/pharmacy/pharmacydashboard']);
                    }
                    else if (this.rolename === 'doctor') {
                        this.router.navigate(['/doctor/doctordashboard']);
                    }
                    else{
                        
                    }

                }
                if (!response.isSuccess) {
                    this.loginError = response.message || 'Invalid username or password.';
                }
            },
            (err) => {
                this.loginError = 'Server error. Please try again later.';
                Helper.ShowExecptions(err);
            },
            () => { }
        );
    }

}