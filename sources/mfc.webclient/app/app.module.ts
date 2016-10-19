import { NgModule } from '@angular/core';
import { RequestOptions } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';

import { AdminModule } from './admin/admin.module';

import { AppComponent } from './app.component';
import { WorkComponent } from './work/work.component';
import { MenuComponent } from './menu/menu.component';
import { UserComponent } from './user/user.component';
import { ActionTypeEditComponent } from './admin/actiontypes/actiontype-edit.component';
import { FileStatusEditComponent } from './admin/filestatuses/filestatus-edit.component';
import { CustomerTypeEditComponent } from './admin/customer-types/customer-type-edit.component';

import { routing, appRoutingProviders} from './app.router';
import { DefaultRequestOptions } from './infrastructure/default-request-options';

@NgModule({
  imports: [
    BrowserModule,
    AdminModule,
    ModalModule.forRoot(),
    BootstrapModalModule,
    routing
  ],
  declarations: [
    AppComponent,
    MenuComponent,
    WorkComponent,
    UserComponent,
  ],
  bootstrap: [AppComponent],
  providers: [
    appRoutingProviders,
    { provide: RequestOptions, useClass: DefaultRequestOptions },
  ],
  entryComponents: [
     FileStatusEditComponent,
     ActionTypeEditComponent,
     CustomerTypeEditComponent
  ]
})
export class AppModule { }