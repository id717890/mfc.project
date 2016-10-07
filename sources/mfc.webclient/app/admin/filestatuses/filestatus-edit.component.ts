import { Component, Output, Input, EventEmitter } from "@angular/core";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { FileStatus } from './filestatus.service'

export class FileStatusEditContext extends BSModalContext {
    public title: string;
    public filestatus: FileStatus;
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
    templateUrl: 'app/admin/filestatuses/filestatus-edit.component.html',
    providers: [ Modal ]
})

export class FileStatusEditComponent implements CloseGuard, ModalComponent<FileStatusEditContext> {
    context: FileStatusEditContext;
    is_changing: boolean;

    public caption: string;

    constructor(public dialog: DialogRef<FileStatusEditContext>) {
        this.context = dialog.context;

        this.is_changing = !dialog.context.filestatus;
        this.caption = this.is_changing ? dialog.context.filestatus.caption : '';
        
        dialog.setCloseGuard(this);
    }

    onKeyUp(value) {
        this.caption = value;
        this.dialog.close();
    }

    beforeDismiss(): boolean {
        return false;
    }

    beforeClose(): boolean {
        return this.caption !== '';
    }
}