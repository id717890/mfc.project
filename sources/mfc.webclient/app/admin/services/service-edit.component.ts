import { Component, Output, Input, EventEmitter } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { Service } from './../../models/service.model';

import {BaseEditComponent, BaseEditContext} from './../../infrastructure/base.component/base-edit.component';

@Component({
    selector: 'modal-content',
    styles: [`
        .input-line {
            margin-bottom: 25px;
            width: 100%;
        }
        .input-buttons {
            margin-top: 10px;
        }
    `],
    templateUrl: 'app/admin/services/service-edit.component.html',
    providers: [ Modal ]
})

export class ServiceEditComponent extends BaseEditComponent<Service> {
    constructor(public dialog: DialogRef<BaseEditContext<Service>>) {
        super(dialog);
    }
}