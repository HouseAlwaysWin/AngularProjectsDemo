import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-nav-top',
  templateUrl: './nav-top.component.html',
  styleUrls: ['./nav-top.component.scss']
})
export class NavTopComponent implements OnInit {
  @Output() public sidenavToggle = new EventEmitter();
  constructor() { }

  onToggleSideNav() {
    console.log('toggle');
    this.sidenavToggle.emit();
  }

  ngOnInit(): void {
  }

}
