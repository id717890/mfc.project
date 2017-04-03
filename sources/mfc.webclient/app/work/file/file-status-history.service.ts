import { Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

import { FileStatusHistory } from '../../models/file-status-history.model';
import { BaseService } from './../../infrastructure/base.component/base.service';

@Injectable()
export class FileStatusHistoryService extends BaseService<FileStatusHistory> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'file-history';
    }

    getById(id: number): Promise<File[]> {
        return this._http.get(this.getApiTag() + "/" + id)
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }
}