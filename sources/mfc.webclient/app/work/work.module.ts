import { NgModule }      from '@angular/core';
import { Http, HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { WorkComponent } from './work.component';
import { AcceptionComponent } from './acception/acception-list.component';  // Приемы

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
    ],
    declarations: [
        AcceptionComponent,
    ],
    providers: [
    ]
})

export class WorkModule { }