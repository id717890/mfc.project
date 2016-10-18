import { Component, Output, Input, EventEmitter } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { CustomerTypeService } from './customer-type.service';
import { CustomerType } from './customer-type';

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
    templateUrl: 'app/admin/customer-types/customer-type-edit.component.html',
    providers: [ Modal ]
})

export class CustomerTypeEditComponent extends BaseEditComponent<CustomerType> {
    constructor(public dialog: DialogRef<BaseEditContext<CustomerType>>) {
        super(dialog);
    }
}