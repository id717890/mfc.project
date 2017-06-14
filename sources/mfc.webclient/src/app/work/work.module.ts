import { NgModule } from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MaterialModule, MdNativeDateModule} from '@angular/material';

import { WorkComponent } from './work.component';

// Приемы
import { ActionListComponent } from './action/action-list.component';
import { ActionService } from './action/action.service';
import { DateService } from './../infrastructure/assistant/date.service';
import { ActionEditComponent } from './action/action-edit.component';

// Дела
import { FileListComponent } from './file/file-list.component';
import { FileService } from './file/file.service';
//import { FileEditComponent } from './file/file-edit.component';
//import { FileControlEditComponent } from './file/file-control-edit.component';
//import { FileHistoryComponent } from './file/file-history.component';
//import { FileStatusHistoryService } from './file/file-status-history.service';
//import { FileAcceptComponent } from './file/file-accept.component';

// Пакеты
//import { PackageListComponent } from './package/package-list.component';
//import { PackageService } from './package/package.service';
//import { PackageFileService } from './package/package-file.service';
//import { PackageEditComponent } from './package/package-edit.component';

//import { MyDatePickerModule } from 'mydatepicker';
import { Ng2PaginationModule } from 'ng2-pagination'; //https://github.com/michaelbromley/ng2-pagination
//import { Ng2CompleterModule } from "ng2-completer";  //https://github.com/oferh/ng2-completer

import { BusyModule } from '../shared/busy/busy.module';
import { BusyConfig } from '../shared/busy/busy-config';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        MaterialModule,
        MdNativeDateModule,
        //Ng2CompleterModule,
        ReactiveFormsModule,
        BusyModule.forRoot(
            new BusyConfig({
                message: 'Загрузка...',
                backdrop: true,
                delay: 50,
                minDuration: 100
            })
        ),
        Ng2PaginationModule
        /*
        MyDatePickerModule,
        */
    ],
    entryComponents: [
        ActionEditComponent,
    ],
    declarations: [
        ActionListComponent, 
        ActionEditComponent,
        FileListComponent, //FileEditComponent, FileControlEditComponent, FileHistoryComponent, FileAcceptComponent,
        //PackageListComponent, PackageEditComponent*/
    ],
    providers: [
        ActionService
        , DateService 
        , FileService
        /*PackageService, PackageFileService,
        FileStatusHistoryService*/
    ]
})

export class WorkModule { }