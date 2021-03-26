import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/apiResponse';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../models/basket';
import { IProduct } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalsSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotals$ = this.basketTotalsSource.asObservable();
  shipping = 0;

  constructor(
    private http: HttpClient,
    private translate: TranslateService) { }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.getBasket(basketId);
    }
  }

  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .subscribe((res: IApiResponse<IBasket>) => {
        if (res.isSuccessed) {
          this.basketSource.next(res.data);
        }
        else {
          console.log(res.message);
        }
      });
  }

  addBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket/updateBasket', basket)
      .subscribe((res: IApiResponse<IBasket>) => {
        if (res.isSuccessed) {
          this.basketSource.next(res.data);
        }
        else {
          console.log(res.message);
        }
      }, error => {
        console.log(error);
      });
  }

  addBasketItem(basketItem: IBasketItem) {
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    return this.http.post(this.baseUrl + 'basket/basketItemQuantity', {
      basketId: basket.id,
      basketItem: basketItem
    }).subscribe((res: IApiResponse<IBasket>) => {
      if (res.isSuccessed) {
        this.basketSource.next(res.data);
      }
      else {
        console.log(res.message);
      }
    });
  }

  getCurrentBasketValue(): IBasket {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const productItems: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    console.log(basket);
    basket.basketItems = this.addOrUpdateItem(
      basket.basketItems, productItems, quantity
    );
    this.addBasket(basket);
  }

  addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity
    }
    console.log(items);
    return items;
  }

  removeBasketItem(basketItem: IBasketItem) {
    let basket = this.getCurrentBasketValue()

    if (basket.basketItems.length > 0) {
      let headers = new HttpHeaders({
        'Content-Type': 'text/json'
      });
      let body = JSON.stringify({
        basketId: basket.id,
        basketItem: basketItem
      });

      let options = {
        headers,
        body
      };

      return this.http.delete(this.baseUrl + 'basket/removeBasketItem', options)
        .subscribe((res: IApiResponse<IBasket>) => {
          console.log(res);
          if (res.isSuccessed) {
            this.basketSource.next(res.data);
          }
          else {
            console.log(res.message);
          }
        });
    }
    else {
      this.basketSource.next(null);
      localStorage.removeItem('basket_id');
    }
  }

  incrementItemQuantity(item: IBasketItem) {
    if (item.quantity < 100) {
      item.quantity++;
    }
    this.addBasketItem(item);
  }

  decrementItemQuantity(item: IBasketItem) {
    if (item.quantity > 0) {
      item.quantity--;
    }
    this.addBasketItem(item);
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();

  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    var langCode = this.translate.currentLang;
    return {
      id: item.id,
      name: item.name,
      price: item.price,
      imgUrl: item.imgUrl,
      description: item.description,
      quantity,
      langCode,
      productBrandId: item.productBrandId,
      productCategoryId: item.productCategoryId
    }
  }
}
