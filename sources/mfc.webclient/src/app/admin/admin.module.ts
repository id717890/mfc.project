import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
//import { BusyModule, BusyConfig } from 'angular2-busy';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from './customer-types/customer-type-list.component';
import { CustomerTypeEditComponent } from './customer-types/customer-type-edit.component';
import { UserListComponent } from './users/user-list.component';
//import { UserEditComponent } from './users/user-edit.component';
//import { UserPasswordComponent } from './users/user-password.component';
//import { FileStatusEditComponent } from './file-statuses/file-status.edit.component';
//import { FileStatusListComponent } from './file-statuses/file-status.list.component';
//import { ActionTypeEditComponent } from './action-types/action-type.edit.component';
import { ActionTypeListComponent } from './action-types/action-type.list.component';

// OrganizationType
import { OrganizationTypeListComponent } from './organization-types/organization-type-list.component';
import { OrganizationTypeEditComponent } from './organization-types/organization-type-edit.component';

//import { FileStageListComponent } from './file-stages/file-stage-list.component';
//import { ServiceListComponent } from './services/service-list.component';
//import { ServiceEditComponent } from './services/service-edit.component';
import { OrganizationListComponent } from './organizations/organization-list.component';
//import { OrganizationEditComponent } from './organizations/organization-edit.component';


import { adminRouting } from './admin.router';

import { UserService } from './users/user.service';
import { CustomerTypeService } from './customer-types/customer-type.service';
//import { FileStatusService } from './file-statuses/file-status.service';
import { ActionTypeService } from './action-types/action-type.service';
import { OrganizationTypeService } from './organization-types/organization-type.service';
//import { FileStageService } from './file-stages/file-stage.service';
//import { ServiceService } from './services/service.service';
import { OrganizationService } from './organizations/organization.service';

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
    adminRouting,ReactiveFormsModule,    FormsModule,
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
    //FileStatusListComponent, FileStatusEditComponent,
    ActionTypeListComponent, //ActionTypeEditComponent,
    OrganizationTypeListComponent, //OrganizationTypeEditComponent,
    //FileStageListComponent,
    //ServiceListComponent, ServiceEditComponent,
    OrganizationListComponent//, OrganizationEditComponent
    // ,DialogOverviewExampleDialog
    ,OrganizationTypeEditComponent
  ],
  entryComponents: [CustomerTypeEditComponent, TestComponent, OrganizationTypeEditComponent],
  providers: [
    UserService,
    CustomerTypeService,
    //FileStatusService,
    ActionTypeService,
    OrganizationTypeService,
    //FileStageService,
    //ServiceService,
    OrganizationService
  ]
})

export class AdminModule { }