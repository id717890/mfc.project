import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule, MdDialogModule, MdButtonModule, MdCardModule, MdNativeDateModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { RequestOptions } from '@angular/http';

import { WorkModule } from './work/work.module';
import { AdminModule } from './admin/admin.module';
import { Material } from './shared/material.module';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { WorkComponent } from './work/work.component';

// Dialogs
import { DialogService } from './infrastructure/dialog/dialog.service';
import { DialogConfirm } from './infrastructure/dialog/dialog-confirm.component';
import { DialogAlert } from './infrastructure/dialog/dialog-alert.component';

import { ActionPermissionService } from './infrastructure/security/action-permission.service';

import { DefaultRequestOptions } from './infrastructure/default-request-options';
import { routing, appRoutingProviders } from './app.router';


@NgModule({
  imports: [
    BrowserModule, MaterialModule, Material, MdNativeDateModule, BrowserAnimationsModule, routing, WorkModule, AdminModule,
  ],
  exports: [
    DialogConfirm, DialogAlert,
  ],
  declarations: [
    AppComponent, MenuComponent, WorkComponent, DialogConfirm, DialogAlert
  ],
  bootstrap: [
    AppComponent
  ],
  providers: [
    DialogService,
    appRoutingProviders,
    { provide: RequestOptions, useClass: DefaultRequestOptions },
    ActionPermissionService
  ],
  entryComponents: [DialogConfirm, DialogAlert],
})
export class AppModule { }
