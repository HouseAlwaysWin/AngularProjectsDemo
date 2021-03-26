import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { IProduct } from '../models/product';
import { IProductBrand } from '../models/productBrand';
import { IProductCategory } from '../models/productCategory';
import { ShopParams } from '../models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  shopParams: ShopParams = new ShopParams();
  products: IProduct[] = [];
  productTotalCount: number = 0;
  brands: IProductBrand[] = [];
  categories: IProductCategory[] = [];
  sortSelected = 'name';

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to Hight', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ]

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getProductBrands();
    this.getProductCategories();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      this.shopParams.pageIndex = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.productTotalCount = response.totalCount;
    });
  }

  onSortSelected() {
    console.log(this.sortSelected);
    this.shopParams.sort = this.sortSelected;
    this.getProducts();
  }

  getProductCategories() {
    this.shopService.getCategories().subscribe(response => {
      this.categories = [{ id: 0, name: 'All' }, ...response.data];
    }, error => {
      console.log(error);
    });
  }

  getProductBrands() {
    this.shopService.getBrands().subscribe(response => {
      this.brands = [{ id: 0, name: 'All' }, ...response.data]
    }, error => {
      console.log(error);
    });
  }

  onBrandSelected(id: number) {
    this.shopParams.brandId = id;
    this.shopParams.pageIndex = 0;
    this.getProducts();
  }

  onCategorySelected(id: number) {
    this.shopParams.categoryId = id;
    this.shopParams.pageIndex = 0;
    this.getProducts();
  }

  setPage(e: PageEvent) {
    this.shopParams.pageSize = e.pageSize;
    this.shopParams.pageIndex = e.pageIndex;
    this.getProducts();
  }


}
