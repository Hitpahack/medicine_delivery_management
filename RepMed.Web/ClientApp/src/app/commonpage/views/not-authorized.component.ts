import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterModule, Router } from '@angular/router';

@Component({
    selector: 'app-not-found',
    templateUrl: './not-authorized.component.html',
    styleUrls: ['./not-authorized.component.css']
})

export class NotAuthorizedComponent implements OnInit {
    constructor(private renderer: Renderer2, private router: Router) { }

    ngOnInit() {

    }
    //     goHome() {
    //         this.router.navigate(['/']);  // Change to your home or dashboard route
    //     }
}