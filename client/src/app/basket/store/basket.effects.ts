import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, switchMap, tap, withLatestFrom } from "rxjs/operators";
import { IApiResponse } from "src/app/models/apiResponse";
import { Basket, BasketItem, IBasket, IBasketItem } from "src/app/models/basket";
import { environment } from "src/environments/environment";
import { BasketService } from "../basket.service";
import * as BasketActions from '../store/basket.actions';
import * as appReducer from '../../store/app.reducer'
import { Store } from "@ngrx/store";
import { of } from "rxjs";

@Injectable()
export class BasketEffects {
  baseUrl = environment.apiUrl;

  constructor(
    private action$: Actions,
    private store: Store<appReducer.AppState>,
    private http: HttpClient
  ) {
  }

  getBasket = createEffect(() =>
    this.action$.pipe(
      ofType(BasketActions.GetBasket),
      switchMap(() => {
        let basketId = localStorage.getItem('basket_id');
        if (!basketId) {
          basketId = this.createBasket().id;
        }
        return this.http.get(this.baseUrl + 'basket?id=' + basketId)
          .pipe(
            map((res: IApiResponse<IBasket>) => {
              return BasketActions.UpdateBasketSuccess({ basket: res.data });
            }),
            catchError(error => {
              return of(BasketActions.FailedAction({ error }));
            })
          );
      })
    ))

  updateBasket = createEffect(() =>
    this.action$.pipe(
      ofType(
        BasketActions.AddProductToBasket,
        BasketActions.UpdateBasket,
        BasketActions.UpdateShipping,
        BasketActions.UpdateOrAddBasketItem,
        BasketActions.RemoveBasketItem,
        BasketActions.IncrementItemQuantity,
        BasketActions.DecrementItemQuantity),
      withLatestFrom(this.store.select('basket')),
      switchMap(([action, state]) => {
        let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? this.createBasket();
        return this.http.post(this.baseUrl + 'basket/updateBasket', basket)
          .pipe(
            map((res: IApiResponse<IBasket>) => {
              return BasketActions.UpdateBasketSuccess({ basket: res.data });
            }),
            catchError(error => {
              return of(BasketActions.FailedAction({ error }));
            })
          )
      })
    ))


  deleteBasket = createEffect(() =>
    this.action$.pipe(
      ofType(BasketActions.DELETEBASKET),
      withLatestFrom(this.store.select('basket')),
      switchMap(([action, state]) => {
        console.log(state);
        return this.http.delete(`${this.baseUrl}basket/${state.basket.id}`).pipe(
          map(() => {
            let basketId = localStorage.getItem('basket_id');
            if (basketId) {
              localStorage.removeItem('basket_id');
            }
            return BasketActions.DeleteBasket();
          }),
          catchError(error => {
            return of(BasketActions.FailedAction({ error }));
          })
        );
      })
    ),
    { dispatch: false }
  );

  createPaymentIntent = createEffect(() =>
    this.action$.pipe(
      ofType(BasketActions.CreatePaymentIntent),
      withLatestFrom(this.store.select('basket')),
      switchMap(([action, state]) => {
        return this.http.post(`${this.baseUrl}payments/${state.basket.id}`, {})
          .pipe(
            map((res: IApiResponse<IBasket>) => {
              return BasketActions.CreatePaymentIntentSuccess(res.data)
            }),
            catchError(error => {
              return of(BasketActions.FailedAction({ error }));
            })
          );
      })
    ))


  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }


}

