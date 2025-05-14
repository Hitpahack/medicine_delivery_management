import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../../admin/admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { StaffService } from "../../../pharmacy/services/staffmanagement/staff.services";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { Role } from "../../../viewmodels/User/role.model";
import { AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminCommonServices } from "../../../../app/admin/services/Common/admin.commonservices";
import { CountryDto } from "../../../viewmodels/address/country.dto";
import { StateDto } from "../../../viewmodels/address/state.dto";
import { CityDto } from "../../../viewmodels/address/city.dto";
import { AutoValidateDirective } from "../../../common/form.validator";
declare const window: any;

@Component({
    selector: 'admin-add-user',
    templateUrl: './addstaff.component.html',
    standalone: true,
    styleUrls: ['./addstaff.component.css'],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class AddStaffComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    addStaffForm: FormGroup;
    editstaffForm: FormGroup;
    addUserData: AddPersonDto;
    pharmacyId: string | null = null;
    countries: CountryDto[] = [];
    states: StateDto[] = [];
    cities: CityDto[] = [];
    roles: Role[] = [];
    errorMessage: string = '';
    today = new Date();
    minDate = new Date(this.today.getFullYear() - 100, this.today.getMonth(), this.today.getDate());
    maxxDate = this.today;

    constructor(
        public validator: CustomValidator,
        public StaffService: StaffService,
        private route: ActivatedRoute,
        public AdminCommonServices: AdminCommonServices,
        public fb: FormBuilder,
        public router: Router
    ) {
        super();
    }
    ngAfterViewInit(): void {
        if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
            window.Helpers.initPasswordToggle();
        }
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

    userId: number;
    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        this.userId = this.route.snapshot.params['id'];
        if (this.userId && this.userId !== 0) {
            // For Edit
            this.editstaffForm = this.initEditForm();
            this.initEditForm();
            //this.loadRoles();
            //this.loadUserData(this.userId);
            this.loadRoles(() => {
                this.loadUserData(this.userId);
            });
        } else {
            // For Add
            this.addStaffForm = this.initAddForm();
            this.initAddForm();
            this.loadRoles();
        }
    }

    //load roles 

    loadRoles(callback?: () => void) {
        this.StaffService.getRoles(Number(this.pharmacyId)).subscribe((res) => {
            if (res?.isSuccess) {
                this.roles = res.data;
                if (callback) {
                    callback();
                }
            }
        });
    }

    // load user data for update
    loadUserData(id: number) {
        this.StaffService.getUserbyId(this.userId).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                const user = response.data;
                const address = response.data.address;
                this.editstaffForm.patchValue({
                    firstname: user.firstName,
                    lastname: user.lastName,
                    email: user.email,
                    mobile: user.mobile,
                    Role: user.roleName,
                    id: user.id,
                    personId: user.personId,
                    gender: user.gender,
                    dateofBirth: this.dateMethod(user.dateOfBirth)
                });
                if (user.address) {
                    this.editstaffForm.get('address').patchValue({
                        addressline: response.data.address.addressLine,
                        countryId: response.data.address.countryId,
                        stateId: response.data.address.stateId,
                        cityId: response.data.address.cityId,
                        pincode: response.data.address.pincode
                    });

                }
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

    // Add User Form
    initAddForm(): FormGroup {
        const form = this.fb.group({
            firstname: new FormControl(null, [Validators.required, Validators.pattern('^[a-zA-Z\s]*$')]),
            lastname: new FormControl(null, [Validators.required, Validators.pattern('^[a-zA-Z\s]*$')]),
            mobile: new FormControl(null, [Validators.required]),
            Role: new FormControl(null, [Validators.required]),
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
            Password: new FormControl(null, [Validators.required, this.validator.validateStrongPassword]),
            ConfirmPassword: new FormControl(null, [Validators.required]),
            gender: new FormControl(null),
            dateofBirth: new FormControl(null, this.dateRangeValidator.bind(this)),
            address: this.fb.group({
                addressline: new FormControl(null),
                countryId: new FormControl(),
                stateId: new FormControl(),
                cityId: new FormControl(),
                pincode: new FormControl(null),
                latitude: new FormControl(0),
                longitude: new FormControl(0),
            }),
        }, {
            validators: this.validator.passwordMatchValidator
        });

        form.get('Password')?.valueChanges.subscribe(() => {
            form.updateValueAndValidity({ onlySelf: false });
        });

        form.get('ConfirmPassword')?.valueChanges.subscribe(() => {
            form.updateValueAndValidity({ onlySelf: false });
        });

        return form;
    }

    // Edit User Form
    initEditForm(): FormGroup {
        return this.fb.group({
            firstname: new FormControl(null, [Validators.required, Validators.pattern('^[a-zA-Z\s]*$')]),
            lastname: new FormControl(null, [Validators.required, Validators.pattern('^[a-zA-Z\s]*$')]),
            mobile: new FormControl(null, [Validators.required]),
            Role: new FormControl(null, [Validators.required]),
            email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
            gender: new FormControl(null),
            dateofBirth: new FormControl(null, this.dateRangeValidator.bind(this)),
            pharmacyId: new FormControl(this.pharmacyId),
            address: this.fb.group({
                addressline: new FormControl(null),
                countryId: new FormControl(),
                stateId: new FormControl(),
                cityId: new FormControl(),
                pincode: new FormControl(null),
                latitude: new FormControl(0),
                longitude: new FormControl(0),
            })
        });
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


    selectedCountry: number = 0;
    onCountryDropdownChange(event: any) {
        const selectedValue = event.target.value;
        this.selectedCountry = selectedValue ? Number(selectedValue) : 0;

        this.AdminCommonServices.getstatebyId(this.selectedCountry).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.states = response.data;
            } else {
                console.error("Failed to load country data", response);
            }
        });
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

    onSubmit() {
        if (this.userId && this.userId !== 0) {
            if (this.editstaffForm.invalid) {
                this.validator.markInvalidFieldsTouched(this.editstaffForm);
                return;
            }
            this.StaffService.edituser(this.editstaffForm.value, this.userId).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Staff Updated successfully.');
                        this.router.navigate(['/pharmacy/staff/list']);
                    } else {
                        console.error('API returned isSuccess: false');
                        this.errorMessage = response.message || 'Failed to add user.';
                        Helper.ShowError(this.errorMessage);  // Optional toast/popup
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                    Helper.ShowError(this.errorMessage);  // Optional toast/popup
                }
            })
            // if (false) {
            //   this.validator.markInvalidFieldsTouched(this.editUserForm);
            //   return;
            // }
        } else {
            if (this.addStaffForm.invalid) {
                this.validator.markInvalidFieldsTouched(this.addStaffForm);
                return;
            }
            const dto: AddPersonDto = {
                ...this.addStaffForm.value,  // Preserve the other values
                pharmacyId: this.pharmacyId   // Add the pharmacyId
            };
            this.StaffService.add(dto).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Staff added successfully.');
                        this.router.navigate(['/pharmacy/staff/list']);
                    } else {
                        console.error('API returned isSuccess: false');
                        this.errorMessage = response.message || 'Failed to add user.';
                        Helper.ShowError(this.errorMessage);  // Optional toast/popup
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                    Helper.ShowError(this.errorMessage);  // Optional toast/popup
                }
            });
        }

    }

    disablePaste(event: ClipboardEvent): void {
        event.preventDefault();
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
}