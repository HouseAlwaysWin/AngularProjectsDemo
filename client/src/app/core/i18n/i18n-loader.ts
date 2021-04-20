import { Observable } from "rxjs";
import { TranslateLoader } from '@ngx-translate/core';
import * as zh_tw from '../../../assets/i18n/zh-TW.json';
import * as en_us from '../../../assets/i18n/en-US.json';
import { of } from "rxjs";


const trans = {
  "zh-TW": zh_tw,
  "en-US": en_us
}

export class I18nLoader implements TranslateLoader {

  constructor() {
  }

  public getTranslation(lang: string): Observable<any> {
    return of(trans[lang].default);
  }
}
