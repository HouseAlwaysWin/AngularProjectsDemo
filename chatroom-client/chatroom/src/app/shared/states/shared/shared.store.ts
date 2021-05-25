import { Injectable } from "@angular/core";
import { Store, StoreConfig } from "@datorama/akita";


export interface SharedState {
  gLoading: boolean;
}

export function createInitialState(): SharedState {
  return {
    gLoading: false
  }
}

@Injectable({
  providedIn: 'root'
})
@StoreConfig({ name: 'shared' })
export class SharedStore extends Store<SharedState>{
  constructor() {
    super(createInitialState());
  }
}
