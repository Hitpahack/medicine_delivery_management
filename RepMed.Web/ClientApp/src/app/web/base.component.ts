import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    
})

export class BaseComponent implements OnInit 
{
    _router:Router;
    constructor(public router: Router) {
        this._router = router;
    }

    ngOnInit(): void {
        
    }
    
}

