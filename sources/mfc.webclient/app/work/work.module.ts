import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { WorkComponent } from './work.component';
import { AcceptionListComponent } from './acception/acception-list.component';  // Приемы
import { AcceptionService } from './acception/acception.service';  // Приемы
import { MyDatePickerModule } from 'mydatepicker';  // https://github.com/kekeh/mydatepicker
import { Ng2PaginationModule } from 'ng2-pagination'; //https://github.com/michaelbromley/ng2-pagination

import { BusyModule, BusyConfig } from 'angular2-busy';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        BusyModule.forRoot(
            new BusyConfig({
                message: 'Загрузка...',
                backdrop: true,
                delay: 50,
                minDuration: 100
            })
        ),
        MyDatePickerModule,
        Ng2PaginationModule
    ],
    declarations: [
        AcceptionListComponent,
    ],
    providers: [
        AcceptionService
    ]
})

export class WorkModule { }