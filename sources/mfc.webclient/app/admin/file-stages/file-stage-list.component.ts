import { Component } from '@angular/core';
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

export class FileStageListComponent {
    private fileStages: FileStage[];
    private fileStatuses: FileStatus[];
    busy: Promise<any>;

    constructor(
        private fileStageService: FileStageService,
        private fileStatusService: FileStatusService
    ) {
        fileStatusService.get().then(x => this.fileStatuses = x["data"]);
    }

    ngOnInit() {
        this.busy = this.fileStageService.get().then(x => this.fileStages = x);
    }

    onChangeStatus(fileStage: string, status_id: number) {
        let find_stage = this.fileStages.find(x => x.code == fileStage);
        let find_status = this.fileStatuses.find(x => x.id == status_id);
        find_stage.status = find_status;
    }

    saveChanges() {
        for (let i in this.fileStages) {
            this.fileStageService.update(this.fileStages[i]).then(isOk => {
                if (isOk) {
                } else {
                    alert('Ошибка сохранения');
                }
            })
        }
    }
}