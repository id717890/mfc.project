import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
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
        return this._http.get('http://localhost:4664/api/filestatus')
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    getById(id:number): Promise<FileStatus> {
        return this._http.get('http://localhost:4664/api/filestatus/' + id)
            .toPromise()
            .then(x => x.json())
            .catch(this.handlerError)
    }

    create(filestatus: FileStatus): Promise<FileStatus> {
        let body = JSON.stringify(filestatus);
        let options = new RequestOptions({ headers : new Headers({ 'Content-Type': 'application/json' }) });

        return this._http.post(this.apiUrl + 'filestatus/', body, options)
            .flatMap((x:Response) => {
                var location = x.headers.get('Location');
                console.log('headers: ' + x.headers.keys());
                console.log('location: ' + location);
                return this._http.get(location);
            }).map((x:Response) => x.json())
            .toPromise()
            .then(x => new FileStatus(x.id, x.caption));
    }

    update(filestatus: FileStatus) {
        console.log(filestatus);
        let body = JSON.stringify(filestatus);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this._http.put(this.apiUrl + 'filestatus/' + filestatus.id, body, options)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    delete(filestatus: FileStatus) {
        console.log(filestatus);
        
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this._http.delete(this.apiUrl + 'filestatus/' + filestatus.id, options)
            .toPromise()
            .then()
            .catch(this.handlerError);
    }

    private handlerError(error: any) {
        console.log(error);
    }
}