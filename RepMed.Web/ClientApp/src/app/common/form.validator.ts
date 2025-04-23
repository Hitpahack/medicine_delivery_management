import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { FormGroupDirective, AbstractControl, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appAutoValidate]',
})
export class AutoValidateDirective implements OnInit {
  private subscriptions: Subscription[] = [];

  constructor(
    private formGroupDir: FormGroupDirective,
    private el: ElementRef,
    private renderer: Renderer2
  ) {}

  ngOnInit() {
    const form = this.formGroupDir.form;
    const controls = this.el.nativeElement.querySelectorAll('[formControlName]');

    controls.forEach((controlEl: HTMLElement) => {
      const controlName = controlEl.getAttribute('formControlName');
      const control = this.findControlByName(form, controlName!);

      if (!control) return;

      const sub = control.valueChanges.subscribe(() => {
        if (control.touched || control.dirty) {
          this.showError(controlEl, control.errors);
        }
      });

      this.renderer.listen(controlEl, 'blur', () => {
        control?.markAsTouched();
        this.showError(controlEl, control?.errors);
      });

      this.subscriptions.push(sub);
    });
  }

  private findControlByName(form: AbstractControl, controlName: string): AbstractControl | null {
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

  private showError(el: HTMLElement, errors: any) {
    const parent = el.parentElement;
    this.removeOldError(el);

    const hasErrors = !!errors;
    if (hasErrors) {
      const errorMsg = this.getErrorMessage(errors);
      const div = this.renderer.createElement('div');
      this.renderer.setStyle(div, 'color', 'red');
      this.renderer.setStyle(div, 'fontSize', '0.8rem');
      this.renderer.setStyle(div, 'marginTop', '4px');
      this.renderer.addClass(div, 'error');
      const text = this.renderer.createText(errorMsg);
      this.renderer.appendChild(div, text);
  
      this.renderer.appendChild(parent, div);
      this.renderer.addClass(parent, 'has-error');
    } else {
      this.renderer.removeClass(parent, 'has-error');
    }
  }

  private removeOldError(el: HTMLElement) {
    const parent = el.parentElement;
    const oldMsg = Array.from(parent?.children || []).find(
      (child: any) => child.tagName === 'DIV' && child.innerText && child.style.color === 'red'
    );
    if (oldMsg) {
      this.renderer.removeChild(parent, oldMsg);
    }
  }

  private getErrorMessage(errors: any): string {
    if (!errors) return '';

    const messages: string[] = [];

    for (const errorKey of Object.keys(errors)) {
      const errorValue = errors[errorKey];

      switch (errorKey) {
        case 'required':
          messages.push('This field is required');
          break;
        case 'email':
         // messages.push('Invalid email address');
          break;
        case 'minlength':
          messages.push(`Minimum ${errorValue.requiredLength} characters required`);
          break;
        case 'maxlength':
          messages.push(`Maximum ${errorValue.requiredLength} characters allowed`);
          break;
        case 'pattern':
          messages.push('Invalid format');
          break;
        case 'min':
          messages.push(`Minimum value is ${errorValue.min}`);
          break;
        case 'max':
          messages.push(`Maximum value is ${errorValue.max}`);
              break;
          case 'date':
              messages.push(`Enter a valid date`);
              break;
        default:
          if (typeof errorValue === 'string') {
            messages.push(errorValue); // Allow custom validator to return string
          } else {
            messages.push(`${errorKey} validation failed`);
          }
      }
    }

    return messages.join(', ');
  }
}
