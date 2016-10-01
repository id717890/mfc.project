import { NgModule }      from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';

import { AdminComponent } from './admin.component';
import { UserListComponent } from './users/user-list.component';

import { adminRouting } from './admin.router';

import { UserService } from './users/user.service';

@NgModule({
    imports: [BrowserModule, HttpModule, adminRouting],
    declarations: [
      AdminComponent,
      UserListComponent
    ],
    providers: [
      UserService
    ]
})

export class AdminModule { }