import { Injectable, OnInit } from "@angular/core";
import { MatPaginatorIntl } from "@angular/material/paginator";
import { TranslateService } from "@ngx-translate/core";

@Injectable()
export class PaginatorLocalize extends MatPaginatorIntl {



  constructor(private translate: TranslateService) {
    super();
    translate.onLangChange.subscribe(event => {
      this.getTranslations();
    });
    this.getTranslations();
  }

  getTranslations() {

    this.translate.get('Paginator').subscribe(trans => {
      this.itemsPerPageLabel = trans['ItemsPerPage'];
      this.nextPageLabel = '下一頁';
      this.previousPageLabel = '上一頁';
      this.getRangeLabel = this.getRangeTag;
      this.changes.next();
    })
  }

  getRangeTag(page: number, pageSize: number, length: number) {
    if (length == 0 || pageSize == 0) {
      return this.translate.instant('Paginator.PageRangeZero', { total: length });
    }

    length = Math.max(length, 0);

    const startIndex = page * pageSize;

    const endIndex = startIndex < length ?
      Math.min(startIndex + pageSize, length) :
      startIndex + pageSize;
    return this.translate.instant('Paginator.PageRange', { start: startIndex + 1, end: endIndex, total: length });
  }

}
