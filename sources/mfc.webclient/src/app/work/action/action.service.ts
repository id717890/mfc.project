import { Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

import { Action } from '../../models/action.model';
import { BaseService } from './../../infrastructure/base.component/base.service';
import { AppSettings } from '../../Infrastructure/application-settings';
import { ResponseData } from '../../Infrastructure/base.component/response-data';

@Injectable()
export class ActionService extends BaseService<Action> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'actions';
    }

    get(): Promise<ResponseData<Action>> {
        let params: URLSearchParams = new URLSearchParams();
        params.set("pageIndex", "1");
        params.set("pageSize", AppSettings.DEFAULT_PAGE_SIZE.toString());
        return this._http.get(this.getApiTag(), { search: params })
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }

    postCopyAction(actions: Action[]) {
        return this._http.post(this.getApiTag() + "/copy", JSON.stringify(actions))
            .toPromise()
            .catch(this.handlerError);
    }
}