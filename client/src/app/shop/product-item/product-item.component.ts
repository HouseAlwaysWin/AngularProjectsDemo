import { animate, group, query, style, transition, trigger } from '@angular/animations';
import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/models/product';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss'],
})
export class ProductItemComponent implements OnInit {
  @Input() product: IProduct;
  currentSide: boolean = true;
  apiHost = environment.apihost;
  constructor() { }

  ngOnInit(): void {
  }

  rotateSide() {
    this.currentSide = !this.currentSide;
  }



}
