import { get } from 'jquery';
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

    pharmacy: {
      add: (id: number) => environment.admin_apiv1 + '/pharmacy/addeditpharmacy/' + id,
      getPaged: environment.admin_apiv1 + '/pharmacy/getpharmacies/',
      get: (id: number) => environment.admin_apiv1 + '/pharmacy/get/' + id,

    },
    user: {
      add: (id: number) => environment.admin_apiv1 + '/users/addedituser/' + id,
      get: (id: number) => environment.admin_apiv1 + '/users/get/' + id,
      update: (id: number) => environment.admin_apiv1 + '/users/addedituser/' + id,
      setpassword: environment.admin_apiv1 + '/users/change-password',
      userroles: 'https://localhost:44379/api/v1/masters/getroles',
      changepassword: environment.admin_apiv1 +'/users/SetPassword'
    }
  }
}
