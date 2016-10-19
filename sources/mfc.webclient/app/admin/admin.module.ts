import { NgModule }      from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AdminComponent } from './admin.component';
import { CustomerTypeListComponent } from './customer-types/customer-type-list.component';
import { CustomerTypeEditComponent } from './customer-types/customer-type-edit.component';
import { UserListComponent } from './users/user-list.component';
import { UserEditComponent } from './users/user-edit.component';
import { UserPasswordComponent } from './users/user-password.component';
import { FileStatusEditComponent } from './filestatuses/filestatus-edit.component';
import { FileStatusListComponent } from './filestatuses/filestatus-list.component';
import { ActionTypeEditComponent } from './actiontypes/actiontype-edit.component';
import { ActionTypeListComponent } from './actiontypes/actiontype-list.component';
import { OrganizationTypeListComponent } from './organization-types/organization-type-list/organization-type-list.component';
import { FileStageListComponent } from './file-stages/file-stage-list.component';

import { adminRouting } from './admin.router';

import { UserService } from './users/user.service';
import { CustomerTypeService } from './customer-types/customer-type.service';
import { FileStatusService } from './filestatuses/filestatus.service';
import { ActionTypeService } from './actiontypes/actiontype.service';
import { OrganizationTypeService } from './organization-types/organization-type.service';
import { FileStageService } from './file-stages/file-stage.service';
import { BusyModule, BusyConfig } from 'angular2-busy'; //Сторонний пакет для реализации busy indicatpr angular2-busy https://github.com/devyumao/angular2-busy

@NgModule({
  imports: [
    BrowserModule,
    HttpModule,
    adminRouting,
    FormsModule,
    BusyModule.forRoot(
      new BusyConfig({
        message: 'Загрузка...',
        backdrop: true,
        /*template: `
          <div class="panel panel-default ">
            <div class="sk-wave">
              <div class="sk-rect sk-rect1"></div>
              <div class="sk-rect sk-rect2"></div>
              <div class="sk-rect sk-rect3"></div>
              <div class="sk-rect sk-rect4"></div>
              <div class="sk-rect sk-rect5"></div>
              <div class="sk-text">{{message}}</div>       
            </div>
          </div>    
                `,*/
        delay: 50,
        minDuration: 100
      })
    )
  ],
  declarations: [
    AdminComponent,
    CustomerTypeListComponent, CustomerTypeEditComponent,
    UserListComponent, UserEditComponent, UserPasswordComponent,   //Справочник "Пользователи"
    FileStatusListComponent, FileStatusEditComponent,
    ActionTypeListComponent, ActionTypeEditComponent,
    OrganizationTypeListComponent,
    FileStageListComponent,                                                             //Настройка этапов движения дела
  ],
  providers: [
    UserService,
    CustomerTypeService,
    FileStatusService,
    ActionTypeService,
    OrganizationTypeService,
    FileStageService
  ]
})

export class AdminModule { }