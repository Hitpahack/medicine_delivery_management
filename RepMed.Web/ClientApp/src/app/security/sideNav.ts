export class SideNav {
    label: string;
    module?: string; // Used for access control
    route?: string;
    children?: SideNav[];

    constructor(_label: string, _module?: string, _route?: string, _children?: SideNav[]) {
        this.label = _label;
        this.module = _module;
        this.route = _route;
        this.children = _children;
    }
}