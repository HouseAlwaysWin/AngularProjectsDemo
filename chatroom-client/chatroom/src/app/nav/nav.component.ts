import { ElementRef, HostListener, Renderer2, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountQuery } from '../shared/states/account/account.query';
import { AccountService } from '../shared/states/account/account.service';
import { AccountStore } from '../shared/states/account/account.store';
import { faComment, faChevronCircleDown } from '@fortawesome/free-solid-svg-icons';
import { UtilitiesService } from '../shared/services/utilities.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  faComment = faComment;
  faChevronCircleDown = faChevronCircleDown;

  showMenu: boolean = false;
  headPhotoUrl: string = '';

  @ViewChild('menu', { static: false }) menu: ElementRef;

  constructor(
    private router: Router,
    private accountService: AccountService,
    public accountQuery: AccountQuery,
    public accountStore: AccountStore,
    private utilitiesService: UtilitiesService,
  ) { }

  ngOnInit(): void {
    this.accountService.getUserDetail().subscribe(res => {
      this.readMainPhoto();
    })


  }

  readMainPhoto() {
    this.accountQuery.mainPhoto$.subscribe(url => {
      this.headPhotoUrl = url;
    })
  }

  documentClickListener(target: any): void {
    if (!this.menu?.nativeElement?.contains(target)) {
      this.showMenu = false;
    }
  }


  ngAfterViewInit(): void {
    this.utilitiesService.documentClickedTarget
      .subscribe(target => this.documentClickListener(target))
  }

  logout() {
    this.toggleMenu();
    this.accountService.logout();
    this.accountStore.update({
      isAuth: false
    });
    this.router.navigateByUrl('/');
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
  }
}
