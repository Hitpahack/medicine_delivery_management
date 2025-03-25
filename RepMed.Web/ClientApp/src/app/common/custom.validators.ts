import { Injectable } from "@angular/core";
import { AbstractControl } from "@angular/forms";

@Injectable({
    providedIn: 'root',
  })
  export class CustomValidator {
    constructor() {}

    public ValidateEmail(control: AbstractControl) {
       
        if (!control.value || control.value.length == 0) {
          return null;
        }

        let regularExp = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if ( control.value !== undefined && !regularExp.test(control.value.trim()) ) {
          return {  email: true, error: "invalid email address entered!" };
        }
        return null;
      }
}