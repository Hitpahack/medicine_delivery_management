import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterModule, Router } from '@angular/router';

@Component({
    selector: 'app-not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.css']
})

export class NotFoundComponent implements OnInit {
    constructor(private renderer: Renderer2, private router: Router) { }

     ngOnInit() {
        
      }
}