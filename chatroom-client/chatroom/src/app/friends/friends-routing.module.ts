import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FriendsComponent } from './friends.component';
import { FriendsAddComponent } from './friends-add/friends-add.component';

const routes: Routes = [
  { path: '', component: FriendsComponent },
  { path: 'add', component: FriendsAddComponent }
];


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class FriendsRoutingModule { }
