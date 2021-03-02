import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-nav-top',
  templateUrl: './nav-top.component.html',
  styleUrls: ['./nav-top.component.scss']
})
export class NavTopComponent implements OnInit {
  @Output() navSideToggle = new EventEmitter();
  constructor(public translate: TranslateService) {
    translate.addLangs(['en']);
    const browserLang = navigator.language;
    translate.use(browserLang.match(/en|zh-TW/) ? browserLang : 'en');
  }

  onNavSideToggle() {
    this.navSideToggle.emit();
  }

  ngOnInit(): void {
  }



}
