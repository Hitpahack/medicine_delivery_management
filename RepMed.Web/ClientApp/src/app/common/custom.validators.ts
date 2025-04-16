import { Injectable } from "@angular/core";
import { AbstractControl, FormGroup, ValidationErrors } from "@angular/forms";

@Injectable({
  providedIn: 'root',
})
export class CustomValidator {
  constructor() { }

  public ValidateEmail(control: AbstractControl) {
    if (!control.value || control.value.length == 0) {
      return null;
    }

    let regularExp = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (control.value !== undefined && !regularExp.test(control.value.trim())) {
      return { email: true, error: "invalid email address entered!" };
    }
    return null;
  }

  public futureDateValidator(control: AbstractControl): { [key: string]: any } | null {
    const selectedDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0); // remove time part

    if (selectedDate < today) {
      return { pastDate: true };
    }
    return null;
  }

  public futureDateValidator_old(control: AbstractControl): { [key: string]: any } | null {
    const selectedDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0); // remove time part

    if (selectedDate > today) {
      return { futureDate: true };
    }
    return null;
  }

  public pastDateOnly(control: AbstractControl): { [key: string]: any } | null {
    const dob = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0); // remove time part
  
    if (dob > today) {
      return { futureDate: true };
    }
    return null;
  }

  public markInvalidFieldsTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control && control.invalid) {
        control.markAsTouched({ onlySelf: true });
      }
    });
  }

  passwordMatchValidator(control: AbstractControl) {
    const password = control.get('Password')?.value;
    const confirmpassword = control.get('ConfirmPassword')?.value;
    return password === confirmpassword ? null : { passwordMismatch: true };
  }
  
}