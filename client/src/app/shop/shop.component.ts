import { ArrayDataSource } from '@angular/cdk/collections';
import { NestedTreeControl } from '@angular/cdk/tree';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { IApiPagingResponse } from '../models/apiResponse';
import { IProduct, IProductCategory } from '../models/product';
import { ShopParams } from '../models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {

  @ViewChild('searchInput') searchInput: ElementRef;


  shopParams: ShopParams = new ShopParams();
  products$: Observable<IApiPagingResponse<IProduct[]>>;

  categories: ArrayDataSource<IProductCategory>;
  categories$: Observable<IProductCategory[]>;
  categoriesTreeControl: NestedTreeControl<IProductCategory>;

  sortSelected = 'name';

  searchControl = new FormControl();
  searchOptions: IProduct[];


  isLoading: boolean = false;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'PriceLowToHight', value: 'priceAsc' },
    { name: 'PriceHightToLow', value: 'priceDesc' }
  ]

  constructor(private shopService: ShopService
  ) {
  }

  ngOnInit(): void {
    this.products$ = this.shopService.productList$;
    this.categories$ = this.shopService.productCategories$;

    this.onAutoCompleteOptions();
    this.getProducts();
    this.getProductCategories();
  }

  getProducts() {
    this.isLoading = true;
    this.shopService.getProducts(this.shopParams).subscribe(() => {
      this.isLoading = false;
    });
  }


  onAutoCompleteOptions() {

    this.searchControl.valueChanges
      .pipe(
        debounceTime(500),
        map(value => {
          if (!value) {
            this.getProducts();
          } else {
            this.shopService.getAutocomplete({
              search: value,
              pageIndex: 0,
              pageSize: this.shopParams.pageSize
            }).subscribe(p => {
              this.searchOptions = p.data;
            });
          }
        })).subscribe();
  }

  onSearch() {
    this.shopParams.pageIndex = 0;
    this.Search();
  }


  private Search() {
    if (this.searchControl.value) {
      this.isLoading = true;
      this.shopService.getSearch({
        search: this.searchControl.value,
        pageIndex: this.shopParams.pageIndex,
        pageSize: this.shopParams.pageSize
      }).subscribe(p => {
        this.searchOptions = p.data;
        this.isLoading = false;
      });
    }
  }

  onSortSelected() {
    console.log(this.sortSelected);
    this.shopParams.sort = this.sortSelected;
    this.getProducts();
  }

  getProductCategories() {
    this.shopService.getCategories().subscribe(() => {
      this.categories = new ArrayDataSource(this.categories$);

      this.categoriesTreeControl = new NestedTreeControl<IProductCategory>(node => node.children);
    }, error => {
      console.log(error);
    });
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
    if (this.searchControl.value) {
      this.Search();
    } else {
      this.getProducts();
    }
  }


}
