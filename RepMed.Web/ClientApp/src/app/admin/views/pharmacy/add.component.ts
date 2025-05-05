import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
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
        public AdminCommonServices: AdminCommonServices,
        private route: ActivatedRoute
    ) {
        super();
    }
    isReadonly: boolean;

    minExpiryDate!: string;
    maxExpiryDate!: string;

    phForm: FormGroup;
    AddEditPhForm: FormGroup;

    PharmacyDto: PharmacyDto;
    maxDate = new Date().toISOString().split('T')[0];
    errorMessage: string = '';
    countries: CountryDto[] = [];
    states: StateDto[] = [];
    cities: CityDto[] = [];
    uniqueId = '';
    ngAfterViewInit(): void {
        if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
            window.Helpers.initPasswordToggle();
        }
    }

    dateMethod(dateString: Date): string {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    pharmacyId: number;
    ngOnInit(): void {
        this.pharmacyId = this.route.snapshot.params['id'];
        this.AddEditPhForm = this.initForm();

        this.uniqueId = Math.random().toString(36).substring(2);

        this.AdminCommonServices.getcountry().subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.countries = response.data;
            } else {
                console.error("Failed to load country data", response);
            }
        })

        if (this.pharmacyId && this.pharmacyId !== 0) {
            this.isReadonly = true;
            this.phForm = this.initForm();

            this.PharmacyService.getpharmacybyId(this.pharmacyId).subscribe((response: PharmacyDto) => {
                const userData = response.user;
                const pharmacyData = response.pharmacy;
                console.log("pharmacyData", pharmacyData);

                const pharmacyBankDetailsData = response.pharmacyBankDetails;
                this.AddEditPhForm.get('pharmacy').patchValue({
                    ownerName: pharmacyData.ownerName,
                    storeName: pharmacyData.storeName,
                    businessName: pharmacyData.businessName,
                    licenseNumber: pharmacyData.licenseNumber,
                    licenseExpiry: this.dateMethod(pharmacyData.licenseExpiry),
                    gstNumber: pharmacyData.gstnumber,
                    registeredMobile: pharmacyData.registeredMobile,
                    officialEmail: pharmacyData.officialEmail,
                    address1: pharmacyData.address1,
                    address2: pharmacyData.address2,
                    countryId: pharmacyData.countryId,
                    stateId: pharmacyData.stateId,
                    cityId: pharmacyData.cityId,

                })
                this.AddEditPhForm.get('user').patchValue({
                    email: userData.email,
                    gender: userData.gender,
                    picture: userData.picture,
                    mobile: userData.mobile,
                    firstName: userData.firstName,
                    lastName: userData.lastName
                })
                this.AddEditPhForm.get('pharmacyBankDetails').patchValue({
                    bankName: pharmacyBankDetailsData.bankName,
                    accountholderName: pharmacyBankDetailsData.accountHolderName,
                    accountNumber: pharmacyBankDetailsData.accountHolderName,
                    ifscCode: pharmacyBankDetailsData.ifsccode,
                    branchName: pharmacyBankDetailsData.branchName,
                    upiId: pharmacyBankDetailsData.upiId,
                })
            });
        } else {
            console.log("pharmacyId else section", this.pharmacyId);
            const today = new Date();
            const maxDate = new Date();
            this.minExpiryDate = today.toISOString().split('T')[0];
            maxDate.setFullYear(today.getFullYear() + 100);
            this.minExpiryDate = today.toISOString().split('T')[0];
            this.maxExpiryDate = maxDate.toISOString().split('T')[0];
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

    initForm(): FormGroup {
        return this.fb.group({
            pharmacy: this.fb.group({
                ownerName: new FormControl(null, [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)]),
                storeName: new FormControl(null, [Validators.required, Validators.pattern(/^[A-Za-z0-9\s]+$/)]),
                businessName: new FormControl(null, [Validators.required, Validators.pattern(/^[A-Za-z0-9\s]+$/)]),
                licenseNumber: new FormControl(null, [Validators.required, Validators.pattern('^[A-Za-z0-9]+$')]),
                licenseExpiry: [null, [Validators.required, this.validator.futureDateValidator]],
                gstNumber: new FormControl(null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]),
                registeredMobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
                officialEmail: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
                address1: new FormControl(null),
                address2: new FormControl(null),
                countryId: new FormControl(0),
                stateId: new FormControl(0),
                cityId: new FormControl(0),
                storeEmail2: new FormControl(null),
                storeEmail1: new FormControl(null),
                storeMobile1: new FormControl(null),
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
                accountholderName: new FormControl(null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]),
                accountNumber: new FormControl(null, [Validators.required, Validators.maxLength(16)]),
                ifscCode: new FormControl(null, [Validators.required, Validators.pattern('^[A-Za-z0-9]+$')]),
                branchName: new FormControl(null, [Validators.required]),
                upiId: new FormControl(null, [Validators.pattern(/^[a-zA-Z0-9.\-_]{2,256}@[a-zA-Z]{2,64}$/)])
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
        console.log("callonsubmit.");
        
        if (this.pharmacyId && this.pharmacyId !== 0) {
            console.log("inside log submit.");
            
            if (this.phForm.invalid) {
                this.validator.markInvalidFieldsTouched(this.phForm);
                //return;
            }

            this.PharmacyService.editpharmacy(this.AddEditPhForm.value, this.pharmacyId).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message);
                        this.router.navigate(['/admin/pharmacy']);
                    } else {
                        this.errorMessage = response.message;
                        Helper.ShowError(this.errorMessage);
                    }
                },
                error: (err) => {
                    this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                    Helper.ShowError(this.errorMessage);  // Optional toast/popup
                }
            })
        } else {
            if (this.AddEditPhForm.invalid) {
                this.validator.markInvalidFieldsTouched(this.AddEditPhForm);
                return;
            }
            console.log("pharmacyadd me.");

            const dto: PharmacyDto = this.AddEditPhForm.value;
            this.PharmacyService.add(dto).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Pharmacy added successfully.');
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
}
