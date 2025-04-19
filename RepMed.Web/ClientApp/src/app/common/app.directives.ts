import {
    Directive,
    ElementRef,
    Renderer2,
    OnInit
  } from '@angular/core';
  import {
    AbstractControl,
    NgControl
  } from '@angular/forms';
  
  @Directive({
    selector: '[validator]'
  })
  export class FormControlErrorsDirective implements OnInit {
  
    constructor(
      private controlDir: NgControl,
      private el: ElementRef,
      private renderer: Renderer2
    ) 
    {

    }
  
    ngOnInit(): void {debugger;
      const control = this.controlDir.control as AbstractControl;
  
      if (!control) return;
  
      control.statusChanges.subscribe(() => {
        const input = this.el.nativeElement;
        const parent = input.parentNode;
        const existingLabel = input.nextElementSibling;
  
        if (control.invalid && control.touched) {
          const message = this.getErrorMessage(control.errors);
  
          // Add red border
          this.renderer.addClass(parent, 'invalid-control');
  
          if (existingLabel && existingLabel.classList.contains('control-lbl-error')) {
            this.renderer.setProperty(existingLabel, 'textContent', message);
          } else {
            const errorLabel = this.renderer.createElement('label');
            this.renderer.setProperty(errorLabel, 'textContent', message);
            this.renderer.addClass(errorLabel, 'error');
            this.renderer.addClass(errorLabel, 'control-lbl-error');
            this.renderer.insertBefore(parent, errorLabel, input.nextSibling);
          }
        } else {
          this.renderer.removeClass(parent, 'invalid-control');
          if (existingLabel && existingLabel.classList.contains('control-lbl-error')) {
            this.renderer.removeChild(parent, existingLabel);
          }
        }
      });
    }
  
    private getErrorMessage(errors: any): string {
      if (!errors) return '';
  
      if (errors['required']) return 'This field is required';
      if (errors['email']) return 'Invalid email format';
      if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters`;
      if (errors['maxlength']) return `Maximum ${errors['maxlength'].requiredLength} characters`;
      if (errors['pattern']) return 'Invalid pattern';
  
      return 'Invalid input';
    }
  }
  