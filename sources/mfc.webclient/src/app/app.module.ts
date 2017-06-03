import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule, MdDialogModule, MdButtonModule, MdCardModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { RequestOptions } from '@angular/http';

import { WorkModule } from './work/work.module';
import { AdminModule } from './admin/admin.module';
import { Material } from './shared/material.module';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { WorkComponent } from './work/work.component';

import { ActionPermissionService } from './infrastructure/security/action-permission.service';

import { DefaultRequestOptions } from './infrastructure/default-request-options';
import { routing, appRoutingProviders } from './app.router';


@NgModule({
  imports: [
    BrowserModule, Material, BrowserAnimationsModule, routing, WorkModule, AdminModule,
  ],
  declarations: [
    AppComponent, MenuComponent, WorkComponent
  ],
  bootstrap: [
    AppComponent
  ],
  providers: [
    appRoutingProviders,
    { provide: RequestOptions, useClass: DefaultRequestOptions },
    ActionPermissionService
  ]
})
export class AppModule { }
