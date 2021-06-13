import { Injectable } from "@angular/core";
import { DataQuery } from "./data.query";
import { DataStore } from "./data.store";

@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(
    private dataQuery: DataQuery,
    private dataStore: DataStore,
  ) {
  }


  get query() {
    return this.dataQuery;
  }

  get store() {
    return this.dataStore;
  }

}


