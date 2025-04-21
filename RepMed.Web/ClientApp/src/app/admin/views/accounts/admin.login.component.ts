import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from "../../admin.base.component";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";
import { AutoValidateDirective } from "src/app/common/form.validator";
import { AfterViewInit } from '@angular/core';
declare const window: any;



@Component({
    selector: 'admin-login',
    templateUrl: './admin.login.component.html',
    styles: [''],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class AdminLoginComponent extends AdminBaseComponent implements OnInit, AfterViewInit {

    constructor(
        public validator: CustomValidator,
        public accountservice: adminAccountsService

    ) {
        super();
    }

    ngAfterViewInit(): void {
        if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
            window.Helpers.initPasswordToggle();
        }
    }
    loginForm: FormGroup;
    ngOnInit(): void {
        this.loginForm = this.initForm();
    }


    initForm(): FormGroup {
        return this.fb.group({
            email: new FormControl(null, [Validators.required,this.validator.ValidateEmail]),
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
                        sessionStorage.setItem('userId', response.data.id.toString());
                        sessionStorage.setItem('personid', response.data.person.id.toString());
                        this.router.navigate(['/admin/dashboard']);
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