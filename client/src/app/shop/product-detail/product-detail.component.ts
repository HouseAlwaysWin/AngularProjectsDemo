import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatRadioChange } from '@angular/material/radio';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem, IBasketTotals } from 'src/app/models/basket';
import { IProduct, IProductAttributeValue } from 'src/app/models/product';
import { ShopService } from '../shop.service';
import * as appReducer from '../../store/app.reducer';
import * as ShopActions from '../store/shop.actions';
import * as BasketActions from '../../basket/store/basket.actions';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: IProduct;
  totalPrice: number;
  quantity: number = 1;
  cartQuantity: number = 0;
  basket: IBasket;
  basketItem: IBasketItem;
  attrOptions = {};

  constructor(private activeRoute: ActivatedRoute,
    public translate: TranslateService,
    public dialog: MatDialog,
    private store: Store<appReducer.AppState>) { }
  ngOnInit(): void {
    this._getProductInfo();
    this.translate.onLangChange.subscribe(result => {
      this._getProductInfo();
    })
    this._getAndSetProductState();
    this._getAndSetBasketState();
  }




  onOptionChange(data: MatRadioChange, attrOptionKey: string) {
    let option: IProductAttributeValue = data.value;
    let preOption = this.attrOptions[attrOptionKey];

    this.totalPrice = (this.totalPrice - preOption.priceAdjustment) + option.priceAdjustment;
    this.attrOptions[attrOptionKey] = option;

    this._setSelectBasketState();

    this.quantity = 1;
  }

  incrementItemQuantity() {
    this.quantity++;
  }

  decrementItemQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  addItemToBasket() {
    let keyAndAttrs = this._getBasketKeyAndAttrs(this.attrOptions);
    const itemToAdd: IProduct = {
      id: this.product.id,
      name: `${this.product.name}_${keyAndAttrs.attrs}`,
      description: this.product.description,
      price: this.totalPrice,
      productAttributes: this.product.productAttributes,
      productCategory: this.product.productCategory,
      productPictures: this.product.productPictures
    };
    this.store.dispatch(BasketActions.AddProductToBasket({ productToAdd: itemToAdd, key: keyAndAttrs.key, quantity: this.quantity }));
    this._setSelectBasketState();
    this.quantity = 1;
  }

  private _getBasketKeyAndAttrs(attrOptions): { key: string, attrs: string } {
    let attrStr = '';
    for (var [key, value] of Object.entries(attrOptions)) {
      attrStr += `_${key}_${value['name']}`;
    }
    let basketItemKey = `${this.product.id}_${this.product.name}${attrStr}`
    return { key: basketItemKey, attrs: attrStr };
  }

  private _getAndSetProductState() {
    this.store.select('shop').subscribe(res => {
      this.product = res.product;
      if (this.product) {
        this._getDefaultTotalPrice();
      }
    })
  }

  private _getAndSetBasketState() {
    this.store.select('basket').subscribe(res => {
      this.basket = res.basket;
    })
  }

  private _setSelectBasketState() {
    let key = this._getBasketKeyAndAttrs(this.attrOptions).key;
    this.store.select('basket').subscribe(res => {
      if (res.basket) {
        let index = res.basket.basketItems.findIndex(i => i.id === key);
        if (index !== -1) {
          this.cartQuantity = res.basket.basketItems[index].quantity;
        }
        else {
          this.cartQuantity = 0;
        }
      }
    });
  }

  private _getProductInfo() {
    var id = this.activeRoute.snapshot.paramMap.get('id');
    this.store.dispatch(ShopActions.GetProductById({ id }));
  }

  private _getDefaultTotalPrice() {
    let optionsPrice = this.product.productAttributes.map(a => {
      this.attrOptions[a.name] = a.productAttributeValue[0];
      return a.productAttributeValue[0]
    }).reduce((pa, na) => pa + na.priceAdjustment, 0)

    this.totalPrice = this.product.price + optionsPrice;
  }


}
