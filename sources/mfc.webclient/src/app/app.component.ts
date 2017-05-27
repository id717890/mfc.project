import { Component } from '@angular/core';

@Component({
  selector: 'mfc-app',
  templateUrl: 'app/app.component.html'
})
export class AppComponent  { 
  name = 'Angular'; 
  
  showDialog = false;

  onShowDialog(){
    this.showDialog = !this.showDialog;
    console.log(this.showDialog);
  }
  
  constructor() { }
}
