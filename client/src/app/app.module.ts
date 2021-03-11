import { NavSideComponent } from './navigation/nav-side/nav-side.component';
import { NavTopComponent } from './navigation/nav-top/nav-top.component';
import { BrowserModule, TransferState } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { HomeComponent } from './home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AccountService } from './account/account.service';
import { ShopComponent } from './shop/shop.component';
import { I18nInterceptor } from './core/interceptors/i18n.interceptor';
import { I18nLoader, I18nLoaderFactory } from './core/i18n/i18n-loader';

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
    BrowserAnimationsModule,
    MaterialModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    AngularFireModule.initializeApp(environment.firebaseConfig),
    AngularFirestoreModule,
    HttpClientModule,
    TranslateModule.forRoot({
      defaultLanguage: 'zh-TW',
      loader: {
        provide: TranslateLoader,
        useFactory: I18nLoaderFactory,
        deps: []
      }
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: I18nInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }