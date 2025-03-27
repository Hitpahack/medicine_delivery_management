import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
    templateUrl: './example.component.html',
})

export class AdminBaseComponent implements OnInit 
{
    _router:Router;

    public onFormSubmit: EventEmitter<any> = new EventEmitter<any>();
    public onFormSuccess: EventEmitter<any> = new EventEmitter<any>();
    public formInProgress: boolean;

    constructor(public router: Router, public fb: FormBuilder) {
        this._router = router;
    }

    ngOnInit(): void {}

    validateForm(form:FormGroup):boolean {

        for (const key in form.controls) {
            if (form.controls.hasOwnProperty(key)) {
                const control: FormControl = <FormControl>form.controls[key];
                control.updateValueAndValidity();
            }
        }
        return form.valid
    }

    ngEventListioner():void{
       
        if (this.onFormSubmit) {
            this.onFormSubmit.subscribe((res) => {
                this.formInProgress = true;
                
            })
    
        }

        if (this.onFormSuccess) {
            this.onFormSuccess.subscribe((res) => {
                this.formInProgress = false;
                if (res) {
                    if (res["dialog"]) {
                        res["dialog"].closeAll();
                    }
                    
                }

                
            })
        }
    }
}

