// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
    admin_apiv1: 'https://localhost:44379/api/v1/admin',
    admin_api: 'https://localhost:44379/api/v1',
  secureKey: "7e794937-f63c-493a-9e64-3a12f2cc3028",
  loginSession_Minute: 15,
}; 

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
