import { environment } from '../../../src/environments/environment';

export const admin_apiconfig =
{
  requestSettings: {
    header: {
      'Content-Type': 'application/json',
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
      login: environment.admin_apiv1 + '/accounts/login',

      },

    addpharmacy: {
      add: (id: number) => environment.admin_apiv1 + '/pharmacy/addeditpharmacy/' + id,

    },
    user: {
      add: (id: number) => environment.admin_apiv1 + '/users/addedituser/' + id,
      get: (id: number) => environment.admin_apiv1 + '/users/' + id,
      update: (id: number) => environment.admin_apiv1 + '/users/addedituser/' + id,
      setpassword: (id: number) => environment.admin_apiv1 + '/users/set-password/' + id,

    }
  }
}
