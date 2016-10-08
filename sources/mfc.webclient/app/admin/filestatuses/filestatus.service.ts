import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from "rxjs/Observable";

import "rxjs/Rx";
import 'rxjs/add/operator/toPromise';

import { BaseService } from '../../infrastructure/base-service';

export class FileStatus {
    constructor(public id: number, public caption: string) { }
}

@Injectable()
export class FileStatusService extends BaseService {
    constructor(http: Http) {
        super(http);
    }

    get(): Promise<FileStatus[]> {
        return this._http.get(this.apiUrl + 'filestatus')
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    getById(id:number): Promise<FileStatus> {
        return this._http.get(this.apiUrl + 'filestatus/' + id)
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    create(filestatus: FileStatus): Promise<FileStatus> {
        let body = JSON.stringify(filestatus);
        return this._http.post(this.apiUrl + 'filestatus/', body)
            .flatMap((x:Response) => {
                var location = x.headers.get('Location');
                return this._http.get(location);
            }).map((x:Response) => x.json())
            .toPromise()
            .then(x => new FileStatus(x.id, x.caption));
    }

    update(filestatus: FileStatus) {
        let body = JSON.stringify(filestatus);
        return this._http.put(this.apiUrl + 'filestatus/' + filestatus.id, body)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    delete(filestatus: FileStatus) {
        return this._http.delete(this.apiUrl + 'filestatus/' + filestatus.id)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    private handlerError(error: any) {
        console.log(error);
    }
}