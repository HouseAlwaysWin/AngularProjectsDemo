import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Res } from '../shared/models/response';
import { UserDetail, UserPhoto } from '../shared/models/user';
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

  selectPhoto: UserPhoto;

  deleteBtnEnable: boolean = false;

  // imgPreviews: ImgPreview[] = [];
  imgPreviews = {};





  @ViewChild('photosList', { static: false }) photosList: ElementRef;


  constructor(
    private http: HttpClient,
    private accountService: AccountService,
    private accountQuery: AccountQuery,
    private utilitiesService: UtilitiesService,
    private accountStore: AccountStore) {

  }

  ngOnInit(): void {
    this.accountService.getUserDetail()
      .pipe(takeUntil(this._onDestroy))
      .subscribe(res => {
        this.getMainPhoto();
      })
    this.selectPhotoClickListener();
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
    console.log(this.photoNextIndex);
    console.log(this.photos.length);
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
    if (this.selectPhoto) {
      this.accountService.deleteUserPhoto(this.selectPhoto).subscribe(() => {
        this.getMainPhoto();
        console.log('finished delete photo');
      });
    }
    else {
      this.deleteBtnEnable = true;
    }
  }




  browseFile(event) {
    this.prepareFilesList(event.target.files);
  }

  dropFile(files: File[]) {
    this.prepareFilesList(files);
  }

  uploadFile() {
    console.log(this.uploadPhotofiles);
    if (this.uploadPhotofiles.length > 0) {
      this.accountService.uploadUserPhoto(this.uploadPhotofiles)
        .subscribe(result => {
          console.log('upload Result:' + result);
        });
    }
  }

  readFileImg(file: File, index: number) {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      if (e.target.result) {
        this.imgPreviews[index] = e.target.result
      }
      console.log(this.imgPreviews);
    };
    reader.readAsDataURL(file);
  }

  removeUploadImg(index) {
    this.uploadPhotofiles.splice(index, 1);
  }

  uploadFilesSimulator(index: number) {
    setTimeout(() => {
      if (index === this.uploadPhotofiles.length) {
        return;
      } else {
        const progressInterval = setInterval(() => {
          if (this.uploadPhotofiles[index].progress === 100) {
            clearInterval(progressInterval);
            this.uploadFilesSimulator(index + 1);
          } else {
            this.uploadPhotofiles[index].progress += 5;
          }
        }, 200);
      }
    }, 1000);
  }

  prepareFilesList(files: Array<any>) {
    for (let i = 0; i < files.length; i++) {
      // for (const item of files) {
      files[i].progress = 0;
      this.uploadPhotofiles.push(files[i]);
      this.readFileImg(files[i], i);
    }
    this.uploadFilesSimulator(0);
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

  getImgUrl(index: number) {
    if (this.imgPreviews[index]) {
      return this.imgPreviews[index];
    }
    return '';
  }


}
