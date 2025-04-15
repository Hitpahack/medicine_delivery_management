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
    selector: 'admin-edit-user',
    templateUrl: 'admin.editprofile.component.html',
    standalone: true,
    styles: [''],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class EditProfile extends AdminBaseComponent implements OnInit {
    editUserForm: FormGroup
    addUserData: AddPersonDto;
    constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, private route: ActivatedRoute) {
        super(router, fb);
    }

    ngOnInit(): void {
        this.editUserForm = this.initForm();
        const userId = this.route.snapshot.params['id']; 
        console.log("userids", userId)
        if (userId) {
            this.adminuserservice.getUserbyId(userId).subscribe((response) => {
                if (response?.isSuccess && response.data) {
                    const user = response.data;
                    console.log("User data received:", user.lastName);
                
                    this.editUserForm.patchValue({
                      firstname: user.firstName,
                      lastname: user.lastName,
                      email: user.email,
                      mobile: user.mobile
                    });
                  } else {
                    console.error("Failed to load user data", response);
                  }
            });
          }
    }


    initForm(): FormGroup {
        return this.fb.group({
            firstname: new FormControl(null, [Validators.required]),
            lastname: new FormControl(null, [Validators.required]),
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
            mobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
        });
    }

    onSubmit() {
        console.log("form submited")
        let isValid = this.validateForm(this.editUserForm);
        console.log("isValid")
        if (isValid) {
            const userId = this.route.snapshot.params['id'];

            const dto: AddPersonDto = this.editUserForm.value;
            this.adminuserservice.add(dto, userId).subscribe(response =>{
                console.log("User udated successfully!")
            })
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