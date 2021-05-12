import { NavSideComponent } from './navigation/nav-side/nav-side.component';
import { NavTopComponent } from './navigation/nav-top/nav-top.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { environment } from 'src/environments/environment';
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { I18nInterceptor } from './core/interceptors/i18n.interceptor';
import { I18nLoader } from './core/i18n/i18n-loader';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { PaginatorLocalize } from './material/paginator-localize';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { SharedModule } from './shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { appReducer } from './store/app.reducer';
import { EffectsModule } from '@ngrx/effects';
import { ShopEffects } from './shop/store/shop.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { BasketEffects } from './basket/store/basket.effects';
import { NG_ENTITY_SERVICE_CONFIG } from '@datorama/akita-ng-entity-service';
import { AkitaNgDevtools } from '@datorama/akita-ngdevtools';
import { AkitaNgRouterStoreModule } from '@datorama/akita-ng-router-store';
import { CheckoutEffects } from './checkout/store/checkout.effects';
import { ScrollingModule } from '@angular/cdk/scrolling';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavTopComponent,
    NavSideComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
    AngularFireModule.initializeApp(environment.firebaseConfig),
    AngularFirestoreModule,
    HttpClientModule,
    TranslateModule.forRoot({
      defaultLanguage: 'zh-TW',
      loader: {
        provide: TranslateLoader,
        useClass: I18nLoader
      }
    }),
    StoreModule.forRoot(appReducer),
    EffectsModule.forRoot([ShopEffects, BasketEffects, CheckoutEffects]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: environment.production }),
    environment.production ? [] : AkitaNgDevtools.forRoot(),
    AkitaNgRouterStoreModule,
    ScrollingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: I18nInterceptor, multi: true },
    { provide: MatPaginatorIntl, useClass: PaginatorLocalize },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: NG_ENTITY_SERVICE_CONFIG, useValue: { baseUrl: 'https://jsonplaceholder.typicode.com' } },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
