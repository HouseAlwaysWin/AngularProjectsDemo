import { Injectable, OnInit } from "@angular/core";
import { MatPaginatorIntl } from "@angular/material/paginator";
import { TranslateService } from "@ngx-translate/core";
import { StringFormat } from "../shared/utils/stringFormat";

@Injectable()
export class PaginatorLocalize extends MatPaginatorIntl {



  constructor(private translate: TranslateService) {
    super();
    translate.onLangChange.subscribe(event => {
      this.getTranslations();
    });
  }

  getTranslations() {

    this.translate.get('Paginator').subscribe(trans => {
      console.log(trans);
      this.itemsPerPageLabel = trans['ItemsPerPage'];
      this.nextPageLabel = '下一頁';
      this.previousPageLabel = '上一頁';
      this.getRangeLabel = this.getRangeTag;
      this.changes.next();
    })
  }

  getRangeTag(page: number, pageSize: number, length: number) {
    const trans = this.translate.instant('Paginator');
    if (length == 0 || pageSize == 0) {
      return StringFormat(trans['PageRangeZero'], length);
    }

    length = Math.max(length, 0);

    const startIndex = page * pageSize;

    const endIndex = startIndex < length ?
      Math.min(startIndex + pageSize, length) :
      startIndex + pageSize;
    let result = StringFormat(trans['PageRange'], startIndex + 1, endIndex, length);
    return result;
  }

}
