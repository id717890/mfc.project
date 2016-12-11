import { Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

import { File } from '../../models/file.model';
import { BaseService } from './../../infrastructure/base.component/base.service';

@Injectable()
export class PackageFileService extends BaseService<File> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'package-files';
    }

    getById(id:number): Promise<File[]> {
        return this._http.get(this.getApiTag()+"/"+id)
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }
}