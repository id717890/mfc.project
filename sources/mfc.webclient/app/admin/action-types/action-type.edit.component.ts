import { Component, Output, Input, EventEmitter } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { ActionType } from './action-type.model'

export class ActionTypeEditContext extends BSModalContext {
    public title: string;
    public actionType: ActionType;
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
    templateUrl: 'app/admin/action-types/action-type.edit.component.html',
    providers: [ Modal ]
})

export class ActionTypeEditComponent implements CloseGuard, ModalComponent<ActionTypeEditContext> {
    context: ActionTypeEditContext;
    is_changing: boolean;

    public caption: string;
    public need_make_file: boolean;

    constructor(public dialog: DialogRef<ActionTypeEditContext>) {
        this.context = dialog.context;

        this.is_changing = !dialog.context.actionType;
        this.caption = this.is_changing ? dialog.context.actionType.caption : '';
        this.need_make_file = this.is_changing ? dialog.context.actionType.need_make_file : false;
        
        dialog.setCloseGuard(this);
    }

    beforeDismiss(): boolean {
        return false;
    }

    beforeClose(): boolean {
        return this.caption !== '';
    }

    public checked(actionType: ActionType) {
        actionType.need_make_file = !actionType.need_make_file;
    }
}