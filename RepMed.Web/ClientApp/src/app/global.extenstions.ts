import { AbstractControl } from "@angular/forms";

String.prototype.getErrorMessage = function name(this: string, control: AbstractControl): string {

    if ((control.status === "INVALID" || control.touched) && control.errors) {
        if (control.errors.required)
            return "This field is required";

        if (control.errors.minlength)
            return "This field required min char is:" + control.errors.minlength.requiredLength;

        if (control.errors.email)
            return control.errors.error;

        if (control.errors.maxlength)
            return "This field allow max char is:" + control.errors.maxlength.requiredLength;

        if (control.errors.pattern)
            return "This field has invalid pattern:";

        if (control.errors.min)
            return "This field required min " + control.errors.min.min + "value";

        if (control.errors.max)
            return "This field value should be less then " + control.errors.max.max;

        if (control.errors.requiredtrue)
            return "This field should be checked";

        if (control.errors.mustmatch)
            return control.errors.error;

        if (control.errors.degit)
            return control.errors.error;

        if (control.errors.isEmailExist)
            return control.errors.error;

        if (control.errors.CompareDateRange)
            return control.errors.error;

        if (control.errors.CustomError)
            return control.errors.error;

        if (control.errors.empty) {

            return control.errors.error;
        }

        if (control.errors.cannotContainSpace)
            return control.errors.error;

        if (control.errors.DOB)
            return control.errors.error;

        if (control.errors.mobile)
            return control.errors.error;

        if (control.errors.password)
            return control.errors.error;

        if (control.errors.whitespace)
            return control.errors.error;

        if (control.errors.formate)
            return control.errors.error;

        if (control.errors.maxfilesize) {
            return control.errors.error;
        }
        if (control.errors.guid) {
            return control.errors.error;
        }
        if (control.errors.isAcronymExist) {
            return control.errors.error;
        }
        if (control.errors.minLength) {
            return control.errors.error;
        }
        if (control.errors.maxLength) {
            return control.errors.error;
        }
        if (control.errors.greatherZiro) {
            return control.errors.error;
        }
        if (control.errors.CompareTimeRange) {
            return control.errors.error;
        }
        if (control.errors[0] && control.errors[0].maxfilesize) {
            return control.errors[0].error;
        }
        if (control.errors.passusernamematch) {
            return control.errors.error;
        }
        if (control.errors.isEnrolledOnMsTeam)
            return control.errors.error;

    }
    return "";
}



export { };