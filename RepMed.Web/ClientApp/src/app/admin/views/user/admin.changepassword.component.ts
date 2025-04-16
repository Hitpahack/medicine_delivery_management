import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'admin-Change-password',
    templateUrl: './admin.changepassword.component.html',
    standalone: true,
    styles: [''],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class ChangePassword extends AdminBaseComponent implements OnInit{
    ChangePasswordForm: FormGroup;

    constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, private route: ActivatedRoute) {
        super(router, fb);
    }

    
    ngOnInit(): void {
        this.ChangePasswordForm = this.initForm();
    }

    initForm(): FormGroup {
        return this.fb.group({
          Password: new FormControl(null, [Validators.required]),
          ConfirmPassword: new FormControl(null, [Validators.required]),
        },
        {
          validators: this.validator.passwordMatchValidator
        }
      );
      }

      onSubmit() {
        let isValid = this.validateForm(this.ChangePasswordForm)
        if(isValid){
            this.adminuserservice.changepassword(this.ChangePasswordForm.value).subscribe(response =>{
                alert(response)
            })
        }
        else
        Helper.ShowError('Please fill the required fields');
      }
}