import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";
import { AuthService } from "./auth.service";

@Injectable({ providedIn: 'root' })
export class AuthGuardService implements CanActivate {
    constructor(private authService: AuthService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        debugger;
        //  Step 1: Check if user is logged in (token exists)
        if (!this.authService.isLoggedIn()) {
            this.router.navigate(['/login']);
            return false;
        }

        //  Step 2: Check module permissions (if needed)
        const requiredModule = route.data['module'];
        //var obj = JSON.parse(sessionStorage.getItem('accessData'));
        const allowedModules = this.authService.getUserModules();

        if (allowedModules.includes(requiredModule)) {
            return true;
        } else {
            this.router.navigate(['/not-authorized']);
            return false;
        }
    }
}