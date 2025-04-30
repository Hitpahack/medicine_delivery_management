import { Injectable } from "@angular/core";
import { AbstractControl, FormArray, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

@Injectable({
  providedIn: 'root',
})
export class CustomValidator {
  constructor() { }

  public ValidateEmail(control: AbstractControl) {
    if (!control.value || control.value.length == 0) {
      return null;
    }

    let regularExp = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
    if (control.value !== undefined && !regularExp.test(control.value.trim())) {
      return { email: true, error: "Enter a valid email using only lowercase letters, numbers, and allowed symbols before and after '@' (e.g., john.doe@example.com)." };
    }
    return null;
  }

  public forbidNameValidator(nameRe: RegExp): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const forbidden = nameRe.test(control.value);
      return forbidden ? { forbidden: { value: control.value } } : null;
    }
  }


  public futureDateValidator(control: AbstractControl): { [key: string]: any } | null {
    const value = control.value;
    if (!value) return null;

    const selectedDate = new Date(value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const maxAllowedDate = new Date();
    maxAllowedDate.setFullYear(today.getFullYear() + 100);
    maxAllowedDate.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
      return { pastDate: true };
    }

    if (selectedDate > maxAllowedDate) {
      return { tooFar: true };
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

  // public markInvalidFieldsTouched(formGroup: FormGroup) {
  //   Object.values(formGroup.controls).forEach(control => {
  //     control.markAsTouched();

  //     if ((control as FormGroup).controls) {
  //       Object.values((control as FormGroup).controls).forEach(nestedControl => {
  //         nestedControl.markAsTouched();
  //       });
  //     }
  //   });
  // }
  public markInvalidFieldsTouched(formGroup: FormGroup) {
    Object.entries(formGroup.controls).forEach(([controlName, control]) => {
      control.markAsTouched();

      // Try to focus-blur to trigger UI update
      const element = document.querySelector(`[formControlName="${controlName}"]`) as HTMLElement;
      if (element) {
        element.focus();
        setTimeout(() => element.blur(), 100); // small delay helps UI
      }

      // If nested FormGroup
      if (control instanceof FormGroup) {
        this.markInvalidFieldsTouched(control); // Recursively apply to nested controls
      }

      // If FormArray (optional)
      if (control instanceof FormArray) {
        control.controls.forEach(ctrl => {
          if (ctrl instanceof FormGroup) {
            this.markInvalidFieldsTouched(ctrl);
          } else {
            ctrl.markAsTouched();
          }
        });
      }
    });
  }

  passwordMatchValidator(control: AbstractControl) {
    const password = CustomValidator.findControlByName(control, 'Password');
    const confirmpassword = CustomValidator.findControlByName(control, 'ConfirmPassword');
    if (!password.value || !confirmpassword.value)
      return null;

    let error = null;
    if (password.value !== confirmpassword.value) {
      error = { passwordMismatch: true, error: "password and confirm password doesn't match!" };
      confirmpassword.setErrors(error);
      confirmpassword.markAsTouched();
      confirmpassword.markAsDirty();
    }


    return error;
  }

  validateStrongPassword(control: AbstractControl): ValidationErrors | null {
    const password = control.value;
    if (!password) return null;

    const strongPasswordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;

    return strongPasswordPattern.test(password)
      ? null
      : { strongPassword: 'Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.' };
  }
  static findControlByName(form: AbstractControl, controlName: string): AbstractControl | null {
    // If form is a FormGroup, search inside it
    if (form instanceof FormGroup) {
      // Check if the control is directly in this FormGroup
      if (form.contains(controlName)) {
        return form.get(controlName);
      }

      // Recursively search for nested FormGroups
      for (const controlKey of Object.keys(form.controls)) {
        const control = form.get(controlKey);
        if (control instanceof FormGroup) {
          const nestedControl = this.findControlByName(control, controlName);
          if (nestedControl) {
            return nestedControl;
          }
        }
      }
    }
    return null;
  }

  checkboxRequiredValidator(control: AbstractControl): ValidationErrors | null {
    console.log('checkbox validator called with value:', control.value);
    if (Array.isArray(control.value) && control.value.length === 0) {
      console.log('Returning checkboxRequired error');
      return { checkboxRequired: true };
    }
    return null;
  }

  static markInvalidFieldsTouched(formGroup: any) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      control.markAsTouched({ onlySelf: true });
    });
  }

  dateWithinRangeValidator(minDateStr: string, maxDateStr: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
  
      const dateValue = new Date(value);
      const minDate = new Date(minDateStr);
      const maxDate = new Date(maxDateStr);
  
      if (dateValue < minDate || dateValue > maxDate) {
        return { dateOutOfRange: true };
      }
      return null;
    };
  }


}