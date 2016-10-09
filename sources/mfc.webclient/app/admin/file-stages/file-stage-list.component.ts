import { Component } from '@angular/core';
import { FileStage, FileStageService } from './file-stage.service';
import { FileStatus, FileStatusService } from '../filestatuses/filestatus.service';

@Component({
    selector: 'mfc-file-stage-list',
    templateUrl: 'app/admin/file-stages/file-stage-list.component.html',
    styles:[`
        table tr td {
            padding:10px;
        }
    `]
})

export class FileStageListComponent {
    private fileStages: FileStage[];
    private fileStatuses: FileStatus[];

    constructor(private fileStageService: FileStageService, private fileStatusService: FileStatusService) {
        fileStageService.get().then(x => this.fileStages = x);
        fileStatusService.get().then(x => this.fileStatuses = x);
    }

    onChangeStatus(fileStage: string, status_id: number) {
        let find_stage = this.fileStages.find(x => x.code == fileStage);
        let find_status = this.fileStatuses.find(x => x.id == status_id);
        find_stage.status = find_status;
    }

    saveChanges() {
        for (let i in this.fileStages) {
            this.fileStageService.update(this.fileStages[i]);
        }
        alert("Изменения сохранены");
    }

}