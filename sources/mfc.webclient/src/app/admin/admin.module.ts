import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { AdminComponent } from './admin.component';

// OrganizationType
import { OrganizationTypeListComponent } from './organization-types/organization-type-list.component';
import { OrganizationTypeEditComponent } from './organization-types/organization-type-edit.component';
import { OrganizationTypeService } from './organization-types/organization-type.service';

// Organization
import { OrganizationListComponent } from './organizations/organization-list.component';
import { OrganizationEditComponent } from './organizations/organization-edit.component';
import { OrganizationService } from './organizations/organization.service';

// ActionType
import { ActionTypeListComponent } from './action-types/action-type.list.component';
import { ActionTypeEditComponent } from './action-types/action-type.edit.component';
import { ActionTypeService } from './action-types/action-type.service';

// FileStatus
import { FileStatusListComponent } from './file-statuses/file-status.list.component';
import { FileStatusEditComponent } from './file-statuses/file-status.edit.component';
import { FileStatusService } from './file-statuses/file-status.service';


//Services
import { ServiceService } from './services/service.service';







//import { BusyModule, BusyConfig } from 'angular2-busy';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CustomerTypeListComponent } from './customer-types/customer-type-list.component';
import { CustomerTypeEditComponent } from './customer-types/customer-type-edit.component';
import { UserListComponent } from './users/user-list.component';
//import { UserEditComponent } from './users/user-edit.component';
//import { UserPasswordComponent } from './users/user-password.component';




//import { FileStageListComponent } from './file-stages/file-stage-list.component';
//import { ServiceListComponent } from './services/service-list.component';
//import { ServiceEditComponent } from './services/service-edit.component';


import { adminRouting } from './admin.router';

import { UserService } from './users/user.service';
import { CustomerTypeService } from './customer-types/customer-type.service';
//import { FileStageService } from './file-stages/file-stage.service';

import { BusyConfig } from '../shared/busy/busy-config';
import { BusyModule } from '../shared/busy/busy.module';
import { TestComponent } from './customer-types/test.component';
// import { DialogOverviewExampleDialog } from './organization-types/organization-type-list.component';
import { MaterialModule } from '@angular/material';


@NgModule({
  imports: [
    BrowserModule,
    MaterialModule,
    HttpModule,
    adminRouting, ReactiveFormsModule, FormsModule,
    BusyModule.forRoot(
      new BusyConfig({
        message: 'Загрузка...',
        backdrop: true,
        delay: 30,
        minDuration: 100
      })
    )
  ],
  declarations: [
    AdminComponent,
    CustomerTypeListComponent, CustomerTypeEditComponent, TestComponent,
    UserListComponent, //UserEditComponent, UserPasswordComponent,
    FileStatusListComponent, FileStatusEditComponent,
    ActionTypeListComponent, ActionTypeEditComponent,
    OrganizationTypeListComponent, OrganizationTypeEditComponent,
    OrganizationListComponent, OrganizationEditComponent
    //FileStageListComponent,
    //ServiceListComponent, ServiceEditComponent,
    // ,DialogOverviewExampleDialog
  ],
  entryComponents: [CustomerTypeEditComponent, TestComponent,
    OrganizationTypeEditComponent,
    OrganizationEditComponent,
    ActionTypeEditComponent,
    FileStatusEditComponent
  ],
  providers: [
    UserService,
    CustomerTypeService,
    FileStatusService,
    ActionTypeService,
    OrganizationService,
    OrganizationTypeService,
    //FileStageService,
    ServiceService,
  ]
})

export class AdminModule { }