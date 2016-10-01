import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AdminComponent } from './admin.component';
import { UserListComponent } from './users/user-list.component';
import { adminRouting } from './admin.router';

@NgModule({
    imports: [BrowserModule, adminRouting],
    declarations: [
      AdminComponent,
      UserListComponent
    ]
})

export class AdminModule { }