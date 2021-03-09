import { Component, OnInit } from '@angular/core';
import { FbAuthService } from './account/fb-auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  /**
   *
   */
  constructor(private authService: FbAuthService) {

  }
  ngOnInit(): void {
    this.authService.authListener();
  }
}
