import { NgModule } from '@angular/core';
import { RequestOptions } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';

import { AdminModule } from './admin/admin.module';
import { WorkModule } from './work/work.module';

import { AppComponent } from './app.component';
import { WorkComponent } from './work/work.component';
import { MenuComponent } from './menu/menu.component';

import { ActionTypeEditComponent } from './admin/action-types/action-type.edit.component';
import { FileStatusEditComponent } from './admin/file-statuses/file-status.edit.component';
import { CustomerTypeEditComponent } from './admin/customer-types/customer-type-edit.component';
import { UserEditComponent } from './admin/users/user-edit.component';
import { OrganizationTypeEditComponent } from './admin/organization-types/organization-type-edit.component';
import { ServiceEditComponent } from './admin/services/service-edit.component';
import { OrganizationEditComponent } from './admin/organizations/organization-edit.component';
import { AcceptionEditComponent } from './work/acception/acception-edit.component'

import { routing, appRoutingProviders } from './app.router';
import { DefaultRequestOptions } from './infrastructure/default-request-options';

@NgModule({
  imports: [
    BrowserModule,
    AdminModule,
    WorkModule,
    ModalModule.forRoot(),
    BootstrapModalModule,
    routing
  ],
  declarations: [
    AppComponent,
    MenuComponent,
    WorkComponent,
  ],
  bootstrap: [AppComponent],
  providers: [
    appRoutingProviders,
    { provide: RequestOptions, useClass: DefaultRequestOptions },
  ],
  entryComponents: [
    FileStatusEditComponent,
    ActionTypeEditComponent,
    CustomerTypeEditComponent,
    UserEditComponent,
    OrganizationTypeEditComponent,
    ServiceEditComponent,
    OrganizationEditComponent,
    AcceptionEditComponent
  ]
})
export class AppModule { }