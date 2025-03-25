import { Router, RouterStateSnapshot } from "@angular/router";
import { UserInfo } from "os";
import { environment } from "src/environments/environment";
import { APP_DI_CONTAINER } from "../common/app.di.container";
import { BaseAccountsDto, LoginResponse } from "../viewmodels/accounts/base.accountsdto";
import { EntityRoleDto } from "../viewmodels/roles.dto";
import IdleTimer from "./idle.timer";
declare const CryptoJS: any;

export class Serilization<T> {
    public constructor(private tCtor: new () => T) { }
    public get Data(): string {

        return this.tCtor.name
    }
}

export class IDENTITY_HELPER {

    private static timer: any;
    private static _hubConnectionId: string;

    public static get isUserVerifid(): boolean {
        if (this.wasAway || this.isUserLocked)
            return false;

        return true;
    }
    public static updateSecureData(key: string, data: any) {

        this.removeSecureData(key);
        this.setSecureData(key, data);
    }

    public static setSecureData(key: string, data: any) {

        var secureData = CryptoJS.AES.encrypt(data, environment.secureKey);
        localStorage.setItem(key, secureData)
    }

    public static getSecureData(key: string): string {
        var encData = localStorage.getItem(key);

        if (encData) {

            var bytes = CryptoJS.AES.decrypt(encData, environment.secureKey);
            var plaintext = bytes.toString(CryptoJS.enc.Utf8) as string;
            return plaintext;

        }
        return "";

    }

    public static removeSecureData(key: string) {
        localStorage.removeItem(key);
    }

    public static get wasAway(): boolean {

        var islocked = this.identity.isLocked;
        return Boolean(islocked);
    }

    private static get isUserLocked() {
        return this.identity?.isLocked;
    };
    private static get identity() {
        var data = this.getSecureData("Identity");
        if (data) {

            return new LoginResponse(JSON.parse(data));

        }
        else {
            return new LoginResponse();
        }
    };

    public static setHubConnectionId(connectionId: string): void {

        this._hubConnectionId = connectionId;
    }
    public static setUserLock(): void {

        var neData = this.identity;
        neData.isLocked = true;
        IDENTITY_HELPER.updateSecureData("Identity", JSON.stringify(neData))
    }
    public static setUserRolesPermission(roles:EntityRoleDto[]): void {

        var neData = this.identity;
        neData.roles = roles;
        IDENTITY_HELPER.updateSecureData("Identity", JSON.stringify(neData))
    }

    public static get isLogedIn(): boolean {

        var tokenDate = new Date(this.identity?.token?.tokenValidTill);
        var todaydate = new Date();

        if (tokenDate < todaydate) {
            this.LogOut();
            return false;
        }

        return this.identity.email ? true : false;
    }

    public static get getHubConnectionId(): string {

        return this._hubConnectionId || "";
    }

    public static get getCurrentUser(): BaseAccountsDto {

        return this.identity;
    }

    public static get getToken(): string {
        return this.identity?.token.token;
    }

    public static get getRoles(): EntityRoleDto[] {

        return this.identity?.roles;
    }
    // public static get isMasterAdmin():boolean{
    //     return this.getRoles.filter(s=>s.isMasterAdmin).length>0;
    // }

    public static get getEmail(): string {

        return this.identity?.email;
    }
    public static get getFullName(): string {

        return this.identity?.firstName + " " + this.identity?.lastName;
    }
    public static get getPicture(): string {
       // return this.identity?.picture?.fileUrl;
       return "";
    }
    public static get getUserName(): string {

        return this.identity?.email.split('@')[0];
    }

    public static get getUserId(): string {

        return this.identity?.id;
    }
   
    public static get IsEmailConfirmed(): boolean {

        return this.identity?.emailConfirmed;
    }

    public static isInRole(role: string): boolean {

        return this.identity?.roles.map(r => r.name).includes(role);
    }
    public static logOutSession() {
        this.LogOut();
    }


    public static StartLoginTimer() {



        this.timer = new IdleTimer({
            timeout_minute: environment.loginSession_Minute, //expired after 10 secs
            onTimeout: () => {
                this.IdleTimeout()
            }
        })
    }

    // Reset timers.
    public static ResetLoginTimer() {
        this.timer = null;
        this.StartLoginTimer();

    }

    // Logout the user.
    public static IdleTimeout() {
        //alert("TimeOut");

        if (!this.wasAway) {
            var router = APP_DI_CONTAINER.getInjector().get(Router);
            var state = router.routerState.snapshot;
            this.setUserLock();
            router.navigate(['/locked'], { queryParams: { returnUrl: state.url } });
        }

        //window.location.href = this.logoutUrl;

    }
    public static LogOut(): void {
       //var acService = APP_DI_CONTAINER.getInjector().get(AccountService);
       //acService.logOut();
       //localStorage.clear();
       //var router = APP_DI_CONTAINER.getInjector().get(Router);
       //router.navigate(['/login'], { queryParams: { returnUrl: "/dashboard" } });
    }

    public static SetHeaderParma() {
        //appSetting.requestSettings.header["X-User"] = this.identity?.id;
    }
}



export enum Roles {
    Provider = "provider",
    Locked  = 'locked'
    
}

