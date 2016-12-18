import { Component, AfterViewInit, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";

import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { FileStatusHistoryService } from './file-status-history.service';
import { FileStatusHistory } from '../../models/file-status-history.model';

@Component({
    selector: 'modal-content',
    templateUrl: 'app/work/file/file-history.component.html',
    providers: [Modal]
})

export class FileHistoryComponent implements ModalComponent<FileHistoryContext>, OnInit {
    busy: Promise<any>;
    busyMessage: string = "Загрузка истории статусов...";
    historyFile: FileStatusHistory[];

    constructor(
        public dialog: DialogRef<FileHistoryContext>
        , private _fileStatusHistoryService: FileStatusHistoryService
    ) { }

    ngOnInit() {
        this.busy = this._fileStatusHistoryService.getById(this.dialog.context.id).then(x => {
            this.historyFile = x["data"];
        });
    }

    beforeDismiss(): boolean {
        return false;
    }
}

export class FileHistoryContext extends BSModalContext {
    public id: number;
}