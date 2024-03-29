import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Res } from 'src/app/shared/models/response';
import { UserPhoto, UserShortInfo } from 'src/app/shared/models/user';
import { FormFieldService } from 'src/app/shared/services/form-field.service';
import { AccountService } from 'src/app/shared/services/account.service';
import { PresenceService } from 'src/app/shared/services/presence.service';
import { Friend } from 'src/app/shared/models/friend';

@Component({
  selector: 'app-friends-add',
  templateUrl: './friends-add.component.html',
  styleUrls: ['./friends-add.component.scss']
})
export class FriendsAddComponent implements OnInit {
  friend: UserShortInfo;
  findUser: boolean = false;
  friendList: Friend[];
  publicId: string;
  constructor(
    private accountService: AccountService,
    public formService: FormFieldService,
    private presenceService: PresenceService) { }

  ngOnInit(): void {
    this.accountService.getFriends().subscribe((res: Res<Friend[]>) => {
      console.log(res);
      this.friendList = res.data;
    });
  }

  checkIsFriend(id: number) {
    return this.friendList.find(f => f.friend.id === id);
  }

  findFriendByPublicId() {
    this.accountService.findFriendByPublicId(this.publicId).subscribe((res: Res<UserShortInfo>) => {
      console.log(res);
      if (res.isSuccessed) {
        this.friend = res.data;
        this.findUser = true;
      }
      else {
        this.friend = null;
        this.findUser = false;
      }
    }, (error) => {
      console.log(error);
      this.findUser = false;
    });
  }

  getFriendMainPhoto(photos: UserPhoto[]) {
    let mainUrl = photos.filter(p => p.isMain)[0].url;
    return mainUrl == null ? '' : mainUrl;
  }

  SendFriendRequest(friendId: number) {
    this.presenceService.sendFriendRequest(friendId)
      .then(() => {
        alert('send request');
        // this.messageContent = '';
      }).finally(() => {

      });
  }

  // AddNewFriend(id: number) {
  //   console.log(id);
  //   this.accountService.addNewFriend(id).subscribe(res => {
  //     console.log(res);
  //     if (res.isSuccessed) {
  //       alert("Add successed");
  //     }
  //     else {
  //       alert("Add failed");
  //     }
  //   });
  // }

}
