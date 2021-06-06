import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessageComponent } from './message/message.component';
import { ProfileComponent } from './account/profile/profile.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule)
  },

  {
    path: 'message',
    loadChildren: () => import('./message/message.module').then(m => m.MessageModule)
    // path: 'message/:username',
    // component: MessageComponent
  },
  {
    path: 'friends',
    loadChildren: () => import('./friends/friends.module').then(m => m.FriendsModule)
  },

  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
