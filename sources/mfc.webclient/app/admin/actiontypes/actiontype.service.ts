import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from "rxjs/Observable";

import "rxjs/Rx";
import 'rxjs/add/operator/toPromise';

import { BaseService } from '../../infrastructure/base-service';

export class ActionType {
    constructor(public id: number, public caption: string, public need_make_file: boolean) { }
}

@Injectable()
export class ActionTypeService extends BaseService {
    constructor(http: Http) {
        super(http);
    }

    get(): Promise<ActionType[]> {
        return this._http.get(this.apiUrl + 'actiontype')
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    getById(id:number): Promise<ActionType> {
        return this._http.get(this.apiUrl + 'actiontype/' + id)
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    create(actiontype: ActionType): Promise<ActionType> {
        let body = JSON.stringify(actiontype);
        let options = this.optionsDefaults();
        return this._http.post(this.apiUrl + 'actiontype/', body, options)
            .flatMap((x:Response) => {
                var location = x.headers.get('Location');
                return this._http.get(location);
            }).map((x:Response) => x.json())
            .toPromise()
            .then(x => new ActionType(x.id, x.caption, x.need_make_file));
    }

    update(actiontype: ActionType) {
        let body = JSON.stringify(actiontype);
        let options = this.optionsDefaults();
        return this._http.put(this.apiUrl + 'actiontype/' + actiontype.id, body, options)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    delete(actiontype: ActionType) {
        let options = this.optionsDefaults();
        return this._http.delete(this.apiUrl + 'actiontype/' + actiontype.id, options)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    private handlerError(error: any) {
        console.log(error);
    }
}