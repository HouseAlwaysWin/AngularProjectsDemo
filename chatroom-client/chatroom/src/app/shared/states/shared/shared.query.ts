import { Injectable } from "@angular/core";
import { Query } from "@datorama/akita";
import { SharedState, SharedStore } from "./shared.store";


@Injectable({
  providedIn: 'root'
})
export class SharedQuery extends Query<SharedState> {

  gLoading$ = this.select('gLoading');

  get gLoading() {
    return this.getValue().gLoading;
  }


  constructor(protected store: SharedStore) {
    super(store);
  }

}
