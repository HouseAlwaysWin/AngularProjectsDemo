// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apihost: 'https://localhost:5100',
  apiUrl: 'https://localhost:5100/api/',
  firebaseConfig: {
    apiKey: "AIzaSyBpisra8VJh78OHZWFyf9nslbCAUV2lUeY",
    authDomain: "ecommerce-demo-ff8b1.firebaseapp.com",
    projectId: "ecommerce-demo-ff8b1",
    storageBucket: "ecommerce-demo-ff8b1.appspot.com",
    messagingSenderId: "1009208781701",
    appId: "1:1009208781701:web:1ee300cabff46cc3e8f2ec",
    measurementId: "G-TJK83XCEKN"
  },
  stripeKey: "pk_test_51II6tAHjD7oTH7SYPp6sRD0OhA89NWpAdOSz4hmVpao82ShdycNurUW8zNRXMD7Cd5NB32yeMoAHBs2eIyS0lZgY00UDIoXm6P"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
