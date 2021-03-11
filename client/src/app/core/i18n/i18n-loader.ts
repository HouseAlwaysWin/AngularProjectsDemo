import { Observable } from "rxjs";
import { TranslateLoader } from '@ngx-translate/core';
import * as zh_tw from '../../../assets/i18n/zh-TW.json';
import * as en from '../../../assets/i18n/en.json';
import { of } from "rxjs";


const trans = {
  "zh-TW": zh_tw,
  "en": en
}

export class I18nLoader implements TranslateLoader {

  constructor() {
  }

  public getTranslation(lang: string): Observable<any> {
    return of(trans[lang].default);
  }
}

export function I18nLoaderFactory() {
  return new I18nLoader();
}
