import { Component, AfterViewInit } from '@angular/core';

import { Modal, OneButtonPresetBuilder, BSModalContext } from 'angular2-modal/plugins/bootstrap';
import { BaseListComponent } from './../../infrastructure/base.component/base-list.component';
import { FileStageService } from './file-stage.service';
import { FileStage } from '../../models/file-stage.model';

import { FileStatusService } from '../file-statuses/file-status.service';
import { FileStatus } from '../../models/file-status.model';

@Component({
    selector: 'mfc-file-stage-list',
    templateUrl: 'app/admin/file-stages/file-stage-list.component.html',
    styles: [`
        table tr td {
            padding:10px;
        }
    `]
})

export class FileStageListComponent extends BaseListComponent<FileStage> implements AfterViewInit {
    fileStatuses: FileStatus[];

    constructor(public modal: Modal, private fileStageService: FileStageService, private fileStatusService: FileStatusService) {
        super(modal, fileStageService);
    }

    ngAfterViewInit() {
        this.busy = this.fileStatusService.get().then(x => {
            this.fileStatuses = x["data"];
        });
    }

    onChangeStatus(fileStage: string, status_id: number) {
        let find_stage = this.models.find(x => x.code == fileStage);
        let find_status = this.fileStatuses.find(x => x.id == status_id);
        find_stage.status = find_status;
    }

    saveChanges() {
        for (let i in this.models) {
            this.busy = this.fileStageService.put(this.models[i]).then(isOk => {
                if (isOk) {
                } else {
                    alert('Ошибка сохранения');
                }
            })
        }
    }

    newModel(): FileStage {
        return null;
    };

    cloneModel(model: FileStage): FileStage {
        return null;
    };

    getEditComponent(): any {
        return null;
    }
}