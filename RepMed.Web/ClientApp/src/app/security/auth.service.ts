import { Injectable } from "@angular/core";
import { SideNav } from "./sideNav";

@Injectable({ providedIn: 'root' })
export class AuthService {

    isLoggedIn(): boolean {
        const token = localStorage.getItem('token');
        return !!token;  //when token available return true..
    }

    getUserModules(): string[] {
        let roles: string[];
        roles = [];
        var obj = JSON.parse(sessionStorage.getItem('accessData'));
        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                roles.push(key);
            }
        }

        return roles || [];
    }

    getSideBarLinks(): SideNav[] {
        var obj = JSON.parse(sessionStorage.getItem('accessData'));
        let linkObj = [];
        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                const value = obj[key];
                //linkObj.push(new SideNav(key))            
                //when has child links
                if (value['route'] == null && value['children'] != null && value['children'].length > 0) {

                    let linkInnObj = [];
                    for (let i = 0; i < value['children'].length; i++) {
                        linkInnObj.push(new SideNav(value['children'][i].label, key, value['children'][i].route));
                    }
                    linkObj.push(new SideNav(value['label'], key, value['route'], linkInnObj));
                }
                else if (value['route'] != null && value['children'] == null || value['children'].length === 0) {
                    linkObj.push(new SideNav(value['label'], key, value['route']));
                }
            }
        }

        return linkObj || [];
    }
}