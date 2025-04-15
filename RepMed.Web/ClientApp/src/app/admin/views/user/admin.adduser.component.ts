import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";

@Component({
selector: 'admin-add-user',
templateUrl: './admin.adduser.component.html',
standalone: true,
styles: [''],
imports: [CommonModule,ReactiveFormsModule, FormsModule],
})

export class AddUserComponent extends AdminBaseComponent implements OnInit{
addUserForm: FormGroup;
addUserData: AddPersonDto;
constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice:AdminUserService)
{
    super(router, fb);
}  

ngOnInit(): void {
    this.addUserForm = this.initForm();
  }    

  initForm(): FormGroup {
    return this.fb.group({
      email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
      mobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
      Password : new FormControl(null, [Validators.required]),
      ConfirmPassword : new FormControl(null, [Validators.required]),
    });
  }

  onSubmit() {

    let isValid = this.validateForm(this.addUserForm);
       if(isValid){
        const dto: AddPersonDto = this.addUserForm.value;
           this.adminuserservice.add(dto,0).subscribe({
               next: res => console.log("Success", res),
               error: err => console.error("Error", err)
           });
       }
       else
       Helper.ShowError('Please fill the required fields');
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        // Allow only digits (0�9)
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
  }