import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-nav-top',
  templateUrl: './nav-top.component.html',
  styleUrls: ['./nav-top.component.scss']
})
export class NavTopComponent implements OnInit {
  @Output() navSideToggle = new EventEmitter();
  constructor() { }

  onNavSideToggle() {
    this.navSideToggle.emit();
  }

  ngOnInit(): void {
  }

}
