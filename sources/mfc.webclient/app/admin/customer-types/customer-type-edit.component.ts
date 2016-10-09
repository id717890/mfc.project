import { Component, Output, Input, EventEmitter } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { CustomerTypeService } from './customer-type.service'
import { CustomerType } from './customer-type'

export class CustomerTypeEditContext extends BSModalContext {
    public title: string;
    public customerType: CustomerType;
}

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

export class CustomerTypeEditComponent implements CloseGuard, ModalComponent<CustomerTypeEditContext> {
    context: CustomerTypeEditContext;
    is_changing: boolean;

    public caption: string;

    constructor(public dialog: DialogRef<CustomerTypeEditContext>) {
        this.context = dialog.context;

        this.is_changing = !dialog.context.customerType;
        this.caption = this.is_changing ? dialog.context.customerType.caption : '';
        
        dialog.setCloseGuard(this);
    }

    beforeDismiss(): boolean {
        return false;
    }

    beforeClose(): boolean {
        return this.caption !== '';
    }
}