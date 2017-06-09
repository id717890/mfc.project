import { Component, Inject } from "@angular/core";
import { NgForm } from "@angular/forms";
import { FileStatus } from '../../models/file-status.model'
import { MaterialModule, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { BaseEdit2Component } from './../../infrastructure/base.component/base-edit2.component';
import { BaseContext } from './../../infrastructure/base.component/base-context.component';

@Component({
    selector: 'file-status-create-edit-dlg',
    templateUrl: 'app/admin/file-statuses/file-status.edit.component.html'
})

export class FileStatusEditComponent extends BaseEdit2Component<FileStatus> {
    constructor(
        @Inject(MD_DIALOG_DATA) data: BaseContext<FileStatus>,
        public dialogRef: MdDialogRef<FileStatusEditComponent>
    ) {
        super(data, dialogRef);
    }
}