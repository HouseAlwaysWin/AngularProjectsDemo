import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-nav-side',
  templateUrl: './nav-side.component.html',
  styleUrls: ['./nav-side.component.scss']
})
export class NavSideComponent implements OnInit {
  @Output() navSideClose = new EventEmitter();
  constructor() { }

  onNavSideClose() {
    this.navSideClose.emit();
  }

  ngOnInit(): void {
  }

}
