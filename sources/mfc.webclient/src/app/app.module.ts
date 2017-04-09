import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { DialogComponent } from './dialog/dialog.component';



@NgModule({
  imports: [BrowserModule, BrowserAnimationsModule],
  declarations: [AppComponent, DialogComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
