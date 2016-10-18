import {Component} from '@angular/core';

@Component({
    selector: 'loading-indicator',
    templateUrl: 'app/infrastructure/loading-service/loading.component.html',
    styleUrls: ['app/infrastructure/loading-service/loading.component.css']

})
export class LoadingIndicator { }

export class LoadingPage {
    public loading: boolean;
    constructor(val: boolean) {
        this.loading = val;
    }
    standby() {
        this.loading = true;
    }
    ready() {
        this.loading = false;
    }
}