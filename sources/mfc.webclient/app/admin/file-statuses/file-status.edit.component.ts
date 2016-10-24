import { Component, Output, Input, EventEmitter } from "@angular/core";
import { FormBuilder, Validators } from '@angular/forms';

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { FileStatus } from './file-status.model'

import { BaseEditComponent, BaseEditContext } from './../../infrastructure/base.component/base-edit.component';

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
    templateUrl: 'app/admin/file-statuses/file-status.edit.component.html',
    providers: [ Modal ]
})

export class FileStatusEditComponent extends BaseEditComponent<FileStatus> {
    constructor(public dialog: DialogRef<BaseEditContext<FileStatus>>, formBuilder: FormBuilder) {
        super(dialog, formBuilder.group({ 'caption' : [null, Validators.required] }));

        //Заполняем форму
        this.formGroup.patchValue({ caption: dialog.context.model.caption });
    }

    mapFormToModel(form: any): void {
        this.context.model.caption = form.caption;
    }
}