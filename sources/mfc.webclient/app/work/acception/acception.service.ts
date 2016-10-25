import { Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

import { Acception } from '../../models/acception.model';
import { BaseService } from './../../infrastructure/base.component/base.service';
import { PAGIN_PAGE_SIZE } from '../../Infrastructure/application-messages';

@Injectable()
export class AcceptionService extends BaseService<Acception> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'acceptions';
    }

    get(): Promise<Acception[]> {
        let params: URLSearchParams = new URLSearchParams();
        params.set("pageIndex", "1");
        params.set("pageSize", PAGIN_PAGE_SIZE.toString());
        return this._http.get(this.getApiTag(), { search: params })
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }
}