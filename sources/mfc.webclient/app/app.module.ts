import { NgModule } from '@angular/core';
import { RequestOptions } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';

import { BusyModule, BusyConfig } from 'angular2-busy'; //Сторонний пакет для реализации busy indicatpr angular2-busy https://github.com/devyumao/angular2-busy

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
import { ActionEditComponent } from './work/action/action-edit.component';
import { FileEditComponent } from './work/file/file-edit.component';

import { routing, appRoutingProviders } from './app.router';
import { DefaultRequestOptions } from './infrastructure/default-request-options';
import { ActionPermissionService } from './infrastructure/security/action-permission.service';

import { ErrorLogService } from "./infrastructure/error.service";                   //https://www.bennadel.com/blog/3138-creating-a-custom-errorhandler-in-angular-2-rc-6.htm
import { LOGGING_ERROR_HANDLER_PROVIDERS } from "./infrastructure/error.handler";
import { LOGGING_ERROR_HANDLER_OPTIONS } from "./infrastructure/error.handler";

@NgModule({
  imports: [
    BrowserModule,
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
    ),
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
    ActionPermissionService,
    ErrorLogService,

    // CAUTION: This providers collection overrides the CORE ErrorHandler with our
    // custom version of the service that logs errors to the ErrorLogService.
    LOGGING_ERROR_HANDLER_PROVIDERS,

    // OPTIONAL: By default, our custom LoggingErrorHandler has behavior around
    // rethrowing and / or unwrapping errors. In order to facilitate dependency-
    // injection instead of resorting to the use of a Factory for instantiation,
    // these options can be overridden in the providers collection.
    {
      provide: LOGGING_ERROR_HANDLER_OPTIONS,
      useValue: {
        rethrowError: true,
        unwrapError: false
      }
    }
  ],
  entryComponents: [
    FileStatusEditComponent,
    ActionTypeEditComponent,
    CustomerTypeEditComponent,
    UserEditComponent,
    OrganizationTypeEditComponent,
    ServiceEditComponent,
    OrganizationEditComponent,
    ActionEditComponent,
    FileEditComponent
  ]
})
export class AppModule { }