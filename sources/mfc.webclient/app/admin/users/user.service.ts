import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch'
import { User } from './user.model';
import {BaseService} from './../../infrastructure/base-service';

@Injectable()
export class UserService extends BaseService {
    constructor(http: Http) {
        super(http);
    }

    getUsers(): Promise<User[]> {
        return this._http.get(this.apiUrl + 'user')
            .toPromise()
            .then(this.extractData)
            .catch(this.handlerError)
    }

    createUser(user: User) {
        let body = JSON.stringify(user);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this._http.post(this.apiUrl + 'user', body, options)
            .toPromise()
            .then()
            .catch(this.handlerError)
    }

    updateUser(user: User) {
        let body = JSON.stringify(user);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        let url = this.apiUrl + 'user/' + user.id;

        return this._http.put(url, body, options)
            .toPromise()
            .then()
            .catch(this.handlerError)
    }

    deleteUser(user: User) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        let url = this.apiUrl + 'user/' + user.id;

        return this._http.delete(url, options)
            .toPromise()
            .then()
            .catch(this.handlerError)
    }

    private extractData(res: Response) {
        return res.json();
    }

    private handlerError(error: any) {
        if (error.status === 409) {
            alert(error.json());
        }
        else {
            console.log(error);
        }
    }
}