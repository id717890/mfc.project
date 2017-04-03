import { Component } from "@angular/core";
import { FormBuilder } from '@angular/forms';

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { ActionType } from '../../models/action-type.model'
import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

export class ActionTypeEditContext extends BSModalContext {
    public title: string;
    public actionType: ActionType;
}

@Component({
    selector: 'modal-content',
    templateUrl: 'app/admin/action-types/action-type.edit.component.html',
    providers: [Modal]
})

export class ActionTypeEditComponent extends BaseEditComponent<ActionType> {
    constructor(public dialog: DialogRef<BaseEditContext<ActionType>>) {
        super(dialog);
    }
}