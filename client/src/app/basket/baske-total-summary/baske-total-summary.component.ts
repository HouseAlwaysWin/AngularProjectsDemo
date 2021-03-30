import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-baske-total-summary',
  templateUrl: './baske-total-summary.component.html',
  styleUrls: ['./baske-total-summary.component.scss']
})
export class BaskeTotalSummaryComponent implements OnInit {
  @Input() subtotal: number;
  @Input() shipping: number;
  @Input() total: number;
  @Input() showHint: boolean;
  @Input() showProcessBtn: boolean;
  constructor() { }

  ngOnInit(): void {
  }

}
