import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/states/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(public state: DataService) { }

  ngOnInit(): void {
  }

}
