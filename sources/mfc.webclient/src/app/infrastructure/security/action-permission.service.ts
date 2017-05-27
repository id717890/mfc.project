import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { AbstractService } from './../abstract-service';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ActionPermissionService extends AbstractService {
    constructor(protected _http: Http) {
        super(_http);
    }

    getApiTag(): string {
        return this.apiUrl + 'action-permissions';
    }

    get(module: string): Promise<string[]> {
        return this._http.get(this.getApiTag() + "/" + module)
            .toPromise()
            .then((x: any) => this.extractData(x))
            .catch(this.handlerError);
    }

    extractData(response: Response) {
        return <string[]>response.json();
    }

    handlerError(error: any) {
        console.log(error.message);
    }
}