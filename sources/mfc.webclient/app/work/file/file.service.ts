import { Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { Headers, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';

import { File } from '../../models/file.model';
import { BaseService } from './../../infrastructure/base.component/base.service';
import { AppSettings } from '../../Infrastructure/application-settings';
import { FileStageConstants } from '../../infrastructure/constants/file-stage.constants';


@Injectable()
export class FileService extends BaseService<File> {
    constructor(http: Http) {
        super(http);
    }

    getApiTag(): string {
        return super.getApiTag() + 'files';
    }

    get(): Promise<File[]> {
        let params: URLSearchParams = new URLSearchParams();
        params.set("pageIndex", "1");
        params.set("pageSize", AppSettings.DEFAULT_PAGE_SIZE.toString());
        return this._http.get(this.getApiTag(), { search: params })
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }

    postStatus(id: number, postType: string, comment: string = "") {
        // console.log(id);
        // console.log(postType);
        // console.log(comment);
        // let params: URLSearchParams = new URLSearchParams();

        // params.set("parameters", JSON.stringify({ "post_type" : postType, "comment" : comment }));
        // if (postType != PostTypes.PostTypeStatusCheck)
        //     params.set("comment", comment);

        return this._http.post(this.getApiTag() + "/" + id, JSON.stringify({ "status" : postType, "comment" : comment }))
            .toPromise()
            .then(x => x)
            .catch(this.handlerError);
    }

    getById(id:number): Promise<File[]> {
        return this._http.get(this.getApiTag()+"/"+id)
            .toPromise()
            .then(x => this.extractData(x))
            .catch(this.handlerError);
    }
}