import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent }   from './app.component';
import { MenuComponent }   from './menu/menu.component';
import { WorkspaceComponent} from './workspace/workspace.component';

@NgModule({
    imports: [BrowserModule],
    declarations: [AppComponent, MenuComponent, WorkspaceComponent],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }