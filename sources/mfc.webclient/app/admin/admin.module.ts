import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AdminComponent } from './admin.component';
import { UsersComponent } from './users/users.component';
import { adminRouting } from './admin.router';

@NgModule({
    imports: [BrowserModule, adminRouting],
    declarations: [
      AdminComponent,
      UsersComponent
    ]
})

export class AdminModule { }