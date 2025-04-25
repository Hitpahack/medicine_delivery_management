import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'admin-forget-password',
    templateUrl: './admin.forgetPassword.component.html',
    standalone: true,
    styleUrls: ['./admin.editprofile.component.css'],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class ForgotPassword extends AdminBaseComponent implements OnInit {
    ForgetPassword: FormGroup;
    errorMessage: string = '';
    constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, private route: ActivatedRoute) {
        super(router, fb);
    }

    ngOnInit(): void {
        this.ForgetPassword = this.initForm();
    }

    initForm(): FormGroup {
        const form = this.fb.group({
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail])
        })
        return form;
    }


    onSubmit() {
        if (this.ForgetPassword.invalid) {
            this.validator.markInvalidFieldsTouched(this.ForgetPassword);
            return;
        }
        this.adminuserservice.forgetpassword(this.ForgetPassword.value).subscribe({
            next: (res) => {
                if (res.isSuccess) {
                    Helper.ShowSuccess(res.message || '');
                } else {
                    console.error('API returned isSuccess: false');
                    this.errorMessage = res.message || '';
                    Helper.ShowError(this.errorMessage);
                }
            },
            error: (err) => {
                this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                Helper.ShowError('Something went wrong. Please try again.');
            }
        });
    }
}