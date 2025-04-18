import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from "../../admin.base.component";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";


@Component({
    selector: 'admin-login',
    templateUrl: './admin.login.component.html',
    styles: [''],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})

export class AdminLoginComponent extends AdminBaseComponent implements OnInit {

    constructor(
        public router: Router, public fb: FormBuilder,
        public validator: CustomValidator,
        public accountservice: adminAccountsService

    ) {
        super(router, fb);
    }

    loginForm: FormGroup;
    ngOnInit(): void {
        this.loginForm = this.initForm();
    }


    initForm(): FormGroup {
        return this.fb.group({
            email: new FormControl(null, [this.validator.ValidateEmail, Validators.required]),
            password: new FormControl(null, [Validators.required]),
            remamber: new FormControl(false, [Validators.required]),
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
                        this.router.navigate(['/admin/dashboard']);
                    } else {
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