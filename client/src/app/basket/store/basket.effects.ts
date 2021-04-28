import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { map, switchMap, tap, withLatestFrom } from "rxjs/operators";
import { IApiResponse } from "src/app/models/apiResponse";
import { IBasket } from "src/app/models/basket";
import { environment } from "src/environments/environment";
import { BasketService } from "../basket.service";
import * as BasketActions from '../store/basket.actions';
import * as appReducer from '../../store/app.reducer'
import { Store } from "@ngrx/store";

@Injectable()
export class BasketEffects {
  baseUrl = environment.apiUrl;

  constructor(
    private action$: Actions,
    private store: Store<appReducer.AppState>,
    private http: HttpClient,
    private basketService: BasketService) {
  }

  getBasketById = createEffect(() =>
    this.action$.pipe(
      ofType(BasketActions.GetBasketById),
      switchMap(action => {

        return this.http.get(this.baseUrl + 'basket?id=' + action.id)
          .pipe(
            map((res: IApiResponse<IBasket>) => {
              return BasketActions.GetBasetSuccess(res);
            }));
      })
    ))

  updateBasket = createEffect(() =>
    this.action$.pipe(
      ofType(
        BasketActions.UpdateBasket ||
        BasketActions.UpdateShipping ||
        BasketActions.UpdateOrAddBasketItem ||
        BasketActions.AddProductToBasket ||
        BasketActions.RemoveBasketItem ||
        BasketActions.IncrementItemQuantity ||
        BasketActions.DecrementItemQuantity),
      switchMap(action => {
        return this.http.post(this.baseUrl + 'basket/updateBasket', action)
          .pipe(
            map((res: IApiResponse<IBasket>) => {
              return BasketActions.UpdateBasketSuccess({ basket: res.data, basketTotal: null });
            })
          )
      })
    ))

  deleteBasket = createEffect(() =>
    this.action$.pipe(
      ofType(BasketActions.DELETEBASKET),
      withLatestFrom(this.store.select('basket')),
      switchMap(([action, state]) => {
        return this.http.delete(`${this.baseUrl}'basket/removeBasketItem/${state.basket.id}`).pipe(
          map(() => {
            localStorage.removeItem('basket_id');
            return BasketActions.DeleteBasket();
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
        return this.http.post(`${this.baseUrl}'basket/payments/${state.basket.id}`, {}).pipe(
          map((res: IApiResponse<IBasket>) => {
            return BasketActions.GetBasetSuccess(res)
          })
        );
      })
    ))



}

