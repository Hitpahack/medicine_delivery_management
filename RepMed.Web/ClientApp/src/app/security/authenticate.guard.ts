import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from "@angular/router";
import { Helper } from "../common/helper.extenstions";
import { Actions } from "./Authorized";
import { IDENTITY_HELPER, Roles } from "./identity.helper";


@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router
    ) { }

    private isAuthorized: boolean = false;
    private redirectoLocked: boolean = false;

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (IDENTITY_HELPER.isLogedIn) {
            IDENTITY_HELPER.getRoles?.map(role => {
                if (!route.data) {

                }


                if (route.data.roles && route.data.roles.includes(Roles.Locked)) {
                    this.redirectoLocked = true;
                    return true;
                }

                // if (role.isMasterAdmin) {
                //     this.isAuthorized = true;
                //     return true;
                // }

                // var rolPermission = role.permissions.filter(s => s?.id).map(s => s.permission.trim().toLowerCase());
                // if (rolPermission.length > 0) {
                //     if (route.data.permission && rolPermission.includes(route.data.permission.trim().toLowerCase())) {
                //         this.isAuthorized = true;
                //     }
                // }
                // else {
                //     this.isAuthorized = false;

                // }

            })

            if (!this.isAuthorized && !this.redirectoLocked) {
                this.router.navigate(['/unauthorized'], {
                    queryParams: {
                        returnUrl: "/dashboard",
                        message: "You don't have permission to access this!",
                        title: "Permission Denied!"
                        // reqData: "lockeduser by Admin"
                    }

                });
            }

        }
        else {

            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        }

        // if (!this.isAuthorized && !this.isAway) {
        //     // not logged in so redirect to login page with the return url
        //     this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        // }

        if (IDENTITY_HELPER.wasAway && !this.redirectoLocked) {

            IDENTITY_HELPER.setUserLock();

            this.router.navigate(['/locked'], { queryParams: { returnUrl: state.url } });
            return true;
        }

        return this.isAuthorized;

    }

    // public static hasPermission(permission: string, actions?: Actions): boolean {
    //     if (IDENTITY_HELPER.isLogedIn) {

    //         var isAuthorized = true;
    //         // if (IDENTITY_HELPER.isMasterAdmin) {
    //         //     isAuthorized = true;
    //         // }
    //         // else {

    //             var roles = IDENTITY_HELPER.getRoles;

    //             var rolPermission = roles.map(s => s.permission)
    //             const result = rolPermission.reduce((accumulator, value) => accumulator.concat(value), []).filter(s => s?.id);;
    //             if (result.length > 0) {

    //                 if (!result.map(s => s.permission).includes(permission.trim().toLowerCase())) {
    //                     isAuthorized = false;
    //                 }
    //                 else {

    //                     var acts = result.find(s => s.permission.trim().toLowerCase() === permission.trim().toLowerCase());
    //                     if (acts) {
    //                         switch (actions) {
    //                             case Actions.canAdd:
    //                                 isAuthorized = acts.canAdd ?? false;
    //                                 break;

    //                             case Actions.canEdit:
    //                                 isAuthorized = acts.canEdit ?? false;
    //                                 break;

    //                             case Actions.canDelete:
    //                                 isAuthorized = acts.canDelete ?? false;
    //                                 break;

    //                             case Actions.canListing:
    //                                 isAuthorized = acts.canListing ?? false;
    //                                 break;
    //                             case Actions.canDetail:
    //                                 isAuthorized = acts.canDetail ?? false;
    //                                 break;
    //                         }
    //                     }
    //                     else {
    //                         isAuthorized = false;
    //                     }

    //                 }

    //             }
    //             else { isAuthorized = false }

    //         //}
    //         if(!isAuthorized){
    //             Helper.ShowError("You don't have permission to do this!");
    //         }
    //         return isAuthorized;
    //     }
    //     else {
    //         return false;
    //     }

    // }

    // public static hasPermissionButton(permission: string, actions?: Actions): string {

    //     if (IdentityHelper.isLogedIn) {
    //         var isAuthorized = true;
    //         if (IdentityHelper.isMasterAdmin) {
    //             isAuthorized = true;
    //         }
    //         else {
    //             var roles = IdentityHelper.getRoles;
    //             var rolPermission = roles.map(s => s.permissions)
    //             const result = rolPermission.reduce((accumulator, value) => accumulator.concat(value), []).filter(s => s?.id);

    //             if (result.length > 0) {
    //                 if (!result.map(s => s.permission).includes(permission.trim().toLowerCase())) {
    //                     isAuthorized = false;

    //                 }
    //                 else {

    //                     var acts = result.find(s => s.permission.trim().toLowerCase() === permission.trim().toLowerCase());
    //                     if (acts) {
    //                         switch (actions) {
    //                             case Actions.canAdd:
    //                                 isAuthorized = acts.canAdd ?? false;
    //                                 break;

    //                             case Actions.canEdit:
    //                                 isAuthorized = acts.canEdit ?? false;
    //                                 break;

    //                             case Actions.canDelete:
    //                                 isAuthorized = acts.canDelete ?? false;
    //                                 break;

    //                             case Actions.canListing:
    //                                 isAuthorized = acts.canListing ?? false;
    //                                 break;
    //                             case Actions.canDetail:
    //                                 isAuthorized = acts.canDetail ?? false;
    //                                 break;
    //                         }
    //                     }
    //                     else {
    //                         isAuthorized = false;
    //                     }

    //                 }
    //             }
    //             else {
    //                 isAuthorized = false;
    //             }
    //         }
    //     }
    //     else {
    //         isAuthorized = false;
    //     }


    //     if (!isAuthorized)
    //         return ' data-action="' + isAuthorized + '" disabled="' + !isAuthorized + '" data-ispermit="' + isAuthorized + '" ';
    //     else
    //         //return '';
    //         return ' '; // Govind saini: '' and ' ' are diffrent thing
    // }

}

