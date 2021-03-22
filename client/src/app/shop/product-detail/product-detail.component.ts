import { Component, Input, OnInit } from '@angular/core';
import { IProduct } from 'src/app/models/product';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  @Input() product: IProduct;
  quantity = 1;
  constructor() { }

  ngOnInit(): void {
  }

  decrementQuantity() {

  }

  incrementQuantity() {

  }

  addItemToBasket() {

  }

}
