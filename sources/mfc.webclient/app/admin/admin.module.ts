import { NgModule }      from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from './customer-types/customer-type-list.component';
import { CustomerTypeEditComponent } from './customer-types/customer-type-edit.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { UserCreateComponent } from './users/user-create/user-create.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserPasswordComponent } from './users/user-password/user-password.component';
import { FileStatusEditComponent } from './filestatuses/filestatus-edit.component';
import { FileStatusListComponent } from './filestatuses/filestatus-list.component';
import { ActionTypeEditComponent } from './actiontypes/actiontype-edit.component';
import { ActionTypeListComponent } from './actiontypes/actiontype-list.component';
import { OrganizationTypeListComponent } from './organization-types/organization-type-list/organization-type-list.component';

import { adminRouting } from './admin.router';

import { UserService } from './users/user.service';
import { CustomerTypeService } from './customer-types/customer-type.service';
import { FileStatusService } from './filestatuses/filestatus.service';
import { ActionTypeService } from './actiontypes/actiontype.service';
import { OrganizationTypeService } from './organization-types/organization-type.service';

@NgModule({
    imports: [
      BrowserModule, 
      HttpModule, 
      adminRouting, 
      FormsModule
    ],
    declarations: [
      AdminComponent,
      CustomerTypeListComponent, CustomerTypeEditComponent,
      UserListComponent,UserCreateComponent,UserEditComponent,UserPasswordComponent,
      FileStatusListComponent, FileStatusEditComponent,
      ActionTypeListComponent, ActionTypeEditComponent,
      OrganizationTypeListComponent
    ],
    providers: [
      UserService,
      CustomerTypeService,
      FileStatusService,
      ActionTypeService,
      OrganizationTypeService
    ]
})

export class AdminModule { }