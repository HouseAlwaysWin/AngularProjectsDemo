import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: IProduct;
  productAttrs: string[];
  constructor(private activeRoute: ActivatedRoute,
    private shopService: ShopService) { }

  ngOnInit(): void {

    var id = this.activeRoute.snapshot.paramMap.get('id');
    this.shopService.getProductById(id).subscribe(product => {
      console.log(product);
      this.product = product;
    })

  }

  decrementQuantity() {

  }

  incrementQuantity() {

  }

  addItemToBasket() {

  }

}
