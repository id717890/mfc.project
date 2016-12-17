import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { File } from '../../models/file.model';
import { FileStageConstants } from '../../Infrastructure/constants/file-stage.constants';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/file/file-control-edit.component.html',
    providers: [Modal]
})

export class FileControlEditComponent implements ModalComponent<FileControlEditContext>, OnInit {
    constructor(public dialog: DialogRef<FileControlEditContext>) {
    }

    ngOnInit() {
        switch (this.dialog.context.status) {
            case FileStageConstants.SEND_FOR_CONTROL:
                this.dialog.context.title = "Отправить на провреку";
                break;
            case FileStageConstants.RETURN_FOR_FIX:
                this.dialog.context.title = "Возвратить для исправления";
                break;
            case FileStageConstants.CHECKED:
                this.dialog.context.title = "Подтверждение проверки дела";
                break;
        }
    }

    beforeDismiss(): boolean {
        return false;
    }
}

export class FileControlEditContext extends BSModalContext {
    public title: string;
    public model: string;
    public file: File;
    public status: string;
}