import { Component, Output, Input, EventEmitter } from "@angular/core";
import { FormBuilder, Validators } from '@angular/forms';

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
    providers: [ Modal ]
})

export class ActionTypeEditComponent extends BaseEditComponent<ActionType> {
    is_changing: boolean;

    public caption: string;
    public need_make_file: boolean;

    constructor(public dialog: DialogRef<BaseEditContext<ActionType>>, formBuilder: FormBuilder) {
        super(dialog, formBuilder.group({ 'caption' : [null, Validators.required], 'need_make_file' : []}));

        this.formGroup.patchValue({ caption: dialog.context.model.caption, need_make_file: dialog.context.model.need_make_file });
    }

    mapFormToModel(form: any): void {
        this.context.model.caption = form.caption;
        this.context.model.need_make_file = form.need_make_file;
    }

    public checked(actionType: ActionType) {
        actionType.need_make_file = !actionType.need_make_file;
    }
}