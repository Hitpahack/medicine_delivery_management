import { Injector } from '@angular/core';

export class APP_DI_CONTAINER {
  private static injector: Injector;

  static setInjector(injector: Injector) {
    console.log('Injector set ✅');
    APP_DI_CONTAINER.injector = injector;
  }

  static getInjector(): Injector {
    return APP_DI_CONTAINER.injector;
  }

  static get<T>(token: any): T {
    const inj = APP_DI_CONTAINER.injector;
    if (!inj) {
      throw new Error('Injector is not yet set.');
    }
    return inj.get<T>(token);
  }
}
