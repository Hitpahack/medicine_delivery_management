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
    defaultAvatar: string = '../assets/img/avatars/1.png';
    editUserForm: FormGroup
    addUserData: AddPersonDto;

    errorMessage: string = '';

    countries: CountryDto[] = [];
    states: StateDto[] = [];
    cities: CityDto[] = [];

    today = new Date();
    minDate = new Date(this.today.getFullYear() - 100, this.today.getMonth(), this.today.getDate());
    maxxDate = this.today;

    constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, public AdminCommonServices: AdminCommonServices, private route: ActivatedRoute) {
        super(router, fb);
    }

    maxDate = new Date().toISOString().split('T')[0];

    dateMethod(dateString: Date): string {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        return `${year}-${month}-${day}`;
      }

      dateRangeValidator(control: FormControl) {
        const value = control.value;
        if (!value) return null;
      
        const date = new Date(value);
        if (isNaN(date.getTime())) {
          return { invalidDate: true };
        }
      
        if (date < this.minDate || date > this.maxxDate) {
          return { InValidDate: true };
        }
      
        return null;
      }
    

    ngOnInit(): void {
        this.editUserForm = this.initForm();
        const userId = this.route.snapshot.params['id'];
        if (userId) {
            this.adminuserservice.getUserbyId(userId).subscribe((response) => {
                console.log("userId", userId)
                if (response?.isSuccess && response.data) {
                    const user = response.data;
                    const address = response.data.address;
                    this.editUserForm.patchValue({
                        firstname: user.firstName,
                        lastname: user.lastName,
                        email: user.email,
                        mobile: user.mobile,
                        id: user.id,
                        personId: user.personId,
                        gender: user.gender,
                        dateofBirth: this.dateMethod(user.dateOfBirth),
                        picture: user.picture
                    });

                    this.editUserForm.get('address').patchValue({
                        addressline: response.data.address.addressLine,
                        countryId: response.data.address.countryId,
                        stateId: response.data.address.stateId,
                        cityId: response.data.address.cityId,
                        pincode: response.data.address.pincode
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

    onValueChanged(value: any) {
        this.AdminCommonServices.getstatebyId(value).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.states = response.data;
            } else {
                console.error("Failed to load country data", response);
            }
        })
    }

    onValueChaanged(value: any) {
        this.AdminCommonServices.getcitiesbyId(value).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.cities = response.data;
            } else {
                console.error("Failed to load country data", response);
            }
        })
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

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;

        if (input.files && input.files[0]) {
            const file = input.files[0];
            this.editUserForm.get('imageFile')?.setValue(file);
            console.log("file", file)
            //this.editUserForm.get('needcolumnnamehere')?.setValue(file.name);
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


    initForm(): FormGroup {
        return this.fb.group({
            id: new FormControl(null),
            firstname: new FormControl(null, [Validators.required, Validators.pattern('^[a-zA-Z\s]*$')]),
            lastname: new FormControl(null, [Validators.required, Validators.pattern('^[a-zA-Z\s]*$')]),
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
            mobile: new FormControl(null, [Validators.pattern(/^\d{10}$/)]),
            gender: new FormControl(null),
            dateofBirth: new FormControl(null, this.dateRangeValidator.bind(this)),
            picture: new FormControl(),
            address: this.fb.group({
                addressline: new FormControl(null),
                countryId: new FormControl(),
                stateId: new FormControl(null),
                cityId: new FormControl(null),
                pincode: new FormControl(null),
                latitude: new FormControl(0),
                longitude: new FormControl(0),
            }),
        });
    }

    onSubmit() {
        if (this.editUserForm.valid) {
            const userId = this.route.snapshot.params['id'];
            this.adminuserservice.edituser(this.editUserForm.value, userId)
            .subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'user Updated successfully.');
                        //this.router.navigate(['/admin/dashboard']);
                    } else {
                        console.error('API returned isSuccess: false');
                        this.errorMessage = response.message || 'Failed to add user.';
                        Helper.ShowError(this.errorMessage);
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                    Helper.ShowError(this.errorMessage);
                }
            });
        }
        else {
            this.validator.markInvalidFieldsTouched(this.editUserForm);
        }
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
}