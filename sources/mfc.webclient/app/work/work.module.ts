import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { WorkComponent } from './work.component';

// Приемы
import { ActionListComponent } from './action/action-list.component';
import { ActionService } from './action/action.service';
import { ActionEditComponent } from './action/action-edit.component';

// Дела
import { FileListComponent } from './file/file-list.component';
import { FileService } from './file/file.service';
import { FileEditComponent } from './file/file-edit.component';

// Пакеты
import { PackageListComponent } from './package/package-list.component';
import { PackageService } from './package/package.service';

import { MyDatePickerModule } from 'mydatepicker';  // https://github.com/kekeh/mydatepicker
import { Ng2PaginationModule } from 'ng2-pagination'; //https://github.com/michaelbromley/ng2-pagination
import { Ng2CompleterModule } from "ng2-completer";  //https://github.com/oferh/ng2-completer

import { BusyModule, BusyConfig } from 'angular2-busy';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        Ng2CompleterModule,
        ReactiveFormsModule,
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
        ActionListComponent, ActionEditComponent,
        FileListComponent, FileEditComponent,
        PackageListComponent
    ],
    providers: [
        ActionService,
        FileService,
        PackageService
    ]
})

export class WorkModule { }