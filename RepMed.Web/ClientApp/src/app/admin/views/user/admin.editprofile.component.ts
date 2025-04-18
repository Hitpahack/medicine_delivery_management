import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { ActivatedRoute } from  '@angular/router';

@Component({
    selector: 'admin-edit-user',
    templateUrl: 'admin.editprofile.component.html',
    standalone: true,
    styleUrls: ['./admin.editprofile.component.css'],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class EditProfile extends AdminBaseComponent implements OnInit {
    editUserForm: FormGroup
    addUserData: AddPersonDto;
    constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, private route: ActivatedRoute) {
        super(router, fb);
    }

    ngOnInit(): void {
        console.log("editprofile")
        this.editUserForm = this.initForm();
        const userId = this.route.snapshot.params['id'];
        console.log(userId, "userid")
        if (userId) {
            this.adminuserservice.getUserbyId(userId).subscribe((response) => {
                if (response?.isSuccess && response.data) {
                    const user = response.data;
                    this.editUserForm.patchValue({
                      firstname: user.firstName,
                      lastname: user.lastName,
                      email: user.email,
                      mobile: user.mobile,
                      id:user.id,
                    });
                  } else {
                    console.error("Failed to load user data", response);
                  }
            });
          }
    }


    initForm(): FormGroup {
        return this.fb.group({
            id : new FormControl(null),
            firstname: new FormControl(null),
            lastname: new FormControl(null),
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
            mobile: new FormControl(null),
        });
    }

    onSubmit() {
            const userId = this.route.snapshot.params['id'];
            const dto: AddPersonDto = this.editUserForm.value;
            console.log(userId)
            this.adminuserservice.edituser(dto, userId).subscribe(response =>{
            })
    }


    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
}