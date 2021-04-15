import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/apiResponse';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../models/basket';
import { IDeliveryMethod } from '../models/deliveryMethod';
import { IProduct } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(new Basket());
  basket$ = this.basketSource.asObservable();
  private basketTotalsSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotals$ = this.basketTotalsSource.asObservable();
  shipping = 0;

  constructor(
    private http: HttpClient,
    private translate: TranslateService) { }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (!basketId) {
      this.createBasket();
    }
    this.getBasket(basketId);
  }

  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .subscribe((res: IApiResponse<IBasket>) => {
        if (res.isSuccessed) {
          this.basketSource.next(res.data);
          this.calculateTotal();
        }
        else {
          console.log(res.message);
        }
      });
  }

  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket/updateBasket', basket)
      .subscribe((res: IApiResponse<IBasket>) => {
        if (res.isSuccessed) {
          this.basketSource.next(res.data);
          this.calculateTotal();
        }
        else {
          console.log(res.message);
        }
      }, error => {
        console.log(error);
      });
  }

  setShipping(dm: IDeliveryMethod) {
    this.shipping = dm.price;
    const basket = this.getCurrentBasketValue();
    basket.deliveryMethodId = dm.id;
    basket.shippingPrice = dm.price;
    this.calculateTotal();
    this.setBasket(basket);
  }

  addBasketItem(basketItem: IBasketItem) {
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    return this.http.post(this.baseUrl + 'basket/basketItemQuantity', {
      basketId: basket.id,
      basketItem: basketItem
    }).subscribe((res: IApiResponse<IBasket>) => {
      if (res.isSuccessed) {
        this.basketSource.next(res.data);
        this.calculateTotal();
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
    basket.basketItems = this.addOrUpdateItem(
      basket.basketItems, productItems, quantity
    );
    this.setBasket(basket);
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
            this.calculateTotal();
          }
          else {
            console.log(res.message);
          }
        });
    }
    else {
      this.basketSource.next(new Basket());
      this.calculateTotal();
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
    item.quantity--;
    if (item.quantity <= 0) {
      this.removeBasketItem(item);
      return;
    }
    this.addBasketItem(item);
  }

  deleteLocalBasket() {
    this.basketSource.next(new Basket());
    this.basketTotalsSource.next(null);
    localStorage.removeItem('basket_id');
  }

  createPaymentIntent() {
    return this.http.post(this.baseUrl + `payments/${this.getCurrentBasketValue().id}`, {})
      .pipe(map((res: IApiResponse<IBasket>) => {
        if (res.isSuccessed) {
          this.basketSource.next(res.data);
        }
      }));
  }


  private calculateTotal() {
    var basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.basketItems.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalsSource.next({ shipping, total, subtotal });
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
      imgUrl: item.productPictures[0].urlPath,
      description: item.description,
      productCategoryName: item.productCategory.name,
      quantity,
    }
  }
}
