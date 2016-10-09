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
import { DefaultRequestOptions } from './infrastructure/default-request-options';
import { FileStatusEditComponent } from './admin/filestatuses/filestatus-edit.component';

import { routing, appRoutingProviders} from './app.router';


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
    UserComponent
  ],
  bootstrap: [AppComponent],
  providers: [
    appRoutingProviders,
    { provide: RequestOptions, useClass: DefaultRequestOptions }
  ],
  entryComponents: [ FileStatusEditComponent ]
})
export class AppModule { }