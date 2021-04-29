import { ArrayDataSource } from '@angular/cdk/collections';
import { FlatTreeControl, NestedTreeControl } from '@angular/cdk/tree';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { Observable, Subscription } from 'rxjs';
import { debounceTime, map, tap } from 'rxjs/operators';
import { IApiPagingResponse } from '../models/apiResponse';
import { IProduct, IProductCategory } from '../models/product';
import { ShopParams } from '../models/shopParams';
import { RotatedAnimation, FadeInGrowListAnimation } from '../shared/animations/animation-triggers';
import * as appReducer from '../store/app.reducer';
import { Store } from '@ngrx/store';
import * as ShopActions from './store/shop.actions';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
  animations: [
    RotatedAnimation(),
    FadeInGrowListAnimation()
  ]
})
export class ShopComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput: ElementRef;

  shopParams: ShopParams = new ShopParams();
  products$: Observable<IApiPagingResponse<IProduct[]>>;

  shopSub: Subscription;
  products: IProduct[];
  productTotalcount: number = 0;

  categories: ArrayDataSource<IProductCategory>;
  categoriesSub: Subscription;
  categoriesTreeControl: NestedTreeControl<IProductCategory>;

  treeControl: FlatTreeControl<IProductCategory>;

  sortSelected = 'name';
  searchControl = new FormControl();
  searchOptions: IProduct[];

  isLoading: boolean = false;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'PriceLowToHight', value: 'priceAsc' },
    { name: 'PriceHightToLow', value: 'priceDesc' }
  ]

  constructor(private store: Store<appReducer.AppState>
  ) {
  }

  ngOnDestroy(): void {
    this.shopSub.unsubscribe();
  }

  ngOnInit(): void {
    this.onAutoCompleteOptions();
    this.getProducts();
    this.getProductCategories();
  }

  getProducts() {
    this.store.dispatch(ShopActions.GetProductList(this.shopParams));
    this._SetShopState();
  }

  private _SetShopState() {
    this.shopSub = this.store.select('shop')
      .subscribe((res) => {
        this.products = res.products;
        this.productTotalcount = res.totalCount;
        this.isLoading = res.loading;
        this.searchOptions = res.searchOptions;

        this.categories = new ArrayDataSource(res.productCategories);
        this.categoriesTreeControl = new NestedTreeControl<IProductCategory>(node => node.children);
        this.treeControl = new FlatTreeControl<IProductCategory>(
          node => node.level, node => node.hasChild
        );
      });
  }


  onAutoCompleteOptions() {

    this.searchControl.valueChanges
      .pipe(
        debounceTime(200),
        map(value => {
          if (!value) {
            this.getProducts();
          } else {
            this.store.dispatch(ShopActions.AutoComplete({
              search: value,
              pageIndex: 0,
              pageSize: this.shopParams.pageSize
            }));
          }
        })).subscribe();

  }

  onSearch() {
    if (this.searchControl.value) {
      this.shopParams.pageIndex = 0;
      this.shopParams.search = this.searchControl.value;
      this.store.dispatch(ShopActions.Search(this.shopParams));
      this._SetShopState();
    }
  }

  onSortSelected() {
    this.shopParams.sort = this.sortSelected;
    this.getProducts();
  }

  getProductCategories() {
    this.store.dispatch(ShopActions.GetCategories());
  }

  hasChild(_: number, node: IProductCategory) {
    return !!node.children && node.children.length > 0;
  }

  hasChildCategories(_: number, node: IProductCategory) {
    return !!node.children && node.children.length > 0;
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
