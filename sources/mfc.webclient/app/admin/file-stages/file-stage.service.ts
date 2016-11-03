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

    put(model: FileStage) {
        return this._http.put(this.getApiTag() + "/" + model.code, JSON.stringify(model))
            .toPromise()
            .then((response) => {
                if (response.status == 200) return true;
                else {
                    console.log(response);
                    return false;
                }
            })
            .catch(this.handlerError);
    }
}