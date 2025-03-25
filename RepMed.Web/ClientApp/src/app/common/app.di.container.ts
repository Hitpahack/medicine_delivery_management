import { Injector } from '@angular/core';

export class APP_DI_CONTAINER {

    private static INJECTOR: Injector;
    static setInjector(injector: Injector) 
    {
        APP_DI_CONTAINER.INJECTOR = injector;
    }
    static getInjector(): Injector { return APP_DI_CONTAINER.INJECTOR; }

}  