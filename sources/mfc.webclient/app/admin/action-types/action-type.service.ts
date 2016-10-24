import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from "rxjs/Observable";

import "rxjs/Rx";
import 'rxjs/add/operator/toPromise';

import { ActionType } from './action-type.model';
import { BaseService } from '../../infrastructure/base-service';

@Injectable()
export class ActionTypeService extends BaseService {
    constructor(http: Http) {
        super(http);
    }

    get(): Promise<ActionType[]> {
        return this._http.get(this.apiUrl + 'action-types')
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    getById(id:number): Promise<ActionType> {
        return this._http.get(this.apiUrl + 'action-types/' + id)
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    create(actionType: ActionType): Promise<ActionType> {
        let body = JSON.stringify(actionType);
        return this._http.post(this.apiUrl + 'action-types/', body)
            .flatMap((x:Response) => {
                var location = x.headers.get('Location');
                return this._http.get(location);
            }).map((x:Response) => x.json())
            .toPromise()
            .then(x => new ActionType(x.id, x.caption, x.need_make_file));
    }

    update(actionType: ActionType) {
        let body = JSON.stringify(actionType);
        return this._http.put(this.apiUrl + 'action-types/' + actionType.id, body)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    delete(actionType: ActionType) {
        return this._http.delete(this.apiUrl + 'action-types/' + actionType.id)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    private handlerError(error: any) {
        console.log(error);
    }
}