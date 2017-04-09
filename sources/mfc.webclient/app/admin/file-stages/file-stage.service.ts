import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { BaseService } from './../../infrastructure/base.component/base.service';
import { FileStage } from '../../models/file-stage.model';

@Injectable()
export class FileStageService extends BaseService<FileStage> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'file-stages';
    }
}