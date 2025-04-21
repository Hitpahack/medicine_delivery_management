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
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { AdminCommonServices } from '../../services/Common/admin.commonservices';
import { CountryDto } from "../../../viewmodels/address/country.dto";
import { StateDto } from "../../../viewmodels/address/state.dto";
import { CityDto } from "../../../viewmodels/address/city.dto";


@Component({
    selector: 'admin-edit-user',
    templateUrl: 'admin.editprofile.component.html',
    standalone: true,
    styleUrls: ['./admin.editprofile.component.css'],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule, AutoValidateDirective],
})

export class EditProfile extends AdminBaseComponent implements OnInit {
    defaultAvatar: string = '../assets/img/avatars/1.png'; // default image
    editUserForm: FormGroup
    addUserData: AddPersonDto;

    countries: CountryDto[] = [];
    states: StateDto[] = [];
    cities: CityDto[] = [];

    constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, public AdminCommonServices: AdminCommonServices, private route: ActivatedRoute) {
        super(router, fb);
    }

    maxDate = new Date().toISOString().split('T')[0];

    ngOnInit(): void {
        this.editUserForm = this.initForm();
        const userId = this.route.snapshot.params['id'];
        if (userId) {
            this.adminuserservice.getUserbyId(userId).subscribe((response) => {
                if (response?.isSuccess && response.data) {
                    const user = response.data;
                    this.editUserForm.patchValue({
                        firstname: user.firstName,
                        lastname: user.lastName,
                        email: user.email,
                        mobile: user.mobile,
                        id: user.id,
                        gender: user.gender,
                        dateofBirth: user.dateOfBirth,
                        picture: user.picture
                    });

                    this.editUserForm.get('address')?.patchValue({
                    });
                } else {
                    console.error("Failed to load user data", response);
                }
            });

            this.AdminCommonServices.getcountry().subscribe((response) => {
                if (response?.isSuccess && response.data) {
                    this.countries = response.data;
                } else {
                    console.error("Failed to load country data", response);
                }
            })
        }
    }

    selectedCountry: number;
    onCountryDropdownChange(event: any) {
        this.selectedCountry = event.target.value;
        this.AdminCommonServices.getstatebyId(this.selectedCountry).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.states = response.data;
            } else {
                console.error("Failed to load country data", response);
            }
        })
    }

    selectedCity: number;
    onCityDropdownChange(event: any) {
        this.selectedCity = event.target.value;
        this.AdminCommonServices.getcitiesbyId(this.selectedCity).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.cities = response.data;
            } else {
                console.error("Failed to load country data", response);
            }
        })
    }


    initForm(): FormGroup {
        return this.fb.group({
                id: new FormControl(null),
                firstname: new FormControl(null, [Validators.required]),
                lastname: new FormControl(null, [Validators.required]),
                email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
                mobile: new FormControl(null, [Validators.pattern(/^\d{10}$/)]),
                gender: new FormControl(null),
                dateofBirth: new FormControl(null),
                picture: new FormControl(null),
            address: this.fb.group({
                addressline: new FormControl(null),
                countryId: new FormControl(),
                stateId: new FormControl(null),
                cityId: new FormControl(null),
                pincode: new FormControl(null),
                latitude:new FormControl(0),
                longitude:new FormControl(0),
            }),
        });
    }

    onSubmit() {
        if (this.editUserForm.valid) {
            const userId = this.route.snapshot.params['id'];
            console.log("edituserform", this.editUserForm.value)
            this.adminuserservice.edituser(this.editUserForm.value, userId)
                .subscribe(response => { })
        }
        else {
            this.validator.markInvalidFieldsTouched(this.editUserForm);
        }
    }
    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;

        if (input.files && input.files[0]) {
            const reader = new FileReader();
            reader.onload = e => {
                const avatar = document.getElementById('uploadedAvatar') as HTMLImageElement;
                if (avatar) {
                    avatar.src = e.target?.result as string;
                }
            };
            reader.readAsDataURL(input.files[0]);
        }
    }

    onResetImage(): void {
        const avatar = document.getElementById('uploadedAvatar') as HTMLImageElement;
        if (avatar) {
            avatar.src = this.defaultAvatar;
        }
        const fileInput = document.getElementById('upload') as HTMLInputElement;
        if (fileInput) {
            fileInput.value = '';
        }
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
}