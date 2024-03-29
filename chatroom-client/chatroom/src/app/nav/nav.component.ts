import { ElementRef, HostListener, Renderer2, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../shared/services/account.service';
import { faComment, faChevronCircleDown } from '@fortawesome/free-solid-svg-icons';
import { UtilitiesService } from '../shared/services/utilities.service';
import { Notify } from '../shared/models/notification';
import { Res } from '../shared/models/response';
import { DataService } from '../shared/states/data.service';
import { QAStatus } from '../shared/models/notification';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  faComment = faComment;
  faChevronCircleDown = faChevronCircleDown;

  showMenu: boolean = false;
  showNotify: boolean = false;
  headPhotoUrl: string = '';
  notifies: Notify[];
  notifyCount: number = 0;
  newMessageCount: number = 0;

  @ViewChild('menu', { static: false }) menu: ElementRef;
  @ViewChild('notify', { static: false }) notify: ElementRef;

  constructor(
    private router: Router,
    private accountService: AccountService,
    public state: DataService,
    private utilitiesService: UtilitiesService,
  ) { }

  public get QAStatus(): typeof QAStatus {
    return QAStatus;
  }

  ngOnInit(): void {
    this.accountService.getUserDetail().subscribe(res => {
      this.readMainPhoto();
    })
    this.getNotifies();
  }

  readMainPhoto() {
    this.state.query.mainPhoto$.subscribe(url => {
      this.headPhotoUrl = url;
    })
  }

  documentClickListener(target: any): void {
    if (!this.menu?.nativeElement?.contains(target)) {
      this.showMenu = false;
    }

    if (!this.notify?.nativeElement?.contains(target)) {
      this.showNotify = false;
    }
  }


  ngAfterViewInit(): void {
    this.utilitiesService.documentClickedTarget
      .subscribe(target => this.documentClickListener(target))
  }

  logout() {
    this.toggleMenu();
    this.accountService.logout();
    this.state.store.update({
      isAuth: false
    });
    this.router.navigateByUrl('/');
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
  }

  getNotifies() {
    this.state.query.notifies$.subscribe((notifies) => {
      console.log(notifies);
      this.notifies = notifies;
    });
    this.state.query.notifyNotReadCount$.subscribe(count => {
      console.log(count);
      this.notifyCount = count;
    });

    this.state.query.messageUnreadCount$.subscribe(count => {
      this.newMessageCount = count;
    })
  }

  toggleNotify() {
    this.showNotify = !this.showNotify;
    if (this.showNotify) {
      this.accountService.updateNotify().subscribe();
    }
  }


  acceptRequest(id: number, notifyId: number) {
    this.accountService.acceptFriend(id, notifyId).subscribe(res => {
      console.log('AcceptRequest');
      console.log(res.notifications.notifications.data);
      console.log(res);
      this.notifies = res.notifications.notifications.data;
    });
  }

  cancelRequest(notifyId: number) {
    this.accountService.rejectFriend(notifyId).subscribe();
  }

}
