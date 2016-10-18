import {Component, EventEmitter} from '@angular/core';
declare var jQuery: any;

@Component({
    selector: 'busy-indicator',
    templateUrl: 'app/Infrastructure/busy-service/busy-service.component.html',
    styleUrls: ['app/Infrastructure/busy-service/busy-service.component.css']
})

export class BusyComponent  {
    message: string;

    constructor() {
        this.message="Выполнение операции...";
    }

    public Standby(message?:string) {
        if (message!=null) jQuery("#busy-message").text(message);
        jQuery(".wrapper-loading-indicator").css("display", "block");
    }

    public Ready() {
        jQuery(".wrapper-loading-indicator").css("display", "none");
    }
}