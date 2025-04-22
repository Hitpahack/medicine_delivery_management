import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators, AbstractControl } from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../common/helper.extenstions";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";
import { AdminPharmacyService } from "../../services/pharmacy/admin.pharmacy.services";
import { PharmacyDto } from '../../../viewmodels/pharmacy/Pharmacy.add.dto';
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { CountryDto } from "../../../viewmodels/address/country.dto";
import { StateDto } from "../../../viewmodels/address/state.dto";
import { CityDto } from "../../../viewmodels/address/city.dto";
import { AdminCommonServices } from '../../services/Common/admin.commonservices';
import { AfterViewInit } from '@angular/core';
declare const window: any;

@Component({
    selector: 'app-admin-addpharmacy',
    templateUrl: './add.component.html',
    styleUrls: ['./add.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})
export class AdminAddPharmacyComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(
        public validator: CustomValidator,
        public accountservice: adminAccountsService,
        public PharmacyService: AdminPharmacyService,
        public AdminCommonServices: AdminCommonServices
    ) {
        super();
    }
    minExpiryDate: string = '';
    phForm: FormGroup;
    PharmacyDto: PharmacyDto;
    maxDate = new Date().toISOString().split('T')[0];
    errorMessage: string = '';
    countries: CountryDto[] = [];
    states: StateDto[] = [];
    cities: CityDto[] = [];

    ngAfterViewInit(): void {
        if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
          window.Helpers.initPasswordToggle();
        }
        $('.menu-toggle').on('click', function () {
          $(this).next('.menu-sub').slideToggle();
          $(this).parent().toggleClass('open');
        });
      }
    

    ngOnInit(): void {
        this.phForm = this.initForm();
        const today = new Date();
        this.minExpiryDate = today.toISOString().split('T')[0];
        this.AdminCommonServices.getcountry().subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.countries = response.data;
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

    initForm(): FormGroup {
        return this.fb.group({
            pharmacy: this.fb.group({
                ownerName: new FormControl(null, [Validators.required]),
                storeName: new FormControl(null, [Validators.required]),
                businessName: new FormControl(null, [Validators.required]),
                licenseNumber: new FormControl(null, [Validators.required, Validators.pattern(/^[A-Z]{2}-\d{2}[A-Z]-\d{5}$/)]),
                licenseExpiry: [null, [Validators.required, this.validator.futureDateValidator]],
                gstNumber: new FormControl(null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]),
                registeredMobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
                officialEmail: new FormControl(null, [Validators.required, Validators.email]),
                countryId: new FormControl(null),
                stateId: new FormControl(null),
                cityId: new FormControl(null),
            }),
            user: this.fb.group({
                //firstName: new FormControl(null, [Validators.required]),
                //lastName: new FormControl(null, [Validators.required]),
                email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
                //mobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
                //dateofBirth: new FormControl(null, [Validators.required, this.validator.pastDateOnly]),
                //gender: new FormControl(null, [Validators.required]),
                //picture: new FormControl(null, []),
                Password: new FormControl(null, [Validators.required, this.validator.validateStrongPassword]),
                Role: new FormControl("pharmacy"),
                ConfirmPassword: new FormControl(null, [Validators.required]),
            },
                {
                    validators: this.validator.passwordMatchValidator
                }
            ),
            pharmacyBankDetails: this.fb.group({
                bankName: new FormControl(null, [Validators.required]),
                accountholderName: new FormControl(null, [Validators.required]),
                accountNumber: new FormControl(null, [Validators.required, Validators.maxLength(16)]),
                ifscCode: new FormControl(null, [Validators.required, Validators.pattern(/^[A-Z]{4}0[A-Z0-9]{6}$/)]),
                branchName: new FormControl(null, [Validators.required]),
                upiId: new FormControl(null, [Validators.required, Validators.pattern(/^[a-zA-Z0-9.\-_]{2,256}@[a-zA-Z]{2,64}$/)])
            })
        });
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }

    disableKeyInput(event: KeyboardEvent): void {
        event.preventDefault();
      }
      
      disablePaste(event: ClipboardEvent): void {
        event.preventDefault();
      }

    onSubmit() {
        if (this.phForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.phForm);
            return;
        }
        const dto: PharmacyDto = this.phForm.value;
        this.PharmacyService.add(dto).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    console.log('Success:', response.data);
                    this.router.navigate(['/admin/pharmacy']);
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
