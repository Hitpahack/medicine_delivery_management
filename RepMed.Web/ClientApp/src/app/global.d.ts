import { AbstractControl } from "@angular/forms";

declare global 
{
    interface String {
        getErrorMessage(this: string, control: AbstractControl): string;
    }
    
}

export { };