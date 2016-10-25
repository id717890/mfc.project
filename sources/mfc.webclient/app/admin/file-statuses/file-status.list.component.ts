import { Component } from '@angular/core';
import { OnInit } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';

import { BaseListComponent} from './../../infrastructure/base.component/base-list.component';

import { FileStatusService } from './file-status.service'
import { FileStatus } from '../../models/file-status.model'
import { FileStatusEditComponent } from './file-status.edit.component'

@Component({
    selector: 'mfc-file-status-list',
    templateUrl: 'app/admin/file-statuses/file-status.list.component.html',
    providers: [Modal]
})

export class FileStatusListComponent extends BaseListComponent<FileStatus> implements OnInit {
    busy: Promise<any>;
    busyMessage: string;

    constructor(public modal: Modal, private fileStatusService: FileStatusService) {
        super(modal, fileStatusService);
    }

     newModel(): FileStatus {
         return new FileStatus(null, '');
     };

     cloneModel(model : FileStatus): FileStatus {
         return new FileStatus(model.id, model.caption);
     };

     getEditComponent() : any {
         return FileStatusEditComponent;
     }
}