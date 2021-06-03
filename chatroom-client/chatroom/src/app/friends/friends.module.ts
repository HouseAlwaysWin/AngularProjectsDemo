import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FriendsAddComponent } from './friends-add/friends-add.component';
import { SharedModule } from '../shared/shared.module';
import { FriendsRoutingModule } from './friends-routing.module';
import { FriendsComponent } from './friends.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [FriendsComponent, FriendsAddComponent],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    FriendsRoutingModule
  ],
})
export class FriendsModule { }
