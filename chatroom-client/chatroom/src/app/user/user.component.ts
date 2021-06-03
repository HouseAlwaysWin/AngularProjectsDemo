import { HttpClient, HttpEvent, HttpEventType, HttpHeaders, HttpResponse } from '@angular/common/http';
import { ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Subject } from 'rxjs';
import { catchError, takeUntil } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Res } from '../shared/models/response';
import { UserDetail, UserPhoto, UserShortInfo } from '../shared/models/user';
import { UtilitiesService } from '../shared/services/utilities.service';
import { AccountQuery } from '../shared/states/account/account.query';
import { AccountService } from '../shared/states/account/account.service';
import { AccountStore } from '../shared/states/account/account.store';

export class ImgPreview {
  url: string = '';
}

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit, OnDestroy {

  apiUrl = environment.apiUrl;
  private _onDestroy = new Subject();
  mainPhoto: string;
  totalPhotos: UserPhoto[];
  photos: UserPhoto[];
  photoPrevIndex: number = 0;
  photoNextIndex: number = 5;

  uploadPhotofiles: any = [];
  uploadProgress = {};

  selectPhoto: UserPhoto;

  deleteBtnEnable: boolean = false;

  currentPublicId: string;

  @ViewChild('photosList', { static: false }) photosList: ElementRef;
  @ViewChild('photoInput', { static: false }) photoInput: ElementRef;

  userInfo: UserShortInfo;

  constructor(
    private http: HttpClient,
    private accountService: AccountService,
    private accountQuery: AccountQuery,
    private utilitiesService: UtilitiesService,
    public sanitizer: DomSanitizer,
    private accountStore: AccountStore) {

  }

  ngOnInit(): void {
    this.accountService.getUserDetail()
      .pipe(takeUntil(this._onDestroy))
      .subscribe(res => {
        this.getMainPhoto();
      })
    this.selectPhotoClickListener();
    this.initUserInfo();
  }

  initUserInfo() {
    this.accountQuery.user$.subscribe(user => {
      console.log(user);
      this.userInfo = user;
      this.currentPublicId = user.userPublicId;
    });
  }

  selectPhotoClickListener() {
    this.utilitiesService.documentClickedTarget
      .subscribe(target => {
        if (!this.photosList.nativeElement?.contains(target)) {
          this.selectPhoto = null;
          this.deleteBtnEnable = false;
        }
      })
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  onSelectPhoto(photo: UserPhoto) {
    this.selectPhoto = photo;
    this.deleteBtnEnable = true;

  }

  getMainPhoto() {
    this.accountQuery.photos$.subscribe((user: UserPhoto[]) => {
      this.mainPhoto = user.filter(u => u.isMain)[0]?.url;
      this.totalPhotos = user;
      this.photos = user.slice(this.photoPrevIndex, this.photoNextIndex);
    })
  }

  setAsMain(photo: UserPhoto) {
    this.accountService.setUserPhotoAsMain(photo)
      .pipe(takeUntil(this._onDestroy))
      .subscribe(result => {
        this.getMainPhoto();
      })
  }

  setPrevPhoto() {
    if (this.photoPrevIndex > 0) {
      this.photoPrevIndex = this.photoPrevIndex - 1;
      this.photoNextIndex = this.photoNextIndex - 1;
      this.setPhotosList();
    }
  }

  setNextPhoto() {
    if (this.photoNextIndex < this.totalPhotos.length) {
      this.photoPrevIndex = this.photoPrevIndex + 1;
      this.photoNextIndex = this.photoNextIndex + 1;
      this.setPhotosList();
    }
  }

  setPhotosList() {
    this.photos = this.totalPhotos.slice(this.photoPrevIndex, this.photoNextIndex);
  }

  deletePhotoSelect() {
    if (this.selectPhoto.isMain) {
      alert('you can\'t delete main photo')
      return;
    };
    if (this.selectPhoto) {
      this.accountService.deleteUserPhoto(this.selectPhoto)
        .subscribe(() => {
          this.getMainPhoto();
          this.setPrevPhoto();
          this.setNextPhoto();
        });
    }
    else {
      this.deleteBtnEnable = true;
    }
  }

  browseFile(event) {
    console.log(event.target.files);
    this.prepareFilesList(event.target.files);
  }

  dropFile(files: File[]) {
    this.prepareFilesList(files);
  }


  uploadFile(file: any, index: number) {

    let files = [];
    files.push(file);
    this.accountService.uploadUserPhoto(files)
      .subscribe((event: any) => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            let progress = 0;
            if (event['loaded']) {
              this.uploadPhotofiles[index].progress = Math.round((event['loaded'] * 100) / event['total']);
            }

          case HttpEventType.Response:
            console.log(event);
            let data = event?.body?.data;
            if (data) {
              this.removeUploadImg(index);
              if (data) {
                this.accountStore.update({
                  user: data
                })
              }
            }
        }
      }, error => {
        console.log(error);
        alert(error.error)
      });

  }


  removeUploadImg(index) {
    if (this.photoInput && this.photoInput.nativeElement) {
      this.photoInput.nativeElement.value = '';
    }

    this.uploadPhotofiles.splice(index, 1);
    console.log(this.uploadPhotofiles);
  }

  prepareFilesList(files: Array<any>) {
    for (let i = 0; i < files.length; i++) {
      let file = files[i];
      file.progress = 0;
      file.objectURL = this.sanitizer.bypassSecurityTrustUrl((window.URL.createObjectURL(file)));
      this.uploadPhotofiles.push(file);
      console.log(this.uploadPhotofiles);
    }
  }

  formatBytes(bytes, decimals) {
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = decimals <= 0 ? 0 : decimals || 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  updatePublicId() {
    console.log(this.userInfo.userPublicId)
    this.accountService.updatePublicId(this.userInfo.userPublicId).subscribe((res: Res<UserShortInfo>) => {
      console.log(res);
      this.userInfo = res.data;
      this.currentPublicId = res.data.userPublicId;
    });
  }


}
