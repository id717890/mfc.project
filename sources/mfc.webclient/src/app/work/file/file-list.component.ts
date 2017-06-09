import { Component, AfterViewInit } from '@angular/core';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileService } from './file.service';
// import { OrganizationEditComponent } from './organization-edit.component';
import { DialogService } from '../../infrastructure/dialog/dialog.service';
import { File } from './../../models/file.model';
import { OrganizationType } from './../../models/organization-type.model';
import { MdDialog, MdButton, MdDialogRef } from '@angular/material';

@Component({
    selector: 'mfc-organization-list',
    templateUrl: 'app/work/file/file-list.component.html'
})

export class FileListComponent extends BaseListComponent<File> implements AfterViewInit {
    dateBegin: Date;
    dateEnd: Date;

    constructor(public dialog: MdDialog, protected organizationService: FileService, protected dialogService: DialogService) {
        super(dialog, organizationService, dialogService);
    }

    ngAfterViewInit() {
        this.prepareForm();
    }

    private prepareForm(): void {
        var date = new Date();
        this.dateBegin = new Date(date.getFullYear(), date.getMonth(), 1);
        this.dateEnd= new Date(date.getFullYear(), date.getMonth() + 1, 0);
    }

    newModel(): File {
        return new File(null, '', null, null, null, null, null, null, false);
    };

    cloneModel(model: File): File {
        return new File(
            model.id,
            model.caption,
            model.date,
            model.action,
            model.expert,
            model.controller,
            model.status,
            model.organization,
            model.is_selected
        );
    };

    getEditComponent(): any {
        return null;
    }
}