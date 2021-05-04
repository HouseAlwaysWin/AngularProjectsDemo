import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { catchError, map, switchMap } from "rxjs/operators";
import { IApiResponse } from "src/app/models/apiResponse";
import { IDeliveryMethod } from "src/app/models/deliveryMethod";
import { environment } from "src/environments/environment";
import * as CheckoutActions from "./checkout.actions";


@Injectable()
export class CheckoutEffects {
  baseUrl = environment.apiUrl;

  constructor(
    private action$: Actions,
    private http: HttpClient,
  ) {
  }

  getDeliveryMethod = createEffect(() => this.action$.pipe(
    ofType(CheckoutActions.GetDeliveryMethod),
    switchMap(() => {
      return this.http.get(this.baseUrl + 'orders/deliveryMethods')
        .pipe(
          map((dm: IApiResponse<IDeliveryMethod[]>) => {
            return CheckoutActions.GetDeliveryMethodSuccess(dm);
          }),
          catchError(error => {
            return of(CheckoutActions.FailedAction({ error }));
          })
        );
    })
  ))

}
