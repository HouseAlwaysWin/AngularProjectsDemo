import { createReducer, createReducerFactory, on } from "@ngrx/store";
import { Basket, BasketItem, IBasket, IBasketItem, IBasketTotals } from "src/app/models/basket";
import * as BasketActions from './basket.actions';

export interface State {
  basket: IBasket,
  basketTotal: IBasketTotals,
  loading: boolean,
  paymentIntentSuccess: boolean
}

export const InitialState: State = {
  basket: null,
  basketTotal: null,
  loading: false,
  paymentIntentSuccess: false
}

export const basektReducer = createReducer(
  InitialState,

  on(BasketActions.GetBasetSuccess, (state, action) => {
    return ({
      ...state,
      basket: action.data,
      loading: false
    })
  }),

  on(BasketActions.CreatePaymentIntentSuccess, (state, action) => {
    return ({
      ...state,
      basket: action,
      paymentIntentSuccess: true,
      loading: false
    })
  }),



  on(BasketActions.UpdateBasketSuccess, (state, action) => {
    const shipping = action.basket.shippingPrice;
    const subtotal = action.basket.basketItems.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;

    return ({
      ...state,
      basket: action.basket,
      basketTotal: { shipping, total, subtotal }
    })
  }),

  on(BasketActions.DeleteBasket, (state, action) => {
    console.log(state);
    return ({
      ...state,
      paymentIntentSuccess: false,
      loading: false
    })
  }),




  on(BasketActions.AddProductToBasket, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    let newBasketItem = new BasketItem();
    let item = action.productToAdd;
    newBasketItem.id = action.key;
    newBasketItem.productId = item.id;
    newBasketItem.name = item.name;
    newBasketItem.price = item.price;
    newBasketItem.imgUrl = item.productPictures[0].urlPath;
    newBasketItem.description = item.description
    newBasketItem.productCategoryName = item.productCategory.name
    newBasketItem.quantity = action.quantity;

    basket.basketItems = addOrUpdateItem(basket.basketItems, newBasketItem, action.quantity);
    return ({
      ...state,
      basket
    })

  }),

  on(BasketActions.UpdateShipping, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    basket.deliveryMethodId = action.id;
    basket.shippingPrice = action.price;
    return ({
      ...state,
      basket
    })
  }),

  on(BasketActions.UpdateOrAddBasketItem, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    let index = basket.basketItems.findIndex(b => b.id === action.id);
    basket.basketItems[index] = action.basketItem;

    return ({
      ...state,
      basket
    })
  }),


  on(BasketActions.GetBasketItemById, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    let index = basket.basketItems.findIndex(i => i.id === action.id);
    if (index === -1) {
      return ({
        ...state,
        currentBasketItem: null
      })
    }

    console.log(basket.basketItems[index]);

    return ({
      ...state,
      currentBasketItem: basket.basketItems[index]
    })
  }),

  on(BasketActions.RemoveBasketItem, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    let index = basket.basketItems.findIndex(i => i.id === action.id);
    if (index !== -1) {
      basket.basketItems.splice(index, 1);
    }

    return ({
      ...state,
      basket,
      loading: true
    })

  }),

  on(BasketActions.IncrementItemQuantity, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    let index = basket.basketItems.findIndex(i => i.id === action.id);

    if (index !== -1) {
      if (basket.basketItems[index].quantity < 100) {
        basket.basketItems[index].quantity++;
      }
    }

    return ({
      ...state,
      basket
    })
  }),

  on(BasketActions.DecrementItemQuantity, (state, action) => {
    let basket: IBasket = JSON.parse(JSON.stringify(state.basket)) ?? createBasket();
    let index = basket.basketItems.findIndex(i => i.id === action.id);

    if (index !== -1) {
      if (basket.basketItems[index].quantity > 1) {
        basket.basketItems[index].quantity--;
      }
    }

    return ({
      ...state,
      basket
    })
  }),

  on(BasketActions.CreatePaymentIntent, (state, action) => {
    return ({
      ...state,
      loading: true
    })

  })


)

function createBasket(): IBasket {
  const basket = new Basket();
  localStorage.setItem('basket_id', basket.id);
  return basket;
}

function addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
  const index = items.findIndex(i => i.id === itemToAdd.id);
  if (index === -1) {
    itemToAdd.quantity = quantity;
    items.push(itemToAdd);
  } else {
    items[index].quantity += quantity
  }
  return items;
}
