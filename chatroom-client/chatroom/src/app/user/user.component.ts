import { Component, OnInit } from '@angular/core';
import { UserPhoto } from '../shared/models/user';
import { AccountQuery } from '../shared/states/account/account.query';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  mainPhoto: string;
  totalPhotos: UserPhoto[];
  photos: UserPhoto[];
  photoPrevIndex: number = 0;
  photoNextIndex: number = 5;

  selectPhoto: UserPhoto;


  constructor(private _accountQuery: AccountQuery) {

  }

  onSelectPhoto(photo: UserPhoto) {
    this.selectPhoto = photo;
  }

  setMainPhoto() {
    this.mainPhoto = this._accountQuery.photos.filter(u => u.isMain)[0]?.url;
    this.getPhotos();
  }

  setPrevPhoto() {
    if (this.photoPrevIndex > 0) {
      this.photoPrevIndex = this.photoPrevIndex - 1;
      this.photoNextIndex = this.photoNextIndex - 1;
      this.getPhotos();
    }
  }

  setNextPhoto() {
    console.log(this.photoNextIndex);
    console.log(this.photos.length);
    if (this.photoNextIndex < this.totalPhotos.length) {
      this.photoPrevIndex = this.photoPrevIndex + 1;
      this.photoNextIndex = this.photoNextIndex + 1;
      this.getPhotos();
    }
  }

  getPhotos() {
    this.totalPhotos = this._accountQuery.photos;
    this.photos = this._accountQuery.photos.slice(this.photoPrevIndex, this.photoNextIndex);
    console.log(this.photos);
  }

  ngOnInit(): void {
    this.setMainPhoto();
  }

}
