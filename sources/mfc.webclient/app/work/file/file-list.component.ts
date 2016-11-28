import { Component } from "@angular/core";

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileEditComponent } from './file-edit.component';

import { File } from '../../models/file.model';
import { FileService } from './file.service';

@Component({
    selector: "mfc-file-list",
    templateUrl: "app/work/file/file-list.component.html"
})

export class FileListComponent extends BaseListComponent<File> {
    dateBegin: string;
    dateEnd: string;

    /* Настройки для datepicker */
    myDatePickerOptions = {
        todayBtnTxt: 'Today',
        dateFormat: 'dd.mm.yyyy',
        firstDayOfWeek: 'mo',
        inline: false,
        // sunHighlight: true,
        // height: '34px',
        width: '200px',
        // selectionTxtFontSize: '16px'
    };

    constructor(public modal: Modal, private fileService: FileService) {
        super(modal, fileService);
        this.prepareForm();
    }

    private prepareForm(): void {
        let today = new Date();
        let tomorrow = new Date();
        tomorrow.setDate(today.getDate() + 1);
        this.dateBegin = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
        this.dateEnd = tomorrow.getDate() + '.' + (tomorrow.getMonth() + 1) + '.' + tomorrow.getFullYear();
    }

    newModel(): File {
        return new File(null, '',null,null,null,null,null,null);
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
            model.organization
            );
    };

    getEditComponent(): any {
        return FileEditComponent;
    }
} 