import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import { BaseService } from './../../infrastructure/base.component/base.service';
import { FileStatus } from './file-status.model';

@Injectable()
export class FileStatusService extends BaseService<FileStatus> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag() : string {
        return super.getApiTag() + 'file-statuses';
    }
}