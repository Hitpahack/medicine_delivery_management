import { environment } from '../../../src/environments/environment';

export const admin_apiconfig = 
{
    requestSettings: {
      header: {
          // 'Content-Type': 'application/json',
          //'Access-Control-Allow-Origin': '*',
          //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
          'Accept-Language':'en'
      },
      
      headerFormData: {
          'Content-Disposition': 'multipart/form-data',
          'Accept-Language':'en'
          //'Access-Control-Allow-Origin': '*',
          //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
      },


    },
    endpoints:{
      accounts: {
          login: environment.admin_apiv1 + '/Accounts/login',

      },
    }
}
