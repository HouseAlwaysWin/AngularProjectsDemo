import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {
  @Output() sideNavClose = new EventEmitter();
  constructor() { }

  onSideNavClose() {
    this.sideNavClose.emit();
  }

  ngOnInit(): void {
  }

}
