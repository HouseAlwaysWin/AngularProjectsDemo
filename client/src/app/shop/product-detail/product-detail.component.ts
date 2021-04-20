import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatRadioChange } from '@angular/material/radio';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem, IBasketTotals } from 'src/app/models/basket';
import { IProduct, IProductAttributeValue } from 'src/app/models/product';
import { ShopService } from '../shop.service';

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
  productToBasket: IProduct;
  basket$: Observable<IBasket>;
  basketItem: IBasketItem;
  prevOptions = {};

  constructor(private activeRoute: ActivatedRoute,
    public translate: TranslateService,
    private basketService: BasketService,
    public dialog: MatDialog,
    private shopService: ShopService) { }
  ngOnInit(): void {
    this.getProductInfo();
    this.translate.onLangChange.subscribe(result => {
      this.getProductInfo();
    })
    this.basket$ = this.basketService.basket$;
  }

  getProductInfo() {
    var id = this.activeRoute.snapshot.paramMap.get('id');
    this.shopService.getProductById(id).subscribe(product => {
      this.product = product;
      console.log(this.product);
      this.getDefaultTotalPrice();
      this.basketItem = this.getCurrentBasketItem();

      if (this.basketItem) {
        this.cartQuantity = this.basketItem.quantity;
      }
    });
  }

  getDefaultTotalPrice() {
    let optionsPrice = this.product.productAttributes.map(a => {
      this.prevOptions[a.name] = a.productAttributeValue[0];
      return a.productAttributeValue[0]
    }).reduce((pa, na) => pa + na.priceAdjustment, 0)
    this.totalPrice = this.product.price + optionsPrice;
  }


  onOptionChange(data: MatRadioChange, prevOptionKey: string) {
    let option: IProductAttributeValue = data.value;
    let preOption = this.prevOptions[prevOptionKey];

    this.totalPrice = (this.totalPrice - preOption.priceAdjustment) + option.priceAdjustment;
    this.prevOptions[prevOptionKey] = option;

    this.basketItem = this.getCurrentBasketItem();

    if (this.basketItem) {
      this.cartQuantity = this.basketItem.quantity;
    }
    else {
      this.cartQuantity = 0;
    }
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
    let attrStr = this.getCurrentBasketItemAttr();

    const product: IProduct = {
      id: this.product.id,
      name: this.product.name,
      description: this.product.description,
      price: this.totalPrice,
      productAttributes: this.product.productAttributes,
      productCategory: this.product.productCategory,
      productPictures: this.product.productPictures
    };
    console.log(this.quantity);
    this.basketService.addItemToBasket(product, attrStr, this.quantity);

    this.basketItem = this.getCurrentBasketItem();
    this.cartQuantity = this.basketItem.quantity;
    this.quantity = 1;
  }

  private getCurrentBasketItem(): IBasketItem {
    let attrs = this.getCurrentBasketItemAttr();
    let basketItemKey = `${this.product.id}_${this.product.name}_${attrs}`
    let currentBasket = this.basketService.getBasketItem(basketItemKey);
    return currentBasket;
  }

  private getCurrentBasketItemAttr(): string {
    let attrStr = '';
    for (var [key, value] of Object.entries(this.prevOptions)) {
      attrStr += `_${key}_${value['name']}`;
    }
    return attrStr;
  }



}
