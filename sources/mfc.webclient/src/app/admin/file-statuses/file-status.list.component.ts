import { Component } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileStatusService } from './file-status.service'
import { FileStatus } from '../../models/file-status.model'
import { FileStatusEditComponent } from './file-status.edit.component'
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';
import { DialogService } from '../../infrastructure/dialog/dialog.service';

@Component({
    selector: 'mfc-file-status-list',
    templateUrl: 'app/admin/file-statuses/file-status.list.component.html'
})

export class FileStatusListComponent extends BaseListComponent<FileStatus> {
    busy: Promise<any>;
    busyMessage: string;

    constructor(public dialog: MdDialog, protected actionTypeService: FileStatusService, protected dialogService: DialogService) {
        super(dialog, actionTypeService, dialogService);
    }

    newModel(): FileStatus {
        return new FileStatus(null, '');
    };

    cloneModel(model: FileStatus): FileStatus {
        return new FileStatus(model.id, model.caption);
    };

    getEditComponent(): any {
        return FileStatusEditComponent;
    }
}