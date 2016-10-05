import { NgModule }      from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from './customer-types/customer-type-list.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { UserCreateComponent } from './users/user-create/user-create.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';

import { adminRouting } from './admin.router';

import { UserService } from './users/user.service';
import { CustomerTypeService } from './customer-types/customer-type.service';

@NgModule({
    imports: [BrowserModule, HttpModule, adminRouting, FormsModule],
    declarations: [
      AdminComponent,
      UserListComponent,
      CustomerTypeListComponent,
      UserListComponent,UserCreateComponent,UserEditComponent
    ],
    providers: [
      UserService,
      CustomerTypeService
    ]
})

export class AdminModule { }