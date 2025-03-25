import { Directive, ElementRef } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, NgControl } from '@angular/forms';

@Directive({
   //selector: '[formControl], [formControlName]'
   selector: '[validator]'
})
export class FormControlErrorsDirective {
    

    constructor(
        private controlDir: NgControl,
        private host: ElementRef<HTMLFormElement>) { 
    }
    ngOnInit() {
        
        //console.log(this.host.nativeElement);
        
        this.controlDir.control.statusChanges.subscribe((s:string)=>{
            let ctr = this.controlDir.control as AbstractControl
            let mainDiv = this.host.nativeElement.parentNode as HTMLFormElement;
            let lbl:any = this.host.nativeElement.nextElementSibling;
            if(!ctr.valid){
                var errorMessage = s.getErrorMessage(ctr);
                
                mainDiv.classList.add('invalid-control');
                if( lbl && lbl.classList.contains('control-lbl-error') ){
                    lbl.textContent = errorMessage;
                }
                else {
                    lbl = document.createElement('label');
                    lbl.textContent = errorMessage;
                    lbl.setAttribute('class', 'error control-lbl-error');
                    this.host.nativeElement.parentNode.insertBefore(lbl, this.host.nativeElement.nextSibling);
                }
            }
            else{
                mainDiv.classList.remove('invalid-control');
                if( lbl && lbl.classList.contains('control-lbl-error') )
                lbl.remove();
            }
            
        })
    }
    
}

