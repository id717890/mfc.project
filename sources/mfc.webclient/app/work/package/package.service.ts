import { Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

import { Package } from '../../models/package.model';
import { BaseService } from './../../infrastructure/base.component/base.service';
import { AppSettings } from '../../Infrastructure/application-settings';

@Injectable()
export class PackageService extends BaseService<Package> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'packages';
    }

    get(): Promise<Package[]> {
        let params: URLSearchParams = new URLSearchParams();
        params.set("pageIndex", "1");
        params.set("pageSize", AppSettings.DEFAULT_PAGE_SIZE.toString());
        return this._http.get(this.getApiTag(), { search: params })
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }
}