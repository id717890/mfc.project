import { Component, ViewContainerRef, ViewEncapsulation } from '@angular/core';

import { Overlay } from 'angular2-modal';
import { Modal } from 'angular2-modal/plugins/bootstrap';

@Component({
    selector: 'my-app',
    templateUrl: 'app/app.component.html'

})
export class AppComponent {
    date: Date = new Date()

    constructor(overlay: Overlay, vcRef: ViewContainerRef, public modal: Modal) {
        overlay.defaultViewContainer = vcRef;
    }
}