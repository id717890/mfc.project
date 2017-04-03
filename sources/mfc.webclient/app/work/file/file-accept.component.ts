import { Component, AfterViewInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/file/file-accept.component.html',
    providers: [Modal]
})

export class FileAcceptComponent implements ModalComponent<FileAcceptContext> {
    busy: Promise<any>;
    busyMessage: string = "Загрузка истории статусов...";

    constructor(
        public dialog: DialogRef<FileAcceptContext>
    ) { }

    beforeDismiss(): boolean {
        return false;
    }
}

export class FileAcceptContext extends BSModalContext {
    public accept_list: File[];
    public reject_list: File[];
}