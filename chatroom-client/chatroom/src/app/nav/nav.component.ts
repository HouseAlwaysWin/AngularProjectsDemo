import { ElementRef, Renderer2, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountQuery } from '../shared/states/account/account.query';
import { AccountService } from '../shared/states/account/account.service';
import { AccountStore } from '../shared/states/account/account.store';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  showMenu: boolean = false;
  headPhotoUrl: string = '';

  @ViewChild('menu', { static: false }) menu: ElementRef;
  @ViewChild('menuBtn', { static: false }) menuBtn: ElementRef;
  constructor(
    private router: Router,
    private accountService: AccountService,
    public accountQuery: AccountQuery,
    public accountStore: AccountStore,
    private renderer: Renderer2
  ) { }

  ngOnInit(): void {
    this.closeMenuOutside();
    this.headPhotoUrl = this.accountQuery.user.photos.filter(p => p.isMain)[0]?.url;
  }

  closeMenuOutside() {
    this.renderer.listen('window', 'click', (e: Event) => {
      console.log(e.target);
      if (e.target !== this.menu.nativeElement &&
        e.target !== this.menuBtn.nativeElement) {
        this.showMenu = false;
      }
    });
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
