import { Component, InjectDecorator, Injector, OutputDecorator } from "@angular/core";
import { ActivatedRouteSnapshot, Router } from "@angular/router";
import { Subject } from "rxjs";
import { APP_DI_CONTAINER } from "../common/app.di.container";
import { AuthGuard } from "./authenticate.guard";
import { IDENTITY_HELPER } from "./identity.helper";



export function IsAuthrozed({ permission }): ClassDecorator {
    // Decorator Factory
    return (target: Function) => {
        const router = APP_DI_CONTAINER.getInjector().get(Router);
        const ngOnInit: Function = target.prototype.ngOnInit;
        var isAuthorized = true;
        target.prototype.ngOnInit = function (...args) {
            var activeAction = this.currentAction as Actions;
            //console.log('ngOnInit:', target.name);

            if (IDENTITY_HELPER.isLogedIn) {
                if (IDENTITY_HELPER.isMasterAdmin) {
                    if(ngOnInit)
                    ngOnInit.apply(this, args);
                }
                else {
                    APP_DI_CONTAINER.getInjector().get(adminUserService)
                        .GetRoleInfo(IDENTITY_HELPER.getUserId).subscribe((res) => {
                            if (res.success) {
                                IDENTITY_HELPER.setUserRolesPermission(res.data.roles);

                                var rolPermission = res.data.roles.map(s => s.permissions)
                                const result = rolPermission.reduce((accumulator, value) => accumulator.concat(value), []).filter(s => s?.id);
                                if (result.length > 0) {

                                    if (!result.map(s => s.permission).includes(permission.trim().toLowerCase())) {
                                        isAuthorized = false;

                                    }
                                    else {

                                            var acts = result.find(s => s.permission.trim().toLowerCase() === permission.trim().toLowerCase());
                                            if (acts) {
                                                switch (activeAction) {
                                                    case Actions.canAdd:
                                                        isAuthorized = acts.canAdd ?? false;
                                                        break;

                                                    case Actions.canEdit:
                                                        isAuthorized = acts.canEdit ?? false;
                                                        break;

                                                    case Actions.canDelete:
                                                        isAuthorized = acts.canDelete ?? false;
                                                        break;

                                                    case Actions.canListing:
                                                        isAuthorized = acts.canListing ?? false;
                                                        break;
                                                        case Actions.canDetail:
                                                            isAuthorized = acts.canDetail ?? false;
                                                            break;
                                                }
                                            }
                                            else {
                                                isAuthorized = false;
                                            }


                                    }
                                }
                                else {
                                    isAuthorized = false;
                                }
                                if (!isAuthorized) {
                                    //if (this.dialog)
                                        //APP_DI_CONTAINER.CloseDialog(this.dialog);

                                    router.navigate(['/unauthorized'], {
                                        queryParams: {
                                            returnUrl: "/dashboard",
                                            message: "You don't have permission to access this!",
                                            title: "Permission Denied!"
                                            // reqData: "lockeduser by Admin"
                                        }

                                    });
                                }
                            }
                        },
                            (err) => {
                                // APP_DI_CONTAINER.getInjector().get(ToastrService)
                                //     .warning(err?.error?.message ? 'Permission Error! ' + err.error.message : 'Permission Error! ' + err?.error ? err.error : 'Permission Error!');
                            },
                            () => {

                                if (isAuthorized && ngOnInit) {
                                    ngOnInit.apply(this, args);
                                }
                            })

                }
            }
            else {
                if (this.dialog)
                   // AppCustomJs.CloseDialog(this.dialog);

                router.navigate(['/login']);
            }

        };
    };
}


export enum Actions {
    canEdit = 1,
    canAdd = 2,
    canDelete = 3,
    canListing = 4,
    canDetail = 5
}
